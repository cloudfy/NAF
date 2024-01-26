using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace Naf.Net.OAuth;

public sealed class OAuthCallbackListener : IDisposable
{
    private static readonly int[] _ports = [4657, 24004, 56030, 52001, 60890];
    private readonly HttpListener _listener;
    private readonly bool _forcePort = false;
    private int _activePort = 0;
    private bool _isRunning = false;
    
    private const string LOCALHOST = "http://localhost:{0}/";

    public event EventHandler<AuthorizationCodeEventArgs>? AuthorizationCodeReceived;
    public event EventHandler<TokenEventArgs>? TokenReceived;
    public event EventHandler<AuthorizationErrorEventArgs>? OnErrorReceived;

    /// <summary>
    /// Initialize the OAuthCallbackListener using default ports.
    /// <para>Use <see cref="PossibleCallbackUrls"/> or <see cref="PossiblePorts"/> to determine which default ports are used.</para>
    /// </summary>
    public OAuthCallbackListener() 
    {
        _listener = new();
    }
    /// <summary>
    /// Initialize the OAuthCallbackListener with a specific port.
    /// </summary>
    /// <param name="usePort">Required. Must be an available port.</param>
    public OAuthCallbackListener(int usePort)
    {
        _listener = new();
        _forcePort = true;
        _activePort = usePort;
    }

    /// <summary>
    /// Gets a list of the possible ports that can be used for the OAuth callback.
    /// <para>Register callback of http://localhost:(port) for each port.</para>
    /// </summary>
    public static int[] PossiblePorts => _ports;
    /// <summary>
    /// Gets a list of possible callback URLs that can be used for the OAuth callback.
    /// </summary>
    public static string[] PossibleCallbackUrls 
        => _ports.Select(port => string.Format(LOCALHOST, port)).ToArray();
    /// <summary>
    /// Gets the active port of the listener. If 0, then the listener is not running.
    /// </summary>
    public int ActivePort => _activePort;
    /// <summary>
    /// Gets the active callback URL of the listener.
    /// </summary>
    public string ActiveCallbackUrl => string.Format(LOCALHOST, _activePort);

    /// <summary>
    /// Start listening for OAuth callbacks.
    /// </summary>
    /// <exception cref="ApplicationException">Listener already running.</exception>
    public void Start()
    {
        if (_isRunning)
            throw new ApplicationException("Already running");

        // determine port
        if (_forcePort == false) 
        { 
            foreach (int port in _ports)
            {
                if (IsPortAvailable(port))
                {
                    _activePort = port;
                    break;
                }
            }
        }
        else
        {
            if (IsPortAvailable(_activePort) == false)
                throw new ApplicationException($"Provided port {_activePort} is not available. Please use an available port.");
        }

        _listener.Prefixes.Add(string.Format(LOCALHOST, _activePort));
        _listener.Start();
        _isRunning = true;
        _listener.BeginGetContext(async (x) => { await OnContext(x); }, null);
    }
    private async Task OnContext(IAsyncResult asyncResult)
    {
        try
        {
            var context = _listener.EndGetContext(asyncResult);
            _listener.BeginGetContext(async (x) => { await OnContext(x); }, null);

            await ProcessRequestAsync(context, this);
        }
        catch (HttpListenerException ex) when (ex.Message.Contains("thread exit"))
        {
            // listener stopped due to application exit
        }
        catch
        {
            throw;
        }
    }
   
    public void Stop()
    {
        if (_isRunning)
        {
            _listener.Stop();
        }
    }

    private static async Task ProcessRequestAsync(
        HttpListenerContext context
        , OAuthCallbackListener localOAuthCallback)
    {
        var request = context.Request;
        var response = context.Response;

        var error = request.QueryString["error"];
        if (error is not null)
        {
            var errorDescription = request.QueryString["error_description"];
            localOAuthCallback.OnErrorReceived?.Invoke(
                localOAuthCallback
                , new AuthorizationErrorEventArgs(error, errorDescription));
            await OkResponse(context);
            return;
        }

        var code = request.QueryString["code"];
        var sessionState = request.QueryString["session_state"];
        var state = request.QueryString["state"];
        if (code is not null)
        {
            // authorization code flow
            localOAuthCallback.AuthorizationCodeReceived?.Invoke(
                localOAuthCallback
                , new AuthorizationCodeEventArgs(code, sessionState, state));
        }
        else
        {
            // asume token flow
            using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
            var requestBody = await reader.ReadToEndAsync();

            if (string.IsNullOrEmpty(requestBody) == false)
            {
                localOAuthCallback.TokenReceived?.Invoke(
                    localOAuthCallback
                    , new TokenEventArgs(requestBody));
            }
        }

        await OkResponse(context);
        return;
    }

    /// <summary>
    /// Returns a HTTP 200 respose to the <paramref name="context"/>.
    /// </summary>
    /// <param name="context">Required. Context of request.</param>
    /// <returns></returns>
    private static Task OkResponse(HttpListenerContext context)
    {
        context.Response.StatusCode = 200;
        context.Response.StatusDescription = "OK";
        context.Response.Close();
        return Task.CompletedTask;
    }
    private static bool IsPortAvailable(int port)
    {
        IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

        foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
        {
            if (tcpi.LocalEndPoint.Port != port)
            {
                return true;
            }
        }
        return false;
    }

    public void Dispose() => Stop();
}