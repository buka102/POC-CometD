﻿using CometD.NetCore.Client;
namespace POC;

class PushTopicConnection
{
    BayeuxClient _bayeuxClient = null;
    string channel = "/data/AccountChangeEvent";
    public PushTopicConnection(BayeuxClient bayeuxClient)
    {
        _bayeuxClient = bayeuxClient;
    }
    public void Connect()
    {
        _bayeuxClient.Handshake();
        _bayeuxClient.WaitFor(1000, new[] { BayeuxClient.State.CONNECTED });
        _bayeuxClient.GetChannel(channel).Subscribe(new Listener());
        Console.WriteLine("Waiting event from salesforce for the push topic " + channel.ToString());
    }
    public void Disconect()
    {
        _bayeuxClient.Disconnect();
        _bayeuxClient.WaitFor(1000, new[] { BayeuxClient.State.DISCONNECTED });
    }
}