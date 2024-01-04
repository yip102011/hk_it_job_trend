using hk_it_job_trend_func.Models;

using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace hk_it_job_trend_func
{
    public class TestFunc
    {
        private readonly IConfiguration _config;

        public TestFunc(IConfiguration config)
        {
            _config = config;
        }

        [FunctionName(nameof(TestFunc))]
        public async Task Run([TimerTrigger("0 0 1 1 12 *", RunOnStartup = false)] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            //var cosmosClient = new CosmosClient(connectionString: _config.GetValue<string>("cosmosdb"), new CosmosClientOptions { });
            //var db = cosmosClient.GetDatabase("jobs");

            //var r = await db.CreateContainerIfNotExistsAsync("jobsdb", "/companyMeta/slug");
            //var jobsdb_container = r.Container;

            //await jobsdb_container.UpsertItemAsync(GetJobExample());

            //DateTime maxPostedAt = await GetLastPostedAt(jobsdb_container);


            //var d = DateTime.Parse("2023-12-29T03:11:41Z");

            //log.LogInformation($"Parse: {d}");
            //log.LogInformation($"Now: {DateTime.Now}");
        }

        private async Task<DateTime> GetMaxPostedAt(Container jobsdb_container)
        {
            var maxPostedAt = DateTime.Today.AddDays(-7);

            var query = new QueryDefinition("SELECT MAX(j.postedAt) AS postedAt FROM jobsdb AS j");
            using var feed = jobsdb_container.GetItemQueryIterator<Job>(query);
            FeedResponse<Job> response = await feed.ReadNextAsync();
            var e = response.GetEnumerator();
            if (e.MoveNext())
            {
                maxPostedAt = e.Current.postedAt;
            }

            return maxPostedAt;
        }

        private Job GetJobExample()
        {
            var j = @"
                {
                    ""id"": ""100003010826667"",
                    ""adType"": ""standout"",
                    ""sourceCountryCode"": ""hk"",
                    ""isStandout"": true,
                    ""companyMeta"": {
                        ""id"": ""404826"",
                        ""advertiserId"": ""hk100556992"",
                        ""isPrivate"": false,
                        ""name"": ""HGC Global Communications Limited"",
                        ""logoUrl"": ""https://image-service-cdn.seek.com.au/e4ca4af482ade290049c89ef72bb365817338bfe/ee4dce1061f3f616224767ad58cb2fc751b8d2dc"",
                        ""slug"": ""hgc-global-communications-limited""
                    },
                    ""jobTitle"": ""(Senior) Support Officer"",
                    ""jobUrl"": ""https://hk.jobsdb.com/hk/en/job/senior-support-officer-100003010826667?token=0~05cef792-bc5d-43a8-a203-074f2a621f3a&sectionRank=40&jobId=jobsdb-hk-job-100003010826667"",
                    ""jobTitleSlug"": ""senior-support-officer"",
                    ""description"": ""Responsibilities: Attend to helpdesk phone calls and call registration into helpdesk system; Create and maintain Helpdesk analysis report and records;..."",
                    ""employmentTypes"": [
                        {
                            ""code"": ""full_time"",
                            ""name"": ""Full Time""
                        },
                        {
                            ""code"": ""permanent"",
                            ""name"": ""Permanent""
                        }
                    ],
                    ""sellingPoints"": [
                        ""Flexible working hours"",
                        ""Helpdesk Support"",
                        ""Strong supporting team""
                    ],
                    ""locations"": [
                        {
                            ""code"": ""129"",
                            ""name"": ""Tsing Yi"",
                            ""slug"": ""tsing-yi"",
                            ""children"": null
                        }
                    ],
                    ""categories"": [
                        {
                            ""code"": ""131"",
                            ""name"": ""Information Technology (IT)"",
                            ""children"": null
                        },
                        {
                            ""code"": ""139"",
                            ""name"": ""Support"",
                            ""children"": null
                        }
                    ],
                    ""postingDuration"": ""an hour ago"",
                    ""postedAt"": ""2023-12-29T03:33:54Z"",
                    ""salaryRange"": {
                        ""currency"": null,
                        ""max"": null,
                        ""min"": null,
                        ""period"": ""monthly"",
                        ""term"": null
                    },
                    ""salaryVisible"": false,
                    ""bannerUrl"": ""https://image-service-cdn.seek.com.au/bd923e741ba7815e3834b50627b1d455531545c5/a868bcb8fbb284f4e8301904535744d488ea93c1"",
                    ""isClassified"": false,
                    ""solMetadata"": {
                        ""searchRequestToken"": ""05cef792-bc5d-43a8-a203-074f2a621f3a"",
                        ""token"": ""0~05cef792-bc5d-43a8-a203-074f2a621f3a"",
                        ""jobId"": ""jobsdb-hk-job-100003010826667"",
                        ""section"": ""MAIN"",
                        ""sectionRank"": 40,
                        ""jobAdType"": ""ORGANIC"",
                        ""tags"": {
                            ""mordor__flights"": ""mordor_121"",
                            ""mordor__s"": ""0"",
                            ""jobsdb:userGroup"": ""B""
                        }
                    }
                }
   
                    ";
            return JsonSerializer.Deserialize<Job>(j);
        }
    }
}
