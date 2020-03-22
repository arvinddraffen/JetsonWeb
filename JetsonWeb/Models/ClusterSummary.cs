using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetsonModels;

namespace JetsonWeb.Models
{
    public class ClusterSummary
    {
        public Cluster Cluster { get; set; }

        public IList<NodeSummary> NodeSummaries { get; set; }
    }
}
