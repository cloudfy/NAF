﻿using Naf.Filtering.Exceptions;
using Naf.Filtering.Internals;
using Naf.Filtering.Internals.Parsers;
using Naf.Filtering.Model;
using Naf.Filtering.Query;
using Naf.Filtering.Query.Expressions;

namespace Naf.Filtering;

public static class FilterExpressionParser
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filterValue">Required. Filter to parse into query.</param>
    /// <exception cref="InvalidFilterException"></exception>
    /// <returns></returns>
    public static QueryNode Parse<T>(string filterValue) => Parse(filterValue, typeof(T));

    private static QueryNode Parse(string filterValue, Type modelType)
    {
        var provider = new EdmTypeProvider();
        var edmType = provider.GetByClrType(modelType) as EdmComplexType;

        var parserImpl = new FilterExpressionParserImpl(edmType, provider);
        var queryNode = parserImpl.Parse(new FilterExpressionLexer(filterValue));

        return queryNode;
    }

    private sealed class FilterExpressionParserImpl
    {
        private readonly EdmComplexType _model;
        private readonly EdmTypeProvider _typeProvider;
        private readonly Stack<QueryNode> _nodeStack = new();
        private readonly Queue<Token> _tokens = new();
        //private readonly Type? _modelType;
        private int _groupingDepth;
        private BinaryOperatorKind _nextBinaryOperatorKind = BinaryOperatorKind.None;

        internal FilterExpressionParserImpl(EdmComplexType model, EdmTypeProvider typeProvider)
        {
            _model = model;
            _typeProvider = typeProvider;
        }

        internal QueryNode Parse(FilterExpressionLexer filterExpressionLexer)
        {
            while (filterExpressionLexer.MoveNext())
            {
                var token = filterExpressionLexer.Current;

                switch (token.TokenType)
                {
                    case TokenType.And:
                        _nextBinaryOperatorKind = BinaryOperatorKind.And;
                        UpdateExpressionTree();
                        break;

                    case TokenType.Or:
                        _nextBinaryOperatorKind = BinaryOperatorKind.Or;
                        UpdateExpressionTree();
                        break;

                    default:
                        _tokens.Enqueue(token);
                        break;
                }
            }

            _nextBinaryOperatorKind = BinaryOperatorKind.None;
            UpdateExpressionTree();

            if (_groupingDepth != 0 || _nodeStack.Count != 1)
            {
                throw new ParserException("UnableToParseFilter");
            }

            var node = _nodeStack.Pop();

            if (node is BinaryOperatorNode binaryNode && (binaryNode.Left == null || binaryNode.Right == null))
            {
                throw new ParserException("UnableToParseFilter");
            }

            return node;
        }

        private QueryNode ParseFunctionCallNode()
        {
            BinaryOperatorNode? binaryNode = null;
            FunctionCallNode? node = null;

            var stack = new Stack<FunctionCallNode>();

            while (_tokens.Count > 0)
            {
                var token = _tokens.Dequeue();

                switch (token.TokenType)
                {
                    case TokenType.OpenParentheses:
                        if (_tokens.Peek().TokenType == TokenType.CloseParentheses)
                        {
                            // All OData functions have at least 1 or 2 parameters
                            throw new ParserException("UnableToParseFilter");
                        }

                        _groupingDepth++;
                        stack.Push(node);
                        break;

                    case TokenType.CloseParentheses:
                        if (_groupingDepth == 0)
                        {
                            throw new ParserException("UnableToParseFilter");
                        }

                        _groupingDepth--;

                        if (stack.Count > 0)
                        {
                            var lastNode = stack.Pop();

                            if (stack.Count > 0)
                            {
                                stack.Peek().AddParameter(lastNode);
                            }
                            else
                            {
                                if (binaryNode != null)
                                {
                                    binaryNode.Right = lastNode;
                                }
                                else
                                {
                                    node = lastNode;
                                }
                            }
                        }

                        break;

                    case TokenType.FunctionName:
                        node = new FunctionCallNode(token.Value);
                        break;

                    case TokenType.BinaryOperator:
                        binaryNode = new BinaryOperatorNode(node, token.Value.ToBinaryOperatorKind(), null);
                        break;

                    case TokenType.PropertyName:
                        var propertyAccessNode = CreatePropertyAccessNode(token.Value, _model);

                        if (stack.Count > 0)
                        {
                            stack.Peek().AddParameter(propertyAccessNode);
                        }
                        else
                        {
                            if (binaryNode == null) throw new InvalidOperationException("binaryNode is null in TokenType.PropertyName");
                            binaryNode.Right = propertyAccessNode;
                        }

                        break;

                    case TokenType.Date:
                    case TokenType.DateTimeOffset:
                    case TokenType.Decimal:
                    case TokenType.Double:
                    case TokenType.Duration:
                    case TokenType.Enum:
                    case TokenType.False:
                    case TokenType.Guid:
                    case TokenType.Integer:
                    case TokenType.Null:
                    case TokenType.Single:
                    case TokenType.String:
                    case TokenType.TimeOfDay:
                    case TokenType.True:
                        var constantNode = ConstantNodeParser.ParseConstantNode(token, _typeProvider);

                        if (stack.Count > 0)
                        {
                            stack.Peek().AddParameter(constantNode);
                        }
                        else
                        {
                            if (binaryNode == null) throw new InvalidOperationException("binaryNode is null in TokenType.True");
                            binaryNode.Right = constantNode;
                        }

                        break;

                    case TokenType.Comma:
                        if (_tokens.Count < 2)
                        {
                            // If there is a comma in a function call, there should be another argument followed by a closing comma
                            throw new ParserException("UnableToParseFilter");
                        }

                        break;
                }
            }

            if (binaryNode != null)
            {
                return binaryNode;
            }

            return node;
        }

        private QueryNode ParsePropertyAccessNode()
        {
            QueryNode? result = null;

            QueryNode? leftNode = null;
            var operatorKind = BinaryOperatorKind.None;
            QueryNode? rightNode = null;

            while (_tokens.Count > 0)
            {
                var token = _tokens.Dequeue();

                switch (token.TokenType)
                {
                    case TokenType.BinaryOperator:
                        if (operatorKind != BinaryOperatorKind.None)
                        {
                            result = new BinaryOperatorNode(leftNode, operatorKind, rightNode);
                            leftNode = null;
                            rightNode = null;
                        }

                        operatorKind = token.Value.ToBinaryOperatorKind();
                        break;

                    case TokenType.OpenParentheses:
                        _groupingDepth++;
                        break;

                    case TokenType.CloseParentheses:
                        _groupingDepth--;
                        break;

                    case TokenType.FunctionName:

                        if (leftNode == null)
                        {
                            leftNode = new FunctionCallNode(token.Value);
                        }
                        else if (rightNode == null)
                        {
                            rightNode = new FunctionCallNode(token.Value);
                        }

                        break;

                    case TokenType.PropertyName:
                        var propertyAccessNode = CreatePropertyAccessNode(token.Value, _model);

                        if (leftNode == null)
                        {
                            leftNode = propertyAccessNode;
                        }
                        else if (rightNode == null)
                        {
                            rightNode = propertyAccessNode;
                        }

                        break;

                    case TokenType.Date:
                    case TokenType.DateTimeOffset:
                    case TokenType.Decimal:
                    case TokenType.Double:
                    case TokenType.Duration:
                    case TokenType.Enum:
                    case TokenType.False:
                    case TokenType.Guid:
                    case TokenType.Integer:
                    case TokenType.Null:
                    case TokenType.Single:
                    case TokenType.String:
                    case TokenType.TimeOfDay:
                    case TokenType.True:
                        var constantNode = ConstantNodeParser.ParseConstantNode(token, _typeProvider);

                        if (rightNode is ConstantNode existingConstant)
                        {
                            var arrayNode = ConstantNode.Array(existingConstant);
                            arrayNode.AddElement(constantNode);
                            rightNode = arrayNode;
                        }
                        else if (rightNode is ArrayNode arrayNode)
                        {
                            arrayNode.AddElement(constantNode);
                        }
                        else
                        {
                            rightNode = constantNode;
                        }

                        break;
                }
            }

            result = result == null
                ? new BinaryOperatorNode(leftNode, operatorKind, rightNode)
                : new BinaryOperatorNode(result, operatorKind, leftNode ?? rightNode);

            return result;
        }

        private static PropertyAccessNode CreatePropertyAccessNode(string tokenValue, EdmComplexType edmComplexType)
        {
            var properties = new List<EdmProperty>();

            foreach (var propertyName in tokenValue.Split('/'))
            {
                if (edmComplexType is null)
                {
                    throw new ParserException($"Property {propertyName} not found");
                }

                var currentProperty = edmComplexType.GetProperty(propertyName);
                properties.Add(currentProperty);
                edmComplexType = currentProperty.PropertyType as EdmComplexType;
            }

            var propertyAccessNode = new PropertyAccessNode(properties);
            return propertyAccessNode;
        }

        private QueryNode ParseQueryNode()
        {
            QueryNode? node = null;

            if (_tokens.Count == 0)
            {
                throw new ParserException("UnableToParseFilter");
            }

            switch (_tokens.Peek().TokenType)
            {
                case TokenType.FunctionName:
                    node = ParseFunctionCallNode();
                    break;

                case TokenType.UnaryOperator:
                    var token = _tokens.Dequeue();
                    node = ParseQueryNode();
                    node = new UnaryOperatorNode(node, token.Value.ToUnaryOperatorKind());
                    break;

                case TokenType.OpenParentheses:
                    _groupingDepth++;
                    _tokens.Dequeue();
                    node = ParseQueryNode();
                    break;

                case TokenType.PropertyName:
                    node = ParsePropertyAccessNode();
                    break;

                case TokenType.True:
                    node = ConstantNode.True;
                    break;

                case TokenType.False:
                    node = ConstantNode.False;
                    break;

                default:
                    throw new ParserException(_tokens.Peek().TokenType.ToString());
            }

            return node;
        }

        private void UpdateExpressionTree()
        {
            var initialGroupingDepth = _groupingDepth;

            var node = ParseQueryNode();

            if (_groupingDepth == initialGroupingDepth)
            {
                if (_nodeStack.Count == 0)
                {
                    if (_nextBinaryOperatorKind == BinaryOperatorKind.None)
                    {
                        _nodeStack.Push(node);
                    }
                    else
                    {
                        _nodeStack.Push(new BinaryOperatorNode(node, _nextBinaryOperatorKind, null));
                    }
                }
                else
                {
                    var leftNode = _nodeStack.Pop();

                    if (leftNode is BinaryOperatorNode binaryNode && binaryNode.Right == null)
                    {
                        binaryNode.Right = node;

                        if (_nextBinaryOperatorKind != BinaryOperatorKind.None)
                        {
                            binaryNode = new BinaryOperatorNode(binaryNode, _nextBinaryOperatorKind, null);
                        }
                    }
                    else
                    {
                        binaryNode = new BinaryOperatorNode(leftNode, _nextBinaryOperatorKind, node);
                    }

                    _nodeStack.Push(binaryNode);
                }
            }
            else if (_groupingDepth > initialGroupingDepth)
            {
                _nodeStack.Push(new BinaryOperatorNode(node, _nextBinaryOperatorKind, null));
            }
            else if (_groupingDepth < initialGroupingDepth)
            {
                var binaryNode = (BinaryOperatorNode)_nodeStack.Pop();
                binaryNode.Right = node;

                if (_nextBinaryOperatorKind == BinaryOperatorKind.None)
                {
                    _nodeStack.Push(binaryNode);

                    while (_nodeStack.Count > 1)
                    {
                        var rightNode = _nodeStack.Pop();

                        var binaryParent = (BinaryOperatorNode)_nodeStack.Pop();
                        binaryParent.Right = rightNode;

                        _nodeStack.Push(binaryParent);
                    }
                }
                else
                {
                    if (_groupingDepth == 0 && _nodeStack.Count > 0)
                    {
                        var binaryParent = (BinaryOperatorNode)_nodeStack.Pop();
                        binaryParent.Right = binaryNode;

                        binaryNode = binaryParent;
                    }

                    _nodeStack.Push(new BinaryOperatorNode(binaryNode, _nextBinaryOperatorKind, null));
                }
            }
        }
    }
}
