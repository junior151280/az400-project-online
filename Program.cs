using System.Net.Http.Headers;
using Az400_ProjectOnline;
using System.Text;
using Microsoft.Identity.Client;
using static System.Formats.Asn1.AsnWriter;
using System.Configuration;
class Program
{
    private static readonly string tenantId = ConfigurationManager.AppSettings["TenantId"];
    private static readonly string clientId = ConfigurationManager.AppSettings["ClientId"]; 
    private static readonly string clientSecret = ConfigurationManager.AppSettings["ClientSecret"];
    private static readonly string siteUrl = $"https://{tenantId}.sharepoint.com/sites/pwa";
    private static readonly string projectId = ConfigurationManager.AppSettings["ProjectId"];
    private static readonly string OAuth = "Client"; // "Client", "User" or ""

    static async Task Main()
    {
        var accessToken = "";

        if (OAuth == "Client") {
            IConfidentialClientApplication appClient = ConfidentialClientApplicationBuilder.Create(clientId)
            .WithClientSecret(clientSecret)
            .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
            .WithRedirectUri(AuthorizationCodeFlow.redirectUri)
            .Build();

            var authResult = await AuthorizationCodeFlow.AuthenticateClientUser(appClient);

            if (authResult != null)
            {
                accessToken = authResult.AccessToken;
            }
        }
        else if (OAuth == "User") {
            var pcaOptions = new PublicClientApplicationOptions
            {
                ClientId = clientId,
                TenantId = tenantId,
                RedirectUri = AuthorizationCodeFlow.redirectUri
            };

            var appUser = PublicClientApplicationBuilder
                       .CreateWithApplicationOptions(pcaOptions)
                       .Build();

            var authResult = await AuthorizationCodeFlow.AuthenticateUser(appUser);

            if (authResult != null)
            {
                accessToken = authResult.AccessToken;
            }
        }
        else
        {
            accessToken = await AuthorizationCodeFlow.GetAccessToken();
        }

        if (!string.IsNullOrEmpty(accessToken))
        {
            Console.WriteLine("Access token:\n" + accessToken);

            var projectDetails = await GetProjectDetails(accessToken);
            Console.WriteLine("Project Details:\n" + projectDetails);

            var updatedProjectDetails = await UpdateProjectDatails(projectId, accessToken);
            Console.WriteLine("Updated Project Details:\n" + updatedProjectDetails);
        }
        else
        {
            Console.WriteLine("Access token is empty.");
        }
    }

    // The following method is used to get the project details.
    public static async Task<string> GetProjectDetails(string accessToken)
    {
        var url = $"{siteUrl}/_api/ProjectServer/Projects('{projectId}')";
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        else
        {
            throw new Exception($"Request failed with status code: {response.StatusCode}");
        }
    }

    // The following method is used to update the project details.
    public static async Task<string> UpdateProjectDatails(string projectId, string accessToken)
    {
        var url = $"{siteUrl}/_api/ProjectServer/Projects('{projectId}')";
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var updateData = new
        {
            Name = "Novo Nome do Projeto",
            Description = "Descrição atualizada do projeto"
        };
        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");
        var response = await client.PatchAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        else
        {
            throw new Exception($"Request failed with status code: {response.StatusCode}");
        }
    }
}