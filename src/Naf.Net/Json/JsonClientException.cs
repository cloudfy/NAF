using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Runtime.Net.Json
{
    public class JsonClientException : Exception
    {
        #region === member variables ===
        /// <summary>Member variable for status code.</summary>
        private System.Net.HttpStatusCode _statusCode;
        #endregion

        #region === constructor ===
        /// <summary>
        /// Returns a new instace of the exception class.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="statusCode">Status code from the request.</param>
        public JsonClientException(string message, System.Net.HttpStatusCode statusCode) : base(message)
        {
            _statusCode = statusCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        /// <param name="innerException"></param>
        public JsonClientException(string message, System.Net.HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
        {
            _statusCode = statusCode;
        }
        #endregion

        #region === public properties ===
        /// <summary>
        /// Gets the status code of the exception.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get
            {
                return _statusCode;
            }
        }
        #endregion
    }
}
