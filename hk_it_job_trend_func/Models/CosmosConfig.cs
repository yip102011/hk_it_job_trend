using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hk_it_job_trend_func.Models
{
    public static class CosmosConfig
    {
        public const string CONN_NAME = "cosmosdb";

        public const string DB_NAME = "jobs";
        public const int DB_MAX_THROUGHPUT = 1000;

        public const string JOBSDB_CON_JOBSDB = "jobsdb";
        public const string JOBSDB_KEY_JOBSDB = "/companyMeta/slug";

        public const string JOBSDB_CON_JOBSDB_DETAIL = "jobsdb_detail";
        public const string JOBSDB_KEY_JOBSDB_DETAIL = "/header/company/slug";

        public const string JOBSDB_CON_LEASES = "leases";
        public const string JOBSDB_KEY_LEASES = "/id";
        public const int JOBSDB_TTL_LEASES = -1;

    }
}
