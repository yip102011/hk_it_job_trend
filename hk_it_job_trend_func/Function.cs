using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace hk_it_job_trend_func
{
    public static class Function
    {
        [FunctionName("Function")]
        public static void Run([CosmosDBTrigger(
            databaseName: "databaseName",
            containerName: "containerName",
            Connection = "",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)]IReadOnlyList<ToDoItem> input,
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].id);
            }
        }
    }

    // Customize the model with your own desired properties
    public class ToDoItem
    {
        public string id { get; set; }
        public string Description { get; set; }
    }
}
