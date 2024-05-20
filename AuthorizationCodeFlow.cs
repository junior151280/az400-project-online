using Microsoft.Identity.Client;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

namespace Az400_ProjectOnline
{
    public class AuthorizationCodeFlow
    {
        private static readonly string clientId = ConfigurationManager.AppSettings["ClientId"];
        private static readonly string tenantId = ConfigurationManager.AppSettings["TenantId"];
        private static readonly string clientSecret = ConfigurationManager.AppSettings["ClientSecret"];
        public static readonly string redirectUri = ConfigurationManager.AppSettings["RedirectUri"];

        public static async Task<AuthenticationResult> AuthenticateUser(IPublicClientApplication app)
        {
            try
            {
                var scopes = new[] { "https://mngenvmcap608089.sharepoint.com/.default" };
                var accounts = await app.GetAccountsAsync();
                var firstAccount = accounts.FirstOrDefault();

                return await app.AcquireTokenInteractive(scopes)
                                .WithAccount(firstAccount)
                                .ExecuteAsync();
            }
            catch (MsalException ex)
            {
                Console.WriteLine($"Error acquiring token: {ex.Message}");
                return null;
            }
        }
        public static async Task<AuthenticationResult> AuthenticateClientUser(IConfidentialClientApplication app)
        {
            try
            {
                var scopes = new[] { "https://mngenvmcap608089.sharepoint.com/.default" };
                var accounts = await app.GetAccountsAsync();
                var firstAccount = accounts.FirstOrDefault();

                return await app.AcquireTokenForClient(scopes)
                                .ExecuteAsync();
            }
            catch (MsalException ex)
            {
                Console.WriteLine($"Error acquiring token: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return null;
            }
        }

        // The following method is used to get the access token.
        public static async Task<string> GetAccessToken()
        {
            var scope = "https://graph.microsoft.com/.default";
            var url = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var content = new FormUrlEncodedContent([
                   new KeyValuePair<string, string>("grant_type", "client_credentials"),
               new KeyValuePair<string, string>("client_id", clientId),
               new KeyValuePair<string, string>("client_secret", clientSecret),
               new KeyValuePair<string, string>("scope", scope),
            ]);
            request.Content = content;
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(responseBody);
                return tokenResponse.access_token;
            }
            else
            {
                throw new Exception($"Request failed with status code: {response.StatusCode}");
            }
        }
    }
}
