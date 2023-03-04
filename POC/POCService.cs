using CometD.NetCore.Client;
using CometD.NetCore.Client.Transport;
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;
using System.Net;

namespace POC;

public class POCService : IPOCService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IConfiguration configuration;
    private readonly IAuthService authService;

    public POCService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IAuthService authService)
    {
        this.httpClientFactory = httpClientFactory;
        this.configuration = configuration;
        this.authService = authService;
    }

    public Task<string> CreateAccount(string accountName)
    {
        throw new NotImplementedException();
    }

    public async Task TestCometD()
    {
        var authResponse = await authService.GetAuthResponseAsync();

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        try
        {
            int readTimeOut = 120000;
            string streamingEndpointURI = "/cometd/43.0";
            var options = new Dictionary<String, Object>
            {
                {
                    ClientTransport.TIMEOUT_OPTION, readTimeOut
                }
            };
            NameValueCollection collection = new NameValueCollection();
            collection.Add(HttpRequestHeader.Authorization.ToString(), "OAuth " + authResponse.access_token);
            var transport = new LongPollingTransport(options, new NameValueCollection { collection });
            var serverUri = new Uri(authResponse.instance_url);
            String endpoint = String.Format("{0}://{1}{2}", serverUri.Scheme, serverUri.Host, streamingEndpointURI);
            var bayeuxClient = new BayeuxClient(endpoint, new[] { transport });
            var pushTopicConnection = new PushTopicConnection(bayeuxClient);
            pushTopicConnection.Connect();
            //Close the connection
            Console.WriteLine("Press any key to shut down.\n");
            Console.ReadKey();
            pushTopicConnection.Disconect();
            Console.ReadKey();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

