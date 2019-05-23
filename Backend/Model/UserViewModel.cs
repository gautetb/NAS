using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace NAS.Model
{
    /// <summary>
    /// This class is the view model for the users to be displayed on the admin site as well as retreive data from forms.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Store for the Name property, which contains the full name of the user for the view on the admin site
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Store for the ValidPayment property, used to show if the user has payed their membership this year on the admin site
        /// </summary>
        public bool ValidPayment { get; set; }

        /// <summary>
        /// Store for the Membership property, used to show the type of membership the user has in eAccounting
        /// </summary>
        public string Membership { get; set; }

        /// <summary>
        /// Store for the Username property
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Store for the Password property
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Store for the ConfirmPassword property, used for form validation when logging inn and registering users
        /// </summary>
        [Compare("Password", ErrorMessage = "Passordbekreftelse matcher ikke oppgitt passord")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Store for the Response property, used to return more detailed errormessages when updating users from the admin site
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Store for the AutoUpdatePayment property, used to determine wether the payments status should be updated automatically or manually
        /// </summary>
        public string AutoUpdatePayment { get; set; }
    }
}
