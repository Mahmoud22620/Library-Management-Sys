using Library_Management_Sys.Helpers.Enums;
using Library_Management_Sys.Models;
using System.Security.Claims;

namespace Library_Management_Sys.Services
{
    public class PermissionService : IPermissionService
    {
        public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, Permissions permission)
        {
            var Permission_Str = user.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value;
            Permissions User_Access = (Permissions)Enum.Parse(typeof(Permissions), Permission_Str);
            return (User_Access & permission) == permission;
        }
    }
}
