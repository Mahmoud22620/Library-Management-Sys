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
using Library_Management_Sys.Services;
using Library_Management_Sys.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Library_Management_Sys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BorrowTransactionsController : ControllerBase
    {
        private readonly IBorrowTransactionRepository _borrowTransactionRepository;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        private readonly IActivitylogRepository _activitylogRepository;

        public BorrowTransactionsController(IBorrowTransactionRepository borrowTransactionRepo, IMapper mapper, IPermissionService permissionService, IActivitylogRepository activitylogRepository)
        {
            _borrowTransactionRepository = borrowTransactionRepo;
            _mapper = mapper;
            _permissionService = permissionService;
            _activitylogRepository = activitylogRepository;
        }

        // GET: api/BorrowTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowTransactionDTO>>> GetBorrowTransactions()
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.BorrowTransactions_View);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var transactions = await _borrowTransactionRepository.GetAllAsync();
                await _activitylogRepository.LogActivity(User, Permissions.BorrowTransactions_View);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // GET: api/BorrowTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowTransactionDTO>> GetBorrowTransaction(int id)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.BorrowTransactions_View);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                if (id == 0 || id == null)
                {
                    return BadRequest();
                }
                var transaction = await _borrowTransactionRepository.BorrowTransactionExists(id);
                if (transaction == null)
                {
                    return NotFound();
                }
                await _activitylogRepository.LogActivity(User, Permissions.BorrowTransactions_View);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        //GET: api/BorrowTransactions/Member/5
        [HttpGet("Member/{memberId}")]
        public async Task<ActionResult<IEnumerable<BorrowTransactionDTO>>> GetBorrowTransactionsByMember(int memberId)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.BorrowTransactions_View);
            if (!Allowed)
            {
                return Forbid();
            }
            try
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
                await _activitylogRepository.LogActivity(User, Permissions.BorrowTransactions_View);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        //GET: api/BorrowTransactions/Book/5
        [HttpGet("Book/{bookId}")]
        public async Task<ActionResult<IEnumerable<BorrowTransactionDTO>>> GetBorrowTransactionsByBook(int bookId)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.BorrowTransactions_View);
            if (!Allowed)
            {
                return Forbid();
            }
            try
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
                await _activitylogRepository.LogActivity(User, Permissions.BorrowTransactions_View);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        //PUT: api/BorrowTransactions/Return/5
        [HttpPut("Return/{id}")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.BorrowTransactions_Update);
            if (!Allowed)
            {
                return Forbid();
            }
            try
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
                await _activitylogRepository.LogActivity(User, Permissions.BorrowTransactions_Update);
                return Ok(new { message = "Book returned successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        //PUT: api/BorrowTransactions/Renew/5
        [HttpPut("Renew/{id}")]
        public async Task<IActionResult> RenewBorrowing(int id, [FromBody] DateTime newDueDate)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.BorrowTransactions_Update);
            if (!Allowed)
            {
                return Forbid();
            }
            try
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
                await _activitylogRepository.LogActivity(User, Permissions.BorrowTransactions_Update);
                return Ok(new { message = "Borrowing renewed successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        //PUT: api/BorrowTransactions/Overdue
        [HttpPut("Overdue/{id}")]
        public async Task<IActionResult> MarkOverdueTransactions(int id)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.BorrowTransactions_Update);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                if (id == 0 || id == null)
                {
                    return BadRequest();
                }
                await _borrowTransactionRepository.overdueBook(id);
                await _activitylogRepository.LogActivity(User, Permissions.BorrowTransactions_Update);
                return Ok(new { message = "Transaction marked as overdue if applicable" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // PUT: api/BorrowTransactions
        [HttpPut]
        public async Task<IActionResult> PutBorrowTransaction(BorrowTransactionDTO transactionDto)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.BorrowTransactions_Update);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var transaction = await BorrowTransactionExists(transactionDto.Id);
                if (transaction != null)
                {
                    await _borrowTransactionRepository.UpdateAsync(_mapper.Map<BorrowTransaction>(transactionDto));
                    await _activitylogRepository.LogActivity(User, Permissions.BorrowTransactions_Update);
                    return Ok(new { message = "Borrow transaction updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "Borrow transaction not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // POST: api/BorrowTransactions
        [HttpPost]
        public async Task<ActionResult<BorrowTransactionDTO>> PostBorrowTransaction(BorrowTransactionDTO transactionDto)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.BorrowTransactions_Create);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                if (transactionDto == null)
                {
                    return BadRequest();
                }
                await _borrowTransactionRepository.CreateAndSaveAsync(_mapper.Map<BorrowTransaction>(transactionDto));
                await _activitylogRepository.LogActivity(User, Permissions.BorrowTransactions_Create);
                return Ok(new { message = "Borrow transaction created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // DELETE: api/BorrowTransactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrowTransaction(int id)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.BorrowTransactions_Delete);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var transaction = await _borrowTransactionRepository.GetAsync(t => t.Id == id);
                if (transaction == null)
                {
                    return NotFound();
                }

                await _borrowTransactionRepository.RemoveAsync(transaction);
                await _borrowTransactionRepository.SaveAsync();
                await _activitylogRepository.LogActivity(User, Permissions.BorrowTransactions_Delete);
                return Ok(new { message = "Borrow transaction deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        private async Task<BorrowTransaction> BorrowTransactionExists(int id)
        {
            return await _borrowTransactionRepository.GetAsync(t => t.Id == id);
        }
    }
}
