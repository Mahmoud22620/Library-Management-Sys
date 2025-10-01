using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library_Management_Sys.Models;
using Library_Management_Sys.Repositories.Interfaces;
using AutoMapper;
using Library_Management_Sys.Models.DTOs;

namespace Library_Management_Sys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IMapper _mapper;

        public BooksController(IGenericRepository<Book> bookRepo, IMapper mapper)
        {
            _bookRepository = bookRepo;
            _mapper = mapper;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var booksList = await _bookRepository.GetAllAsync();
            return Ok(_mapper.Map<List<BookDTO>>(booksList));
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(int id)
        {
            var book = await _bookRepository.GetAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookDTO>(book));
        }

        // PUT: api/Books
        [HttpPut]
        public async Task<IActionResult> PutBook(BookDTO bookDto , IFormFile CoverImg)
        {
            var book = await BookExists(bookDto.BookId);
            if (book != null)
            {
                if (CoverImg != null && CoverImg.Length > 0)
                {
                    // Delete old image if exists
                    var oldImagePath = Path.Combine("wwwroot", book.CoverImage?.Split('?')[0]?.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                    var file = bookDto.Title.Replace(" ", "_").ToString() + Path.GetExtension(CoverImg.FileName);
                    var filePath = Path.Combine("wwwroot/images/Books", file);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await CoverImg.CopyToAsync(stream);
                    }
                    var version = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    bookDto.CoverImage = $"/images/Books/{file}?v={version}";
                }
                else
                {
                    bookDto.CoverImage = book.CoverImage; 
                }
                await _bookRepository.UpdateAsync(_mapper.Map<Book>(bookDto));
                return Ok(new { message = "Book updated successfully" });
            }
            else
            {
                return NotFound(new { message = "Book Not Found" });
            }
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<BookDTO>> PostBook(BookDTO bookDto , IFormFile CoverImg)
        {
            if (bookDto == null)
            {
                return BadRequest();
            }

            if(CoverImg != null && CoverImg.Length > 0)
            {
                var file  = bookDto.Title.Replace(" ", "_").ToString() + Path.GetExtension(CoverImg.FileName);
                var filePath = Path.Combine("wwwroot/images/Books", file);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await CoverImg.CopyToAsync(stream);
                }

                var version = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                bookDto.CoverImage = $"/images/Books/{file}?v={version}";


            }

            await _bookRepository.CreateAndSaveAsync(_mapper.Map<Book>(bookDto));
            return Ok(new { message = "Book created successfully" });
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookRepository.GetAsync(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            // Delete cover image file if exists
            var imagePath = Path.Combine("wwwroot", book.CoverImage?.Split('?')[0]?.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

                await _bookRepository.RemoveAsync(book);
            await _bookRepository.SaveAsync();
            return Ok(new { message = "Book deleted successfully" });
        }

        private async Task<Book> BookExists(int id)
        {
            return await _bookRepository.GetAsync(b => b.BookId == id);
        }
    }
}
