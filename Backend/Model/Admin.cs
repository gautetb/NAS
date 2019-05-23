using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS.Model
{
    /// <summary>
    /// This class is the database model for the admins of the app
    /// </summary>
    public class Admin
    {
        /// <summary>
        /// Store for the Id property, which is the primary key for admins in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Store for the Name property, which contains the full name of the admin
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Store for the Usename property, which is used for login to the site
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Store for the Password property, which is hashed in the database
        /// </summary>
        public byte[] Password { get; set; }

        /// <summary>
        /// Store for the Salt property, which is used to salt and hash the passwords during authentication
        /// </summary>
        public byte[] Salt { get; set; }
    }
}
