using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoturBeniIstedigimYere
{
    public class Authorizer
    {
        public static MvcAuthorizer TwitterAuthorize = new MvcAuthorizer
        {
            CredentialStore = new SingleUserInMemoryCredentialStore
            {
                ConsumerKey = ConfigurationManager.AppSettings["consumerKey"],
                ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"],
                AccessToken = ConfigurationManager.AppSettings["accessToken"],
                AccessTokenSecret = ConfigurationManager.AppSettings["accessTokenSecret"]
            }
        };
    }
}
