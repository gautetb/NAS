using System.Web.Mvc;
using NAS.Model;
using NAS.BLL;

namespace NAS.Controllers
{
    /// <summary>
    /// This is the controller for login in as an administrator or registering a new administrator for the site.
    /// </summary>
    public class AdminController : Controller
    {
        private INasLogic _db;
        private IAuthenticatedUser _currentUser;

        /// <summary>
        /// This constructor creates and instance of the class that uses the actual BLL and AuthenticatedUser for extracting information from the HttpContext.
        /// </summary>
        public AdminController()
        {
            _currentUser = new AuthenticatedUser();
            _db = new NasBLL();
        }

        /// <summary>
        /// Constructor that creates and instance of the user logic stub and a fake AuthenticatedUser for unit testing.
        /// </summary>
        public AdminController(INasLogic stub)
        {
            _currentUser = new FakeAuthenticatedUser();
            _db = stub;
        }

        /// <summary>
        /// Returns the view with the form for logging in as administrator
        /// </summary>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Posts the admin from the form to be validated in the database. On successful login an authentication cookie is created
        /// for the admin that logged in. 
        /// </summary>
        /// <returns>
        /// On successful login the user is returned to the <paramref name="ReturnUrl"/>, or index if the <paramref name="ReturnUrl"/> is null.
        /// If the login is unsuccessful the viewmodel is returned to the view for another attempt. 
        /// </returns>
        /// <param name="ReturnUrl">Url to return to on succesful login.</param>
        /// <param name="innAdmin">View model containing the username and password to be validated in the database.</param>
        [HttpPost]
        public ActionResult Login(AdminViewModel innAdmin, string ReturnUrl)
        {
            // Checks if the user exists and if the password is a match
            var validatedAdmin = _db.ValidatedAdmin(innAdmin.Username, innAdmin.Password);

            if (validatedAdmin == null) return View(innAdmin);

            // Creates an authentication cookie for the admin if validation is successful
            _currentUser.SetAuthCookie(validatedAdmin.Name);

            // Determines where to return the admin
            if (ReturnUrl == null) return RedirectToAction("Index", "Home");
            
            return Redirect(ReturnUrl);
      
        }
        
        /// <summary>
        /// Returns the view with the form for registering a new administrator.
        /// </summary>
        [Authorize]
        public ActionResult RegisterAdmin()
        {
            return View();
        }

        /// <summary>
        /// Posts the admin from the form to be registered in the database. On successful registration the admin is redirected to index
        /// </summary>
        /// <returns>
        /// On successful registration the user is returned to the index
        /// If the login is unsuccessful the viewmodel is returned to the view for another attempt. 
        /// </returns>
        /// <param name="innAdmin">View model containing the username and password to be registered in the database.</param>
        [HttpPost]
       // [Authorize]
        public ActionResult RegisterAdmin(AdminViewModel innAdmin)
        {
            var ok = false;

            // Starts registration if modelstate is valid.
            if (ModelState.IsValid)
            {
                ok = _db.RegisterAdmin(innAdmin);
            }

            // Returns to index on successful registration
            if (ok)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(innAdmin);
        }

        /// <summary>
        /// Method for logging out of the admin site
        /// </summary>
        /// <returns>
        /// Returns the user to index, which then in turn returns the user to the login site as the user is no longer logged in and authorized.
        /// </returns>
        public ActionResult Logout()
        {
            _currentUser.LogOut();
            ViewBag.CurrentUser = null;
            return RedirectToAction("Index", "Home");
        }
    }
}