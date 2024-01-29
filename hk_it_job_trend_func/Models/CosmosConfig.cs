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
        public const string JOBSDB_KEY_JOBSDB = "/partition_key";

        public const string JOBSDB_CON_JOBSDB_DETAIL = "jobsdb_detail";
        public const string JOBSDB_KEY_JOBSDB_DETAIL = "/partition_key";

        [Obsolete("This function is not using currently")]
        public const string JOBSDB_CON_JOBSDB_META = "jobsdb_meta";

        [Obsolete("This function is not using currently")]
        public const string JOBSDB_KEY_JOBSDB_META = "/id";

        public const string JOBSDB_CON_LEASES = "leases";
        public const string JOBSDB_KEY_LEASES = "/id";
        public const int JOBSDB_TTL_LEASES = -1;

    }
}
