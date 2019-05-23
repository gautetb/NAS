using Microsoft.Owin.Security.OAuth;
using NAS.BLL;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NAS
{
    /// <summary>
    /// This class handles the authentication and authorization to the web api, by giving access tokens to the users if they are validated in the database
    /// </summary>
    public class WebApiAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private INasLogic _db = new NasBLL();

        /// <summary>
        /// This method validates the user asking for resources from the closed methods in the UserController based on the token they have.
        /// </summary>
        /// <returns>
        /// Task to enable asynchronous execution.
        /// </returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); 
        }

        /// <summary>
        /// This method validates the user asking for a token from the authorization endpoint, by checking the Username and Password fields of the context provided.
        /// When the user is validated the username is added to the token in order to determine which user is asking for a protected resource in the UserController.
        /// </summary>
        /// <returns>
        /// Task to enable asynchronous execution.
        /// </returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            var IsValidatedUser = _db.ValidateUser(context.UserName, context.Password);

            if (IsValidatedUser)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid grant", "provided username and password is incorrect");
            }
        }
    }
}