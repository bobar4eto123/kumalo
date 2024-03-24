
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
            //Getting the logged user from the session and passing it to the Index view (needed for the "Welcome, ...")
            string? loggedUserId = HttpContext.Session.GetString("loggedUserId");
            this.ViewData["loggedUser"] = _context.Users.FirstOrDefault(u => u.Id == loggedUserId);

            //Reading from the DB
            List<DisplayAccountModel> allAccountsToBeDisplayed = new List<DisplayAccountModel>();
            foreach (User user in _context.Users)
            {
                if (user.Role == "Supplier")
                {
                    allAccountsToBeDisplayed.Add(new DisplayAccountModel(user));
                }
            }

            allAccountsToBeDisplayed.RemoveAll(u => u.Id == loggedUserId); //As a logged user, I do not want to see myself in the list with suppliers

            //Passing the list with models to the Index view
            return View(allAccountsToBeDisplayed);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginModel userLoginModel)
        {
            //-----VALIDATION-----//
            
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

            //--------------------//
            

            //If validation is passed, setting the logged user for the session with its Id
            HttpContext.Session.SetString("loggedUserId", userTryingToLogin.Id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            //Cleaning the session means no longer having a logged user
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterModel userRegisterModel)
        {
            //-----VALIDATION-----//

            if (!this.ModelState.IsValid)
                return View();

            User validationUser = _context.Users.FirstOrDefault(u => u.Username == userRegisterModel.Username);
            if (validationUser != null)
            {
                this.ModelState.AddModelError("registerError", "User with this username already exists.");
                return View(userRegisterModel);
            }

            //--------------------//


            //If validation is passed, adding a new user to the DB
            User newUser = new User(userRegisterModel.Username, userRegisterModel.Password, userRegisterModel.Role);
            _context.Users.Add(newUser);
            _context.SaveChanges();

            //Setting the logged user for the session with its Id
            HttpContext.Session.SetString("loggedUserId", newUser.Id);

            //Immediately redirecting to the EditAccount, because a new user has to configure account info
            return RedirectToAction("EditAccount");
        }

        [HttpGet]
        public IActionResult EditAccount()
        {
            //Getting the logged user from the session, in order to pass its info as placeholders in the textboxes
            string? loggedUserId = HttpContext.Session.GetString("loggedUserId");
            User loggedUser = _context.Users.FirstOrDefault(u => u.Id == loggedUserId);

            //Converting it into an EditAccountModel
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
            //-----VALIDATION-----// (for the required fields)

            if (!this.ModelState.IsValid)
                return View();

            //--------------------//

            //Getting the logged user from the DB (it is the user to be modified)
            User loggedUser = _context.Users.FirstOrDefault(u => u.Id == HttpContext.Session.GetString("loggedUserId")); //The app architecture does not
                                                                                                        //allow it to be null so User? is not necessary
            //Saving Account Picture in the folder
            string folder = "accounts/pictures/" + Guid.NewGuid().ToString() + "_" + editAccountModel.AccountPicture.FileName;
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
            editAccountModel.AccountPicture.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            //Saving the given info
            loggedUser.PictureUrl = "/" + folder;
            loggedUser.FirstName = editAccountModel.FirstName;
            loggedUser.LastName = editAccountModel.LastName;
            loggedUser.Age = editAccountModel.Age;
            loggedUser.City = editAccountModel.City;
            loggedUser.PhoneNumber = editAccountModel.PhoneNumber;
            loggedUser.Description = editAccountModel.Description;
            
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult SeeAccount(string id)
        {
            //Getting the logged user from the sessionand passing it to the SeeAccount view (needed for the checks if a user wants to see their 
            //own account or other - there are differences with the LikeOrDislike functionality)
            string? loggedUserId = HttpContext.Session.GetString("loggedUserId");
            this.ViewData["loggedUser"] = _context.Users.FirstOrDefault(u => u.Id == loggedUserId);

            //Getting the user which has to be displayed from the DB
            User user = _context.Users.FirstOrDefault(u => u.Id == id); //The app architecture does not allow it to be null so User? is not necessary

            //Converting to a DisplayAccountModel
            DisplayAccountModel accountToDisplay = new DisplayAccountModel(user);

            //Passing the account which has to be displayed to the View
            return View(accountToDisplay);
        }

        [HttpPost]
        public IActionResult LikeOrDislike(string id)
        {
            //Getting the logged user from the session
            string loggedUserId = HttpContext.Session.GetString("loggedUserId"); //The app architecture does not allow it to be null so User? is not necessary
            
            //Getting the user whose likes will be modified
            User likedUser = _context.Users.FirstOrDefault(u => u.Id == id); //The app architecture does not allow it to be null so User? is not necessary

            //Modifying the likes
            List <string> likesToBeModified = likedUser.ReceivedLikesFrom;
            if (likesToBeModified.Contains(loggedUserId))
            {
                likesToBeModified.Remove(loggedUserId);
            }
            else
            {
                likesToBeModified.Add(loggedUserId);
            }

            _context.SaveChanges();

            //Refreshing the page to see the new likes count
            return RedirectToAction("SeeAccount", new {id = id});
        }

    }
}
