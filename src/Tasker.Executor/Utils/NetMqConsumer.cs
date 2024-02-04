using System;
using NetMQ;
using NetMQ.Sockets;
using Tasker.Logging.Interfaces;

namespace Tasker.Executor.Utils;

public class NetMqConsumer : IDisposable
{
    private readonly SubscriberSocket _subscriberSocket;
    private readonly ITaskerLogger<NetMqConsumer> _logger;

    public NetMqConsumer(ITaskerLogger<NetMqConsumer> logger)
    {
        _logger = logger;
        _subscriberSocket = new SubscriberSocket();
        _subscriberSocket.Options.ReceiveHighWatermark = 1000;
    }

    public void StartListeningTopic(Action<string> messageReceivedCallback)
    {
        try
        {
            _subscriberSocket.Connect("tcp://*:3333");
            _subscriberSocket.SubscribeToAnyTopic();

            while (true)
            {
                var message = _subscriberSocket.ReceiveFrameString();
                messageReceivedCallback.Invoke(message);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error while listening for messages", e);
        }
    }

    public void Dispose()
    {
        _subscriberSocket?.Dispose();
    }
}