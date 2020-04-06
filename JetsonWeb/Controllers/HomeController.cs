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
    [ResponseCache(NoStore =true, Location =ResponseCacheLocation.None)] // disable caching
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ClusterContext db;

        public HomeController(ILogger<HomeController> logger, ClusterContext dbContext)
        {
            this.logger = logger;
            this.db = dbContext;
        }

        public IActionResult Index()
        {
            var clusters = this.db.Clusters.AsNoTracking().ToList();
            return this.View(clusters);
        }

        public IActionResult About()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
