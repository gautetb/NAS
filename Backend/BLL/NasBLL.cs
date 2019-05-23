using System;
using System.Collections.Generic;
using System.Linq;
using NAS.Model;
using NAS.DAL;
using System.Web.Configuration;
using DotNetOpenAuth.OAuth2;

namespace NAS.BLL
{
    /// <summary>
    /// This is the business logic layer of the application. It contains all the evaluation of the information received from Visma as well as sending requests to the data access layer.
    /// </summary>
    /// <remarks>
    /// <para>This class handles all the data requests sent to both Vismas servers as well as the database. It also updates the user base from the membership base in Visma eAccounting</para>
    /// <para>The class can also determine the payment status and type of membership of the members obtained from Visma eAccounting </para>
    /// </remarks>
    public class NasBLL : INasLogic
    {
        private INasDAL _db;

        /// <summary>
        /// This constructor creates and instance of the class that uses the actual data access layer.
        /// </summary>
        public NasBLL()
        {
            _db = new NasDAL();
        }

        /// <summary>
        /// This constructor creates and instance of the class that uses a stub of the data access layer for testing.
        /// </summary>
        /// <param name="stub">A mock of the data access layer for testing.</param>
        public NasBLL(INasDAL stub)
        {
            _db = stub;
        }

        /// <summary>
        /// Sends a request to the DAL to update a user from the admin site and returns a boolean determining if it was successful or not.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the update was successful.
        /// </returns>
        /// <param name="user">User to be updated in the DAL</param>
        public bool UpdateUserAsAdmin(UserViewModel user)
        {
            return _db.UpdateUserAsAdmin(user);
        }

        /// <summary>
        /// Sends a request to the DAL to get a user based on the users id in the database.
        /// </summary>
        /// <returns>
        /// A valid user as UserViewModel if it is successful or a user with an error message if its unsuccessful.
        /// </returns>
        /// <param name="id">The users id in the database.</param>
        public UserViewModel GetUser(int id)
        {
            return _db.GetUser(id);
        }

        /// <summary>
        /// Sends a request to the DAL to validate a user in the database based on the <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <returns>
        /// <c>True</c> if user exits and the password matches.
        /// </returns>
        /// <param name="username">The users username in the database.</param>
        /// <param name="password">The users password in the database.</param>
        public bool ValidateUser(string username, string password)
        {
            return _db.ValidateUser(username, password);
        }

        /// <summary>
        /// Sends a request to the DAL to update an access token received from the Visma eAccounting API.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the update was successful.
        /// </returns>
        /// <param name="state">Access token from the Visma eAccounting API.</param>
        public bool UpdateVismaToken(IAuthorizationState state)
        {
            return _db.UpdateVismaToken(state);
        }

        /// <summary>
        /// Sends a request to the DAL to get a list of users with names that contain the search string.
        /// </summary>
        /// <returns>
        /// User list with users with names that contains the search string. Returns all users if the string is null. 
        /// </returns>
        /// <param name="search">Search string to match with the names of users.</param>
        public List<User> GetUserList(string search)
        {
            return _db.GetUserList(search);
        }

        /// <summary>
        /// Sends a request to the DAL to get an administrator from the database.
        /// </summary>
        /// <returns>
        /// A valid administrator if it is successful.
        /// </returns>
        /// <param name="admin">Administrator view model from the admin site.</param>
        public Admin GetAdmin(AdminViewModel admin)
        {
            return _db.GetAdmin(admin);
        }

        /// <summary>
        /// Sends a request to the DAL to validate an administrator in the database based on the username <paramref name="username"/> and password <paramref name="password"/>.
        /// </summary>
        /// <returns>
        /// <c>True</c> if user exits and the password matches.
        /// </returns>
        /// <param name="username">The administrators username in the database.</param>
        /// <param name="password">The administrators password in the database.</param>
        public Admin ValidatedAdmin(string username, string password)
        {
            return _db.ValidatedAdmin(username, password);
        }

        /// <summary>
        /// Sends a request to the DAL to register a new administrator in the database.
        /// </summary>
        /// <returns>
        /// <c>True</c> if it is successful
        /// </returns>
        /// <param name="innAdmin">A new administrator from the registration form on the admin site.</param>
        public bool RegisterAdmin(AdminViewModel innAdmin)
        {
            return _db.RegisterAdmin(innAdmin);
        }

        /// <summary>
        /// Gets a user for the web api based on the username provided. This is after determining if the user has valid payment status and updating the user in the database.
        /// </summary>
        /// <returns>
        /// User in the form of web api model
        /// </returns>
        /// <param name="username">Username of the user trying to access their info in the database.</param>
        public UserApiModel GetUser(string username)
        {
            try
            {
                // Gets a user from the DAL
                var user = _db.GetUserDb(username);

                // Sends a request for a list of invoices from Visma
                var invoiceList = GetInvoiceListFromVisma();

                // Determines the payment status of the user trying to access their information from the database
                var paymentStatus = DeterminePaymentStatus(user, invoiceList);

                // Alters the user from database model to the web api model
                var outUser = new UserApiModel()
                {
                    Username = user.Username,
                    Name = user.Name,
                    ValidPayment = user.ValidPayment,
                    Membership = user.Membership
                };

                // Updates the payment status of the user in the database
                _db.UpdatePaymentDetails(user, paymentStatus);

                return outUser;
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasBLL, UserApiModel GetUser(string username)");

                return null;
            }
        }

