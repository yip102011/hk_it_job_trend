# hk_it_job_trend

## query example
- Get job list query 
```bash
curl 'https://xapi.supercharge-srp.co/job-search/graphql?country=hk&isSmartSearch=true' \
  -H 'authority: xapi.supercharge-srp.co' \
  -H 'accept: */*' \
  -H 'accept-language: en,zh-TW;q=0.9,zh;q=0.8,en-US;q=0.7,zh-CN;q=0.6' \
  -H 'content-type: application/json' \
  -H 'origin: https://hk.jobsdb.com' \
  -H 'referer: https://hk.jobsdb.com/' \
  -H 'sec-ch-ua: "Not_A Brand";v="8", "Chromium";v="120", "Google Chrome";v="120"' \
  -H 'sec-ch-ua-mobile: ?0' \
  -H 'sec-ch-ua-platform: "Windows"' \
  -H 'sec-fetch-dest: empty' \
  -H 'sec-fetch-mode: cors' \
  -H 'sec-fetch-site: cross-site' \
  -H 'user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36' \
  --data-raw '{"query":"query getJobs($country: String, $locale: String, $keyword: String, $createdAt: String, $jobFunctions: [Int], $categories: [String], $locations: [Int], $careerLevels: [Int], $minSalary: Int, $maxSalary: Int, $salaryType: Int, $candidateSalary: Int, $candidateSalaryCurrency: String, $datePosted: Int, $jobTypes: [Int], $workTypes: [String], $industries: [Int], $page: Int, $pageSize: Int, $companyId: String, $advertiserId: String, $userAgent: String, $accNums: Int, $subAccount: Int, $minEdu: Int, $maxEdu: Int, $edus: [Int], $minExp: Int, $maxExp: Int, $seo: String, $searchFields: String, $candidateId: ID, $isDesktop: Boolean, $isCompanySearch: Boolean, $sort: String, $sVi: String, $duplicates: String, $flight: String, $solVisitorId: String) {\n  jobs(\n    country: $country\n    locale: $locale\n    keyword: $keyword\n    createdAt: $createdAt\n    jobFunctions: $jobFunctions\n    categories: $categories\n    locations: $locations\n    careerLevels: $careerLevels\n    minSalary: $minSalary\n    maxSalary: $maxSalary\n    salaryType: $salaryType\n    candidateSalary: $candidateSalary\n    candidateSalaryCurrency: $candidateSalaryCurrency\n    datePosted: $datePosted\n    jobTypes: $jobTypes\n    workTypes: $workTypes\n    industries: $industries\n    page: $page\n    pageSize: $pageSize\n    companyId: $companyId\n    advertiserId: $advertiserId\n    userAgent: $userAgent\n    accNums: $accNums\n    subAccount: $subAccount\n    minEdu: $minEdu\n    edus: $edus\n    maxEdu: $maxEdu\n    minExp: $minExp\n    maxExp: $maxExp\n    seo: $seo\n    searchFields: $searchFields\n    candidateId: $candidateId\n    isDesktop: $isDesktop\n    isCompanySearch: $isCompanySearch\n    sort: $sort\n    sVi: $sVi\n    duplicates: $duplicates\n    flight: $flight\n    solVisitorId: $solVisitorId\n  ) {\n    total\n    totalJobs\n    relatedSearchKeywords {\n      keywords\n      type\n      totalJobs\n    }\n    solMetadata\n    suggestedEmployer {\n      name\n      totalJobs\n    }\n    queryParameters {\n      key\n      searchFields\n      pageSize\n    }\n    experiments {\n      flight\n    }\n    jobs {\n      id\n      adType\n      sourceCountryCode\n      isStandout\n      companyMeta {\n        id\n        advertiserId\n        isPrivate\n        name\n        logoUrl\n        slug\n      }\n      jobTitle\n      jobUrl\n      jobTitleSlug\n      description\n      employmentTypes {\n        code\n        name\n      }\n      sellingPoints\n      locations {\n        code\n        name\n        slug\n        children {\n          code\n          name\n          slug\n        }\n      }\n      categories {\n        code\n        name\n        children {\n          code\n          name\n        }\n      }\n      postingDuration\n      postedAt\n      salaryRange {\n        currency\n        max\n        min\n        period\n        term\n      }\n      salaryVisible\n      bannerUrl\n      isClassified\n      solMetadata\n    }\n  }\n}\n","variables":{"keyword":"","jobFunctions":[131],"locations":[],"salaryType":1,"jobTypes":[],"createdAt":null,"careerLevels":[],"page":2,"country":"hk","sVi":"","solVisitorId":"65ee7814-3831-41e0-b301-4581c23e8b5d","categories":["131"],"workTypes":[],"userAgent":"Mozilla/5.0%20(Windows%20NT%2010.0;%20Win64;%20x64)%20AppleWebKit/537.36%20(KHTML,%20like%20Gecko)%20Chrome/120.0.0.0%20Safari/537.36","industries":[],"locale":"en"}}' \
  --compressed
```
- Get job list response
```json
{
    "data": {
        "jobs": {
            "total": 4269,
            "totalJobs": 4269,
            "relatedSearchKeywords": [],
            "solMetadata": {
                "requestToken": "806d0a00-9750-4411-b8ad-d7dcef67e474",
                "token": "0~806d0a00-9750-4411-b8ad-d7dcef67e474",
                "sortMode": "RELEVANCE",
                "categories": [
                    "131"
                ],
                "pageSize": 30,
                "pageNumber": 2,
                "totalJobCount": 4269,
                "tags": {
                    "mordor:searchMarket": "JOBS_DB_HK",
                    "mordor:result_count_rst": "4269",
                    "mordor:result_count_vec": "0",
                    "mordor:rt": "12",
                    "mordor:count_vec": "0",
                    "mordor__flights": "mordor_121",
                    "mordor:count_rst": "4269",
                    "mordor:count_ir": "0",
                    "mordor:result_count_ir": "0",
                    "mordor__pf": "1",
                    "mordor__rbo_ntier_p95_k30": "0.201",
                    "jobsdb:userGroup": "B"
                }
            },
            "suggestedEmployer": null,
            "queryParameters": {
                "key": "",
                "searchFields": "",
                "pageSize": 30
            },
            "experiments": {
                "flight": "mordor_121"
            },
            "jobs": [
                {
                    "id": "100003010852485",
                    "adType": "standard",
                    "sourceCountryCode": "hk",
                    "isStandout": false,
                    "companyMeta": {
                        "id": "408800",
                        "advertiserId": "hk100010275",
                        "isPrivate": false,
                        "name": "Guoco Group Ltd",
                        "logoUrl": "",
                        "slug": "guoco-group-ltd"
                    },
                    "jobTitle": "Head of IT",
                    "jobUrl": "https://hk.jobsdb.com/hk/en/job/head-of-it-100003010852485?token=0~806d0a00-9750-4411-b8ad-d7dcef67e474&sectionRank=31&jobId=jobsdb-hk-job-100003010852485",
                    "jobTitleSlug": "head-of-it",
                    "description": "Responsibilities Define and manage the overall IT strategies for the Group office. Oversee and advise the IT strategies and operations of subsidiary...",
                    "employmentTypes": [
                        {
                            "code": "full_time",
                            "name": "Full Time"
                        },
                        {
                            "code": "permanent",
                            "name": "Permanent"
                        }
                    ],
                    "sellingPoints": [],
                    "locations": [
                        {
                            "code": "155",
                            "name": "Central",
                            "slug": "central",
                            "children": null
                        }
                    ],
                    "categories": [
                        {
                            "code": "131",
                            "name": "Information Technology (IT)",
                            "children": null
                        },
                        {
                            "code": "138",
                            "name": "IT Project Management / Team Lead",
                            "children": null
                        },
                        {
                            "code": "140",
                            "name": "IT Management",
                            "children": null
                        },
                        {
                            "code": "150",
                            "name": "Others",
                            "children": null
                        }
                    ],
                    "postingDuration": "12-Jan-24",
                    "postedAt": "2024-01-12T06:45:21Z",
                    "salaryRange": {
                        "currency": null,
                        "max": null,
                        "min": null,
                        "period": "monthly",
                        "term": null
                    },
                    "salaryVisible": false,
                    "bannerUrl": "",
                    "isClassified": false,
                    "solMetadata": {
                        "searchRequestToken": "806d0a00-9750-4411-b8ad-d7dcef67e474",
                        "token": "0~806d0a00-9750-4411-b8ad-d7dcef67e474",
                        "jobId": "jobsdb-hk-job-100003010852485",
                        "section": "MAIN",
                        "sectionRank": 31,
                        "jobAdType": "ORGANIC",
                        "tags": {
                            "mordor__flights": "mordor_121",
                            "mordor__s": "0",
                            "jobsdb:userGroup": "B"
                        }
                    }
                },
                {
                    "id": "100003010854871",
                    "adType": "standout",
                    "sourceCountryCode": "hk",
                    "isStandout": true,
                    "companyMeta": {
                        "id": "jobsdb-hk-100547538",
                        "advertiserId": "hk100547538",
                        "isPrivate": false,
                        "name": "Epic Comm Company Limited",
                        "logoUrl": "https://image-service-cdn.seek.com.au/6e311b265abece66dfcec5a6faa3745c900a6543/ee4dce1061f3f616224767ad58cb2fc751b8d2dc",
                        "slug": "epic-comm-company-limited"
                    },
                    "jobTitle": "PHP Backend developer",
                    "jobUrl": "https://hk.jobsdb.com/hk/en/job/php-backend-developer-100003010854871?token=0~806d0a00-9750-4411-b8ad-d7dcef67e474&sectionRank=32&jobId=jobsdb-hk-job-100003010854871",
                    "jobTitleSlug": "php-backend-developer",
                    "description": "Responsibilities:- Take part in project backend API development, Testing, development, SIT, UAT, maintenance, training, and support- Work with...",
                    "employmentTypes": [
                        {
                            "code": "full_time",
                            "name": "Full Time"
                        },
                        {
                            "code": "permanent",
                            "name": "Permanent"
                        }
                    ],
                    "sellingPoints": [
                        "Double pay",
                        "Performance Bonus",
                        "Recent graduates will be considered"
                    ],
                    "locations": [
                        {
                            "code": "140",
                            "name": "Lai Chi Kok",
                            "slug": "lai-chi-kok",
                            "children": null
                        }
                    ],
                    "categories": [
                        {
                            "code": "131",
                            "name": "Information Technology (IT)",
                            "children": null
                        },
                        {
                            "code": "132",
                            "name": "Application Specialist - Software",
                            "children": null
                        },
                        {
                            "code": "142",
                            "name": "Software Development",
                            "children": null
                        },
                        {
                            "code": "147",
                            "name": "Network & System",
                            "children": null
                        }
                    ],
                    "postingDuration": "14 minutes ago",
                    "postedAt": "2024-01-14T20:13:02Z",
                    "salaryRange": {
                        "currency": null,
                        "max": null,
                        "min": null,
                        "period": "monthly",
                        "term": null
                    },
                    "salaryVisible": false,
                    "bannerUrl": "",
                    "isClassified": false,
                    "solMetadata": {
                        "searchRequestToken": "806d0a00-9750-4411-b8ad-d7dcef67e474",
                        "token": "0~806d0a00-9750-4411-b8ad-d7dcef67e474",
                        "jobId": "jobsdb-hk-job-100003010854871",
                        "section": "MAIN",
                        "sectionRank": 32,
                        "jobAdType": "ORGANIC",
                        "tags": {
                            "mordor__flights": "mordor_121",
                            "mordor__s": "0",
                            "jobsdb:userGroup": "B"
                        }
                    }
                },
                {
                    "id": "100003010854603",
                    "adType": "standout",
                    "sourceCountryCode": "hk",
                    "isStandout": true,
                    "companyMeta": {
                        "id": "408685",
                        "advertiserId": "hk100530187",
                        "isPrivate": false,
                        "name": "Castco Testing Centre Ltd",
                        "logoUrl": "https://image-service-cdn.seek.com.au/9f70b4d47b812ffd23b2698e936a62866fc9ea61/ee4dce1061f3f616224767ad58cb2fc751b8d2dc",
                        "slug": "castco-testing-centre-ltd"
                    },
                    "jobTitle": "IT Deskside Support",
                    "jobUrl": "https://hk.jobsdb.com/hk/en/job/it-deskside-support-100003010854603?token=0~806d0a00-9750-4411-b8ad-d7dcef67e474&sectionRank=33&jobId=jobsdb-hk-job-100003010854603",
                    "jobTitleSlug": "it-deskside-support",
                    "description": "Responsibilities: Develop new/modify existing excel template to speed up the reporting progress. Provide installation, maintenance, upgrade and...",
                    "employmentTypes": [
                        {
                            "code": "full_time",
                            "name": "Full Time"
                        },
                        {
                            "code": "permanent",
                            "name": "Permanent"
                        }
                    ],
                    "sellingPoints": [
                        "5-day work week",
                        "Diploma in Computer Science",
                        "Fresh Graduates are welcome"
                    ],
                    "locations": [
                        {
                            "code": "164",
                            "name": "Fanling",
                            "slug": "fanling",
                            "children": null
                        }
                    ],
                    "categories": [
                        {
                            "code": "131",
                            "name": "Information Technology (IT)",
                            "children": null
                        },
                        {
                            "code": "139",
                            "name": "Support",
                            "children": null
                        }
                    ],
                    "postingDuration": "13-Jan-24",
                    "postedAt": "2024-01-13T06:42:21Z",
                    "salaryRange": {
                        "currency": null,
                        "max": null,
                        "min": null,
                        "period": "monthly",
                        "term": null
                    },
                    "salaryVisible": false,
                    "bannerUrl": "https://image-service-cdn.seek.com.au/1fd57b99faed2ba91a45708942ded3c16380db4c/a868bcb8fbb284f4e8301904535744d488ea93c1",
                    "isClassified": false,
                    "solMetadata": {
                        "searchRequestToken": "806d0a00-9750-4411-b8ad-d7dcef67e474",
                        "token": "0~806d0a00-9750-4411-b8ad-d7dcef67e474",
                        "jobId": "jobsdb-hk-job-100003010854603",
                        "section": "MAIN",
                        "sectionRank": 33,
                        "jobAdType": "ORGANIC",
                        "tags": {
                            "mordor__flights": "mordor_121",
                            "mordor__s": "0",
                            "jobsdb:userGroup": "B"
                        }
                    }
                }
            ]
        }
    }
}
```

