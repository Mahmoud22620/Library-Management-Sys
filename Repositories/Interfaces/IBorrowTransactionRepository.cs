using Library_Management_Sys.Models;
using Library_Management_Sys.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_Sys.Repositories.Interfaces
{
    public interface IBorrowTransactionRepository : IGenericRepository<BorrowTransaction>
    {
        public Task<BorrowTransactionDTO> BorrowTransactionExists(int id);
        public Task<IEnumerable<BorrowTransactionDTO>> GetAllTransactions();
        public Task<IEnumerable<BorrowTransactionDTO>> GetAllTransactionsByMember(int memberId);
        public Task<IEnumerable<BorrowTransactionDTO>> GetAllTransactionsByBook(int bookid);
        public Task BorrowBook (BorrowTransactionDTO borrowTransactionDto);
        public Task ReturnBook (int bookid);
        public Task RenewBorrowing (int bookId, DateTime newDueDate);
        public Task overdueBook (int bookId);
        public Task<IEnumerable<BorrowTransactionDTO>> GetOverdueTransactions();

    }
}
