using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DotNetOpenAuth.OAuth2;
using NAS.Model;

namespace NAS.DAL
{
    /// <summary>
    /// This is the data access layer of the application. It contains all the methods to add, get and update information in the application database.
    /// </summary>
    public class NasDAL : INasDAL, IDisposable
    {
        private DBContext _db = new DBContext();

        /// <summary>
        /// Turns a view model user into database model user and adds in to the user table in the database.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the adding was successful.
        /// </returns>
        /// <param name="customer">User from Visma to be added to the database.</param>
        /// <param name="membership">Type of membership the customer has.</param>
        public bool AddUser(DataCustomer customer, string membership)
        {
            try
            {
                var newUser = new User()
                {
                    UserId = customer.Id.ToString(),
                    Name = customer.Name,
                    Username = customer.EmailAddress,
                    Membership = membership
                };

                _db.UserTable.Add(newUser);
                _db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasDAL, bool AddUser(DataCustomer customer, string membership)");
                return false;
            }

        }

        /// <summary>
        /// Checks if a user exists in the database based on the userId from Visma.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the user exists.
        /// </returns>
        /// <param name="userId">UserId from Visma</param>
        public bool CheckUser(Guid userId)
        {
            return _db.UserTable.Any(user => user.UserId == userId.ToString());
        }

        /// <summary>
        /// Updates an existing user with info from a NAS member from Visma.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the update was successful.
        /// </returns>
        /// <param name="innUser">NAS member from Visma</param>
        /// <param name="membership">Type of membership determined in the BLL</param>
        public bool UpdateUserFromVisma(DataCustomer innUser, string membership)
        {
            try
            {
                // Gets the user to be edited from the user table
                var editUser = _db.UserTable.FirstOrDefault(d => d.UserId == innUser.Id.ToString());

                // Sets the fields from the Customer to user to be updated in the user table.
                editUser.Username = innUser.EmailAddress;
                editUser.Name = innUser.Name;
                editUser.Membership = membership;

                _db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasDAL, bool UpdateUserFromVisma(DataCustomer innUser, string membership)");

                return false;
            }
        }

        /// <summary>
        /// Gets a list of users from the database, based on the search string provided
        /// </summary>
        /// <returns>
        /// List of users.
        /// </returns>
        /// <param name="search">Searchstring from the searchfield on the admin site</param>
        public List<User> GetUserList(string search)
        {
            if (search == null)
            {
                return _db.UserTable.ToList();
            }

            return _db.UserTable.Where(x => x.Name.Contains(search)).ToList();
        }

        /// <summary>
        /// Gets a list of users from the database that are marked as active in the user table.
        /// </summary>
        /// <returns>
        /// List of active users.
        /// </returns>
        public List<User> GetActiveUserList()
        {
            return _db.UserTable.Where(x => x.ActiveUser).ToList();
        }

        /// <summary>
        /// Adds a password to a user in the usertable and sets the user as active if it is a new registration.
        /// </summary>
        /// <returns>
        /// A <c>string</c> containing "Success" if successful or an error message describing what went wrong.
        /// </returns>
        /// <param name="innUser">User to be updated in the database</param>
        /// <param name="password">Password for the user</param>
        public string RegisterUser(User innUser, string password)
        {
            try
            {

                // Creates salt for the password
                var salt = CreateSalt();

                // Hashes the given password string with the salt created
                var hash = CreateHash(password, salt);

                // Sets the fields of the user before saving changes in the database
                innUser.Password = hash;
                innUser.Salt = salt;
                innUser.ActiveUser = true;

                _db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasDAL, string RegisterUser(User innUser, string password)");

                return "Det har oppstått en feil, vennligst prøv igjen senere";
            }      
        }

        /// <summary>
        /// Validates a user in the database based on the username given and if the password matches.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the user is validated.
        /// </returns>
        /// <param name="username">User to be validated</param>
        /// <param name="password">Password for the user</param>
        public bool ValidateUser(string username, string password)
        {
            try
            {   
                // Gets the user based on the username
                var user = _db.UserTable.FirstOrDefault(d => d.Username == username);

                // Creates hash from the password given as parameter and salt registered in the database
                var testPassword = CreateHash(password, user.Salt);

                // Checks if created hash matches the hash in the database and returns true if it matches
                if (user.Password.SequenceEqual(testPassword)) return true;
                
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasDAL, bool ValidateUser(string username, string password)");

            }

            return false;
        }

