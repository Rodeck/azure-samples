using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace durable_functions
{
    public class Website
    {
        public string Url { get; set; }
        public string Name { get; set; }
    }

    public static class PdfFactoryOrch
    {
        [FunctionName("PdfFactoryOrch")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var inputs = context.GetInput<List<Website>>();

            var outputs = new List<string>();

            foreach (var input in inputs)
            {
                outputs.Add(await context.CallActivityAsync<string>(nameof(SaveWebsite), input));
            }
            return outputs;
        }

        [FunctionName(nameof(SaveWebsite))]
        public static async Task SaveWebsite(
            [ActivityTrigger] Website website,
            IBinder binder,
            ILogger log)
        {
            using var client = new HttpClient();
            var result = await client.GetAsync(website.Url);

            var outBlobId = Guid.NewGuid();
            var outboundBlob = new BlobAttribute($"webpages/{website.Name}-{outBlobId}.html", FileAccess.Write) { Connection = "AzureWebJobsStorage" };
            using var outputBlob = binder.Bind<Stream>(outboundBlob);

            var webpageContent = await result.Content.ReadAsStreamAsync();
            // Write webpage content to outputBlob
            await webpageContent.CopyToAsync(outputBlob);

            log.LogInformation("Saying hello to {name}.", website.Name);
        }

        [FunctionName("PdfFactoryOrch_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.

            var websites = await req.Content.ReadAsAsync<WebsitesRequest>();
            string instanceId = await starter.StartNewAsync("PdfFactoryOrch", websites.Websites);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        public class WebsitesRequest
        {
            public List<Website> Websites { get; set; }
        }
    }
}