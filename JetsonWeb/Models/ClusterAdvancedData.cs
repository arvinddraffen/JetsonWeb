using System.Collections.Generic;

namespace JetsonWeb.Models
{
    /// <summary>
    /// Data to be sent to the advanced real-time view for the cluster.
    /// </summary>
    public class ClusterAdvancedData
    {
        /// <summary>
        /// Gets or sets the Id of the cluster for which to send utilization statistics.
        /// </summary>
        public uint ClusterID { get; set; }

        /// <summary>
        /// Gets or sets the timestamp at which the earliest data is reported from any node's latest data.
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the time for each node at which it last reported data.
        /// </summary>
        public List<string> PerNodeLastDataReceived { get; set; }

        /// <summary>
        /// Gets or sets a list of the latest voltage utilization points for each node.
        /// </summary>
        public List<float> PerNodeVoltageUtilization { get; set; }

        /// <summary>
        /// Gets or sets a list of the Ids for each node.
        /// </summary>
        public List<string> NodeIDs { get; set; }

        /// <summary>
        /// Gets or sets a list of the IP Addresses for each node.
        /// </summary>
        public List<string> NodeIpAddresses { get; set; }

        /// <summary>
        /// Gets or sets a list of the Operating Systems running on each node.
        /// </summary>
        public List<string> NodeOperatingSystems { get; set; }

        /// <summary>
        /// Gets or sets a list of the uptime for each node.
        /// </summary>
        public List<string> NodeUpTimes { get; set; }
    }
}
