using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JetsonModels;
using JetsonModels.Context;
using JetsonModels.Database;
using JetsonWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JetsonWeb.Controllers
{
    /// <summary>
    /// Provides data to the real-time <see cref="Cluster"/> data view for dynamic page updating.
    /// </summary>
    public class GraphDataLiveViewController : Controller
    {
        private readonly ClusterContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphDataLiveViewController"/> class.
        /// Provides data to the real-time <see cref="Cluster"/> data view for dynamic page updating.
        /// </summary>
        /// <param name="dbContext">Context for the database containing <see cref="Cluster"/> data.</param>
        public GraphDataLiveViewController(ClusterContext dbContext)
        {
            this.db = dbContext;
        }

        /// <summary>
        /// Returns data for the real-time data view to allow dynamic page updating.
        /// </summary>
        /// <param name="id">The id of the cluster for which to return data.</param>
        /// <returns>A JSON encoded version of the latest <see cref="Cluster"/> data.</returns>
        public ActionResult<ClusterUtilizationData> GetClusterUtilization(uint id)
        {
            var cluster = this.db.Clusters
                .Include(c => c.Nodes)
                .First(c => c.Id == id);

            if (cluster == null)
            {
                return this.NotFound();
            }

            var searchDepth = cluster.Nodes.Count * 5;

            var recentPowerAll = this.db.PowerData
                   .OrderByDescending(e => e.Timestamp)
                   .Take(searchDepth)
                   .ToList();

            var recentUtilizationAll = this.db.UtilizationData
                .OrderByDescending(e => e.TimeStamp)
                .Take(searchDepth)
                .ToList();

            List<string> nodeIds = new List<string>();
            List<float> perNodeMemoryUsedGB = new List<float>();
            List<float> perNodeMemoryAvailableGB = new List<float>();
            List<float> perNodePowerUtilization = new List<float>();
            List<float> perNodeCurrentUtilization = new List<float>();
            List<List<float>> perNodeCPUUtilization = new List<List<float>>(recentUtilizationAll.First().Cores.Count());
            for (int i = 0; i < recentUtilizationAll.First().Cores.Count(); i++)
            {
                perNodeCPUUtilization.Add(new List<float>());
            }

            uint clusterMemoryUsed = 0;
            uint clusterMemoryAvailable = 0;
            uint maxNodeMemory = 0;
            DateTime timeStamp = DateTime.Now;

            foreach (var node in cluster.Nodes)
            {
                NodeUtilization recentUtilization = recentUtilizationAll.First(e => e.GlobalNodeId == node.GlobalId);
                NodePower recentPower = recentPowerAll.First(e => e.GlobalNodeId == node.GlobalId);
                uint memoryUsed = recentUtilization.MemoryUsed;
                uint memoryAvailable = recentUtilization.MemoryAvailable;

                perNodeMemoryUsedGB.Add((float)memoryUsed / 1024);
                perNodeMemoryAvailableGB.Add((float)memoryAvailable / 1024);

                clusterMemoryUsed += memoryUsed;
                clusterMemoryAvailable += memoryAvailable;

                if ((memoryUsed + memoryAvailable) > maxNodeMemory)
                {
                    maxNodeMemory = memoryUsed + memoryAvailable;
                }

                float powerUtilization = recentPower.Power;
                perNodePowerUtilization.Add(powerUtilization);

                float currentUtilization = recentPower.Current;
                perNodeCurrentUtilization.Add(currentUtilization);

                List<float> perCoreCPUUsage = new List<float>();
                foreach (var core in recentUtilization.Cores)
                {
                    perCoreCPUUsage.Add(core.UtilizationPercentage / recentUtilization.Cores.Count());
                }

                for (int i = 0; i < perCoreCPUUsage.Count(); i++)
                {
                    perNodeCPUUtilization[i].Add(perCoreCPUUsage[i]);
                }

                nodeIds.Add(node.Id.ToString());

                if (DateTime.Compare(recentUtilization.TimeStamp, timeStamp) < 0)
                {
                    timeStamp = recentUtilization.TimeStamp;
                }
            }

            ClusterUtilizationData clusterUtilization = new ClusterUtilizationData();
            clusterUtilization.ClusterID = cluster.Id;
            clusterUtilization.NodeIDs = nodeIds;
            clusterUtilization.ClusterMemoryUsedGB = (float)clusterMemoryUsed / 1024F;
            clusterUtilization.ClusterMemoryAvailableGB = (float)clusterMemoryAvailable / 1024F;
            clusterUtilization.MaxNodeMemoryGB = (float)maxNodeMemory / 1024F;
            clusterUtilization.PerNodeMemoryUsedGB = perNodeMemoryUsedGB;
            clusterUtilization.PerNodeMemoryAvailableGB = perNodeMemoryAvailableGB;
            clusterUtilization.PerNodePowerUtilization = perNodePowerUtilization;
            clusterUtilization.PerNodeCurrentUtilization = perNodeCurrentUtilization;
            clusterUtilization.PerNodeCpuUtilization = perNodeCPUUtilization;
            clusterUtilization.TimeStamp = timeStamp.ToShortDateString() + " " + timeStamp.ToLongTimeString();

            return clusterUtilization;
        }
    }
}