using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Vidly.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        //[BindNever]
        public Genre? Genre { get; set; }

        public int GenreId { get; set; }

        [Required(ErrorMessage = "Release Date is required")]
        public DateTime ReleasedDate { get; set; }

        [Required(ErrorMessage = "Date Added is required")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Number Of Stack")]
        [Min20NumberOfStack]
        public int NumberOfStack { get; set; }

        public byte NumberOfAvailable { get; set; }
    }
}
