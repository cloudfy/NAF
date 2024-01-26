using System;

namespace Naf.Net.OAuth;

public sealed class AuthorizationCodeEventArgs : EventArgs 
{
    internal AuthorizationCodeEventArgs(string code, string? sessionState, string? state)
    {
        Code = code;
        SessionState = sessionState;
        State = state;
    }

    public string Code { get; }
    public string? SessionState { get; }
    public string? State { get; }
}