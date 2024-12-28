using CarInsuranceManage.Database;
using CarInsuranceManage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CarInsuranceManage.Controllers
{
    public class UserController : Controller
    {
        private readonly CarInsuranceDbContext _dbContext;

        public UserController(CarInsuranceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View("~/Views/Admin/User/CreateUser.cshtml");
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                _dbContext.users.Add(user);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("~/Views/Admin/User/CreateUser.cshtml", user);
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _dbContext.users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View("~/Views/Admin/User/EditUser.cshtml", user);
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                _dbContext.users.Update(user);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("~/Views/Admin/User/EditUser.cshtml", user);
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var user = _dbContext.users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            _dbContext.users.Remove(user);
            _dbContext.SaveChanges();
            return Json(new { success = true }); // Trả về JSON thông báo xóa thành công
        }


        [HttpGet]
        public IActionResult Index()
        {
            var users = _dbContext.users.ToList();
            return View("~/Views/Admin/User/Index.cshtml", users);
        }

        [HttpGet]
        public IActionResult ConfirmDelete(int id)
        {
            var user = _dbContext.users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View("~/Views/Admin/User/ConfirmDelete.cshtml", user);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int user_id)
        {
            var user = _dbContext.users.Find(user_id);
            if (user == null)
            {
                return NotFound();
            }
            _dbContext.users.Remove(user);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult FeedBack()
        {
            var usersFeedBack = _dbContext.contacts.ToList();
            return View("~/Views/Admin/User/FeedBack.cshtml", usersFeedBack);
        }


    }


}