        /// <summary>
        /// Sends a request to the DAL to register a password to an existing member from the mobile application. This is after determining if the member has a user in the database as well as valid payment.
        /// </summary>
        /// <returns>
        /// <c>string</c> containing "Success" if successful or an error message describing what went wrong.
        /// </returns>
        /// <para>Users are added from Vismas servers so a user needs to be a member before registering in the mobile app. The user is updated in the database after registration. </para>
        /// <param name="innUser">A user with username and password to be registered.</param>
        public string RegisterUser(UserViewModel innUser)
        {
            try
            {
                // Gets the user from the database
                var user = _db.GetUserDb(innUser.Username);

                // Returns an error message if the user does not have a registered membership in the database and therefore Visma eAccounting
                if (user == null) return "Det er ingen aktive medlemmer med den eposten";

                // If a user is already an active user the registration is not valid and an error message is returned
                if (user.ActiveUser) return "En bruker er allerede registrert med den eposten";

                // Sends a request for a list of invoices from Visma
                var invoiceList = GetInvoiceListFromVisma();

                // Determines the payment status of the user from the database
                var paymentStatus = DeterminePaymentStatus(user, invoiceList);

                // Sets the new payment status for the user before registration.
                user.ValidPayment = paymentStatus;

                // Registers the user
                return _db.RegisterUser(user, innUser.Password);
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasBLL, string RegisterUser(UserViewModel innUser)");

                return "Det har oppstått en feil, vennligst prøv igjen senere";
            } 
        }

        /// <summary>
        /// A method that invokes the methods to add and update users from the member list in Visma eAccounting.
        /// </summary>
        /// <returns>
        /// <c>True</c> if both methods are invokes successfully
        /// </returns>
        /// <para>The method checks if there is a token in the database and redirects the administrator the Vismas authentication site if a new token is needed. </para>
        public bool UpdateUserBase()
        {
            var dbToken = _db.GetToken();

            if (dbToken == null) RequestToken();

            return AddUsersFromVisma() && UpdateActiveUsersFromVisma();
        }

        /// <summary>
        /// Requests an updated member list from the Visma eAccounting API, and uses it to update current users or add new users.
        /// </summary>
        /// <returns>
        /// <c>True</c> if it is successful
        /// </returns>
        private bool AddUsersFromVisma()
        {
            try
            {
                // Sends a request for a list of customers from Visma
                var customerList = GetCustomerListFromVisma();

                // Iterates through the list of customers from Visma. The user is added if it is new and updated if it already exists
                foreach (var customer in customerList.Data.ToList())
                {

                    var userExist = _db.CheckUser(customer.Id);

                    // Determines what kind of membership the user has
                    var membership = DetermineMembership(customer);

                    if (userExist)
                    {
                        _db.UpdateUserFromVisma(customer, membership);
                    }
                    else
                    {
                        _db.AddUser(customer, membership);
                    }
                }

                return true;

            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasBLL, bool AddUsersFromVisma()");
                return false;
            }
        }

        /// <summary>
        /// Requests an updated invoice list from the Visma eAccounting API, and uses it to update the users marked as active in the database.
        /// </summary>
        /// <returns>
        /// <c>True</c> if it is successful
        /// </returns>
        /// <para> A user is marked as active if it has been registered in the mobile app or the admin site.</para>
        private bool UpdateActiveUsersFromVisma()
        {
            try
            {
                // Gets a list of active users from the database
                var userList = _db.GetActiveUserList();

                // Sends a request for a list of invoices from Visma
                var invoiceList = GetInvoiceListFromVisma();

                // Iterates through the list of active users and updates their payment status
                foreach (var user in userList)
                {
                    var paymentStatus = DeterminePaymentStatus(user, invoiceList);
                    _db.UpdatePaymentDetails(user, paymentStatus);
                }

                return true;

            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasBLL, bool UpdateActiveUsersFromVisma()");
                return false;
            }
        }

        /// <summary>
        /// Determines the payment status of the user from an updated list of invoices from Visma using the the userId and the type of membership the user has.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the user has valid payment status based on information from Visma
        /// </returns>
        /// <param name="user">The user to be evaluated by the method</param>
        /// <param name="invoiceList">An updated list of invoices from Visma.</param>
        private bool DeterminePaymentStatus(User user, InvoiceResponse invoiceList)
        {
            try
            {
                // Returns true if the membership is marked as livsvarig
                if (user.Membership == "Livsvarig") return true;
                
                // Finds an invoice from the invoice list with matching userId from the parameters
                var userInvoice = invoiceList.Data.ToList().Find(x => x.CustomerId.ToString() == user.UserId);

                // If the user has been issued an invoice this year and the remaining amount is 0 or less the user has valid payment
                if (userInvoice.DueDate.Year == DateTime.Now.Year && userInvoice.RemainingAmount <= 0)  return true;

                return false;

            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasBLL, bool DeterminePaymentStatus(User user, InvoiceResponse invoiceList)");
            }

            return false;
        }

