using AutoMapper;
using Library_Management_Sys.Models;
using Library_Management_Sys.Models.DTOs;
using Library_Management_Sys.Models.Enums;
using Library_Management_Sys.Repositories.Interfaces;
using Library_Management_Sys.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Management_Sys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly IGenericRepository<Author> _authorRepository;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;

        public AuthorsController(IGenericRepository<Author> authorRepo, IMapper mapper , IPermissionService permissionService)
        {
            _authorRepository = authorRepo;
            _mapper = mapper;
            _permissionService = permissionService;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors()
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Authors_View);
            if (!Allowed)
            {
                return Forbid();
            }
            var authorsList = await _authorRepository.GetAllAsync();
            return Ok(_mapper.Map<List<AuthorDTO>>(authorsList));
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int id)
        {

            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Authors_View);
            if (!Allowed)
            {
                return Forbid();
            }
            var author = await _authorRepository.GetAsync(a => a.AuthorId == id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AuthorDTO>(author));
        }

        // PUT: api/Authors
        [HttpPut]
        public async Task<IActionResult> PutAuthor(AuthorDTO authorDto)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Authors_Update);
            if (!Allowed)
            {
                return Forbid();
            }
            var author = await AuthorExists(authorDto.AuthorId);
            if (author != null)
            {
                await _authorRepository.UpdateAsync(_mapper.Map<Author>(authorDto));
                return Ok(new { message = "Author updated successfully" });
            }
            else
            {
                return NotFound(new { message = "Author Not Found" });
            }
        }

        // POST: api/Authors
        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> PostAuthor(AuthorDTO authorDto)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Authors_Create);
            if (!Allowed)
            {
                return Forbid();
            }
            if (authorDto == null)
            {
                return BadRequest();
            }
            await _authorRepository.CreateAndSaveAsync(_mapper.Map<Author>(authorDto));
            return Ok(new { message = "Author created successfully" });
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Authors_Delete);
            if (!Allowed)
            {
                return Forbid();
            }
            var author = await _authorRepository.GetAsync(a => a.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            await _authorRepository.RemoveAsync(author);
            await _authorRepository.SaveAsync();
            return Ok(new { message = "Author deleted successfully" });
        }

        private async Task<Author> AuthorExists(int id)
        {
            return await _authorRepository.GetAsync(a => a.AuthorId == id);
        }
    }
}