        /// <summary>
        /// Validates an admin from the database if the username given exists and the password matches.
        /// </summary>
        /// <returns>
        /// A validated Admin if validation is successful, else null
        /// </returns>
        /// <param name="username">Admin to be validated</param>
        /// <param name="password">Password for the user</param>
        public Admin ValidatedAdmin(string username, string password)
        {
            try
            {
                // Gets the admin based on the username
                var user = _db.AdminTable.FirstOrDefault(d => d.Username == username);

                // Creates hash from the password given as parameter and salt registered in the database
                var testPassword = CreateHash(password, user.Salt);

                // Checks if created hash matches the hash in the database and returns true if it matches
                if (user.Password.SequenceEqual(testPassword)) return user;

            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasDAL, bool ValidateAdmin(string username, string password)");

            }

            return null;
        }

        /// <summary>
        /// Adds a new admin to the database.
        /// </summary>
        /// <returns>
        /// <c>True</c> if it is successful.
        /// </returns>
        /// <param name="innAdmin">Admin to be added to the database</param>
        public bool RegisterAdmin(AdminViewModel innAdmin)
        {
            try
            {
                // Turns the view model admin into a database model admin
                var newAdmin = new Admin()
                {
                    Name = innAdmin.Name,
                    Username = innAdmin.Username,
                };

                // Creates salt for the password
                var salt = CreateSalt();

                // Hashes the given password string with the salt created
                var hash = CreateHash(innAdmin.Password, salt);

                // Sets the fields of the admin before adding the admin and saving changes in the database
                newAdmin.Password = hash;
                newAdmin.Salt = salt;

                _db.AdminTable.Add(newAdmin);
                _db.SaveChanges();


                return true;
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasDAL, bool RegisterAdmin(AdminViewModel innAdmin)");

                return false;
            }
        }

