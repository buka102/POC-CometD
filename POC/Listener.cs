using CometD.NetCore.Bayeux;
using CometD.NetCore.Bayeux.Client;

namespace POC;

public class Listener : IMessageListener
{
    public void OnMessage(IClientSessionChannel channel, IMessage message)
    {
        var convertedJson = message.Json;
        Console.WriteLine(convertedJson);
    }
}