using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using hk_it_job_trend_func.Models;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace hk_it_job_trend_func
{
    public class JobsdbDetaiCrawler
    {


        [FunctionName(nameof(JobsdbDetaiCrawler))]
        public async Task Run([CosmosDBTrigger(
            databaseName: "jobs",
            containerName: "jobsdb",
            Connection = "cosmosdb",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)]IReadOnlyList<Job> jobList, ILogger log)
        {
            foreach (var job in jobList)
            {
                using HttpClient client = new HttpClient();
                var detailPage = await client.GetStringAsync(job.jobUrl);

                log.LogInformation("test");
            }

            //if (input != null && input.Count > 0)
            //{
            //    log.LogInformation("Documents modified " + input.Count);
            //    log.LogInformation("First document Id " + input[0].id);
            //}
        }
    }
}
