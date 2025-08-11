using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Please Enter Name of a Customer! ")]
        [MaxLength(255)]

        public string Name { get; set; } = string.Empty;
        public bool IsSubscribed{ get; set; }

        [Display(Name = "Membership Type")]
        //[BindNever]
        public MemberShipType? memberShipType { get; set; }
        public byte MemberShipTypeId { get; set; }

        //[DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        [Min18YearsIfaMember]
        public DateTime? Birthdate { get; set; }
    }
}
