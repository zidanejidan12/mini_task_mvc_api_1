using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using System;

namespace MyWebFormApp.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryAPIController : ControllerBase
    {
        private readonly ICategoryBLL _categoryBLL;

        public CategoryAPIController(ICategoryBLL categoryBLL)
        {
            _categoryBLL = categoryBLL ?? throw new ArgumentNullException(nameof(categoryBLL));
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _categoryBLL.GetAll();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            try
            {
                var category = _categoryBLL.GetById(id);
                return Ok(category);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryCreateDTO categoryDto)
        {
            try
            {
                _categoryBLL.Insert(categoryDto);
                return Ok("Category created successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, CategoryUpdateDTO categoryDto)
        {
            try
            {
                categoryDto.CategoryID = id;
                _categoryBLL.Update(categoryDto);
                return Ok($"Category with ID {id} updated successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                _categoryBLL.Delete(id);
                return Ok($"Category with ID {id} deleted successfully");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
