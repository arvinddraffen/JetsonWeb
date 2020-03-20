using System.Linq;
using System.Threading.Tasks;

using JetsonModels;
using JetsonWeb.Data;
using Microsoft.AspNetCore.Mvc;

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
            var cluster = this.db.Clusters.Find(id);
            if (cluster == null)
            {
                return this.NotFound();
            }

            this.ViewData["Nodes"] = cluster.Nodes.ToList();

            return this.View(cluster);
        }

        /// <summary>
        /// Utilization/ClusterUtilizationAdvanced
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="Cluster"/>.</param>
        /// <returns>A <see cref="ViewResult"/> object of the <see cref="Cluster"/> with the specified it.</returns>
        public ActionResult ClusterUtilizationAdvanced(uint id)
        {
            var cluster = this.db.Clusters.Find(id);
            if (cluster == null)
            {
                return this.NotFound();
            }

            this.ViewData["Nodes"] = cluster.Nodes.ToList();

            return this.View(cluster);
        }
    }
}