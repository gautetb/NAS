using System;
using System.IO;
using System.Net;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using DotNetOpenAuth.OAuth2;
using NAS.Model;

namespace NAS
{
    /// <summary>
    /// This class contains all the methods needed to aquire authorization to Visma's eAccounting API and ask for resources such as: 
    /// <list type="bullet">
    /// <item>
    /// <description>List of members.</description>
    /// </item>
    /// <item>
    /// <description>List of invoices.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para> The data returned from the Visma eAccounting API is from the member base of the specific eAccounting admin user asking for resources.</para>
    /// <para> The info and credentials provided from Visma in order to get access is stored in web.config and accessed using WebConfigurationManager</para>
    /// </remarks>
    public class VismaAuthProvider
    {

        /// <summary>
        /// Creates a new webserver client needed to ask for authorization and resources from Visma. 
        /// </summary>
        /// <returns>
        /// A webserverclient containing all the credentials and info needed to access the Visma eAccounting API
        /// </returns>
        /// Check out <see cref="GetAuthorizationServerDescription()"/> to see how a server description is aquired.
        public static WebServerClient CreateClient()
        {
            // Sets client id provided by Visma gotten from the web.config file
            var clientId = WebConfigurationManager.AppSettings["ClientId"];

            // Sets client secret provided by Visma gotten from the web.config file
            var clientSecret = WebConfigurationManager.AppSettings["ClientSecret"];

            // Gets server description from the method GetAuthorizationServerDescription.
            var serverDesc = GetAuthorizationServerDescription();

            // Creates the actual client
            var client = new WebServerClient(serverDesc, clientId, clientSecret);

            return client;
        }

        /// <summary>
        /// Requests authorization from the server without an existing token. This method  will parse the request url and extract the authorization code returned from Visma after 
        /// entering valid credentials on their login site.
        /// </summary>
        /// <returns>
        /// An access token used to request resources from the Visma eAccounting web API
        /// </returns>
        /// <param name="client">WebServerClient used to ask for access token and resources.</param>
        public static IAuthorizationState RequestAuthorization(WebServerClient client)
        {
            // Processes authorization by parsing codes from the URI gotten in return from Visma and creates an authorization state.
            var state = client.ProcessUserAuthorization();

            return state;
        }

        /// <summary>
        /// Requests a new authorization state using an existing valid state, which contains a refresh token.
        /// </summary>
        /// <returns>
        /// A refreshed access token used to request resources from the Visma eAccounting web API.
        /// </returns>
        /// <param name="client">WebServerClient used to ask for access token and resources.</param>
        public static IAuthorizationState RequestAuthorizationRefresh(IAuthorizationState state)
        {
            // Creates a client in order to ask for a new authorization state
            var client = CreateClient();
            
            // Refreshes the old authorization state with the a refresh token
            client.RefreshAuthorization(state);

            return state;
        }

        /// <summary>
        /// Requests a protecte reource, based on the path provided, from the Visma eAccounting API.
        /// </summary>
        /// <returns>
        /// A protected resource from Visma eAccounting. In this app that means a list of customers or invoices.
        /// </returns>
        /// <param name="client">WebServerClient used to ask for access token and resources.</param>
        /// <param name="state">AuthorizationState containing the access token.</param>
        /// <param name="path">A path that determines what API endpoint resources are requested from.</param>
        public static T GetProtectedResource<T>(WebServerClient client, IAuthorizationState state, string path)
        {
            // Sets the uri to the api server host provided by Visma gotten from the web.config file
            var apiServerHost = WebConfigurationManager.AppSettings["ApiServerHost"];

            // Creates an api request and and sets method and content type
            var request = (HttpWebRequest)WebRequest.Create(new Uri(String.Format("{0}{1}", apiServerHost, path)));
            request.Method = "GET";
            request.ContentType = "application/json";

            // Sends the actual request with the authorization state which contains the tokens
            client.AuthorizeRequest(request, state);

            // Gets the request and stores it in webResponse variable
            var webResponse = request.GetResponse();

            // The response is is read into a string and deserialized 
            var response = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
            var jss = new JavaScriptSerializer();
            var result = jss.Deserialize<T>(response);

            return result;
        }

        /// <summary>
        /// Generates a description of the authorization server endpoint of Visma's eAccounting API with
        /// info provided from Visma and stored in the web.config file.
        /// </summary>
        /// <returns>
        /// An authorization server description with authorization- and token endspoints 
        /// </returns>
        private static AuthorizationServerDescription GetAuthorizationServerDescription()
        {
            // Sets the uri to the authorization server host provided by Visma gotten from the web.config file
            var authServerHost = WebConfigurationManager.AppSettings["AuthServerHost"];

            // Generates strings to the two different endpoints requested to gain access to Visma resources
            var authorizationEndpoint = String.Format(@"{0}/connect/authorize", authServerHost);
            var tokenEndpoint = String.Format(@"{0}/connect/token", authServerHost);

            // Creates the server description with the right protocol, and the two endpoints turned into URIs
            var serverDesc = new AuthorizationServerDescription() {
                ProtocolVersion = ProtocolVersion.V20,
                AuthorizationEndpoint = new Uri(authorizationEndpoint),
                TokenEndpoint = new Uri(tokenEndpoint)
            };

            return serverDesc;
        }
    }
}
