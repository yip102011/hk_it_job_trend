using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace hk_it_job_trend_func.Models
{
    public class JobsdbResponse
    {
        public Jobs jobs { get; set; }
    }
    public class Jobs
    {
        public int total { get; set; }
        public int totalJobs { get; set; }
        public List<object> relatedSearchKeywords { get; set; }
        public SolMetadata solMetadata { get; set; }
        public object suggestedEmployer { get; set; }
        public QueryParameters queryParameters { get; set; }
        public Experiments experiments { get; set; }
        public Job[] JobArray { get; set; }
    }
    public class Experiments
    {
        public string flight { get; set; }
    }
    public class QueryParameters
    {
        public string key { get; set; }
        public string searchFields { get; set; }
        public int pageSize { get; set; }
    }
    public class SolMetadata
    {
        public string requestToken { get; set; }
        public string token { get; set; }
        public string sortMode { get; set; }
        public List<string> categories { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int totalJobCount { get; set; }
        public dynamic tags { get; set; }
    }
}

namespace hk_it_job_trend_func.Models
{
    public class Job
    {
        public string id { get; set; }
        public string adType { get; set; }
        public string sourceCountryCode { get; set; }
        public bool isStandout { get; set; }
        public CompanyMeta companyMeta { get; set; }
        public string jobTitle { get; set; }
        public string jobUrl { get; set; }
        public string jobTitleSlug { get; set; }
        public string description { get; set; }
        public List<EmploymentType> employmentTypes { get; set; }
        public List<string> sellingPoints { get; set; }
        public List<Location> locations { get; set; }
        public List<Category> categories { get; set; }
        public string postingDuration { get; set; }
        public DateTime postedAt { get; set; }
        public SalaryRange salaryRange { get; set; }
        public bool salaryVisible { get; set; }
        public string bannerUrl { get; set; }
        public bool isClassified { get; set; }
        public SolMetadata solMetadata { get; set; }
    }

    public class Category
    {
        public string code { get; set; }
        public string name { get; set; }
        public object children { get; set; }
    }

    public class CompanyMeta
    {
        public string id { get; set; }
        public string advertiserId { get; set; }
        public bool isPrivate { get; set; }
        public string name { get; set; }
        public string logoUrl { get; set; }
        public string slug { get; set; }
    }

    public class EmploymentType
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class Location
    {
        public string code { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public object children { get; set; }
    }

    public class SalaryRange
    {
        public object currency { get; set; }
        public object max { get; set; }
        public object min { get; set; }
        public string period { get; set; }
        public object term { get; set; }
    }

    public class JobSolMetadata
    {
        public string searchRequestToken { get; set; }
        public string token { get; set; }
        public string jobId { get; set; }
        public string section { get; set; }
        public int sectionRank { get; set; }
        public string jobAdType { get; set; }
        public dynamic tags { get; set; }
    }}