using Library_Management_Sys.Models;
using Library_Management_Sys.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Library_Management_Sys.Repositories
{
    public class BooksRepository : GenericRepository<Book> , IBooksRepository
    {
        private readonly LibraryDbContext _db;
        private readonly IBookAuthorsRepository  _bookAuthorsRepository;
        public BooksRepository(LibraryDbContext db, IBookAuthorsRepository bookAuthorsRepository) : base(db)
        {
            _db = db;
            _bookAuthorsRepository = bookAuthorsRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksData(Expression<Func<Book, bool>>? filter = null)
        {
            IQueryable<Book> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
           query = query.Include(b => b.Category)
                         .Include(b => b.Publisher)
                         .Include(b => b.Authors)
                             .ThenInclude(ba => ba.Author);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string query)
        {
            var ByName = await GetAllBooksData(b => EF.Functions.Like(b.Title.ToLower(), $"%{query.ToLower()}%"));
            var ByCategory = await GetAllBooksData(b => EF.Functions.Like(b.Category.Name.ToLower(), $"%{query.ToLower()}%"));
            var ByAuthor = await _bookAuthorsRepository.GetAllAsync(ba => EF.Functions.Like(ba.Author.Name.ToLower(), $"%{query.ToLower()}%"),ba=>ba.Author);
            var ByAuthorBooks = ByAuthor.Select(ba => ba.Book).Distinct();
            return ByName.Union(ByCategory).Union(ByAuthorBooks);

        }

        public async Task UpdateBookStatus(int bookId, Helpers.Enums.BookStatus status)
        {
            var book = await GetAsync(b => b.BookId == bookId);
            if (book != null)
            {
                book.Status = status;
                await UpdateAsync(book);
            }
        }
    }
}
