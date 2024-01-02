using System;
using System.Threading.Tasks;

using Azure.Identity;

using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace hk_it_job_trend_func
{
    public class CosmosTest
    {

        [FunctionName(nameof(CosmosTest))]
        public void Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            using CosmosClient client = new CosmosClient(connectionString: Environment.GetEnvironmentVariable("cosmosdb"));
            client.CreateDatabaseIfNotExistsAsync("jobs");
            //client.
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
