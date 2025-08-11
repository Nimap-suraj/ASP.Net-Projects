using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Security;

//using System.Data.Entity;
using Vidly.Data;
using Vidly.Models;
using Vidly.ViewModel;

namespace Vidly.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public JsonResult GetCustomerList(string term)
        {
            var customers = _context.Customers
                .Where(c => c.Name.Contains(term))
                 .Select(c => new { id = c.Id, name = c.Name }) // ✅ Include `id`
                .ToList();

            return Json(customers);
        }

        public IActionResult Index()
        {
            var customers = GetCustomers();
            //var customers = _context.Customers.ToList();
            return View(customers);
        }
        public ActionResult Details(int id)
        {
            var customer = GetCustomers().FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers.Include(c => c.memberShipType).ToList();
        }


        public ActionResult New()
        {
            var memberShiptypes = _context.MemberShipType.ToList();
            var ViewModel = new CustomerFormViewModel
            {
                Customer = new Customer(),
                memberShipTypes = memberShiptypes
            };
            return View("CustomerForm",ViewModel); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            if(!ModelState.IsValid)
            {
                var ViewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    memberShipTypes = _context.MemberShipType.ToList()
                };
                return View("CustomerForm", ViewModel);
            }
            if (customer.Id == 0)
            {   
                 
                _context.Customers.Add(customer);
            }
            else
            {
                var customerInDB = _context.Customers.Include(c => c.memberShipType).FirstOrDefault(c => c.Id == customer.Id);
                customerInDB.Name = customer.Name;
                customerInDB.Birthdate = customer.Birthdate;
                customerInDB.IsSubscribed = customer.IsSubscribed;
                customerInDB.MemberShipTypeId = customer.MemberShipTypeId;
            }
             _context.SaveChanges();
            return RedirectToAction("Index","Customer");
        }
        // Page
        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                memberShipTypes = _context.MemberShipType.ToList()
            };
            return View("CustomerForm", viewModel);
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return Json(new { success = false, message = "Customer not found" });
            }

            _context.Customers.Remove(customer);
            _context.SaveChanges();

            return Json(new { success = true });
        }

    }
}
