using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Security;

namespace NAS
{
    /// <summary>
    /// This is the interface for extracting information from the HttpContext. The AuthenticatedUser and a fake version implements this interface.
    /// This interface makes it possible to take advantage of polymorphism, making it easy for the controllers to invoke the actual AuthenticatedUser for regular operations
    /// or the fake on for unit testing.
    /// </summary>
    public interface IAuthenticatedUser
    {
        string GetUsername();
        string GetUsernameFromToken();
        void LogOut();
        void SetAuthCookie(string username);
    }

    public class AuthenticatedUser : IAuthenticatedUser
    {
        /// <summary>
        /// This method extracts the name of the admin from the HttpContext
        /// </summary>
        /// <returns>
        /// The name in the form of a <c>string</c>
        /// </returns>
        public string GetUsername()
        {
            var username = HttpContext.Current.User.Identity.Name;

            return username;
        }

        /// <summary>
        /// Creates an authentication ticket for the authenticated admin and adds it to the cookies.
        /// </summary>
        public void SetAuthCookie(string username)
        {
            FormsAuthentication.SetAuthCookie(username, false);
        }
        /// <summary>
        /// Removes the forms-authentication ticket from the browser.
        /// </summary>
        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// This method extracts the name of the user from the token supplied from the /token endpoint on succesful authentication
        /// </summary>
        /// <returns>
        /// The name in the form of a <c>string</c>
        /// </returns>
        public string GetUsernameFromToken()
        {
            var identity = (ClaimsIdentity) HttpContext.Current.User.Identity;
            List<Claim> claims = identity.Claims.ToList();
            var username = claims[1].Value;

            return username;
        }
    }

    /// <summary>
    /// This is the fake AuthenticatedUser used to mock the HttpContext. It implements some of the methods needed to test the presentation layer. These implementations produce predictable results
    /// to test different outcomes in the controllers.
    /// </summary>
    public class FakeAuthenticatedUser : IAuthenticatedUser
    {

        public string GetUsername()
        {
            return "Fake User";
        }

        public string GetUsernameFromToken()
        {
            return "testuser";
        }

        public void LogOut()
        {
            
        }

        public void SetAuthCookie(string username)
        {
            
        }
    }
}
