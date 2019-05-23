using System.Net.Http;
using System.Web.Http;
using NAS.BLL;
using NAS.Model;

namespace NAS.Controllers
{
    /// <summary>
    /// This is the client API that provides the endpoints for the mobile app to register a password to a user and/or asking for customer information.
    /// This controller sends requests to the business logic layer, as well as to the AuthenticatedUser interface to extract the username from the access token.
    /// </summary>
    public class UserController : ApiController
    {
        private INasLogic _bll;
        private IAuthenticatedUser _currentUser;

        /// <summary>
        /// This constructor creates and instance of the class that uses the actual BLL and AuthenticatedUser for extracting information from the HttpContext.
        /// </summary>
        public UserController() 
        {
            _bll = new NasBLL();
            _currentUser = new AuthenticatedUser();
        }

        /// <summary>
        /// Constructor that creates and instance of the user logic stub and a fake AuthenticatedUser for unit testing.
        /// </summary>
        public UserController(INasLogic stub)
        {
            _currentUser = new FakeAuthenticatedUser();
            _bll = stub;
        }

        /// <summary>
        /// This is the endpoint for authorized users to get the user information needed from the database
        /// </summary>
        /// <returns>
        /// HttpResponseMessage containing user information from the database
        /// </returns>
        [Authorize]
        public HttpResponseMessage Get()
        {
            // Gets the username from the token provided on successful authentication.
            var username = _currentUser.GetUsernameFromToken();

            // Gets the correct user from the database
            var user = _bll.GetUser(username);

            return Request.CreateResponse(user);

        }

        /// <summary>
        /// This is the endpoint for members of NAS to register a password to their user in the database.
        /// </summary>
        /// <returns>
        /// HttpResponseMessage containing the response from the database with an errormessage if unsuccessful or Success if it worked.
        /// </returns>
        public HttpResponseMessage Post(UserViewModel innUser)
        {
            // Registers a user and stores the response in a variable to be returned to the client
            var responsemessage = _bll.RegisterUser(innUser);

            // If the response is Success the message is return to the client
            if (responsemessage == "Success")
            { 
                return Request.CreateResponse(responsemessage);
            }

            // Returns a specified errormessage if unsuccessful
            return Request.CreateResponse(responsemessage);

        }

    }
}
