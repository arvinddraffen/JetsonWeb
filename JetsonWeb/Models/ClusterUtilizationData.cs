using System.Collections.Generic;

using JetsonModels.Database;

namespace JetsonWeb.Models
{
    /// <summary>
    /// Holds data used by the views for <see cref="Cluster"/> utilization statistics.
    /// </summary>
    public class ClusterUtilizationData
    {
        /// <summary>
        /// Gets or sets the Cluster Id of the <see cref="Cluster"/>.
        /// </summary>
        public uint ClusterID { get; set; }

        /// <summary>
        /// Gets or sets the timestamp at when the <see cref="Cluster"/> data was reported.
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the total memory usage of the <see cref="Cluster"/> in GB.
        /// </summary>
        public float ClusterMemoryUsedGB { get; set; }

        /// <summary>
        /// Gets or sets the total memory free in the <see cref="Cluster"/> in GB.
        /// </summary>
        public float ClusterMemoryAvailableGB { get; set; }

        /// <summary>
        /// Gets or sets the local Node IDs for the <see cref="Cluster"/>, used for ChartJS labels.
        /// </summary>
        public List<string> NodeIDs { get; set; }

        /// <summary>
        /// Gets or sets the maximum memory for any given <see cref="Node"/> in the <see cref="Cluster"/>.
        /// </summary>
        public float MaxNodeMemoryGB { get; set; }

        /// <summary>
        /// Gets or sets a list of memory used by each <see cref="Node"/> in the <see cref="Cluster"/>.
        /// </summary>
        public List<float> PerNodeMemoryUsedGB { get; set; }

        /// <summary>
        /// Gets or sets a list of the amount of free memory in each <see cref="Node"/> in the <see cref="Cluster"/>.
        /// </summary>
        public List<float> PerNodeMemoryAvailableGB { get; set; }

        /// <summary>
        /// Gets or sets a list of the current (A) used by each <see cref="Node"/> in the <see cref="Cluster"/>.
        /// </summary>
        public List<float> PerNodeCurrentUtilization { get; set; }

        /// <summary>
        /// Gets or sets a list of the power consumption (W) used by each <see cref="Node"/> in the <see cref="Cluster"/>.
        /// </summary>
        public List<float> PerNodePowerUtilization { get; set; }

        /// <summary>
        /// Gets or sets a list of the CPU utilization for each <see cref="Node"/> in the <see cref="Cluster"/>, organized by <see cref="CpuCore"/>.
        /// </summary>
        public List<List<float>> PerNodeCpuUtilization { get; set; }
    }
}
