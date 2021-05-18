using File.V1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Streaming.Yielding.App.Server
{
    public class FileService : File.V1.File.FileBase
    {
        private readonly ILogger _logger;
        private readonly FileContent _fileData;

        public FileService(ILoggerFactory loggerFactory, FileContent fileData)
        {
            _logger = loggerFactory.CreateLogger<FileService>();
            _fileData = fileData ?? throw new System.ArgumentNullException(nameof(fileData));
        }

        public async override Task GetFileData(Empty request, IServerStreamWriter<FileLinesReply> responseStream, ServerCallContext context)
        {
            var lines = _fileData.GetLinesAsync();
            await foreach (var line in lines)
            {
                await responseStream.WriteAsync(new FileLinesReply { Line = line });
            }
        }
    }
}