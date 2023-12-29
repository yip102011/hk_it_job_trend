using System;
using System.Threading.Tasks;

using Azure.Core;
using Azure.Identity;

using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace hk_it_job_trend_func
{
    public class CosmosTest
    {
        [FunctionName("CosmosTest")]
        public static async Task Run([TimerTrigger("0 */5 * * * *"
                                                                                    #if DEBUG
                                                                                    , RunOnStartup =true
                                                                                    #endif
            )] TimerInfo myTimer, ILogger log)
        {

            TokenCredential credential = new DefaultAzureCredential();
            using CosmosClient client = new(connectionString: Environment.GetEnvironmentVariable("cosmos_db"));
            //client.

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
