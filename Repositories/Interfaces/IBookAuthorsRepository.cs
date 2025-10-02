using Library_Management_Sys.Models;

namespace Library_Management_Sys.Repositories.Interfaces
{
    public interface IBookAuthorsRepository : IGenericRepository<BookAuthor>
    {
        Task<IEnumerable<BookAuthor>> GetAuthorsByBookIdAsync(int bookId);
        Task<IEnumerable<BookAuthor>> GetBooksByAuthorIdAsync(int authorId);
    }
}
