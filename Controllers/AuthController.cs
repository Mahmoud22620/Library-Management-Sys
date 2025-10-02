using Library_Management_Sys.Models;
using Library_Management_Sys.Models.DTOs;
using Library_Management_Sys.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Library_Management_Sys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly UserManager<User> _UserManager;
        public AuthController(IAuthService authService, UserManager<User> userManager)
        {
            this.authService = authService;
            _UserManager = userManager;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await authService.RegisterAsync(registerDTO);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await authService.GetTokenAsync(loginDTO);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDTO RoleForm)
        {
            var user = await _UserManager.FindByNameAsync(RoleForm.Username);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }
            var result = await authService.AssignRoleAsync(RoleForm.Username, RoleForm.Role);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Failed to assign role.", errors = result.Errors });
            }
            return Ok(new { message = "Role assigned successfully." });
        }
    }
}
