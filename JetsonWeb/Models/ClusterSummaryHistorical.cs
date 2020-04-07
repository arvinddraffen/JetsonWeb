using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetsonModels.Database;

namespace JetsonWeb.Models
{
    /// <summary>
    /// A model for providing historical data for a particular <see cref="Cluster"/>.
    /// </summary>
    public class ClusterSummaryHistorical
    {
        /// <summary>
        /// Gets or sets the <see cref="Cluster"/> for which this historical cluster data point is for.
        /// </summary>
        public Cluster Cluster { get; set; }

        /// <summary>
        /// Gets or sets a list of historical node summary points (constituted by the <see cref="Node"/>s in the cluster.
        /// </summary>
        public IList<NodesSummaryHistorical> NodesSummariesHistorical { get; set; }
    }
}
