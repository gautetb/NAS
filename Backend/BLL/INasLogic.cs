using System.Collections.Generic;
using DotNetOpenAuth.OAuth2;
using NAS.Model;

namespace NAS.BLL
{
    /// <summary>
    /// This is the interface for business logic of the application. The BLL and the BLLstub implements this interface.
    /// This interface makes it possible to take advantage of polymorphism, making it easy for the controllers to invoke the actual BLL for regular operations
    /// or the stub for unit testing.
    /// </summary>
    public interface INasLogic
    {
        UserApiModel GetUser(string username);
        UserViewModel GetUser(int id);
        bool ValidateUser(string username, string password);
        bool UpdateVismaToken(IAuthorizationState state);
        List<User> GetUserList(string search);
        string RegisterUser(UserViewModel innUser);
        Admin GetAdmin(AdminViewModel admin);
        Admin ValidatedAdmin(string innAdmin, string password);
        bool RegisterAdmin(AdminViewModel innAdmin);
        bool UpdateUserBase();
        bool UpdateUserAsAdmin(UserViewModel user);
        bool RequestToken();
    }
}
