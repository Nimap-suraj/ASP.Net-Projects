using GenericRepositoryPattern.DTOs;
using GenericRepositoryPattern.Entity;
using GenericRepositoryPattern.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GenericRepositoryPattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetALL()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsyc(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Category category)
        {
            var existing = await _unitOfWork.Categories.GetByIdAsyc(id);
            if (existing == null)
                return NotFound();

            existing.Name = category.Name;
            existing.Description = category.Description;

            _unitOfWork.Categories.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsyc(id);
            if (category == null)
                return NotFound();

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
