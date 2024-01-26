using System.IO;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class PdfParser
    {
        [FunctionName("PdfParser")]
        [StorageAccount("MyStorageConnection")]
        public void Run([BlobTrigger("pdf-templates/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob,
                        string name,
                        [Blob("pdf-templates-results/{name}.txt", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream outpuJson,
                        ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            
            var parser = new BitMiracleParser();
            // convert stream to byte buffer
            var template = parser.Parse(myBlob);

            var json = JsonSerializer.Serialize(template);
            // convert json to byte array
            var jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
            outpuJson.Write(jsonBytes);
        }
    }
}
