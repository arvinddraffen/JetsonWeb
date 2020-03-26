using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetsonModels;

namespace JetsonWeb.Models
{
    public class ClusterSummaryHistorical
    {
        public Cluster Cluster { get; set; }

        public IList<NodesSummaryHistorical> NodesSummariesHistorical { get; set; }
    }
}
