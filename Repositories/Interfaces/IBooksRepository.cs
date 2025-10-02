using Library_Management_Sys.Models;
using System.Linq.Expressions;

namespace Library_Management_Sys.Repositories.Interfaces
{
    public interface IBooksRepository : IGenericRepository<Book>
    {
        //search books by title, author, or Category
        Task<IEnumerable<Book>> SearchBooksAsync(string query);

        Task<IEnumerable<Book>> GetAllBooksData(Expression<Func<Book, bool>>? filter = null);
    }
}
