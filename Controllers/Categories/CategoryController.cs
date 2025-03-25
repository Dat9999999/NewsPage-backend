using Microsoft.AspNetCore.Mvc;
using NewsPage.Models.entities;
using NewsPage.Models.RequestDTO;
using NewsPage.Models.ResponseDTO;
using NewsPage.repositories.interfaces;

namespace NewsPage.Controllers.Categories
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();

            // Mapping Category entity to CategoryDTO
            var categoryDTOs = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Topic = c.Topic // Trả về toàn bộ object Topic
            }).ToList();

            return Ok(categoryDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();

            var categoryDTO = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Topic = category.Topic // Trả về object Topic
            };

            return Ok(categoryDTO);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CategoryCreateDTO categoryCreateDTO)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryCreateDTO.Name,
                TopicId = categoryCreateDTO.TopicId
            };

            var addedCategory = await _categoryRepository.AddAsync(category);

            var categoryDTO = new CategoryDTO
            {
                Id = addedCategory.Id,
                Name = addedCategory.Name,
                Topic = addedCategory.Topic
            };

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, categoryDTO);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryUpdateDTO categoryDto)
        {
            if (id != categoryDto.Id) return BadRequest("ID không khớp.");

            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null) return NotFound();

            existingCategory.Name = categoryDto.Name;
            existingCategory.TopicId = categoryDto.TopicId;

            var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);
            if (updatedCategory == null) return BadRequest("Cập nhật thất bại.");

            var updatedCategoryDTO = new CategoryDTO
            {
                Id = updatedCategory.Id,
                Name = updatedCategory.Name,
                Topic = updatedCategory.Topic // Trả về object Topic nếu có
            };

            return Ok(updatedCategoryDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();

            await _categoryRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
