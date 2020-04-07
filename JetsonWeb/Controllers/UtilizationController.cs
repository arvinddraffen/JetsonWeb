﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetsonModels.Context;
using JetsonModels.Database;
using JetsonWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JetsonWeb.Controllers
{
    /// <summary>
    /// Defines controllers for the Utilization views.
    /// </summary>
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)] // disable caching
    public class UtilizationController : Controller
    {
        private readonly ClusterContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="UtilizationController"/> class.
        /// Initializes the database context (<see cref="ClusterContext"/> to retrieve data from the database.
        /// </summary>
        /// <param name="dbContext">The database context by which data is retrieved from the database.</param>
        public UtilizationController(ClusterContext dbContext)
        {
            this.db = dbContext;
        }

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

            // Since Utilization and Power statistics are already indexed by Timestamp, retreiving
            // the most recent n-entries is very efficient. Scanning them in-memory to find the most
            // recent for each node ID is faster than taking round-trips the database.
            var searchDepth = cluster.Nodes.Count * 5;

            var recentPowerAll = this.db.PowerData
                   .OrderByDescending(e => e.Timestamp)
                   .Take(searchDepth)
                   .ToList();

            var recentUtilizationAll = this.db.UtilizationData
                .OrderByDescending(e => e.TimeStamp)
                .Take(searchDepth)
                .ToList();

            foreach (var node in cluster.Nodes)
            {
                result.NodeSummaries.Add(new NodeSummary()
                {
                    Node = node,
                    Id = node.Id,
                    RecentPower = recentPowerAll.First(e => e.GlobalNodeId == node.GlobalId),
                    RecentUtilization = recentUtilizationAll.First(e => e.GlobalNodeId == node.GlobalId),
                });
            }

            if (cluster.Nodes.Count() < 1)
            {
                return this.RedirectToAction("Error", "Home");
            }

            this.ViewData["ReportingInterval"] = (uint)cluster.RefreshRate.TotalSeconds;
            this.ViewData["LastUpdated"] = DateTime.Now;

            return this.View(result);
        }

        /// <summary>
        /// Utilization/ClusterUtilizationAdvanced.
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="Cluster"/>.</param>
        /// <returns>A <see cref="ViewResult"/> object of the <see cref="Cluster"/> with the specified id.</returns>
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

            // Since Utilization and Power statistics are already indexed by Timestamp, retreiving
            // the most recent n-entries is very efficient. Scanning them in-memory to find the most
            // recent for each node ID is faster than taking round-trips the database.
            var searchDepth = cluster.Nodes.Count * 5;

            var recentPowerAll = this.db.PowerData
                   .OrderByDescending(e => e.Timestamp)
                   .Take(searchDepth);

            var recentUtilizationAll = this.db.UtilizationData
                .OrderByDescending(e => e.TimeStamp)
                .Take(searchDepth);

            foreach (var node in cluster.Nodes)
            {
                result.NodeSummaries.Add(new NodeSummary()
                {
                    Node = node,
                    Id = node.Id,
                    RecentPower = recentPowerAll.First(e => e.GlobalNodeId == node.GlobalId),
                    RecentUtilization = recentUtilizationAll.First(e => e.GlobalNodeId == node.GlobalId),
                });
            }

            this.ViewData["ReportingInterval"] = (uint)cluster.RefreshRate.TotalSeconds;
            this.ViewData["LastUpdated"] = DateTime.Now;

            return this.View(result);
        }

        /// <summary>
        /// /Utilization/ClusterUtilizationHistorical.
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="Cluster"/>.</param>
        /// <param name="timeRange">The time range over which to return historical data.</param>
        /// <returns>A <see cref="ViewResult"/> object of the <see cref="Cluster"/> with the specified id.</returns>
        public ActionResult ClusterUtilizationHistorical(uint id, string timeRange)
        {
            var cluster = this.db.Clusters
                .Include(c => c.Nodes)
                .First(c => c.Id == id);

            if (cluster == null)
            {
                return this.NotFound();
            }

            var result = new ClusterSummaryHistorical()
            {
                Cluster = cluster,
                NodesSummariesHistorical = new List<NodesSummaryHistorical>(),
            };

            uint reportingInterval = (uint)cluster.RefreshRate.TotalSeconds;

            DateTime start, end;

            switch (timeRange)
            {
                case "day":
                    start = DateTime.Now.AddDays(-1);
                    end = DateTime.Today.AddDays(1);
                    this.ViewBag.RangeStart = start;
                    this.ViewBag.RangeEnd = end;
                    this.ViewBag.RangeString = "Past Day";
                    break;
                case "week":
                    start = DateTime.Now.AddDays(-7);
                    end = DateTime.Today.AddDays(1);
                    this.ViewBag.RangeStart = start;
                    this.ViewBag.RangeEnd = end;
                    this.ViewBag.RangeString = "Past Week";
                    break;
                case "hour":
                default:
                    start = DateTime.Now.AddHours(-1);
                    end = this.ViewBag.RangeEnd = DateTime.Now.AddHours(1);
                    this.ViewBag.RangeStart = start;
                    this.ViewBag.RangeEnd = end;
                    this.ViewBag.RangeString = "Past Hour";
                    break;
            }

            // Setup nodes
            foreach (var node in cluster.Nodes)
            {
                result.NodesSummariesHistorical.Add(new NodesSummaryHistorical()
                {
                    Node = node,
                    Id = node.Id,
                    HistoricalPower = new List<NodePower>(),
                    HistoricalUtilization = new List<NodeUtilization>(),
                });
            }

            // Load all data points that occured at each interval.
            // Select a point from that interval for each node, and append it into the Historical... list.
            const int numberOfDataPoints = 100;

            var intervals = this.GenerateEvenIntervals(start, end, numberOfDataPoints);
            intervals.Add(DateTime.Now); // ensure the last data point is current if possible

            DateTime latestTimestamp = DateTime.Now.AddDays(-14);

            foreach (var interval in intervals)
            {
                var lowerInterval = interval.AddSeconds(-reportingInterval);
                var upperInterval = interval.AddSeconds(reportingInterval);

                var powerEntries = this.db.PowerData
                    .Where(e => e.Timestamp >= lowerInterval && e.Timestamp <= upperInterval)
                    .ToList();

                var utilizationEntries = this.db.UtilizationData
                    .Where(e => e.TimeStamp >= lowerInterval && e.TimeStamp <= upperInterval)
                    .ToList();

                if (powerEntries.Any() && utilizationEntries.Any())
                {
                    // TODO, maybe check for the case where one or more nodes don't have a datapoint at this interval.
                    foreach (var nodeSummary in result.NodesSummariesHistorical)
                    {
                        nodeSummary.HistoricalPower.Add(powerEntries.First(x => x.GlobalNodeId == nodeSummary.Node.GlobalId));
                        nodeSummary.HistoricalUtilization.Add(utilizationEntries.First(x => x.GlobalNodeId == nodeSummary.Node.GlobalId));
                        if (powerEntries.First(x => x.GlobalNodeId == nodeSummary.Node.GlobalId).Timestamp > latestTimestamp)
                        {
                            latestTimestamp = powerEntries.First(x => x.GlobalNodeId == nodeSummary.Node.GlobalId).Timestamp;
                        }
                    }
                }
            }

            this.ViewBag.PowerDataCount = result.NodesSummariesHistorical.First().HistoricalPower.Count();
            this.ViewBag.UtilizationDataCount = result.NodesSummariesHistorical.First().HistoricalUtilization.Count();
            this.ViewBag.ReportingInterval = reportingInterval;
            this.ViewData["LastUpdated"] = latestTimestamp;

            return this.View(result);
        }

        private List<DateTime> GenerateEvenIntervals(DateTime start, DateTime end, int intervalCount)
        {
            var range = end.Subtract(start);
            var intervalLength = range.Ticks / intervalCount;

            var samplingIntervals = Enumerable
                .Range(0, intervalCount)
                .Select(i => start.AddTicks(i * intervalLength))
                .ToList();

            return samplingIntervals;
        }
    }
}