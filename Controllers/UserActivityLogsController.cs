using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library_Management_Sys.Models;
using Library_Management_Sys.Repositories.Interfaces;
using Library_Management_Sys.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Library_Management_Sys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserActivityLogsController : ControllerBase
    {
        private readonly IActivitylogRepository _activitylogRepository;
        private readonly IMapper _mapper;

        public UserActivityLogsController(IActivitylogRepository activitylogRepository , IMapper mapper)
        {
            _activitylogRepository = activitylogRepository;
            _mapper = mapper;
        }

        // GET: api/UserActivityLogs
        [HttpGet("AllActivityLogs")]
        public async Task<ActionResult<IEnumerable<ActivityLogDTO>>> GetUserActivityLogs()
        {
            var Logs  = await _activitylogRepository.GetAllAsync();
            return _mapper.Map<List<ActivityLogDTO>>(Logs);
        }

        // GET: api/UserActivityLogs/5
        [HttpGet("ActivityLogsForUser")]
        public async Task<ActionResult<IEnumerable<ActivityLogDTO>>> GetUserActivityLogs(string username)
        {
            var userActivityLog = await _activitylogRepository.GetAllAsync(a => a.UserName == username);

            if (userActivityLog == null)
            {
                return NotFound();
            }

            return _mapper.Map<List<ActivityLogDTO>>(userActivityLog);
        }

        // DELETE: api/UserActivityLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserActivityLog(int id)
        {
            var userActivityLog = await _activitylogRepository.GetAsync(a => a.Id == id);
            if (userActivityLog == null)
            {
                return NotFound();
            }

            _activitylogRepository.RemoveAsync(userActivityLog);

            return NoContent();
        }
    }
}
