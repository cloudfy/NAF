using System;

namespace Naf.Net.OAuth;

public sealed class AuthorizationErrorEventArgs : EventArgs
{
    internal AuthorizationErrorEventArgs(string error, string? errorDescription)
    {
        Error = error;
        ErrorDescription = errorDescription;
    }

    public string Error { get; }
    public string? ErrorDescription { get; }
}
