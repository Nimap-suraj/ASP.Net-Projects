using Vidly.Models;

namespace Vidly.ViewModel
{
    public class RandomMovieViewModel
    {
        public Movie? Movie { get; set; }
        public List<Customer> Customers { get; set; } = new List<Customer>();
    }
}
