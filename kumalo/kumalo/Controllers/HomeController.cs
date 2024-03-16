using kumalo.Data;
using kumalo.Data.Entities;
using kumalo.Models;
using Microsoft.AspNetCore.Mvc;
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
            this._context = context;
        }

        public IActionResult Index()
        {
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

            return View(allUsersToBeDisplayed);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserLoginAndRegisterModel userRegisterModel)
        {
            User newUser = new User(userRegisterModel.Username, userRegisterModel.Password);
            _context.Users.Add(newUser);
            _context.SaveChanges();


            ////
            /// Tozi newUser da bude lognatiq user.
            ///

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
            User loggedUser = new User("gosho", "azisEBog12!3"); // = HttpContext.Session.GetObject<Users>(loggedUser);
            ///
            ///
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
