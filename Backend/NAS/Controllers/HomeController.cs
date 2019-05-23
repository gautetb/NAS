using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NAS.BLL;
using NAS.Model;
using PagedList;


namespace NAS.Controllers
{
    /// <summary>
    /// This is the controller that handles the front page of the admin site
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private INasLogic _db;
        private IAuthenticatedUser _currentUser;

        /// <summary>
        /// This constructor creates and instance of the class that uses the actual BLL and AuthenticatedUser for extracting information from the HttpContext.
        /// </summary>
        public HomeController()
        {
            _currentUser = new AuthenticatedUser();
            _db = new NasBLL();
        }

        /// <summary>
        /// Constructor that creates and instance of the user logic stub and a fake AuthenticatedUser for unit testing.
        /// </summary>
        public HomeController(INasLogic stub)
        {
            _currentUser = new FakeAuthenticatedUser();
            _db = stub;
        }

        /// <summary>
        /// This methods shows the front page of the admin site. It includes a list of all users, with options to:
        /// <list type="bullet">
        /// <item>
        /// <description>Sort users.</description>
        /// </item>
        /// <item>
        /// <description>Search based on names.</description>
        /// </item>
        /// <item>
        /// <description>Edit users.</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <returns>
        /// Returns a paged list of the users registered in the database.
        /// </returns>
        /// <remarks>
        /// <para>The view is a list of all the users show 10 at the time which are shown in different pages determined by the <paramref name="page"/> number.</para>
        /// </remarks>
        /// <param name="searching">Search string to search for spesific users in the searchfield on the front page.</param>
        /// <param name="sort">String to determine in what order the list is supposed to be returned (and showed) in.</param>
        /// <param name="currentFilter">Current filter from the viewbag to keep the search string when changing pages browsing through the users.</param>
        /// <param name="page">Which page the admin is currenty on.</param>
        public ActionResult Index(string searching, string sort, string currentFilter, int? page)
        {
            // Gets the username of the authenticated user and adds it to the viewbag to be displayed             
            ViewBag.CurrentUser = _currentUser.GetUsername();

            // Adds the current sorting to the viewbag
            ViewBag.CurrentSort = sort;

            // Alternates between different ways of sorting the userlist
            ViewBag.NameSortParm = String.IsNullOrEmpty(sort) ? "Name_desc" : "";
            ViewBag.UsernameSortParm = sort == "Username" ? "Username_desc" : "Username";
            ViewBag.ValidPaymentSortParm = sort == "ValidPayment" ? "ValidPayment_desc" : "ValidPayment";
            ViewBag.ActiveUserSortParm = sort == "ActiveUser" ? "ActiveUser_desc" : "ActiveUser";
            ViewBag.MembershipSortParm = sort == "Membership" ? "Membership_desc" : "Membership";


            // Current filter is set to the search string if the search string is empty or null
            if (String.IsNullOrEmpty(searching)) searching = currentFilter;

            // Sets the new filter to currentfilter in viewbag
            ViewBag.CurrentFilter = searching;

            // Gets a list from the database with names of users that contain the search string
            List<User> allUsers = _db.GetUserList(searching);

            // Sorts the list based on the sort string provided from the view.
            switch (sort)
            {
                case "Name_desc":
                    allUsers = allUsers.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Username_desc":
                    allUsers = allUsers.OrderByDescending(s => s.Username).ToList();
                    break;
                case "Username":
                    allUsers = allUsers.OrderBy(s => s.Username).ToList();
                    break;
                case "ValidPayment":
                    allUsers = allUsers.OrderBy(s => s.ValidPayment).ToList();
                    break;
                case "ValidPayment_desc":
                    allUsers = allUsers.OrderByDescending(s => s.ValidPayment).ToList();
                    break;
                case "Membership":
                    allUsers = allUsers.OrderBy(s => s.Membership).ToList();
                    break;
                case "Membership_desc":
                    allUsers = allUsers.OrderByDescending(s => s.Membership).ToList();
                    break;
                case "ActiveUser_desc":
                    allUsers = allUsers.OrderByDescending(s => s.ActiveUser).ToList();
                    break;
                case "ActiveUser":
                    allUsers = allUsers.OrderBy(s => s.ActiveUser).ToList();
                    break;
                default:  // Name ascending 
                    allUsers = allUsers.OrderBy(s => s.Name).ToList();
                    break;
            }

            // Sets the pagenumber to current pagenumber if not null, or 1 if null
            int pageNumber = (page ?? 1);

            // Returns the list of users to the view as a paged list
            return View(allUsers.ToPagedList(pageNumber, 10));
        }

        /// <summary>
        /// Updates the userbase from visma.
        /// </summary>
        /// <returns>
        /// Redirects to index if the update was successful or the errorpage if something went wrong.
        /// </returns>
        public ActionResult AddUsersFromVisma()
        {
            var success = _db.UpdateUserBase();

            if (success)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Error");
        }

        /// <summary>
        /// Method to show the errorpage if something goes wrong when using the admin page.
        /// </summary>
        /// <returns>
        /// Errorpage.
        /// </returns>
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Returns the view with the form for editing a user.
        /// </summary>
        /// <returns>
        /// View with the user found in the database.
        /// </returns>
        /// <param name="id">Database id of the user to be updated from the admin page.</param>
        public ActionResult EditUser(int id)
        {
            var user = _db.GetUser(id);
            return View(user);
        }

        /// <summary>
        /// Posts the user from the form to be edited in the database. On successful update the admin is redirected to index
        /// </summary>
        /// <returns>
        /// On successful update the admin is returned to the index
        /// If the update is unsuccessful the viewmodel is returned to the view for another attempt. 
        /// </returns>
        /// <param name="user">View model containing the user parameters to be changed in the database.</param>
        [HttpPost]
        public ActionResult EditUser(UserViewModel user)
        {
            var updateSuccess = false;

            // Starts update if modelstate is valid.
            if (ModelState.IsValid)
            {
                updateSuccess = _db.UpdateUserAsAdmin(user);
            }

            // Returns to index on successful update
            if (updateSuccess)
            {
                return RedirectToAction("Index");
            }

            return View(user);
        }
    }
}
