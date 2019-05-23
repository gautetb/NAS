using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetOpenAuth.OAuth2;
using NAS.BLL;
using NAS.DAL;
using NAS.Model;

namespace NAS.BLL
{
    /// <summary>
    /// This is the business logic stub. It implements some of the methods needed to test the presentation layer. These implementations produce predictable results
    /// to test different outcomes in the controllers.
    /// </summary>
    public class NasBLLstub : INasLogic
    {
        private INasDAL _db = new NasDALstub();

        public UserApiModel GetUser(string username)
        {
            var user = new UserApiModel()
            {
                Username = "testuser",
                Name = "test",
                ValidPayment = true,
                Membership = "Membership"
            };

            return user;
        }

        public UserViewModel GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool UpdateVismaToken(IAuthorizationState state)
        {
            return true;
        }

        public List<User> GetUserList(string search)
        {
            throw new NotImplementedException();
        }

        public string RegisterUser(UserViewModel innUser)
        {
            // Gets the user from the database
            var user = _db.GetUserDb(innUser.Username);

            // Returns an error message if the user does not have a registered membership in the database and therefore Visma eAccounting
            if (user == null) return "Det er ingen aktive medlemmer med den eposten";

            // If a user is already an active user the registration is not valid and an error message is returned
            if (user.ActiveUser) return "En bruker er allerede registrert med den eposten";

            return _db.RegisterUser(user, innUser.Password);

        }

        public Admin GetAdmin(AdminViewModel admin)
        {
            throw new NotImplementedException();
        }

        public Admin ValidatedAdmin(string innAdmin, string password)
        {
            throw new NotImplementedException();
        }

        public bool RegisterAdmin(AdminViewModel innAdmin)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUserBase()
        {
            return true;
        }

        public bool UpdateUserAsAdmin(UserViewModel user)
        {
            throw new NotImplementedException();
        }

        public bool RequestToken()
        {
            throw new NotImplementedException();
        }
    }
}
