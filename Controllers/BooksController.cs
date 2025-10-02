using AutoMapper;
using Library_Management_Sys.Helpers.Enums;
using Library_Management_Sys.Models;
using Library_Management_Sys.Models.DTOs;
using Library_Management_Sys.Repositories.Interfaces;
using Library_Management_Sys.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_Sys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        private readonly IActivitylogRepository _activitylogRepository;

        public BooksController(IBooksRepository bookRepo, IMapper mapper , IPermissionService permissionService, IActivitylogRepository activitylogRepository )
        {
            _bookRepository = bookRepo;
            _mapper = mapper;
            _permissionService = permissionService;
            _activitylogRepository = activitylogRepository;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Books_View);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var booksList = await _bookRepository.GetAllBooksData();
                await _activitylogRepository.LogActivity(User, Permissions.Books_View);
                return Ok(_mapper.Map<List<BookDTO>>(booksList));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        //GET: api/Books/Search?query=keyword
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> SearchBooks(string query)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Books_View);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var booksList = await _bookRepository.SearchBooksAsync(query);
                await _activitylogRepository.LogActivity(User, Permissions.Books_View);
                return Ok(_mapper.Map<List<BookDTO>>(booksList));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(int id)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Books_View);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var book = await _bookRepository.GetAsync(b => b.BookId == id,b => b.Authors, b => b.Category, b => b.Publisher);

                if (book == null)
                {
                    return NotFound();
                }

                await _activitylogRepository.LogActivity(User, Permissions.Books_View);
                return Ok(_mapper.Map<BookDTO>(book));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // PUT: api/Books
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PutBook( BookUpdateDTO bookDto, IFormFile CoverImg)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Books_Update);
            if (!Allowed)
            {
                return Forbid();
            }
            try
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
                    await _activitylogRepository.LogActivity(User, Permissions.Books_Update);
                    return Ok(new { message = "Book updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "Book Not Found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // POST: api/Books
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<BookDTO>> PostBook( BookUpdateDTO bookDto,IFormFile CoverImg)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Books_Create);
            if (!Allowed)
            {
                return Forbid();
            }
            try
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
                await _activitylogRepository.LogActivity(User, Permissions.Books_Create);
                return Ok(new { message = "Book created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Books_Delete);
            if (!Allowed)
            {
                return Forbid();
            }
            try
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
                await _activitylogRepository.LogActivity(User, Permissions.Books_Delete);
                return Ok(new { message = "Book deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        private async Task<Book> BookExists(int id)
        {
            return await _bookRepository.GetAsync(b => b.BookId == id);
        }
    }
}
