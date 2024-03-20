using kumalo.Data;
using kumalo.Data.Entities;
using kumalo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace kumalo.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //Setting session data
            /*
            HttpContext.Session.SetString("Username", "Gosho");
            HttpContext.Session.SetString("LastName", "Marinov");
            HttpContext.Session.SetInt32("Age", 22);
            */

            List<UserDisplayModel> allUsersToBeDisplayed = new List<UserDisplayModel>();

            foreach (User user in _context.Users)
            {
                allUsersToBeDisplayed.Add(new UserDisplayModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    City = user.City,
                    PhoneNumber = user.PhoneNumber,
                    Description = user.Description,
                    LikesCount = user.LikesCount,
                });
            }

            string? loggedUserId = HttpContext.Session.GetString("loggedUserId");
            this.ViewData["loggedUser"] = _context.Users.FirstOrDefault(u => u.Id == loggedUserId);

            return View(allUsersToBeDisplayed);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginAndRegisterModel userLoginModel)
        {
            if (!this.ModelState.IsValid)
                return View();

            User? userTryingToLogin = _context.Users.FirstOrDefault(u => u.Username == userLoginModel.Username);

            if (userTryingToLogin == null) 
            {
                this.ModelState.AddModelError("loginError", "User does not exist.");
                return View();
            }
            if (userTryingToLogin.Password != userLoginModel.Password)
            {
                userLoginModel.Password = string.Empty;
                this.ModelState.AddModelError("loginError", "Incorrect password.");
                return View(userLoginModel);
            }

            HttpContext.Session.SetString("loggedUserId", userTryingToLogin.Id);

            return RedirectToAction("Index"); 
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserLoginAndRegisterModel userRegisterModel)
        {
            //validation
            //
            //

            User newUser = new User(userRegisterModel.Username, userRegisterModel.Password);
            _context.Users.Add(newUser);
            _context.SaveChanges();

            HttpContext.Session.SetString("loggedUserId", newUser.Id);

            return RedirectToAction("EditAccount");
        }

        [HttpGet]
        public IActionResult EditAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditAccount(AccountModel accountModel)
        {
            /*
            string userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
               
            }
            User loggedUser = _context.Users.FirstOrDefault(u => u.Id == userId);
            ///
            ///
            */

            User loggedUser = _context.Users.FirstOrDefault(u => u.Id == HttpContext.Session.GetString("loggedUserId"));
            loggedUser.FirstName = accountModel.FirstName;
            loggedUser.LastName = accountModel.LastName;
            loggedUser.Age = accountModel.Age;
            loggedUser.City = accountModel.City;
            loggedUser.PhoneNumber = accountModel.PhoneNumber;
            loggedUser.Description = accountModel.Description;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult SeeUser(string id)
        {
            User user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                //////
                ///////
                ///////
                ///////
                return NotFound();
            }

            UserDisplayModel userToReturn = new UserDisplayModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                City = user.City,
                PhoneNumber = user.PhoneNumber,
                Description = user.Description,
                LikesCount = user.LikesCount
            };

            return View(userToReturn);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
