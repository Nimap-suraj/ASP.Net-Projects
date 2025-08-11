using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class Min20NumberOfStack : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var movie = (Movie)validationContext.ObjectInstance;
            //if(movie.GenreId == 0)
            //{
            //    return ValidationResult.Success;
            //}

            var stocks = movie.NumberOfStack;
            return (movie.NumberOfStack >= 0 && movie.NumberOfStack <= 20)
            ? ValidationResult.Success
            : new ValidationResult("Stocks must be between 1 and 20");

        }
    }
}
