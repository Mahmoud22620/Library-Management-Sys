using Library_Management_Sys.Models;
using Library_Management_Sys.Models.Enums;
using System.Security.Claims;

namespace Library_Management_Sys.Repositories.Interfaces
{
    public interface IActivitylogRepository : IGenericRepository<UserActivityLog>
    {
        Task LogActivity(ClaimsPrincipal user , Permissions action );
    }
}
