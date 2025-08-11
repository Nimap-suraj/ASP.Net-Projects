using Vidly.Models;

namespace Vidly.ViewModel
{
    public class CustomerFormViewModel
    {
        public IEnumerable<MemberShipType> memberShipTypes { get; set; }
        public Customer Customer { get; set; }
    }
}
