using hk_it_job_trend_func.Models;

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace hk_it_job_trend_func
{
    public class JobsdbCrawler
    {
        private readonly IConfiguration _config;

        // only used when config "VISITOR_GUID" not set
        private const string DEFAULT_VISITOR_GUID = "effc67eb-b913-418f-9914-ebf1bfbb3809";
        private const string listingDateFieldName = "listingDate";
        private string cosmosConnStr { get { return _config.GetValue<string>(CosmosConfig.CONN_NAME); } }
        private ILogger _logger;
        public JobsdbCrawler(IConfiguration config)
        {
            _config = config;
        }

        [FunctionName(nameof(JobsdbCrawler))]
        [FixedDelayRetry(0, "00:00:00")]
        public async Task Run([TimerTrigger("0 0 17 * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            _logger = log;
            //log.LogInformation($"function started at: {DateTime.Now}");

            // get cosmosdb container
            log.LogInformation("get cosmosdb container");
            Container jobsdbContainer = await GetCosmosContainer(cosmosConnStr, CosmosConfig.DB_NAME, CosmosConfig.JOBSDB_CON_JOBSDB, CosmosConfig.JOBSDB_KEY_JOBSDB);

            //defind a last date, stop query when job posted date before last date.
            var lastListingDate = await GetLastListingDate(jobsdbContainer, defaultValue: DateTime.Today);
            log.LogInformation($"lastListingDate: {lastListingDate.ToString("yyyy-MM-dd HH:mm:ss")}");

            // send query to get job
            var jobList = new List<JsonObject>();
            int maxQueryPage = 1;
            var visitorGuid = _config.GetValue("VISITOR_GUID", DEFAULT_VISITOR_GUID);
            for (int page = 1; page <= maxQueryPage; page++)
            {
                log.LogInformation($"start query page {page}");
                JsonObject pageJson;
                try
                {
                    pageJson = await GetJobsPageJson(page);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Get Jobs Page Error, page: {page}");
                    throw;
                }

                var jobJsonArray = pageJson["data"].AsArray();

                //stop at this page if any job reach last posted date
                var maxListedAt = jobJsonArray.Max(job => job[listingDateFieldName].GetValue<DateTime>());
                if (maxListedAt <= lastListingDate)
                {
                    log.LogInformation($"job list count: {jobList.Count}");
                    break;
                }

                // add paged list to list
                var pagedJobList = jobJsonArray.Select(j => j.AsObject());
                jobList.AddRange(pagedJobList);

                // add some delay, incase jobsdb block me
                await Task.Delay(Random.Shared.Next(100, 500));
            }

            log.LogInformation($"start upsert job list to cosmosdb, job count: {jobList.Count}");

            // start from oldest job, job should order by listingDateFieldName desc default
            jobList.Reverse();

            // cosmosdb require id to be string
            jobList.ForEach(job => { job["id"] = job["id"].GetValue<int>().ToString(); });

            // insert into cosmosdb one by one, incase error occur
            foreach (var job in jobList)
            {
                var upsertJob = JObject.Parse(job.ToJsonString());
                log.LogInformation($"[job id: {upsertJob.Value<string>("id")}] start upsert");

                try
                {
                    await jobsdbContainer.UpsertItemAsync(upsertJob);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Save job to cosmosdb error, {upsertJob}");
                    throw;
                }
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

        private async Task<DateTime> GetLastListingDate(Container jobsdb_container, DateTime defaultValue)
        {
            var query = new QueryDefinition($"SELECT MAX(j.{listingDateFieldName}) AS {listingDateFieldName} FROM jobsdb AS j");
            using var feed = jobsdb_container.GetItemQueryIterator<JObject>(query);
            if (feed.HasMoreResults == false)
            {
                return defaultValue;
            }

            var response = await feed.ReadNextAsync();
            DateTime maxListingDate = response.FirstOrDefault()?.Value<DateTime?>(listingDateFieldName) ?? defaultValue;
            return maxListingDate;
        }

        private async Task<JsonObject> GetJobsPageJson(int page, string visitorGuid = DEFAULT_VISITOR_GUID)
        {
            string baseUrl = "https://hk.jobsdb.com/api/chalice-search/v4/search";
            var queryParams = new Dictionary<string, string>();
            queryParams["siteKey"] = "HK-Main";
            queryParams["sourcesystem"] = "houston";
            //queryParams["userqueryid"] = "132bf9afd02e341071614907b92d1843-5465178";
            //queryParams["userid"] = "34fdb779-2485-45c6-a758-8b7019a4c413";
            //queryParams["usersessionid"] = "34fdb779-2485-45c6-a758-8b7019a4c413";
            //queryParams["eventCaptureSessionId"] = "34fdb779-2485-45c6-a758-8b7019a4c413";
            queryParams["page"] = page.ToString();
            queryParams["seekSelectAllPages"] = "true";
            queryParams["classification"] = "6281";
            queryParams["sortmode"] = "ListedDate";
            queryParams["pageSize"] = "30";
            queryParams["include"] = "seodata";
            queryParams["locale"] = "en-HK";
            //queryParams["solId"] = "b7ffa1b6-0cae-4749-9fff-b311300739d6";

            string url = baseUrl;
            foreach (var (name, value) in queryParams) { url = QueryHelpers.AddQueryString(url, name, value); }

            using HttpClient client = new HttpClient();
            var response = await client.GetFromJsonAsync<JsonObject>(url);
            return response;
        }

        [Obsolete("This function is not using currently")]
        private async Task<string> GetVisitorId()
        {
            const string visitorIdDBFieldName = "visitor_id";
            Container container = await GetCosmosContainer(cosmosConnStr, CosmosConfig.DB_NAME, CosmosConfig.JOBSDB_CON_JOBSDB_META, CosmosConfig.JOBSDB_KEY_JOBSDB_META);

            //get vistior id from cosmosdb
            var query = new QueryDefinition($"SELECT value FROM c WHERE c.key = '{visitorIdDBFieldName}'");
            var feed = container.GetItemQueryIterator<JObject>(query);
            if (feed.HasMoreResults == true)
            {
                var feedResponse = await feed.ReadNextAsync();
                var vistiorIdFromDB = feedResponse.FirstOrDefault()?.Value<string>(visitorIdDBFieldName);
                if (string.IsNullOrWhiteSpace(vistiorIdFromDB) == false)
                {
                    return vistiorIdFromDB;
                }
            }

            // if vistior id is null of empty
            // get vistior from cookie by access jobdb.com
            var client = new HttpClient();
            var httpRequest = new HttpRequestMessage();
            httpRequest.RequestUri = new Uri("https://hk.jobsdb.com/");
            httpRequest.Method = HttpMethod.Get;
            var httpResponse = await client.SendAsync(httpRequest);
            httpResponse.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values);
            var vistiorId = values.Where(v => v.StartsWith("JobseekerVisitorId=")).FirstOrDefault()?.Replace("JobseekerVisitorId=", string.Empty);
            if (string.IsNullOrWhiteSpace(vistiorId))
            {
                throw new Exception($"Get vistior id fail, cookie: {string.Join('|', values)}, status code: {httpResponse.StatusCode}, vistior id: {vistiorId}");
            }

            // store vistior id into cosmosdb 
            var item = new { key = visitorIdDBFieldName, value = vistiorId };
            var createResponse = await container.CreateItemAsync(item);
            if (createResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Store vistior id to cosmosdb fail, container id: {container.Id}, status code: {createResponse.StatusCode}, vistior id: {vistiorId}, key: {visitorIdDBFieldName}");
            }

            return vistiorId;
        }
    }
}
