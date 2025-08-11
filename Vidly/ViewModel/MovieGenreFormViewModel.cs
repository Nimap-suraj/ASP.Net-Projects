using Vidly.Models;

namespace Vidly.ViewModel
{
    public class MovieGenreFormViewModel
    {
        public IEnumerable<Genre> Genre { get; set; }
        public Movie Movie { get; set; } 
    }
}
