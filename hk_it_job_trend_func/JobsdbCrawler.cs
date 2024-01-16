using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;

using hk_it_job_trend_func.Models;

using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace hk_it_job_trend_func
{
    public class JobsdbCrawler
    {
        private readonly IConfiguration _config;

        // only used when config "VISITOR_GUID" not set
        private const string DEFAULT_VISITOR_GUID = "effc67eb-b913-418f-9914-ebf1bfbb3809";
        private const string postedAt = "postedAt";

        public JobsdbCrawler(IConfiguration config)
        {
            _config = config;
        }

        [FunctionName(nameof(JobsdbCrawler))]
        public async Task Run([TimerTrigger("0 0 1 * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            //log.LogInformation($"function started at: {DateTime.Now}");

            // get cosmosdb container
            log.LogInformation("get cosmosdb container");
            var cosmosConnStr = _config.GetValue<string>(CosmosConfig.CONN_NAME);
            Container jobsdb_container = await GetCosmosContainer(cosmosConnStr, CosmosConfig.DB_NAME, CosmosConfig.JOBSDB_CON_JOBSDB, CosmosConfig.JOBSDB_KEY_JOBSDB);

            //defind a last date, stop query when job posted date before last date.
            var last_posted_at = await GetLastPostedAt(jobsdb_container, defaultValue: DateTime.Today);
            log.LogInformation($"last_posted_at: {last_posted_at.ToString("yyyy-MM-dd HH:mm:ss")}");

            // send query to get job
            var jobList = new List<JsonObject>();
            using var graphQLClient = new GraphQLHttpClient("https://xapi.supercharge-srp.co/job-search/graphql?country=hk&isSmartSearch=true", new SystemTextJsonSerializer());
            int max_query_page = 100;
            var visitorGuid = _config.GetValue("VISITOR_GUID", DEFAULT_VISITOR_GUID);
            for (int page = 1; page < max_query_page; page++)
            {
                log.LogInformation($"start query page {page}");
                GraphQLRequest request = createGraphQLRequest(page, visitorGuid);
                var qlResponse = await graphQLClient.SendQueryAsync<JsonObject>(request);
                var jobsObj = qlResponse.Data["jobs"];

                //var total = jobsObj["total"];
                //var pageSize = jobsObj["solMetadata"]?["pageSize"];
                //var pageNumber = jobsObj["solMetadata"]?["pageNumber"];
                //var totalJobCount = jobsObj["solMetadata"]?["totalJobCount"];

                var jobJsonArray = jobsObj["jobs"].AsArray();
                var pagedJobList = jobJsonArray.Select(j => j.Deserialize<JsonObject>());
                jobList.AddRange(pagedJobList);

                //stop at this page if any job reach last posted date
                if (pagedJobList.Any(j =>
                {
                    return j[postedAt].AsValue().Deserialize<DateTime>() <= last_posted_at;
                }))
                {
                    log.LogInformation($"job list count: {jobList.Count}");
                    break;
                }

                // add some delay, incase jobsdb block me
                await Task.Delay(Random.Shared.Next(100, 500));
            }

            log.LogInformation($"start upsert job list to cosmosdb, job count: {jobList.Count}");

            // start from oldest job, job should order by postedAt because 
            jobList.Reverse();

            // insert into cosmosdb one by one, incase error occur
            foreach (var job in jobList)
            {                
                var upsertJob = JObject.Parse(job.ToJsonString());

                //log.LogInformation($"[job id: {upsertJob.Value<string>("id")}] start upsert");
                await jobsdb_container.UpsertItemAsync(upsertJob);
            }

            log.LogInformation($"function finised at: {DateTime.Now}");
        }

        private async Task<Container> GetCosmosContainer(string connectionString, string databaseName, string containerName, string partitionKey)
        {
            var cosmosClient = new CosmosClient(connectionString: connectionString, new CosmosClientOptions { });
            var db = cosmosClient.GetDatabase(databaseName);
            var jobsdb_container = (await db.CreateContainerIfNotExistsAsync(containerName, partitionKey)).Container;
            return jobsdb_container;
        }

        private async Task<DateTime> GetLastPostedAt(Container jobsdb_container, DateTime defaultValue)
        {
            // default today 00:00
            var maxPostedAt = defaultValue;

            var query = new QueryDefinition($"SELECT MAX(j.{postedAt}) AS {postedAt} FROM jobsdb AS j");
            using var feed = jobsdb_container.GetItemQueryIterator<JObject>(query);

            if (feed.HasMoreResults)
            {
                var response = await feed.ReadNextAsync();
                var e = response.GetEnumerator();
                if (e.MoveNext())
                {
                    var d = e.Current.Value<DateTime>(postedAt);
                    if (d > DateTime.MinValue)
                    {
                        maxPostedAt = d;
                    }
                }
            }

            return maxPostedAt;
        }

        private static GraphQLRequest createGraphQLRequest(int page, string visitorGuid)
        {
            const int JOB_FUNCTION__INFORMATION_TECHNOLOGY = 131;
            return new GraphQLRequest
            {
                Query = @"
                            query getJobs($country: String, $locale: String, $keyword: String, $createdAt: String, $jobFunctions: [Int], $categories: [String], $locations: [Int], $careerLevels: [Int], $minSalary: Int, $maxSalary: Int, $salaryType: Int, $candidateSalary: Int, $candidateSalaryCurrency: String, $datePosted: Int, $jobTypes: [Int], $workTypes: [String], $industries: [Int], $page: Int, $pageSize: Int, $companyId: String, $advertiserId: String, $userAgent: String, $accNums: Int, $subAccount: Int, $minEdu: Int, $maxEdu: Int, $edus: [Int], $minExp: Int, $maxExp: Int, $seo: String, $searchFields: String, $candidateId: ID, $isDesktop: Boolean, $isCompanySearch: Boolean, $sort: String, $sVi: String, $duplicates: String, $flight: String, $solVisitorId: String) {
                              jobs(
                                country: $country
                                locale: $locale
                                keyword: $keyword
                                createdAt: $createdAt
                                jobFunctions: $jobFunctions
                                categories: $categories
                                locations: $locations
                                careerLevels: $careerLevels
                                minSalary: $minSalary
                                maxSalary: $maxSalary
                                salaryType: $salaryType
                                candidateSalary: $candidateSalary
                                candidateSalaryCurrency: $candidateSalaryCurrency
                                datePosted: $datePosted
                                jobTypes: $jobTypes
                                workTypes: $workTypes
                                industries: $industries
                                page: $page
                                pageSize: $pageSize
                                companyId: $companyId
                                advertiserId: $advertiserId
                                userAgent: $userAgent
                                accNums: $accNums
                                subAccount: $subAccount
                                minEdu: $minEdu
                                edus: $edus
                                maxEdu: $maxEdu
                                minExp: $minExp
                                maxExp: $maxExp
                                seo: $seo
                                searchFields: $searchFields
                                candidateId: $candidateId
                                isDesktop: $isDesktop
                                isCompanySearch: $isCompanySearch
                                sort: $sort
                                sVi: $sVi
                                duplicates: $duplicates
                                flight: $flight
                                solVisitorId: $solVisitorId
                              ) {
                                total
                                totalJobs
                                relatedSearchKeywords {
                                  keywords
                                  type
                                  totalJobs
                                }
                                solMetadata
                                suggestedEmployer {
                                  name
                                  totalJobs
                                }
                                queryParameters {
                                  key
                                  searchFields
                                  pageSize
                                }
                                experiments {
                                  flight
                                }
                                jobs {
                                  id
                                  adType
                                  sourceCountryCode
                                  isStandout
                                  companyMeta {
                                    id
                                    advertiserId
                                    isPrivate
                                    name
                                    logoUrl
                                    slug
                                  }
                                  jobTitle
                                  jobUrl
                                  jobTitleSlug
                                  description
                                  employmentTypes {
                                    code
                                    name
                                  }
                                  sellingPoints
                                  locations {
                                    code
                                    name
                                    slug
                                    children {
                                      code
                                      name
                                      slug
                                    }
                                  }
                                  categories {
                                    code
                                    name
                                    children {
                                      code
                                      name
                                    }
                                  }
                                  postingDuration
                                  postedAt
                                  salaryRange {
                                    currency
                                    max
                                    min
                                    period
                                    term
                                  }
                                  salaryVisible
                                  bannerUrl
                                  isClassified
                                  solMetadata
                                }
                              }
                            }
                ",
                OperationName = "getJobs",
                Variables = new
                {
                    keyword = "",
                    jobFunctions = new int[] { JOB_FUNCTION__INFORMATION_TECHNOLOGY },
                    locations = new string[] { },
                    salaryType = 1,
                    jobTypes = new string[] { },
                    createdAt = "",
                    careerLevels = new string[] { },
                    page,
                    sort = "createdAt",
                    country = "hk",
                    sVi = "",
                    solVisitorId = visitorGuid,
                    categories = new string[] { JOB_FUNCTION__INFORMATION_TECHNOLOGY.ToString() },
                    workTypes = new string[] { },
                    userAgent = "Mozilla/5.0%20(Windows%20NT%2010.0;%20Win64;%20x64)%20AppleWebKit/537.36%20(KHTML,%20like%20Gecko)%20Chrome/120.0.0.0%20Safari/537.36%20Edg/120.0.0.0",
                    industries = new string[] { },
                    locale = "en"
                }
            };
        }
    }
}
