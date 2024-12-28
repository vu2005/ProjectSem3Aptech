using CarInsuranceManage.Database;
using CarInsuranceManage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CarInsuranceManage.Controllers
{
    public class contactsController : Controller
    {
        private readonly CarInsuranceDbContext _dbContext;

        public contactsController(CarInsuranceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

     
       
        [HttpPost]
        public IActionResult Deletecontacts(int id)
        {
            var contact = _dbContext.contacts.Find(id); // Corrected to contacts
            if (contact == null)
            {
                return NotFound();
            }
            _dbContext.contacts.Remove(contact); // Corrected to remove the contact
            _dbContext.SaveChanges();
            return Json(new { success = true }); // Return success in JSON format
        }

    }


}