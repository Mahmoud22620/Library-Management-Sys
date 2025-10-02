using Library_Management_Sys.Models.Enums;
using System.Security.Claims;

namespace Library_Management_Sys.Services
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, Permissions permission);
    }
}
