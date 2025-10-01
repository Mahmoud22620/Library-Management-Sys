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
    public class BorrowTransactionsController : ControllerBase
    {
        private readonly IBorrowTransactionRepository _borrowTransactionRepository;
        private readonly IMapper _mapper;

        public BorrowTransactionsController(IBorrowTransactionRepository borrowTransactionRepo, IMapper mapper)
        {
            _borrowTransactionRepository = borrowTransactionRepo;
            _mapper = mapper;
        }

        // GET: api/BorrowTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowTransactionDTO>>> GetBorrowTransactions()
        {
            return Ok(await _borrowTransactionRepository.GetAllAsync());
        }

        // GET: api/BorrowTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowTransactionDTO>> GetBorrowTransaction(int id)
        {
            if (id == 0 || id == null)
            {
                return BadRequest();
            }
            return Ok(await _borrowTransactionRepository.BorrowTransactionExists(id));

        }

        //GET: api/BorrowTransactions/Member/5
        [HttpGet("Member/{memberId}")]
        public async Task<ActionResult<IEnumerable<BorrowTransactionDTO>>> GetBorrowTransactionsByMember(int memberId)
        {
            if (memberId == 0 || memberId == null)
            {
                return BadRequest();
            }
            var transactions = await _borrowTransactionRepository.GetAllTransactionsByMember(memberId);
            if (transactions == null || !transactions.Any())
            {
                return NotFound(new { message = "No borrow transactions found for the specified member." });
            }
            return Ok(transactions);
        }

        //GET: api/BorrowTransactions/Book/5
        [HttpGet("Book/{bookId}")]
        public async Task<ActionResult<IEnumerable<BorrowTransactionDTO>>> GetBorrowTransactionsByBook(int bookId)
        {
            if (bookId == 0 || bookId == null)
            {
                return BadRequest();
            }
            var transactions = await _borrowTransactionRepository.GetAllTransactionsByBook(bookId);
            if (transactions == null || !transactions.Any())
            {
                return NotFound(new { message = "No borrow transactions found for the specified book." });
            }
            return Ok(transactions);
        }


        //PUT: api/BorrowTransactions/Return/5
        [HttpPut("Return/{id}")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var transaction = await BorrowTransactionExists(id);
            if (transaction == null)
            {
                return NotFound(new { message = "Borrow transaction not found" });
            }
            if (transaction.ReturnDate != null)
            {
                return BadRequest(new { message = "Book has already been returned" });
            }
            await _borrowTransactionRepository.ReturnBook(id);
            return Ok(new { message = "Book returned successfully" });
        }

        //PUT: api/BorrowTransactions/Renew/5
        [HttpPut("Renew/{id}")]
        public async Task<IActionResult> RenewBorrowing(int id, [FromBody] DateTime newDueDate)
        {
            var transaction = await BorrowTransactionExists(id);
            if (transaction == null)
            {
                return NotFound(new { message = "Borrow transaction not found" });
            }
            if (transaction.ReturnDate != null)
            {
                return BadRequest(new { message = "Cannot renew a returned book" });
            }
            if (transaction.ReturnDate >= newDueDate)
            {
                return BadRequest(new { message = "New due date must be later than the current due date" });
            }
            await _borrowTransactionRepository.RenewBorrowing(id, newDueDate);
            return Ok(new { message = "Borrowing renewed successfully" });
        }

        //PUT: api/BorrowTransactions/Overdue
        [HttpPut("Overdue/{id}")]
        public async Task<IActionResult> MarkOverdueTransactions(int id)
        {
            if (id == 0 || id == null)
            {
                return BadRequest();
            }
            await _borrowTransactionRepository.overdueBook(id);
            return Ok(new { message = "Transaction marked as overdue if applicable" });
        }
        // PUT: api/BorrowTransactions
        [HttpPut]
        public async Task<IActionResult> PutBorrowTransaction(BorrowTransactionDTO transactionDto)
        {
            var transaction = await BorrowTransactionExists(transactionDto.Id);
            if (transaction != null)
            {
                await _borrowTransactionRepository.UpdateAsync(_mapper.Map<BorrowTransaction>(transactionDto));
                return Ok(new { message = "Borrow transaction updated successfully" });
            }
            else
            {
                return NotFound(new { message = "Borrow transaction not found" });
            }
        }

        // POST: api/BorrowTransactions
        [HttpPost]
        public async Task<ActionResult<BorrowTransactionDTO>> PostBorrowTransaction(BorrowTransactionDTO transactionDto)
        {
            if (transactionDto == null)
            {
                return BadRequest();
            }
            await _borrowTransactionRepository.CreateAndSaveAsync(_mapper.Map<BorrowTransaction>(transactionDto));
            return Ok(new { message = "Borrow transaction created successfully" });
        }

        // DELETE: api/BorrowTransactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrowTransaction(int id)
        {
            var transaction = await _borrowTransactionRepository.GetAsync(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            await _borrowTransactionRepository.RemoveAsync(transaction);
            await _borrowTransactionRepository.SaveAsync();
            return Ok(new { message = "Borrow transaction deleted successfully" });
        }

        private async Task<BorrowTransaction> BorrowTransactionExists(int id)
        {
            return await _borrowTransactionRepository.GetAsync(t => t.Id == id);
        }
    }
}
