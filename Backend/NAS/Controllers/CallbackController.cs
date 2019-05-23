using NAS.BLL;
using System.Web.Mvc;

namespace NAS.Controllers
{
    /// <summary>
    /// This is the controller that handles the response from Visma eAccounting API and sends the httpcontext to the right methods in the business logic layer.
    /// </summary>
    public class CallbackController : Controller
    {
        private INasLogic _db;

        /// <summary>
        /// Constructor that creates and instance of the class.
        /// </summary>
        public CallbackController()
        {
            _db = new NasBLL();
        }

        /// <summary>
        /// This is the endpoint of the redirect URI given to Visma, which receives the code to be used for requesting an access token from the authorization server.
        /// </summary>
        /// <returns>
        /// On successful saving of the access token the user is redirected home, or to an error site if something went wrong.
        /// </returns>
        /// <remarks>
        /// <para>The method asks for an authorization token from the server and stores it in the database.</para>
        /// </remarks>
        public ActionResult Index()
        {
            
            var client = VismaAuthProvider.CreateClient();
            var state = VismaAuthProvider.RequestAuthorization(client);

            var successUpdateVismaToken = _db.UpdateVismaToken(state);

            if (successUpdateVismaToken)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Error", "Home");
        }

    }
}