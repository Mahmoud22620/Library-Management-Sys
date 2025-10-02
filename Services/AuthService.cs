using Library_Management_Sys.Helpers;
using Library_Management_Sys.Helpers.Enums;
using Library_Management_Sys.Models;
using Library_Management_Sys.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_Management_Sys.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _UserManager;
        private readonly JWT _JWT;
        private readonly RoleManager<IdentityRole<Guid>> _RoleManager;
        public AuthService(UserManager<User> userManager, IOptions<JWT> Jwt , RoleManager<IdentityRole<Guid>> roleManager )
        {
            _UserManager = userManager;
            _JWT = Jwt.Value;
            _RoleManager = roleManager;
        }
        public async Task<AuthDTO> GetTokenAsync(LoginDTO model)
        {
            var Auth = new AuthDTO();
            var user = await _UserManager.FindByEmailAsync(model.Email);
            if (user is null || !await _UserManager.CheckPasswordAsync(user, model.Password))
            {
                Auth.IsAuthenticated = false;
                Auth.Message = "Email or Password is incorrect";
                return Auth;
            }
            var jwtSecurityToken = await CreateJWTTokenAsync(user);
            var roles = await _UserManager.GetRolesAsync(user);
            Auth.IsAuthenticated = true;
            Auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            Auth.Email = user.Email;
            Auth.UserName = user.UserName;
            Auth.ExpireOn = jwtSecurityToken.ValidTo;
            Auth.Role = await _UserManager.GetRolesAsync(user) as List<string>;

            
            await _UserManager.UpdateAsync(user);

            return Auth;
        }

        public async Task<AuthDTO> RegisterAsync(RegisterDTO model)
        {
            if (await _UserManager.FindByEmailAsync(model.Email) is not null)
            {
                return new AuthDTO { Message = "Email is already registered", IsAuthenticated = false };
            }

            if (await _UserManager.FindByNameAsync(model.UserName) is not null)
            {
                return new AuthDTO { Message = "Username is already registered", IsAuthenticated = false };
            }

            var permission = RolePermission.None;

            switch (model.Role)
            {
                case "Admin":
                    permission = RolePermission.Admin;
                    break;
                case "Librarian":
                    permission = RolePermission.Librarian;
                    break;
                case "Staff":
                    permission = RolePermission.Staff;
                    break;
                default:
                    return new AuthDTO { Message = "Invalid role specified. Please choose either 'Admin', 'Librarian', or 'Member'.", IsAuthenticated = false };
            }

            var user = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                User_permissions = permission,
            };

            var result = await _UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new AuthDTO { Message = errors, IsAuthenticated = false };
            }

            await _UserManager.AddToRoleAsync(user, model.Role);

            var jwtSecurityToken = await CreateJWTTokenAsync(user);

           
            await _UserManager.UpdateAsync(user);

            return new AuthDTO
            {
                Email = user.Email,
                ExpireOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Role = await _UserManager.GetRolesAsync(user) as List<string>,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName,
            };

        }
        public async Task<JwtSecurityToken> CreateJWTTokenAsync(User user)
        {
            var userClaims = await _UserManager.GetClaimsAsync(user);
            var roles = await _UserManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("uid", user.Id.ToString()),
                new Claim("permissions", ((int)user.User_permissions).ToString())
            }.Union(userClaims).Union(roleClaims);

            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWT.Key));
            var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _JWT.Issuer,
                audience: _JWT.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_JWT.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

       //Change Role for existing user
       public async Task<IdentityResult> AssignRoleAsync(string username, string role)
       {
            var user = await _UserManager.FindByNameAsync(username);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            // Check if role exists
            if (!await _RoleManager.RoleExistsAsync(role))
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{role}' does not exist" });

            var currentRoles = await _UserManager.GetRolesAsync(user);

            // Remove old roles
            var removeResult = await _UserManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return removeResult;

            // Add new role
            var addResult = await _UserManager.AddToRoleAsync(user, role);
            return addResult;
       }

    }
}
