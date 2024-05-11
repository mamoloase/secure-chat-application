using Protocol.Extensions;
using Protocol.Transport.Authentication;

namespace Protocol.Transport.Managers;
public class AuthenticationManager
{
    public AuthenticationStatus Status { get; set; }
        = AuthenticationStatus.IsAnonymous;

    public List<ConnectionParameter> Parameters { get; set; } = new();

    public async Task<string> GenerateAuthenticationToken()
        => (await AuthenticationExtensions.GenerateToken()).ToLower();
}
