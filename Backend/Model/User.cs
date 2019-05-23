using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS.Model
{
    /// <summary>
    /// This class is the database model for the users of the app.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Store for the Id property, which is the primary key for users in the database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Store for the UserId property of the members in Visma eAccounting, which is used to match users with their invoices.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Store for the Name property, which contains the full name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Store for the Usename property, which is used for login/register to the app.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Store for the ValidPayment property, which depicts if the user has payed their membership this year.
        /// </summary>
        public bool ValidPayment { get; set; }

        /// <summary>
        /// Store for the ActiveUser property, which depicts if a member from eAccounting is an active user of the app.
        /// </summary>
        public bool ActiveUser { get; set; }

        /// <summary>
        /// Store for the ManualValidPayment property, which depicts if the payment status is to be evaluated by the app or set manually by an admin.
        /// </summary>
        public bool ManualValidPayment { get; set; }

        /// <summary>
        /// Store for the Membership property, which is the type of membership the user has in eAccounting
        /// </summary>
        public string Membership { get; set; }

        /// <summary>
        /// Store for the Password property, which is hashed in the database
        /// </summary>
        public byte[] Password { get; set; }

        /// <summary>
        /// Store for the Salt property, which is used to salt and hash the passwords during authentication/registration
        /// </summary>
        public byte[] Salt { get; set; }


    }
}
