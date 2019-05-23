using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS.Model
{
    /// <summary>
    /// This class is the model for the user information returned when the mobile app request a user from the web api in the UserController
    /// </summary>
    public class UserApiModel
    {
        /// <summary>
        /// Store for the Name property, which contains the full name of the user for the mobile app
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Store for the Username property
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Store for the ValidPayment property, used to show if the user has payed their membership this year on the mobile app
        /// </summary>
        public bool ValidPayment { get; set; }

        /// <summary>
        /// Store for the Membership property, used to show the type of membership the user has in eAccounting
        /// </summary>
        public string Membership { get; set; }

        /// <summary>
        /// Store for the Response property, used to return more detailed errormessages when registering a password to a user from the mobile app
        /// </summary>
        public string Response { get; set; }
    }
}
