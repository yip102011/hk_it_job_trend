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

        public JobsdbDetaiCrawler(IConfiguration config)
        {
            _config = config;
        }

        [FunctionName(nameof(JobsdbDetaiCrawler))]
        public async Task Run([CosmosDBTrigger(
            databaseName: CosmosConfig.DB_NAME,
            containerName: CosmosConfig.JOBSDB_CON_JOBSDB,
            Connection = CosmosConfig.CONN_NAME,
            LeaseContainerName = CosmosConfig.JOBSDB_CON_LEASES,
            CreateLeaseContainerIfNotExists = true)]IReadOnlyList<JObject> jobList, ILogger log)
        {
            log.LogInformation($"function started at: {DateTime.Now}");
            log.LogInformation($"change feed job count: {jobList.Count}");

            // get cosmosdb container
            log.LogInformation("get cosmosdb container");
            var cosmosConnStr = _config.GetValue<string>(CosmosConfig.CONN_NAME);
            Container jobsdb_container = await GetCosmosContainer(cosmosConnStr, CosmosConfig.DB_NAME, CosmosConfig.JOBSDB_CON_JOBSDB_DETAIL, CosmosConfig.JOBSDB_KEY_JOBSDB_DETAIL);

            using var graphQLClient = new GraphQLHttpClient("https://xapi.supercharge-srp.co/job-search/graphql?country=hk&isSmartSearch=true", new SystemTextJsonSerializer());
            foreach (var job in jobList)
            {
                if (job.TryGetValue("id", out JToken idJToken) == false)
                {
                    log.LogInformation($"job don't have id, skip");
                    continue;
                }
                var jobId = idJToken.Value<string>();
                log.LogInformation($"[job id: {jobId}] start query ");
                var visitorGuid = _config.GetValue("VISITOR_GUID", DEFAULT_VISITOR_GUID);

                var request = createGraphQLRequest(jobId, visitorGuid);
                var qlResponse = await graphQLClient.SendQueryAsync<JsonObject>(request);
                var jobDetail = qlResponse.Data["jobDetail"];

                log.LogInformation($"[job id: {jobId}] start upsert ");
                await jobsdb_container.UpsertItemAsync(JObject.Parse(jobDetail.ToJsonString()));
                log.LogInformation($"[job id: {jobId}] upsert success ");
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
