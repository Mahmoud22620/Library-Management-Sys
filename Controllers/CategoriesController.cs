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
using Microsoft.AspNetCore.Authorization;
using Library_Management_Sys.Helpers.Enums;

namespace Library_Management_Sys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        private readonly IActivitylogRepository _activitylogRepository;

        public CategoriesController(IGenericRepository<Category> categoryRepo, IMapper mapper, IPermissionService permissionService, IActivitylogRepository activitylogRepository)
        {
            _categoryRepository = categoryRepo;
            _mapper = mapper;
            _permissionService = permissionService;
            _activitylogRepository = activitylogRepository;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Categories_View);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var CategoriesList = await _categoryRepository.GetAllAsync();
                await _activitylogRepository.LogActivity(User, Permissions.Categories_View);
                return Ok(_mapper.Map<List<CategoryDTO>>(CategoriesList));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Categories_View);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var category = await _categoryRepository.GetAsync(p => p.CategoryId == id);

                if (category == null)
                {
                    return NotFound();
                }

                await _activitylogRepository.LogActivity(User, Permissions.Categories_View);
                return Ok(_mapper.Map<CategoryDTO>(category));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // PUT: api/Categories
        [HttpPut]
        public async Task<IActionResult> PutCategory(CategoryDTO categoryDto)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Categories_Update);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var category = await CategoryExists(categoryDto.CategoryId);
                if (category != null)
                {
                    await _categoryRepository.UpdateAsync(_mapper.Map<Category>(categoryDto));
                    await _activitylogRepository.LogActivity(User, Permissions.Categories_Update);
                    return Ok(new { message = "Category updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "Category Not Found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> PostCategory(CategoryDTO categoryDto)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Categories_Create);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                if (categoryDto == null)
                {
                    return BadRequest();
                }
                await _categoryRepository.CreateAndSaveAsync(_mapper.Map<Category>(categoryDto));
                await _activitylogRepository.LogActivity(User, Permissions.Categories_Create);
                return Ok(new { message = "Category created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            bool Allowed = await _permissionService.HasPermissionAsync(User, Permissions.Categories_Delete);
            if (!Allowed)
            {
                return Forbid();
            }
            try
            {
                var category = await _categoryRepository.GetAsync(p => p.CategoryId == id);
                if (category == null)
                {
                    return NotFound();
                }

                await _categoryRepository.RemoveAsync(category);
                await _categoryRepository.SaveAsync();
                await _activitylogRepository.LogActivity(User, Permissions.Categories_Delete);
                return Ok(new { message = "Category deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        private async Task<Category> CategoryExists(int id)
        {
            return await _categoryRepository.GetAsync(p => p.CategoryId == id);
        }
    }
}
