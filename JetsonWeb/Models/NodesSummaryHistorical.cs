using System.Collections.Generic;

using JetsonModels.Database;

namespace JetsonWeb.Models
{
    /// <summary>
    /// A model for providing historical data for a particular <see cref="Node"/>.
    /// </summary>
    public class NodesSummaryHistorical
    {
        /// <summary>
        /// Gets or sets the <see cref="Node"/> for which this historical node data point is for.
        /// </summary>
        public Node Node { get; set; }

        /// <summary>
        /// Gets or sets the Id of the <see cref="Node"/> for which this historical node utilization point is for.
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets a list of historical node utilization points for the <see cref="Node"/>.
        /// </summary>
        public IList<NodeUtilization> HistoricalUtilization { get; set; }

        /// <summary>
        /// Gets or sets the list of historical power points for the <see cref="Node"/>.
        /// </summary>
        public IList<NodePower> HistoricalPower { get; set; }
    }
}
