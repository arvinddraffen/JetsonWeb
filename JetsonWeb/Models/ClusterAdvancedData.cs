using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JetsonWeb.Models
{
    public class ClusterAdvancedData
    {
        public uint ClusterID { get; set; }

        public string TimeStamp { get; set; }

        public List<string> PerNodeLastDataReceived { get; set; }

        public List<float> PerNodeVoltageUtilization { get; set; }

        public List<string> NodeIDs { get; set; }

        public List<string> NodeIpAddresses { get; set; }

        public List<string> NodeOperatingSystems { get; set; }

        public List<string> NodeUpTimes { get; set; }
    }
}