        /// <summary>
        /// Determines the type of membership the user has from customer details from Visma.
        /// </summary>
        /// <returns>
        /// A <c>string</c> containing the type of membership the user has.
        /// </returns>
        /// <para> The method is used to turn 11 types of membership from Visma into 4 types to be displayed in the mobile app.</para>
        /// <param name="customer">A user gotten from the Visma eAccounting API.</param>
        public string DetermineMembership(DataCustomer customer)
        {
            string membership = "Ikke aktivt medlemskap";

            try
            {
                // Iterates through the list of customer labes from a member in Visma and determines what membership the user has.
                foreach (var label in customer.CustomerLabels)
                {

                    switch (label.Name)
                    {
                        case "Enkeltmedlemskap":
                            membership = "Enkeltmedlemskap";
                            break;
                        case "vikingredaksjonen":
                            membership = "Enkeltmedlemskap";
                            break;
                        case "sekretær enkeltmedlem":
                            membership = "Enkeltmedlemskap";
                            break;
                        case "enkeltmedlem uten epost":
                            membership = "Enkeltmedlemskap";
                            break;
                        case "utland enkeltmedlem":
                            membership = "Enkeltmedlemskap";
                            break;
                        case "støttemedlem":
                            membership = "Enkeltmedlemskap";
                            break;
                        case "Familiemedlemskap":
                            membership = "Familiemedlemskap";
                            break;
                        case "Familiemedlemskap 2":
                            membership = "Familiemedlemskap";
                            break;
                        case "Studentmedlemskap":
                            membership = "Studentmedlemskap";
                            break;
                        case "Studentrepresentant":
                            membership = "Studentmedlemskap";
                            break;
                        case "Livsvarig":
                            membership = "Livsvarig";
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasBLL, string DetermineMembership(DataCustomer customer)");
            }

            return membership;
        }

        /// <summary>
        /// Redirects an administrator from the admin site to Vismas authentication server to get an access token.
        /// </summary>
        /// <returns>
        /// <c>True</c> if it is successful.
        /// </returns>
        /// <param name="customer">A user gotten from the Visma eAccounting API.</param>
        public bool RequestToken()
        {
            try
            {
                // Creates a list of scopes needed to access the right token
                var scopes = new List<string> { "offline_access", "ea:api", "ea:sales" };

                // Gets the redirect URI in this application from the web.config file
                var redirectUri = new Uri(WebConfigurationManager.AppSettings["RedirectUri"]);

                // Creates a new WebServerClient
                var client = VismaAuthProvider.CreateClient();

                // Requests a brand new access token from Visma
                client.RequestUserAuthorization(scopes, redirectUri);

                return true;
            }
            catch (Exception e)
            {
                // Creates a new error filer object
                var errorLog = new ErrorFiler();

                // Logs the error to the error log, with the name of the exception and where it occured
                errorLog.WriteError(e.GetType().FullName, "NasBLL, bool AskForToken()");

                return false;
            }
        }

        /// <summary>
        /// Requests a list of invoices from Visma, using a valid token.
        /// </summary>
        /// <returns>
        /// List of invoices
        /// </returns>
        public InvoiceResponse GetInvoiceListFromVisma()
        {
            // Creates a new WebServerClient
            var client = VismaAuthProvider.CreateClient();

            // Gets the access token stored in the database
            var dbToken = _db.GetToken();

            // Sends a request for a list of invoices to the Visma eAccounting web api and stores it in the invoiceList variable
            var invoiceList = VismaAuthProvider.GetProtectedResource<InvoiceResponse>(client, dbToken, "/v2/customerInvoices");

            // Requests a refreshed token from Vismas servers using the refreshtoken from the last token
            var refreshedToken = VismaAuthProvider.RequestAuthorizationRefresh(dbToken);

            // Updates the token in the database
            _db.UpdateVismaToken(refreshedToken);

            return invoiceList;
        }

        /// <summary>
        /// Requests a list of customers from Visma, using a valid token.
        /// </summary>
        /// <returns>
        /// List of invoices
        /// </returns>
        public CustomerResponse GetCustomerListFromVisma()
        {
            // Creates a new WebServerClient
            var client = VismaAuthProvider.CreateClient();

            // Gets the access token stored in the database
            var dbToken = _db.GetToken();

            // Sends a request for a list of customers to the Visma eAccounting web api and stores it in the customerList variable
            var customerList = VismaAuthProvider.GetProtectedResource<CustomerResponse>(client, dbToken, "/v2/customers");

            // Requests a refreshed token from Vismas servers using the refreshtoken from the last token
            var refreshedToken = VismaAuthProvider.RequestAuthorizationRefresh(dbToken);

            // Updates the token in the database
            _db.UpdateVismaToken(refreshedToken);

            return customerList;
        }
    }
}
