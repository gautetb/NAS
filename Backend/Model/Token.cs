using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetOpenAuth.OAuth2;

namespace NAS.Model
{
    /// <summary>
    /// This class is the database model the authorization states from Vismas eAccounting API
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Store for the Id property, which is the primary key for tokens in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Store for the RefreshToken property, which is used to get a new refreshed token from Visma
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Store for the AccessToken property, which is used to get resources from Visma
        /// </summary>
        public string AccessToken { get; set; }

        // <summary>
        /// Store for the AccessTokenIssueDateUtc property, which is when the token was issued from Visma
        /// </summary>
        public DateTime? AccessTokenIssueDateUtc { get; set; }

        // <summary>
        /// Store for the AccessTokenExpirationUtc property, which is when the token expires and is no longer valid
        /// </summary>
        public DateTime? AccessTokenExpirationUtc { get; set; }

    }
}
