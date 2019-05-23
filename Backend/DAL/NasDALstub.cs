using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using DotNetOpenAuth.OAuth2;
using NAS.Model;

namespace NAS.DAL
{
    /// <summary>
    /// This is the data access stub. It implements some of the methods needed to test the presentation layer. These implementations produce predictable results
    /// to test different outcomes in the controllers.
    /// </summary>
    public class NasDALstub : INasDAL
    {
        public UserViewModel GetUser(int id)
        {
            if (id == 1)
            {
                var returnUser = new UserViewModel()
                {
                    Name = "Name",
                    Username = "Username",
                    ValidPayment = true,
                    Membership = "Membership"
                };

                return returnUser;
            }


            var nullUser = new UserViewModel()
            {
                Name = "Error"
            };

            return nullUser;

        }

        public User GetUserDb(string username)
        {
            var successUser = new User()
            {
                Username = "testuser"
                
            };

            var userTaken = new User()
            {
                ActiveUser = true
            };

            var dbFailUser = new User()
            {
                Username = "dbFail"
            };

            if (username == "dbFail") return dbFailUser;

            if (username == "testuser") return successUser;

            if (username == "userTaken") return userTaken;

            return null;
        }

        public bool AddUser(DataCustomer customer, string membership)
        {
            throw new NotImplementedException();
        }

        public IAuthorizationState GetToken()
        {
            throw new NotImplementedException();
        }

        public bool CheckUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public List<User> GetUserList(string search)
        {
            var userList = new List<User>();

            var user1 = new User()
            {
                Name = "Chris",
                Username = "chris@mail.com",
                ValidPayment = true,
                Membership = "Enkeltmedlemskap"
            };

            var user2 = new User()
            {
                Name = "Sara",
                Username = "sara@mail.com",
                ValidPayment = false,
                Membership = "Livsvarig"
            };

            var user3 = new User()
            {
                Name = "Paul",
                Username = "paul@mail.com",
                ValidPayment = true,
                Membership = "Studentmedlemskap"
            };

            if (search == "Chris")
            {
                userList.Add(user1);
            }

            if (String.IsNullOrEmpty(search))
            {
                userList.Add(user1);
                userList.Add(user2);
                userList.Add(user3);
            }

            return userList;

        }

        public List<User> GetActiveUserList()
        {
            throw new NotImplementedException();
        }

        public string RegisterUser(User innUser, string password)
        {
            if (innUser.Username == "testuser") return "Success";

            return "Det har oppstått en feil, vennligst prøv igjen senere";
        }

        public bool ValidateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Admin GetAdmin(AdminViewModel admin)
        {
            throw new NotImplementedException();
        }

        public Admin ValidatedAdmin(string username, string password)
        {
            var admin = new Admin()
            {
                Username = "Valid",
                Name = "Valid admin"

            };

            if (username == "username" && password == "password")
            {
                return admin;
            }

            return null;
        }

        public bool RegisterAdmin(AdminViewModel innAdmin)
        {
            if (innAdmin.Username == "DbFail")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool UpdatePaymentDetails(User user, bool paymentStatus)
        {
            throw new NotImplementedException();
        }

        public bool UpdateVismaToken(IAuthorizationState state)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUserAsAdmin(UserViewModel user)
        {
            if (user.Name == "NewUser")
            {
                return true;
            }

            return false;
        }

        public bool UpdateUserFromVisma(DataCustomer user, string membership)
        {
            throw new NotImplementedException();
        }
    }
}
