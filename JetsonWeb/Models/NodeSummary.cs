using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetsonModels.Database;

namespace JetsonWeb.Models
{
    public class NodeSummary
    {
        public Node Node { get; set; }

        public uint Id { get; set; }

        public NodeUtilization RecentUtilization { get; set; }

        public NodePower RecentPower { get; set; }
    }
}
