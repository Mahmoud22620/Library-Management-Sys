using Library_Management_Sys.Models;
using Library_Management_Sys.Repositories.Interfaces;
using System.Net;

namespace Library_Management_Sys.Repositories
{
    public class BookAuthorsRepository : GenericRepository<BookAuthor> , IBookAuthorsRepository
    {
        private readonly LibraryDbContext _db;
        public BookAuthorsRepository(LibraryDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<BookAuthor>> GetAuthorsByBookIdAsync(int bookId)
        {
            return await GetAllAsync(ba => ba.BookId == bookId, ba => ba.Author);
        }

        public async Task<IEnumerable<BookAuthor>> GetBooksByAuthorIdAsync(int authorId)
        {
            return await GetAllAsync(ba => ba.AuthorId == authorId, ba => ba.Book);
        }
    }
}
