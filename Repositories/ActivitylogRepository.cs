using Library_Management_Sys.Models;
using Library_Management_Sys.Models.Enums;
using Library_Management_Sys.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Library_Management_Sys.Repositories
{
    public class ActivitylogRepository : GenericRepository<UserActivityLog> , IActivitylogRepository
    {
        private readonly LibraryDbContext _context;
        private readonly UserManager<User> _UserManager;
        public ActivitylogRepository(LibraryDbContext context , UserManager<User> userManager) : base(context)
        {
            _context = context;
            _UserManager = userManager;
        }

        public async Task LogActivity(ClaimsPrincipal user, Permissions action)
        {
            var userid  = user.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var user_data = await _UserManager.FindByIdAsync(userid);
            if (user_data != null)
            {
                await CreateAndSaveAsync(new UserActivityLog
                {
                    UserId = user_data.Id,
                    UserName = user_data.UserName,
                    Action = action,
                    Timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
