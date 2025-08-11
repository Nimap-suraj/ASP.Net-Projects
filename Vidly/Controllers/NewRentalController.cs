using Microsoft.AspNetCore.Mvc;
using Vidly.Data;
using Vidly.DTO;
using Vidly.Models;
using System.Linq;

namespace Vidly.Controllers
{
    public class NewRentalController : Controller
    {
        private readonly ApplicationDbContext _context;
        public NewRentalController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateNewRentals([FromForm] NewRentalDto rental)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == rental.CustomerId);
            if (customer == null)
                return BadRequest("Invalid customer ID.");

            var movies = _context.Movies.Where(m => rental.MovieIds.Contains(m.Id)).ToList();
            if (movies.Count != rental.MovieIds.Count)
                return BadRequest("One or more MovieIds are invalid.");

            foreach (var movie in movies)
            {
                if (movie.NumberOfAvailable == 0)
                    return BadRequest($"Movie '{movie.Name}' is not available.");

                movie.NumberOfAvailable--;

                var rentalEntry = new Rental
                {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now
                };

                _context.Rental.Add(rentalEntry);
            }

            _context.SaveChanges();

            return Ok();
        }
    }
}
