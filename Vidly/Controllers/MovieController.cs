using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vidly.Data;
using Vidly.Models;
using Vidly.ViewModel;

namespace Vidly.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult GetMovieList(string term)
        {
            var movie = _context.Movies
                .Where(c => c.Name.Contains(term))
                 .Select(c => new { id = c.Id, name = c.Name }) // ✅ Include `id`
                .ToList();

            return Json(movie);
        }
        //Basic
        //https://localhost:7193/Movie/Random
        //public IActionResult RandomBasic()
        //{
        //    var movie = new Movie()
        //    {
        //        Name = "silent Voice"
        //    };

        //    return View(movie);
        //}
        //https://localhost:7193/Movie/edit?Id=100 
        // public ActionResult Edit(int id)
        //{
        //    return Content("Id: " + id);
        //    // we can't return MovieId becuse default id is id Not MovieId
        //}
        //https://localhost:7193/Movie?PageSize=1000&SortBy=java we can change this 
        // default is https://localhost:7193/Movie it will show default value which we have set
        //public ActionResult Index(int? PageSize, string SortBy)
        //{
        //    if (!PageSize.HasValue)
        //    {
        //        PageSize = 1;
        //    }
        //    if (string.IsNullOrEmpty(SortBy))
        //    {
        //        SortBy = "DotNet";
        //    }
        //    return Content($"PageSize: {PageSize}& sortBy: {SortBy}");
        //}

        //public ActionResult ByReleasedDate(int year, int month)
        //{
        //    return Content($"Year : {year} & month: {month}");
        //}
        public List<Movie> GetMovies()
        {
            return _context.Movies.Include(c => c.Genre).ToList();
        }
        public IActionResult Index()
        {
            var movie = GetMovies();
            return View(movie);
        }
       
        public ActionResult Details(int id)
        {
            var movie = GetMovies().FirstOrDefault(c => c.Id == id);
            if(movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }
        public IActionResult Random()
        {
            var movie = new Movie()
            {
                Name = "silent Voice"
            };
            var customers = new List<Customer>
            {
                new Customer{Name ="Customer 1"},
                new Customer{Name ="Customer 2"}
            };
            var ViewModel = new RandomMovieViewModel()
            {
                Movie = movie,
                Customers = customers,
            };

            return View(ViewModel);
        }
        
        public IActionResult Add()
        {
            var genre = _context.Genre.ToList();
            var ViewModel = new MovieGenreFormViewModel
            {
                Movie = new Movie(),
                Genre = genre
            };
            return View("AddMovie",ViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMovieToDB(Movie movie)
        {
            if(!ModelState.IsValid)
            {
                var ViewModel = new MovieGenreFormViewModel
                {
                    Movie = movie,
                    Genre = _context.Genre.ToList()
                };
                return View("Index", movie);
            }
            if(movie.Name == null)
            {
                return NotFound();
            }
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return RedirectToAction("Index","Movie");
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.FirstOrDefault(c => c.Id == id);
            if(movie == null)
            {
                return NotFound();
            }
            var ViewModel = new MovieGenreFormViewModel
            {
                Movie = movie,
                Genre = _context.Genre.ToList(),
            };
            return View("EditMovie",ViewModel);
        }
        [HttpPost]
        public ActionResult Update(Movie movie)
        {
            
            var movieInDb = _context.Movies.FirstOrDefault(m => m.Id == movie.Id);
            if (movieInDb == null)
                return NotFound();

            movieInDb.Name = movie.Name;
            movieInDb.ReleasedDate = movie.ReleasedDate;
            movieInDb.DateAdded = movie.DateAdded;
            movieInDb.GenreId = movie.GenreId;
            movieInDb.NumberOfStack = movie.NumberOfStack;

            _context.SaveChanges();

            return RedirectToAction("Index", "Movie");
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null)
            {
                return Json(new { success = false, message = "movie not found" });
            }

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return Json(new { success = true });
        }

    }
}
