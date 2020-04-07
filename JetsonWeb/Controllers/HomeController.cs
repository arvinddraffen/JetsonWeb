using System.Diagnostics;
using System.Linq;
using JetsonModels;
using JetsonModels.Context;
using JetsonWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JetsonWeb.Controllers
{
    /// <summary>
    /// Generates views for the frontpage, about page, and error pages of JetsonWeb.
    /// </summary>
    [ResponseCache(NoStore =true, Location =ResponseCacheLocation.None)] // disable caching
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ClusterContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// Initializes the database context for data retrieval.
        /// </summary>
        /// <param name="logger">Can use used for logging, if required.</param>
        /// <param name="dbContext">The database context used to retrieve Cluster data.</param>
        public HomeController(ILogger<HomeController> logger, ClusterContext dbContext)
        {
            this.logger = logger;
            this.db = dbContext;
        }

        /// <summary>
        /// Provides data for the frontpage of JetsonWeb.
        /// </summary>
        /// <returns>A view of clusters to populate the listing of clusters on the frontpage.</returns>
        public IActionResult Index()
        {
            var clusters = this.db.Clusters.AsNoTracking().ToList();
            return this.View(clusters);
        }

        /// <summary>
        /// Provides data for the about page of JetsonWeb.
        /// </summary>
        /// <returns>A generic view which is not based on a particular model.</returns>
        public IActionResult About()
        {
            return this.View();
        }

        /// <summary>
        /// Provides data for the error page of JetsonWeb.
        /// </summary>
        /// <returns>A default view of <see cref="ErrorViewModel"/>.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
