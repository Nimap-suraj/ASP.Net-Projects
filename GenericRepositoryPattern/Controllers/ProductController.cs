using GenericRepositoryPattern.Entity;
using GenericRepositoryPattern.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenericRepositoryPattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetALL()
        {
            var categories = await _unitOfWork.products.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _unitOfWork.products.GetByIdAsyc(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            await _unitOfWork.products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            var existing = await _unitOfWork.products.GetByIdAsyc(id);
            if (existing == null)
                return NotFound();

            existing.Name = product.Name;

            _unitOfWork.products.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.products.GetByIdAsyc(id);
            if (product == null)
                return NotFound();

            _unitOfWork.products.Delete(product);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
