
using System;
using System.Collections.Generic;

using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace hk_it_job_trend_func.Models
{
    public partial class JobsdbResponse
    {
        [J("jobs")] public Jobs Jobs { get; set; }
    }

    public partial class Jobs
    {
        [J("total")] public long Total { get; set; }
        [J("totalJobs")] public long TotalJobs { get; set; }
        [J("relatedSearchKeywords")] public dynamic[] RelatedSearchKeywords { get; set; }
        [J("solMetadata")] public SolMetadata SolMetadata { get; set; }
        [J("suggestedEmployer")] public dynamic SuggestedEmployer { get; set; }
        [J("queryParameters")] public QueryParameters QueryParameters { get; set; }
        [J("experiments")] public Experiments Experiments { get; set; }
        [J("jobs")] public Job[] JobArray { get; set; }

    }

    public partial class Experiments
    {
        [J("flight")] public string Flight { get; set; }
    }

    public partial class QueryParameters
    {
        [J("key")] public string Key { get; set; }
        [J("searchFields")] public string SearchFields { get; set; }
        [J("pageSize")] public long PageSize { get; set; }
    }

    public partial class SolMetadata
    {
        [J("requestToken")] public Guid RequestToken { get; set; }
        [J("token")] public string Token { get; set; }
        [J("sortMode")] public string SortMode { get; set; }
        [J("categories")] public string[] Categories { get; set; }
        [J("pageSize")] public long PageSize { get; set; }
        [J("pageNumber")] public long PageNumber { get; set; }
        [J("totalJobCount")] public long TotalJobCount { get; set; }

        
        [J("tags")] public dynamic Tags { get; set; }
    }
}



namespace hk_it_job_trend_func.Models
{
    public partial class Job
    {
        [J("id")] public string id { get; set; }
        [J("adType")] public string AdType { get; set; }
        [J("sourceCountryCode")] public string SourceCountryCode { get; set; }
        [J("isStandout")] public bool IsStandout { get; set; }
        [J("companyMeta")] public CompanyMeta CompanyMeta { get; set; }
        [J("jobTitle")] public string JobTitle { get; set; }
        [J("jobUrl")] public Uri JobUrl { get; set; }
        [J("jobTitleSlug")] public string JobTitleSlug { get; set; }
        [J("description")] public string Description { get; set; }
        [J("employmentTypes")] public EmploymentType[] EmploymentTypes { get; set; }
        [J("sellingPoints")] public string[] SellingPoints { get; set; }
        [J("locations")] public Location[] Locations { get; set; }
        [J("categories")] public Category[] Categories { get; set; }
        [J("postingDuration")] public string PostingDuration { get; set; }
        [J("postedAt")] public DateTimeOffset PostedAt { get; set; }
        [J("salaryRange")] public SalaryRange SalaryRange { get; set; }
        [J("salaryVisible")] public bool SalaryVisible { get; set; }
        [J("bannerUrl")] public string BannerUrl { get; set; }
        [J("isClassified")] public bool IsClassified { get; set; }
        [J("solMetadata")] public JobSolMetadata SolMetadata { get; set; }
    }

    public partial class Category
    {
        [J("code")] public string Code { get; set; }
        [J("name")] public string Name { get; set; }
        [J("children")] public dynamic Children { get; set; }
    }

    public partial class CompanyMeta
    {
        [J("id")] public string Id { get; set; }
        [J("advertiserId")] public string AdvertiserId { get; set; }
        [J("isPrivate")] public bool IsPrivate { get; set; }
        [J("name")] public string Name { get; set; }
        [J("logoUrl")] public string LogoUrl { get; set; }
        [J("slug")] public string Slug { get; set; }
    }

    public partial class EmploymentType
    {
        [J("code")] public string Code { get; set; }
        [J("name")] public string Name { get; set; }
    }

    public partial class Location
    {
        [J("code")] public string Code { get; set; }
        [J("name")] public string Name { get; set; }
        [J("slug")] public string Slug { get; set; }
        [J("children")] public dynamic Children { get; set; }
    }

    public partial class SalaryRange
    {
        [J("currency")] public dynamic Currency { get; set; }
        [J("max")] public dynamic Max { get; set; }
        [J("min")] public dynamic Min { get; set; }
        [J("period")] public string Period { get; set; }
        [J("term")] public dynamic Term { get; set; }
    }

    public partial class JobSolMetadata
    {
        [J("searchRequestToken")] public Guid SearchRequestToken { get; set; }
        [J("token")] public string Token { get; set; }
        [J("jobId")] public string JobId { get; set; }
        [J("section")] public string Section { get; set; }
        [J("sectionRank")] public long SectionRank { get; set; }
        [J("jobAdType")] public string JobAdType { get; set; }
        [J("tags")] public dynamic Tags { get; set; }
    }
}