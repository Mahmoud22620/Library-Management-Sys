using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class PublishersController : ControllerBase
    {
        private readonly IGenericRepository<Publisher> _publisherRepository;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        private readonly IActivitylogRepository _activitylogRepository;

        public PublishersController(IGenericRepository<Publisher> publisherRepo, IMapper mapper, IPermissionService permissionService, IActivitylogRepository activitylogRepository)
        {
            _publisherRepository = publisherRepo;
            _mapper = mapper;
            _permissionService = permissionService;
            _activitylogRepository = activitylogRepository;
        }

        // GET: api/Publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDTO>>> GetPublishers()
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Publishers_View);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var publishersList = await _publisherRepository.GetAllAsync();
                await _activitylogRepository.LogActivity(User, Permissions.Publishers_View);
                return Ok(_mapper.Map<List<PublisherDTO>>(publishersList));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // GET: api/Publishers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDTO>> GetPublisher(int id)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Publishers_View);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var publisher = await _publisherRepository.GetAsync(p => p.PublisherId == id);

                if (publisher == null)
                {
                    return NotFound();
                }

                await _activitylogRepository.LogActivity(User, Permissions.Publishers_View);
                return Ok(_mapper.Map<PublisherDTO>(publisher));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // PUT: api/Publishers
        [HttpPut]
        public async Task<IActionResult> PutPublisher(PublisherDTO publisherDto)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Publishers_Update);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var publisher = await PublisherExists(publisherDto.PublisherId);
                if (publisher != null)
                {
                    await _publisherRepository.UpdateAsync(_mapper.Map<Publisher>(publisherDto));
                    await _activitylogRepository.LogActivity(User, Permissions.Publishers_Update);
                    return Ok(new { message = "Publisher updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "Publisher Not Found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // POST: api/Publishers
        [HttpPost]
        public async Task<ActionResult<PublisherDTO>> PostPublisher(PublisherDTO publisherDto)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Publishers_Create);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                if (publisherDto == null)
                {
                    return BadRequest();
                }
                await _publisherRepository.CreateAndSaveAsync(_mapper.Map<Publisher>(publisherDto));
                await _activitylogRepository.LogActivity(User, Permissions.Publishers_Create);
                return Ok(new { message = "Publisher created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Publishers_Delete);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var publisher = await _publisherRepository.GetAsync(p => p.PublisherId == id);
                if (publisher == null)
                {
                    return NotFound();
                }

                await _publisherRepository.RemoveAsync(publisher);
                await _publisherRepository.SaveAsync();
                await _activitylogRepository.LogActivity(User, Permissions.Publishers_Delete);
                return Ok(new { message = "Publisher deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        private async Task<Publisher> PublisherExists(int id)
        {
            return await _publisherRepository.GetAsync(p => p.PublisherId == id);
        }
    }
}
