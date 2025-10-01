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
    public class CategoriesController : ControllerBase
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController( IGenericRepository<Category> categoryRepo , IMapper mapper )
        {
            _categoryRepository = categoryRepo;
            _mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
             var CategoriesList = await _categoryRepository.GetAllAsync();
            return Ok(_mapper.Map<List<CategoryDTO>>(CategoriesList));

        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetAsync(p => p.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CategoryDTO>(category));
        }

        // PUT: api/Categories/5
        [HttpPut]
        public async Task<IActionResult> PutCategory( CategoryDTO categoryDto)
        {
            var category = CategoryExists(categoryDto.CategoryId);
            if (category!=null)
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
            if(categoryDto == null)
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
