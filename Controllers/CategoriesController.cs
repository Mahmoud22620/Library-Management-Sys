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
    public class CategoriesController : ControllerBase
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;

        public CategoriesController(IGenericRepository<Category> categoryRepo, IMapper mapper, IPermissionService permissionService)
        {
            _categoryRepository = categoryRepo;
            _mapper = mapper;
            _permissionService = permissionService;
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
            var CategoriesList = await _categoryRepository.GetAllAsync();
            return Ok(_mapper.Map<List<CategoryDTO>>(CategoriesList));

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
            var category = await _categoryRepository.GetAsync(p => p.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CategoryDTO>(category));
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
            var category = await CategoryExists(categoryDto.CategoryId);
            if (category != null)
            {
                await _categoryRepository.UpdateAsync(_mapper.Map<Category>(categoryDto));
                return Ok(new { message = "Category updated successfully" });
            }
            else
            {
                return NotFound(new { message = "Category Not Found" });
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
            if (categoryDto == null)
            {
                return BadRequest();
            }
            await _categoryRepository.CreateAndSaveAsync(_mapper.Map<Category>(categoryDto));
            return Ok(new { message = "Category created successfully" });
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
            var category = await _categoryRepository.GetAsync(p => p.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryRepository.RemoveAsync(category);
            await _categoryRepository.SaveAsync();
            return Ok(new { message = "Category deleted successfully" });
        }

        private async Task<Category> CategoryExists(int id)
        {
            return await _categoryRepository.GetAsync(p => p.CategoryId == id);
        }
    }
}
