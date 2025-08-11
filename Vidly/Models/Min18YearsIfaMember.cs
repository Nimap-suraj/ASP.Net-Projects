using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class Min18YearsIfaMember :ValidationAttribute
    {
        protected override ValidationResult IsValid(Object Value,ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;     
            if(customer.MemberShipTypeId == MemberShipType.Unknown ||customer.MemberShipTypeId == MemberShipType.PayasYouGo)
            {
                return ValidationResult.Success;
            }
            if(customer.Birthdate == null)
            {
                return new ValidationResult("Birthdate is Required.");
            }
            var age = DateTime.Today.Year - customer.Birthdate.Value.Year;
            return ( age >= 18 ) ? ValidationResult.Success : new ValidationResult("customer should be at least 18 years old  to go om Membership!");

        }
    }
}