        /// <summary>
        /// Gets an admin from the admin table based on the username given
        /// </summary>
        /// <returns>
        /// Admin from the database
        /// </returns>
        /// <param name="innAdmin">Admin from view</param>
        public Admin GetAdmin(AdminViewModel innAdmin)
        {
            return _db.AdminTable.FirstOrDefault(d => d.Username == innAdmin.Username);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        /// <summary>
        /// Gets the token from the token table.
        /// </summary>
        /// <returns>
        /// Access token for Visma eAccounting API
        /// </returns>
        public IAuthorizationState GetToken()
        {
            try
            {
                // Gets the token from the token table
                var dbToken = _db.TokenTable.FirstOrDefault();

                // Turns the token into an Authorization state used to access the Visma eAccounting API
                IAuthorizationState setToken = new AuthorizationState() {

                    AccessToken = dbToken.AccessToken,
                    AccessTokenExpirationUtc = dbToken.AccessTokenExpirationUtc,
                    AccessTokenIssueDateUtc = dbToken.AccessTokenIssueDateUtc,
                    RefreshToken = dbToken.RefreshToken

                };

                return setToken;
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasDAL, IAuthorizationState GetToken()");

                return null;
            }
        }

        /// <summary>
        /// Gets a user from the user table based on the id in the database. The user from the database is turned into a UserViewModel to display in the views.
        /// </summary>
        /// <returns>
        /// User as a UserViewModel
        /// </returns>
        /// <param name="id">Users id in the database</param>
        public UserViewModel GetUser(int id)
        {
            try
            {
                // Gets user from database
                var innUser = _db.UserTable.Find(id);

                // Turns it into a UserViewModel fit for display in views
                var outUser = new UserViewModel()
                {
                    Username = innUser.Username,
                    Name = innUser.Name,
                    ValidPayment = innUser.ValidPayment,
                    Membership = innUser.Membership
                };

                return outUser;
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasDAL, UserViewModel GetUser(int id)");
            }

            // Returns a user with an errormessage
            var errorUser = new UserViewModel()
            {
                Response = "Ingen bruker funnet"
            };

            return errorUser;
        }

        /// <summary>
        /// Gets a user from the user table based on the username.
        /// </summary>
        /// <returns>
        /// User as a database model
        /// </returns>
        /// <param name="username">username</param>
        public User GetUserDb(string username)
        {
            return _db.UserTable.FirstOrDefault(x => x.Username == username);
        }

        /// <summary>
        /// Turns an authorization state from Visma into a token and adds it to the database.
        /// </summary>
        /// <returns>
        /// <c>True</c> if it is successful
        /// </returns>
        /// <param name="state">Access token from Visma</param>
        public bool SaveToken(IAuthorizationState state)
        {
            try
            {
                var dbToken = new Token()
                {
                    RefreshToken = state.RefreshToken,
                    AccessToken = state.AccessToken,
                    AccessTokenIssueDateUtc = state.AccessTokenIssueDateUtc,
                    AccessTokenExpirationUtc = state.AccessTokenExpirationUtc
                };

                _db.TokenTable.Add(dbToken);

                _db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasDAL, bool SaveToken(IAuthorizationState state)");

                return false;
            }
        }

        /// <summary>
        /// Updates the token in the database if it exists or adds a new one if it the database is empty.
        /// </summary>
        /// <returns>
        /// <c>True</c> if it is successful
        /// </returns>
        /// <param name="state">Access token from Visma </param>
        public bool UpdateVismaToken(IAuthorizationState state)
        {
            try
            {
                // Adds the token if the database is empty
                if (!_db.TokenTable.Any()) 
                {
                    SaveToken(state);

                    return true;
                }

                // Gets the token in the database
                var dbToken = _db.TokenTable.FirstOrDefault();

                // Updates the fields and saves the changes in the database
                dbToken.AccessToken = state.AccessToken;
                dbToken.AccessTokenExpirationUtc = state.AccessTokenExpirationUtc;
                dbToken.AccessTokenIssueDateUtc = state.AccessTokenIssueDateUtc;
                dbToken.RefreshToken = state.RefreshToken;

                _db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasDAL, bool UpdateVismaToken(IAuthorizationState state)");

                return false;
            }
        }

        /// <summary>
        /// Updates the user from the admin site. This method also sets if the payment status of the user should be determined
        /// automatically or manually with the <c>string</c> field AutoUpdatePayment.
        /// </summary>
        /// <returns>
        /// <c>True</c> if it is successful
        /// </returns>
        /// <param name="user">User from view</param>
        public bool UpdateUserAsAdmin(UserViewModel user)
        {
            try
            {
                // Gets user from the database
                var editUser = GetUserDb(user.Username);

                // Checks if the payment status should be automatic or not. If not the payment status is set based on AutoUpdatePayment
                if (user.AutoUpdatePayment == "automatic")
                {
                    editUser.ManualValidPayment = false;
                }
                else 
                {
                    if (user.AutoUpdatePayment == "true")
                    {
                        editUser.ValidPayment = true;
                        editUser.ManualValidPayment = true;
                    }
                    else if (user.AutoUpdatePayment == "false")
                    {
                        editUser.ValidPayment = false;
                        editUser.ManualValidPayment = true;
                    }
                }

                // If the password is not null or empty it is used to update the user and set the users status to active
                if (!string.IsNullOrEmpty(user.Password))
                {
                    // Creates salt for the password
                    var salt = CreateSalt();

                    // Hashes the given password string with the salt created
                    var hash = CreateHash(user.Password, salt);

                    // Sets the fields of the user to be updated
                    editUser.Password = hash;
                    editUser.Salt = salt;
                    editUser.ActiveUser = true;
                }

                _db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasDAL, bool UpdateUserAsAdmin(UserViewModel user)");

                return false;
            }
        }

        /// <summary>
        /// Updates the payment details of the user determined in the BLL, if the payment details are set to be updated automatically.
        /// </summary>
        /// <returns>
        /// <c>True</c> if it is successful
        /// </returns>
        /// <param name="user">User from view</param>
        /// <param name="paymentStatus">Users paymentstatus determined in the BLL</param>
        public bool UpdatePaymentDetails(User user, bool paymentStatus)
        {
            try
            {
                var editUser = _db.UserTable.Find(user.Id);

                if (user.ManualValidPayment == false)
                {
                    editUser.ValidPayment = paymentStatus;
                }

                _db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasDAL, bool UpdatePaymentDetails(User user, bool paymentStatus)");

                return false;
            }
        }

        /// <summary>
        /// Creates a hash from the password and salt passed as arguments
        /// </summary>
        /// <returns>
        /// Hashed password.
        /// </returns>
        /// <para>This code from the webapps class in OsloMet</para>
        /// <param name="password">Password to be hashed</param>
        /// <param name="salt">Salt for the password</param>
        public static byte[] CreateHash(string password, byte[] salt)
        {
            const int keyLength = 24;
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            return pbkdf2.GetBytes(keyLength);
        }

        /// <summary>
        /// Creates salt for hashing passwords before adding them to the database-
        /// </summary>
        /// <returns>
        /// Salt
        /// </returns>
        /// <para>This code from the webapps class in OsloMet</para>
        public static byte[] CreateSalt()
        {
            var csprng = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csprng.GetBytes(salt);
            return salt;
        }
    }
    }

