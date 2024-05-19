using System.Net.Http.Headers;
using Az400_ProjectOnline;
using System.Text;
class Program
{
    private static readonly string tenantId = "<your-tenant-id>";
    private static readonly string clientId = "<your-client-id>";
    private static readonly string clientSecret = "<your-client-secret>";
    private static readonly string scope = "https://graph.microsoft.com/.default";
    private static readonly string siteUrl = $"https://{tenantId}.sharepoint.com/sites/pwa";
    private static readonly string projectId = "<project-id>";

    static async Task Main()
    {
        var accessToken = await GetAccessToken();
        Console.WriteLine("Access token:\n" + accessToken);

        var projectDetails = await GetProjectDetails(accessToken);
        Console.WriteLine("Project details:\n" + projectDetails);

        projectDetails = await UpdateProjectDatails(projectId, accessToken);
        Console.WriteLine("Project details updated successfully\n." + projectDetails);
    }

    // The following method is used to get the access token.
    public static async Task<string> GetAccessToken()
    {
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