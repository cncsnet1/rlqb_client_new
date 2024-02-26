using System;
using System.Net;
using rlqb_client.utils;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Formatting.Json;
using Serilog.Sinks.Network.Formatters;
using Serilog.Sinks.Network.Sinks.TCP;
using Serilog.Sinks.Tcp.Sinks;

public class ReconnectingNetworkSink : ILogEventSink, IDisposable
{

    private readonly string host;
    private readonly string port;
    private readonly object lockObject = new object();
    private TCPSink currentSink;

    public ReconnectingNetworkSink(string host, string port)
    {
        this.host = host;
        this.port = port;
        
        CreateSink();
    }

    private void CreateSink()
    {
        try
        {
            
            currentSink = new TCPSink(new Uri("tcp://" + host + ":" + port), new LogstashJsonFormatter());
            Console.WriteLine("gogogo");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating sink: {ex.Message}");
        }

    }

    public void Emit(LogEvent logEvent)
    {
        lock (lockObject)
        {
            try
            {
                currentSink.Emit(logEvent);
            }
            catch (Exception ex)
            {
                HandleNetworkError(ex);
            }
        }
    }

    private void HandleNetworkError(Exception ex)
    {
        // Log the error or take necessary actions
        Console.WriteLine($"Error sending log message: {ex.Message}");

        // Attempt to reconnect
        Reconnect();
    }

    private void Reconnect()
    {
        lock (lockObject)
        {
            // Dispose the current sink
            currentSink.Dispose();

            // Optional: Add a delay before attempting reconnection
            // Thread.Sleep(5000);

            // Recreate the sink
            CreateSink();

            Console.WriteLine("Reconnected successfully");
        }
    }

    public void Dispose()
    {
        lock (lockObject)
        {
            currentSink?.Dispose();
        }
    }
}
