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
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace hk_it_job_trend_func
{
    public class JobsdbDetaiCrawler
    {
        private readonly IConfiguration _config;

        // only used when config "VISITOR_GUID" not set
        private const string DEFAULT_VISITOR_GUID = "effc67eb-b913-418f-9914-ebf1bfbb3809";
        private const string postedAt = "postedAt";

        private const string COSMOS_DATABASE_ID = "jobs";
        private const string COSMOS_CONTAINER_ID = "jobsdb_detail";
        private const string COSMOS_PARTITION_KEY = "/header/company/slug";
        private const string COSMOS_CONN_STR_CONFIG_KEY = "cosmosdb";

        public JobsdbDetaiCrawler(IConfiguration config)
        {
            _config = config;
        }

        [FunctionName(nameof(JobsdbDetaiCrawler))]
        public async Task Run([CosmosDBTrigger(
            databaseName: COSMOS_DATABASE_ID,
            containerName: "jobsdb",
            Connection = COSMOS_CONN_STR_CONFIG_KEY,
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)]IReadOnlyList<Job> jobList, ILogger log)
        {
            log.LogInformation($"function started at: {DateTime.Now}");
            log.LogInformation($"job count: {jobList.Count}");

            // get cosmosdb container
            log.LogInformation("get cosmosdb container");
            Container jobsdb_container = await GetCosmosContainer();

            using var graphQLClient = new GraphQLHttpClient("https://xapi.supercharge-srp.co/job-search/graphql?country=hk&isSmartSearch=true", new SystemTextJsonSerializer());
            foreach (var job in jobList)
            {
                log.LogInformation($"start query job id: {job.id}");

                var visitorGuid = _config.GetValue("VISITOR_GUID", DEFAULT_VISITOR_GUID);

                var request = createGraphQLRequest(job.id, visitorGuid);
                var qlResponse = await graphQLClient.SendQueryAsync<JsonObject>(request);
                var jobDetail = qlResponse.Data["jobDetail"];

                log.LogInformation($"start upsert job id: {job.id}");
                await jobsdb_container.UpsertItemAsync(JObject.Parse(jobDetail.ToJsonString()));
                log.LogInformation($"upsert success");
            }

            log.LogInformation($"function finised at: {DateTime.Now}");
        }

        private async Task<Container> GetCosmosContainer()
        {
            var cosmosClient = new CosmosClient(connectionString: _config.GetValue<string>(COSMOS_CONN_STR_CONFIG_KEY), new CosmosClientOptions { });
            var db = cosmosClient.GetDatabase(COSMOS_DATABASE_ID);
            var jobsdb_container = (await db.CreateContainerIfNotExistsAsync(COSMOS_CONTAINER_ID, COSMOS_PARTITION_KEY)).Container;
            return jobsdb_container;
        }

        private static GraphQLRequest createGraphQLRequest(string jobId, string visitorGuid)
        {
            return new GraphQLRequest
            {
                Query = @"query getJobDetail($jobId: String, $locale: String, $country: String, $candidateId: ID, $solVisitorId: String, $flight: String) {
                          jobDetail(
                            jobId: $jobId
                            locale: $locale
                            country: $country
                            candidateId: $candidateId
                            solVisitorId: $solVisitorId
                            flight: $flight
                          ) {
                            id
                            pageUrl
                            jobTitleSlug
                            applyUrl {
                              url
                              isExternal
                            }
                            isExpired
                            isConfidential
                            isClassified
                            accountNum
                            advertisementId
                            subAccount
                            showMoreJobs
                            adType
                            header {
                              banner {
                                bannerUrls {
                                  large
                                }
                              }
                              salary {
                                max
                                min
                                type
                                extraInfo
                                currency
                                isVisible
                              }
                              logoUrls {
                                small
                                medium
                                large
                                normal
                              }
                              jobTitle
                              company {
                                name
                                url
                                slug
                                advertiserId
                              }
                              review {
                                rating
                                numberOfReviewer
                              }
                              expiration
                              postedDate
                              postedAt
                              isInternship
                            }
                            companyDetail {
                              companyWebsite
                              companySnapshot {
                                avgProcessTime
                                registrationNo
                                employmentAgencyPersonnelNumber
                                employmentAgencyNumber
                                telephoneNumber
                                workingHours
                                website
                                facebook
                                size
                                dressCode
                                nearbyLocations
                              }
                              companyOverview {
                                html
                              }
                              videoUrl
                              companyPhotos {
                                caption
                                url
                              }
                            }
                            jobDetail {
                              summary
                              jobDescription {
                                html
                              }
                              jobRequirement {
                                careerLevel
                                yearsOfExperience
                                qualification
                                fieldOfStudy
                                industryValue {
                                  value
                                  label
                                }
                                skills
                                employmentType
                                languages
                                postedDate
                                closingDate
                                jobFunctionValue {
                                  code
                                  name
                                  children {
                                    code
                                    name
                                  }
                                }
                                benefits
                              }
                              whyJoinUs
                            }
                            location {
                              location
                              locationId
                              omnitureLocationId
                            }
                            sourceCountry
                          }
                        }",
                OperationName = "getJobDetail",
                Variables = new
                {
                    jobId = jobId,
                    country = "hk",
                    locale = "en",
                    candidateId = "",
                    solVisitorId = visitorGuid
                }
            };
        }
    }
}
