using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace GenericWebAPI.Utilities;

public abstract class HttpConnector
{
    private readonly RestClient _client;

    // Required headers
    private readonly string _userAgent;
    private readonly string _acceptLanguage;
    
    protected HttpConnector(IConfiguration configuration)
    {
        _client = new RestClient(configuration.GetSection("WebGenAPI:WebScrape")["Hostname"]);
        _userAgent = configuration.GetSection("WebGenAPI:WebScrape:Headers")["User-Agent"];
        _acceptLanguage = configuration.GetSection("WebGenAPI:WebScrape:Headers")["Accept-Language"];
    }
    
    protected async Task<HtmlDocument> ExecuteCall(string endpoint, Method method, object? body = null)
    {
        var request = new RestRequest(endpoint, method);
        
        AddRequiredHeaders(request);

        if (body != null)
        {
            request.AddJsonBody(body);
        }
        
        var response = await _client.ExecuteAsync(request);

        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(response.Content);

        return doc;
    }
    
    protected void AddRequiredHeaders(RestRequest request)
    {
        request.AddHeader("User-Agent", _userAgent);
        request.AddHeader("Accept-Language", _acceptLanguage);
    }
}