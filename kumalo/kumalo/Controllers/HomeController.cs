
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
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;     

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {

            List<DisplayAccountModel> allAccountsToBeDisplayed = new List<DisplayAccountModel>();

            foreach (User user in _context.Users)
            {
                allAccountsToBeDisplayed.Add(new DisplayAccountModel
                {
                    Id = user.Id,
                    PictureUrl = user.PictureUrl,
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

            return View(allAccountsToBeDisplayed);
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
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
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
            if (!this.ModelState.IsValid)
                return View();

            User validationUser = _context.Users.FirstOrDefault(u => u.Username == userRegisterModel.Username);
            if (validationUser != null)
            {
                this.ModelState.AddModelError("registerError", "User with this username already exists.");
                return View(userRegisterModel);
            }

            User newUser = new User(userRegisterModel.Username, userRegisterModel.Password);
            _context.Users.Add(newUser);
            _context.SaveChanges();

            HttpContext.Session.SetString("loggedUserId", newUser.Id);

            return RedirectToAction("EditAccount");
        }


        [HttpGet]
        public IActionResult SeeAccount(string id)
        {
            string? loggedUserId = HttpContext.Session.GetString("loggedUserId");
            this.ViewData["loggedUser"] = _context.Users.FirstOrDefault(u => u.Id == loggedUserId);

            User user = _context.Users.FirstOrDefault(u => u.Id == id); //ne moje da e null

            DisplayAccountModel accountToReturn = new DisplayAccountModel
            {
                Id = user.Id,
                PictureUrl = user.PictureUrl,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                City = user.City,
                PhoneNumber = user.PhoneNumber,
                Description = user.Description,
                LikesCount = user.LikesCount
            };

            return View(accountToReturn);
        }

        [HttpGet]
        public IActionResult EditAccount()
        {
            ViewData["IsEditAccountPage"] = true;
            string? loggedUserId = HttpContext.Session.GetString("loggedUserId");
            User loggedUser = _context.Users.FirstOrDefault(u => u.Id == loggedUserId);
            EditAccountModel editAccountModel = new EditAccountModel
            {
                FirstName = loggedUser.FirstName,
                LastName = loggedUser.LastName,
                Age = loggedUser.Age,
                City = loggedUser.City,
                PhoneNumber = loggedUser.PhoneNumber,
                Description = loggedUser.Description
            };

            return View(editAccountModel);
        }

        [HttpPost]
        public IActionResult EditAccount(EditAccountModel editAccountModel)
        {
            if (!this.ModelState.IsValid)
                return View();

            User loggedUser = _context.Users.FirstOrDefault(u => u.Id == HttpContext.Session.GetString("loggedUserId"));

            //picture things
            string folder = "accounts/pictures/" + Guid.NewGuid().ToString() + "_" + editAccountModel.AccountPicture.FileName;
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
            editAccountModel.AccountPicture.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            loggedUser.PictureUrl = "/" + folder;
            loggedUser.FirstName = editAccountModel.FirstName;
            loggedUser.LastName = editAccountModel.LastName;
            loggedUser.Age = editAccountModel.Age;
            loggedUser.City = editAccountModel.City;
            loggedUser.PhoneNumber = editAccountModel.PhoneNumber;
            loggedUser.Description = editAccountModel.Description;
            
            _context.SaveChanges();

            //edin pop-up "Saved changes"

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Like(string id)
        {
            User userToIncrementLikes = _context.Users.FirstOrDefault(u => u.Id == id);
            userToIncrementLikes.LikesCount++;
            _context.SaveChanges();

            return RedirectToAction("SeeAccount", new {id = id});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
