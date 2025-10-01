using AutoMapper;
using Library_Management_Sys.Models;
using Library_Management_Sys.Models.DTOs;
using Library_Management_Sys.Models.Enums;
using Library_Management_Sys.Repositories.Interfaces;
using System.Net;

namespace Library_Management_Sys.Repositories
{
    public class BorrowTransactionRepository : GenericRepository<BorrowTransaction>, IBorrowTransactionRepository
    {
        public LibraryDbContext _db;
        public IMapper _mapper;
        public BorrowTransactionRepository(LibraryDbContext db , IMapper mapper) : base(db)
        {
            _db = db;
            _mapper = mapper;
        }

        public Task BorrowBook(BorrowTransactionDTO borrowTransactionDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BorrowTransactionDTO> BorrowTransactionExists(int id)
        {
            var transaction = await GetAsync(t => t.Id == id);
            return _mapper.Map<BorrowTransactionDTO>(transaction);
        }

        public Task CheckOverdue_Update()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BorrowTransactionDTO>> GetAllTransactions()
        {
            var transactions =await GetAllAsync();
            return _mapper.Map<IEnumerable<BorrowTransactionDTO>>(transactions);
        }

        public async Task<IEnumerable<BorrowTransactionDTO>> GetAllTransactionsByBook(int bookid)
        {
            var transactions = await GetAllAsync(t => t.BookId == bookid);
            return _mapper.Map<IEnumerable<BorrowTransactionDTO>>(transactions);
        }

        public async Task<IEnumerable<BorrowTransactionDTO>> GetAllTransactionsByMember(int memberId)
        {
            var transactions = await GetAllAsync(t => t.MemberId == memberId);
            return _mapper.Map<IEnumerable<BorrowTransactionDTO>>(transactions);
        }

        public async Task<IEnumerable<BorrowTransactionDTO>> GetOverdueTransactions()
        {
            var transactions = await GetAllAsync(t => t.Status == BorrowTransStatus.Overdue);
            return _mapper.Map<IEnumerable<BorrowTransactionDTO>>(transactions);
        }

        public  Task overdueBook(int bookId)
        {
            var Transaction = GetAsync(b => b.BookId == bookId);
            if (Transaction != null)
            {
                Transaction.Result.Status = BorrowTransStatus.Overdue;
                UpdateAsync(Transaction.Result);
                return  Task.CompletedTask;
            }
            return Task.FromException(new Exception("Transaction not found"));

        }

        public Task RenewBorrowing(int bookId, DateTime newDueDate)
        {
            var Transaction = GetAsync(b => b.BookId == bookId);
            if (Transaction != null)
            {
                Transaction.Result.Status = BorrowTransStatus.Borrowed;
                Transaction.Result.ReturnDate = newDueDate;
                UpdateAsync(Transaction.Result);
                return Task.CompletedTask;
            }
            return Task.FromException(new Exception("Transaction not found"));
        }

        public Task ReturnBook(int bookid)
        {
            var Transaction = GetAsync(b => b.BookId == bookid);
            if (Transaction != null)
            {
                Transaction.Result.Status = BorrowTransStatus.Returned;
                Transaction.Result.ReturnDate = DateTime.UtcNow;
                UpdateAsync(Transaction.Result);
                return Task.CompletedTask;
            }
            return Task.FromException(new Exception("Transaction not found"));
        }
    }
}
