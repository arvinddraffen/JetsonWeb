using System.Collections.Generic;

using JetsonModels.Database;

namespace JetsonWeb.Models
{
    public class ClusterSummary
    {
        public Cluster Cluster { get; set; }

        public IList<NodeSummary> NodeSummaries { get; set; }
    }
}
