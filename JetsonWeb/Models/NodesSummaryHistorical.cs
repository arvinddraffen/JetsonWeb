using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetsonModels;

namespace JetsonWeb.Models
{
    public class NodesSummaryHistorical
    {
        public Node Node { get; set; }

        public uint Id { get; set; }

        public IList<NodeUtilization> HistoricalUtilization { get; set; }

        public IList<NodePower> HistoricalPower { get; set; }
    }
}
