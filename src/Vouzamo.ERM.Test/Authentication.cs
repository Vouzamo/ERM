using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Vouzamo.ERM.Test
{
    [TestClass]
    public class Authentication
    {
        private readonly string AccessKey = "";
        private readonly string SecretKey = "";
        private readonly string Username = "";
        private readonly string Password = "";
        private readonly string NewPassword = "";

        private AmazonCognitoIdentityProviderClient GetCognitoProvider()
        {
            return new AmazonCognitoIdentityProviderClient(new BasicAWSCredentials(AccessKey, SecretKey), new AmazonCognitoIdentityProviderConfig()
            {
                RegionEndpoint = RegionEndpoint.USEast1
            });
        }

        private CognitoUser GetCognitoUser(string username, string userPoolId, string clientId)
        {
            var provider = GetCognitoProvider();
            var userPool = new CognitoUserPool(userPoolId, clientId, provider);

            return new CognitoUser(username, clientId, userPool, provider);
        }

        [TestMethod]
        public async Task SignIn()
        {
            var userPoolId = "us-east-1_6u0RWKWaV";
            var audience = "3gqq1t3c01f55dd02srt13le9l";

            var user = GetCognitoUser(Username, userPoolId, audience);

            var authRequest = new InitiateSrpAuthRequest()
            {
                Password = Password
            };
            var authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);
            while (authResponse.AuthenticationResult == null)
            {
                if (authResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
                {
                    authResponse = await user.RespondToNewPasswordRequiredAsync(new RespondToNewPasswordRequiredRequest()
                    {
                        SessionID = authResponse.SessionID,
                        NewPassword = NewPassword
                    });
                }
            }

            if (authResponse.AuthenticationResult != null)
            {
                // it worked!
            }
        }
    }
}
