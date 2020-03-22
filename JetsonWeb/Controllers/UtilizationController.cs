using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetsonModels;
using JetsonWeb.Data;
using JetsonWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JetsonWeb.Controllers
{
    /// <summary>
    /// Defines controllers for the Utilization views.
    /// </summary>
    public class UtilizationController : Controller
    {
        private ClusterContext db = new ClusterContext();

        /// <summary>
        /// Utilization/ClusterUtilization.
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="Cluster"/>.</param>
        /// <returns>A <see cref="ViewResult"/> object of the <see cref="Cluster"/> with the specified id.</returns>
        public ActionResult ClusterUtilization(uint id)
        {
            var cluster = this.db.Clusters
                .Include(c => c.Nodes)
                .First(c => c.Id == id);

            if (cluster == null)
            {
                return this.NotFound();
            }

            var result = new ClusterSummary()
            {
                Cluster = cluster,
                NodeSummaries = new List<NodeSummary>(),
            };

            foreach (var node in cluster.Nodes)
            {
                result.NodeSummaries.Add(new NodeSummary()
                {
                    Node = node,
                    Id = node.Id,
                    RecentPower = this.db.PowerData.Where(e => e.GlobalNodeId == node.GlobalId).OrderByDescending(e => e.Timestamp).First(),
                    RecentUtilization = this.db.UtilizationData.Where(e => e.GlobalNodeId == node.GlobalId).OrderByDescending(e => e.TimeStamp).First(),
                });
            }

            return this.View(result);
        }

        /// <summary>
        /// Utilization/ClusterUtilizationAdvanced
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="Cluster"/>.</param>
        /// <returns>A <see cref="ViewResult"/> object of the <see cref="Cluster"/> with the specified it.</returns>
        public ActionResult ClusterUtilizationAdvanced(uint id)
        {
            var cluster = this.db.Clusters
                .Include(c => c.Nodes)
                .First(c => c.Id == id);

            if (cluster == null)
            {
                return this.NotFound();
            }

            var result = new ClusterSummary()
            {
                Cluster = cluster,
                NodeSummaries = new List<NodeSummary>(),
            };

            foreach (var node in cluster.Nodes)
            {
                result.NodeSummaries.Add(new NodeSummary()
                {
                    Node = node,
                    Id = node.Id,
                    RecentPower = this.db.PowerData.Where(e => e.GlobalNodeId == node.GlobalId).OrderByDescending(e => e.Timestamp).First(),
                    RecentUtilization = this.db.UtilizationData.Where(e => e.GlobalNodeId == node.GlobalId).OrderByDescending(e => e.TimeStamp).First(),
                });
            }

            return this.View(result);
        }
    }
}