using System;

namespace Naf.Net.OAuth;

public sealed class TokenEventArgs : EventArgs
{
    internal TokenEventArgs(string token)
    {
        Token = token;
    }

    public string Token { get; }
}