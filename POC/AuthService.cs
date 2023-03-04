using Microsoft.Extensions.Configuration;
using POC.Models;
using System.Text.Json;

namespace POC;

public class AuthService: IAuthService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IConfiguration configuration;

    public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this.httpClientFactory = httpClientFactory;
        this.configuration = configuration;
    }

    public async Task<AuthResponse> GetAuthResponseAsync()
    {
        return await AsyncAuthRequest();
    }

    private async Task<AuthResponse> AsyncAuthRequest()
    {
        var config = this.configuration.GetSection("Salesforce");

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", config["ClientId"]),
            new KeyValuePair<string, string>("client_secret", config["ClientSecret"]),
            new KeyValuePair<string, string>("username", config["Username"]),
            new KeyValuePair<string, string>("password", config["Password"] + config["SecurityToken"])
        });
        ;
        HttpClient _httpClient = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(config["TokenUrl"]),
            Content = content
        };
        var responseMessage = await _httpClient.SendAsync(request);
        var response = await responseMessage.Content.ReadAsStringAsync();
        var responseDyn = JsonSerializer.Deserialize<AuthResponse>(response);

        return responseDyn;
    }
}
