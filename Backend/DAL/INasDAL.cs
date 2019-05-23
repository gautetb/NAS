using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetOpenAuth.OAuth2;
using NAS.Model;

namespace NAS.DAL
{
    /// <summary>
    /// This is the interface for the data access layer of the application. The DAL and the DALstub implements this interface.
    /// This interface makes it possible to take advantage of polymorphism, making it easy for the controllers to invoke the actual DAL for regular operations
    /// or the stub for unit testing.
    /// </summary>
    public interface INasDAL
    {

        UserViewModel GetUser(int Id);

        User GetUserDb(string username);
        bool AddUser(DataCustomer customer, string membership);
        IAuthorizationState GetToken();
        bool CheckUser(Guid userId);
        List<User> GetUserList(string search);
        List<User> GetActiveUserList();
        string RegisterUser(User innUser, string password);
        bool ValidateUser(string username, string password);
        Admin GetAdmin(AdminViewModel admin);
        Admin ValidatedAdmin(string username, string password);
        bool RegisterAdmin(AdminViewModel innAdmin);
        bool UpdatePaymentDetails(User user, bool paymentStatus);
        bool UpdateVismaToken(IAuthorizationState state);
        bool UpdateUserAsAdmin(UserViewModel user);
        bool UpdateUserFromVisma(DataCustomer user, string membership);
    }


}
