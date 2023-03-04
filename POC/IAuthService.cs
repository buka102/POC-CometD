using POC.Models;

namespace POC;

public interface IAuthService
{
    Task<AuthResponse> GetAuthResponseAsync();
}
