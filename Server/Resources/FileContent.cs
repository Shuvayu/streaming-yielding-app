using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Streaming.Yielding.App.Server
{
    public class FileContent
    {
        public async IAsyncEnumerable<string> GetLinesAsync()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Resources\\cars.json");
            string line;
            StreamReader file = new StreamReader(filePath);
            while ((line = await file.ReadLineAsync()) != null)
            {
                await Task.Delay(200);
                yield return line;
            }
        }
    }
}
