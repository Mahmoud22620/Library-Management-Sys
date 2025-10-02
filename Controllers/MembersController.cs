using Microsoft.AspNetCore.Mvc;
using Library_Management_Sys.Models;
using Library_Management_Sys.Repositories.Interfaces;
using AutoMapper;
using Library_Management_Sys.Models.DTOs;
using Library_Management_Sys.Services;
using Library_Management_Sys.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Library_Management_Sys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MembersController : ControllerBase
    {
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        private readonly IActivitylogRepository _activitylogRepository;

        public MembersController(IGenericRepository<Member> memberRepo, IMapper mapper, IPermissionService permissionService, IActivitylogRepository activitylogRepository)
        {
            _memberRepository = memberRepo;
            _mapper = mapper;
            _permissionService = permissionService;
            _activitylogRepository = activitylogRepository;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembers()
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Members_View);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var membersList = await _memberRepository.GetAllAsync();
                await _activitylogRepository.LogActivity(User, Permissions.Members_View);
                return Ok(_mapper.Map<List<MemberDTO>>(membersList));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDTO>> GetMember(int id)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Members_View);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var member = await _memberRepository.GetAsync(m => m.MemberId == id);

                if (member == null)
                {
                    return NotFound();
                }

                await _activitylogRepository.LogActivity(User, Permissions.Members_View);
                return Ok(_mapper.Map<MemberDTO>(member));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // PUT: api/Members
        [HttpPut]
        public async Task<IActionResult> PutMember(MemberDTO memberDto)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Members_Update);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var member = await MemberExists(memberDto.MemberId);
                if (member != null)
                {
                    await _memberRepository.UpdateAsync(_mapper.Map<Member>(memberDto));
                    await _activitylogRepository.LogActivity(User, Permissions.Members_Update);
                    return Ok(new { message = "Member updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "Member Not Found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // POST: api/Members
        [HttpPost]
        public async Task<ActionResult<MemberDTO>> PostMember(MemberDTO memberDto)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Members_Create);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                if (memberDto == null)
                {
                    return BadRequest();
                }
                await _memberRepository.CreateAndSaveAsync(_mapper.Map<Member>(memberDto));
                await _activitylogRepository.LogActivity(User, Permissions.Members_Create);
                return Ok(new { message = "Member created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Members_Delete);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var member = await _memberRepository.GetAsync(m => m.MemberId == id);
                if (member == null)
                {
                    return NotFound();
                }

                await _memberRepository.RemoveAsync(member);
                await _memberRepository.SaveAsync();
                await _activitylogRepository.LogActivity(User, Permissions.Members_Delete);
                return Ok(new { message = "Member deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        private async Task<Member> MemberExists(int id)
        {
            return await _memberRepository.GetAsync(m => m.MemberId == id);
        }
    }
}
