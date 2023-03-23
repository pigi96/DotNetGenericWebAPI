using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace GenericWebAPI.Utilities;

public abstract class MicrosoftGraph
{
    protected readonly HttpClient _httpClient;
    protected readonly IConfidentialClientApplication _confidential;
    
    protected MicrosoftGraph(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _confidential = ConfidentialClientApplicationBuilder.Create(configuration.GetSection("AzureAd")["ClientId"])
            .WithClientSecret(configuration.GetSection("AzureAd")["ClientSecret"])
            .WithAuthority(
                new Uri($"https://login.microsoftonline.com/{configuration.GetSection("AzureAd")["TenantId"]}"))
            .Build();
    }

    protected async Task<string> ExecuteCall(HttpMethod httpMethod, string requestUri, object? body = null)
    {
        var request = new HttpRequestMessage(httpMethod, requestUri);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

        if (body != null)
        {
            var json = JsonSerializer.Serialize(body);
            request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        }

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await request.Content.ReadAsStringAsync();
    }
    
    protected async Task<string> GetAccessToken()
    {
        var scopes = new[] { "https://graph.microsoft.com/.default" };

        var result = await _confidential.AcquireTokenForClient(scopes)
            .ExecuteAsync();

        return result.AccessToken;
    }
}