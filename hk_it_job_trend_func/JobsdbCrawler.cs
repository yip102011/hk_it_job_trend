using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;

using hk_it_job_trend_func.Models;

using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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

        public JobsdbCrawler(IConfiguration config)
        {
            _config = config;
        }

        [FunctionName(nameof(JobsdbCrawler))]
        public async Task Run([TimerTrigger("0 0 1 * * *", RunOnStartup = false)] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation("function start");

            // get cosmosdb container
            log.LogInformation("get cosmosdb container");
            var cosmosClient = new CosmosClient(connectionString: _config.GetValue<string>("cosmosdb"), new CosmosClientOptions { });
            var db = cosmosClient.GetDatabase("jobs");
            var jobsdb_container = (await db.CreateContainerIfNotExistsAsync("jobsdb", "/companyMeta/slug")).Container;

            //defind a last date, stop query when job posted date before last date.
            var last_posted_at = await GetLastPostedAt(jobsdb_container, defaultValue: DateTime.Today);

            // send query to get job
            List<Job> jobList = new List<Job>();
            using var graphQLClient = new GraphQLHttpClient("https://xapi.supercharge-srp.co/job-search/graphql?country=hk&isSmartSearch=true", new SystemTextJsonSerializer());
            int max_query_page = 100;
            for (int page = 1; page < max_query_page; page++)
            {
                log.LogInformation($"start query page {page}");
                GraphQLRequest request = createGraphQLRequest(page);
                var qlResponse = await graphQLClient.SendQueryAsync<JsonObject>(request);
                var jobsObj = qlResponse.Data["jobs"];
                //var total = jobsObj["total"];
                //var pageSize = jobsObj["solMetadata"]?["pageSize"];
                //var pageNumber = jobsObj["solMetadata"]?["pageNumber"];
                //var totalJobCount = jobsObj["solMetadata"]?["totalJobCount"];
                var jobJsonArray = jobsObj["jobs"].AsArray();

                var queriedJobList = jobJsonArray.Select(j => j.Deserialize<Job>());

                jobList.AddRange(queriedJobList);

                //stop query if any job posted date before last date.
                if (queriedJobList.Any(j => j.postedAt <= last_posted_at))
                {
                    break;
                }

                // add some delay, incase jobsdb block me 
                await Task.Delay(Random.Shared.Next(500, 2000));
            }

            // insert one by one, start from oldest job, incase error occur
            jobList.Reverse();
            log.LogInformation($"start upsert job to cosmosdb");
            foreach (var job in jobList)
            {
                await jobsdb_container.UpsertItemAsync(job);
            }

            log.LogInformation($"function finised at: {DateTime.Now}");
        }

        private async Task<DateTime> GetLastPostedAt(Container jobsdb_container, DateTime defaultValue)
        {
            // default today 00:00
            var maxPostedAt = defaultValue;

            var query = new QueryDefinition("SELECT MAX(j.postedAt) AS postedAt FROM jobsdb AS j");
            using var feed = jobsdb_container.GetItemQueryIterator<Job>(query);

            if (feed.HasMoreResults)
            {
                FeedResponse<Job> response = await feed.ReadNextAsync();
                var e = response.GetEnumerator();
                if (e.MoveNext())
                {
                    if (e.Current.postedAt > DateTime.MinValue)
                    {
                        maxPostedAt = e.Current.postedAt;
                    }
                }
            }

            return maxPostedAt;
        }

        private static GraphQLRequest createGraphQLRequest(int page)
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
                    solVisitorId = "3a9cde3c-5dd2-4519-bfdf-a1d7d2acced9",
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
