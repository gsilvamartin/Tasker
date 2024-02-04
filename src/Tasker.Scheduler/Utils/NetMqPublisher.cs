using System;
using NetMQ;
using NetMQ.Sockets;
using Tasker.Logging.Interfaces;

namespace Tasker.Scheduler.Utils;

public class NetMqPublisher : IDisposable
{
    private readonly PublisherSocket _publisherSocket;
    private readonly ITaskerLogger<NetMqPublisher> _logger;

    public NetMqPublisher(ITaskerLogger<NetMqPublisher> logger)
    {
        _logger = logger;
        _publisherSocket = new PublisherSocket("tcp://*:3333");
        _publisherSocket.Options.SendHighWatermark = 1000;
    }

    public void PublishBatchMessages(params string[] messages)
    {
        try
        {
            var batchMessage = string.Join(",", messages);
            PublishStringMessage(batchMessage);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while publishing batch messages", e);
        }
    }

    private void PublishStringMessage(string message)
    {
        try
        {
            _publisherSocket.SendFrame(message);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while publishing message", e);
        }
    }

    public void Dispose()
    {
        _publisherSocket?.Dispose();
    }
}