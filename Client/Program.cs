using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Streaming.Yielding.App.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GrpcChannel channel;
            File.V1.File.FileClient client;
            InitializeClient(out channel, out client);

            await ServerStreamingCallAsync(client);

            Console.WriteLine("Shutting down");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void InitializeClient(out GrpcChannel channel, out File.V1.File.FileClient client)
        {
            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            channel = GrpcChannel.ForAddress("http://localhost:5050",
                            new GrpcChannelOptions { HttpHandler = httpHandler });
            client = new File.V1.File.FileClient(channel);
        }

        private static async Task ServerStreamingCallAsync(File.V1.File.FileClient client)
        {
            using var call = client.GetFileData(new Empty());
            int lineNumber = 0;
            await foreach (var message in call.ResponseStream.ReadAllAsync())
            {
                lineNumber++;
                Console.WriteLine($"Line {lineNumber}: {message.Line}");
            }
        }
    }
}