- Get job detail query
```bash
curl 'https://xapi.supercharge-srp.co/job-search/graphql?country=hk&isSmartSearch=true' \
  -H 'authority: xapi.supercharge-srp.co' \
  -H 'accept: */*' \
  -H 'accept-language: en,zh-TW;q=0.9,zh;q=0.8,en-US;q=0.7,zh-CN;q=0.6' \
  -H 'content-type: application/json' \
  -H 'origin: https://hk.jobsdb.com' \
  -H 'referer: https://hk.jobsdb.com/' \
  -H 'sec-ch-ua: "Not_A Brand";v="8", "Chromium";v="120", "Google Chrome";v="120"' \
  -H 'sec-ch-ua-mobile: ?0' \
  -H 'sec-ch-ua-platform: "Windows"' \
  -H 'sec-fetch-dest: empty' \
  -H 'sec-fetch-mode: cors' \
  -H 'sec-fetch-site: cross-site' \
  -H 'user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36' \
  --data-raw '{"query":"query getJobDetail($jobId: String, $locale: String, $country: String, $candidateId: ID, $solVisitorId: String, $flight: String) {\n  jobDetail(\n    jobId: $jobId\n    locale: $locale\n    country: $country\n    candidateId: $candidateId\n    solVisitorId: $solVisitorId\n    flight: $flight\n  ) {\n    id\n    pageUrl\n    jobTitleSlug\n    applyUrl {\n      url\n      isExternal\n    }\n    isExpired\n    isConfidential\n    isClassified\n    accountNum\n    advertisementId\n    subAccount\n    showMoreJobs\n    adType\n    header {\n      banner {\n        bannerUrls {\n          large\n        }\n      }\n      salary {\n        max\n        min\n        type\n        extraInfo\n        currency\n        isVisible\n      }\n      logoUrls {\n        small\n        medium\n        large\n        normal\n      }\n      jobTitle\n      company {\n        name\n        url\n        slug\n        advertiserId\n      }\n      review {\n        rating\n        numberOfReviewer\n      }\n      expiration\n      postedDate\n      postedAt\n      isInternship\n    }\n    companyDetail {\n      companyWebsite\n      companySnapshot {\n        avgProcessTime\n        registrationNo\n        employmentAgencyPersonnelNumber\n        employmentAgencyNumber\n        telephoneNumber\n        workingHours\n        website\n        facebook\n        size\n        dressCode\n        nearbyLocations\n      }\n      companyOverview {\n        html\n      }\n      videoUrl\n      companyPhotos {\n        caption\n        url\n      }\n    }\n    jobDetail {\n      summary\n      jobDescription {\n        html\n      }\n      jobRequirement {\n        careerLevel\n        yearsOfExperience\n        qualification\n        fieldOfStudy\n        industryValue {\n          value\n          label\n        }\n        skills\n        employmentType\n        languages\n        postedDate\n        closingDate\n        jobFunctionValue {\n          code\n          name\n          children {\n            code\n            name\n          }\n        }\n        benefits\n      }\n      whyJoinUs\n    }\n    location {\n      location\n      locationId\n      omnitureLocationId\n    }\n    sourceCountry\n  }\n}\n","variables":{"jobId":"100003010852574","country":"hk","locale":"en","candidateId":"","solVisitorId":"65ee7814-3831-41e0-b301-4581c23e8b5d"}}' \
  --compressed
```
Get Job Detail response
```json
{
    "data": {
        "jobDetail": {
            "id": "100003010852485",
            "pageUrl": "https://hk.jobsdb.com/hk/en/job/head-of-it-100003010852485",
            "jobTitleSlug": null,
            "applyUrl": {
                "url": "https://hk.jobsdb.com/hk/en/job/100003010852485/apply-preview",
                "isExternal": false
            },
            "isExpired": false,
            "isConfidential": false,
            "isClassified": false,
            "accountNum": 100010275,
            "advertisementId": "100003010852485",
            "subAccount": null,
            "showMoreJobs": null,
            "adType": "standard",
            "header": {
                "banner": {
                    "bannerUrls": {
                        "large": null
                    }
                },
                "salary": {
                    "max": null,
                    "min": null,
                    "type": "monthly",
                    "extraInfo": null,
                    "currency": "HKD",
                    "isVisible": false
                },
                "logoUrls": {
                    "small": null,
                    "medium": null,
                    "large": null,
                    "normal": "https://image-service-cdn.seek.com.au/ab22e59c321596f2f86276d150b4d208565f9bd1/ee4dce1061f3f616224767ad58cb2fc751b8d2dc"
                },
                "jobTitle": "Head of IT",
                "company": {
                    "name": "Guoco Group Ltd",
                    "url": "http://www.guoco.com",
                    "slug": "guoco-group-ltd",
                    "advertiserId": "hk100010275"
                },
                "review": null,
                "expiration": 27,
                "postedDate": "12-Jan-24",
                "postedAt": "2024-01-12T06:45:21Z",
                "isInternship": false
            },
            "companyDetail": {
                "companyWebsite": "http://www.guoco.com",
                "companySnapshot": {
                    "avgProcessTime": "",
                    "registrationNo": "",
                    "employmentAgencyPersonnelNumber": null,
                    "employmentAgencyNumber": null,
                    "telephoneNumber": null,
                    "workingHours": null,
                    "website": null,
                    "facebook": null,
                    "size": "",
                    "dressCode": null,
                    "nearbyLocations": ""
                },
                "companyOverview": {
                    "html": "<p style=\"text-align:justify\"><span style=\"font-family:arial,helvetica,sans-serif;font-size:12pt\">Guoco Group Limited (\"Guoco\") (Stock Code: 53), listed on The Stock Exchange of Hong Kong Limited, is an investment holding and investment management company\n with the vision of achieving long term sustainable returns for its shareholders and creating prime capital value. Guoco's operating subsidiary companies and investment activities are principally located in Hong Kong, the PRC, Singapore, Malaysia, Vietnam,\n the United Kingdom and New Zealand. Guoco has four core businesses, namely, Principal Investment; Property Development and Investment; Hospitality and Leisure Business; and Financial Services.</span></p>"
                },
                "videoUrl": "",
                "companyPhotos": null
            },
            "jobDetail": {
                "summary": [
                    "Manage IT strategies for the Group and subsidiary",
                    "Manage IT infrastructure & cloud platform services",
                    "Review IT security policies and procedures"
                ],
                "jobDescription": {
                    "html": "<div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><p><span style=\"font-family:arial,helvetica,sans-serif\"><strong> </strong><strong>Responsibilities<br /></strong></span></p><ul><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Define and manage the overall IT strategies for the Group office.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Oversee and advise the IT strategies and operations of subsidiary companies within the group.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Manage the group’s IT department to support business strategies and ensure operational efficiency.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Manage and review the BCP and DRP to align with business requirements. </span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Plan, implement, and manage the IT infrastructure and cloud platform services operating within the group.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Develop and review IT security policies and procedures (based on ISO27001/2 standards) for internal controls, risk, and security management.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Design, implement, and maintain a secure IT infrastructure and data management.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Develop, implement, and maintain a robust virtual server farm and security defense setup.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Manage the development, implementation, and support of in-house and third-party applications and systems.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Manage IT budget and optimize IT resource allocation to maximize ROI.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Manage IT-related procurements and relationships with vendors.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Stay up-to-date with emerging IT technologies and IT best practices, and evaluate the potential impacts on the Group and subsidiaries’ IT landscape.</span></div></li></ul><p><span style=\"font-family:arial,helvetica,sans-serif\"><strong>Requirements<br /></strong></span></p><ul><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Bachelor's degree in Computer Science or related discipline.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Minimum 10 years of experience in managing the IT department of a sizable company that engages in diversified businesses preferably in a MNC environment.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Proven experience in overseeing and advising the IT strategies and operations of subsidiary companies within a holding group.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Proven knowledge and experience in some of the following industries: Manufacturing, E-commerce, General Insurance, Hospitality and Leisure, Property Development, and Principal Investment.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Good business acumen and the ability to translate business requirements into effective IT solutions.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Proven knowledge and experience in developing and managing IT infrastructure that includes network design, server farm architecture, storage systems, and cloud technologies.  Experience in managing IT shared services among companies is an advantage.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Proven knowledge and experience in implementing and maintaining enterprise software systems provided by third-party vendors.  PMP or related certification is preferred.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Proficient knowledge and experience in the development, implementation, and support of in-house developed software systems.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">In-depth knowledge and experience in cybersecurity.  Holder of CISSP, CISM, or related certification is an advantage.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Experienced and good communication skills with the senior management of a holding company and its subsidiary companies.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Good leadership and team-building skills in motivating IT personnel within the group and subsidiary companies.</span></div></li><li><div style=\"font-family:inherit;font-size:12pt;text-align:justify\"><span style=\"color:#2e3849;font-family:arial,helvetica,sans-serif;font-size:12pt\">Good command of written and spoken English and Chinese (Cantonese).</span></div></li></ul><p><span style=\"font-size:12pt;font-family:arial,helvetica,sans-serif\">Please apply (preferably in Word or PDF format) with present and expected salaries, quoting job reference (JDBI/1109/HIT) to Chief Human Resources Officer, Group Human Resource Division, 50/F The Center, 99 Queen’s Road Central, Hong Kong or email  or fax to 2285 3209.</span></p></div><div style=\"text-align:justify\"><span style=\"font-size:12pt;font-family:arial,helvetica,sans-serif\">All applications will be treated in strict confidence and used for recruitment purpose within Guoco Group Limited, or Hong Leong Group of which Guoco is a member (together, “the Group”) only. A copy of our personal data policy is available on request.</span></div><div style=\"text-align:justify\"><span style=\"font-family:arial,helvetica,sans-serif\"><span style=\"font-size:12pt\">Applicants not hearing from us within 8 weeks may consider their applications unsuccessful. We may also consider their application for other or future vacancies within the Group. Personal data of unsuccessful applicants will be retained by the Group for a maximum period of one y</span><span style=\"font-size:12pt\">ear.</span></span></div>"
                },
                "jobRequirement": {
                    "careerLevel": "Senior",
                    "yearsOfExperience": "10 years",
                    "qualification": "Degree",
                    "fieldOfStudy": null,
                    "industryValue": {
                        "value": "fin_ser",
                        "label": "Financial Services"
                    },
                    "skills": null,
                    "employmentType": "Full Time, Permanent",
                    "languages": null,
                    "postedDate": "12-Jan-24",
                    "closingDate": null,
                    "jobFunctionValue": [
                        {
                            "code": 131,
                            "name": "Information Technology (IT)",
                            "children": null
                        },
                        {
                            "code": 138,
                            "name": "IT Project Management / Team Lead",
                            "children": null
                        },
                        {
                            "code": 140,
                            "name": "IT Management",
                            "children": null
                        },
                        {
                            "code": 150,
                            "name": "Others",
                            "children": null
                        }
                    ],
                    "benefits": [
                        "Dental insurance",
                        "Double pay",
                        "Medical insurance",
                        "Performance bonus",
                        "Five-day work week"
                    ]
                },
                "whyJoinUs": null
            },
            "location": [
                {
                    "location": "Central",
                    "locationId": "155",
                    "omnitureLocationId": "155"
                }
            ],
            "sourceCountry": "hk"
        }
    }
}
```
