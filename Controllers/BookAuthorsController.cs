using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library_Management_Sys.Models;
using Microsoft.AspNetCore.Authorization;
using Library_Management_Sys.Repositories.Interfaces;

namespace Library_Management_Sys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookAuthorsController : ControllerBase
    {
        private readonly IBookAuthorsRepository _bookAuthorsRepository;

        public BookAuthorsController(IBookAuthorsRepository bookAuthorsRepository)
        {
            _bookAuthorsRepository = bookAuthorsRepository;
        }

        // GET: api/BookAuthors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookAuthor>>> GetBookAuthor()
        {
            return await _bookAuthorsRepository.GetAllAsync();
        }

        // GET: api/GetBooksByAuthor/5
        [HttpGet("GetBooksByAuthor/{id}")]
        public async Task<ActionResult<IEnumerable<BookAuthor>>> GetBooksByAuthor(int id)
        {
            var booksAuthor = await _bookAuthorsRepository.GetBooksByAuthorIdAsync(id);

            if (booksAuthor == null)
            {
                return NotFound();
            }

            return Ok(booksAuthor);
        }

        // GET: api/GetAuthorsOfBook/5
        [HttpGet("GetAuthorsOfBook/{id}")]
        public async Task<ActionResult<IEnumerable<BookAuthor>>> GetAuthorsOfBook(int id)
        {
            var booksAuthor = await _bookAuthorsRepository.GetAuthorsByBookIdAsync(id);

            if (booksAuthor == null)
            {
                return NotFound();
            }

            return Ok(booksAuthor);
        }



        // POST: api/BookAuthors
        [HttpPost]
        public async Task<ActionResult<BookAuthor>> PostBookAuthor(BookAuthor bookAuthor)
        {
            _bookAuthorsRepository.CreateAsync(bookAuthor);
            try
            {
                await _bookAuthorsRepository.SaveAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }

            return CreatedAtAction("GetBookAuthor", new { id = bookAuthor.BookId }, bookAuthor);
        }

        // DELETE: api/BookAuthors/5
        [HttpDelete]
        public async Task<IActionResult> DeleteBookAuthor(BookAuthor bookauth)
        {
            var bookAuthor = await _bookAuthorsRepository.GetAsync(a => a.BookId == bookauth.BookId && a.AuthorId == bookauth.AuthorId);
            if (bookAuthor == null)
            {
                return NotFound();
            }

            await _bookAuthorsRepository.RemoveAsync(bookAuthor);

            return NoContent();
        }
    }
}
