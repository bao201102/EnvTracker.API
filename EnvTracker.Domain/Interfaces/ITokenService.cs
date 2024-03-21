using EnvTracker.Domain.Entities;
using System.Security.Claims;

namespace EnvTracker.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(GenerateTokenReq obj);
        IEnumerable<Claim> Decode(string token);
    }
}
