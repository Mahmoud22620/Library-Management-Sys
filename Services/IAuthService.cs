using Library_Management_Sys.Models;
using Library_Management_Sys.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace Library_Management_Sys.Services
{
    public interface IAuthService
    {
        Task<AuthDTO> RegisterAsync(RegisterDTO model);
        Task<AuthDTO> GetTokenAsync(LoginDTO model);
        Task<JwtSecurityToken> CreateJWTTokenAsync(User user);
        string GenerateRefreshToken();
    }
}
