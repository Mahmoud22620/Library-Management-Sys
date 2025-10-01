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

namespace Library_Management_Sys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IGenericRepository<Author> _authorRepository;
        private readonly IMapper _mapper;

        public AuthorsController(IGenericRepository<Author> authorRepo, IMapper mapper)
        {
            _authorRepository = authorRepo;
            _mapper = mapper;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors()
        {
            var authorsList = await _authorRepository.GetAllAsync();
            return Ok(_mapper.Map<List<AuthorDTO>>(authorsList));
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int id)
        {
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
