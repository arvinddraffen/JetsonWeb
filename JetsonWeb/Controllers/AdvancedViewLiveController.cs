﻿using System;
using System.Collections.Generic;
using System.Linq;

using JetsonModels.Context;
using JetsonModels.Database;
using JetsonWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JetsonWeb.Controllers
{
    /// <summary>
    /// Provides data to the Advanced real-time view for the <see cref="Cluster"/> for dynamic page updating.
    /// </summary>
    public class AdvancedViewLiveController : Controller
    {
        private readonly ClusterContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedViewLiveController"/> class.
        /// Provides data to the advanced real-time view for the <see cref="Cluster"/> for dynamic page updating.
        /// </summary>
        /// <param name="dbContext">Database context by which <see cref="Cluster"/> data from the database is retrieved.</param>
        public AdvancedViewLiveController(ClusterContext dbContext)
        {
            this.db = dbContext;
        }

        /// <summary>
        /// Returns data for the real-time advanced view to allow dynamic page updating.
        /// </summary>
        /// <param name="id">The id of the <see cref="Cluster"/> for which to return data.</param>
        /// <returns>A JSON encoded version of the latest <see cref="Cluster"/> data for the advanced view.</returns>
        public ActionResult<ClusterAdvancedData> GetAdvancedClusterUtilization(uint id)
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

            List<float> perNodeVoltageUtilization = new List<float>();
            List<string> nodeIDs = new List<string>();
            List<string> nodeIpAddresses = new List<string>();
            List<string> nodeOperatingSystems = new List<string>();
            List<string> nodeUpTimes = new List<string>();
            List<string> perNodeLastDataReceived = new List<string>();
            DateTime timeStamp = DateTime.Now;

            foreach (var node in cluster.Nodes)
            {
                NodePower recentPower = recentPowerAll.First(e => e.GlobalNodeId == node.GlobalId);
                NodeUtilization recentUtilization = recentUtilizationAll.First(e => e.GlobalNodeId == node.GlobalId);
                perNodeVoltageUtilization.Add(recentPower.Voltage);
                nodeIDs.Add(node.Id.ToString());
                nodeIpAddresses.Add(node.IPAddress);
                nodeOperatingSystems.Add(node.OperatingSystem);
                nodeUpTimes.Add(string.Format("{0:D3}:{1:D2}:{2:D2}:{3:D2}", (int)node.UpTime.TotalDays, node.UpTime.Hours, node.UpTime.Minutes, node.UpTime.Seconds));

                if (DateTime.Compare(recentPower.Timestamp, timeStamp) < 0)
                {
                    timeStamp = recentPower.Timestamp;
                }

                if (DateTime.Compare(recentPower.Timestamp, recentUtilization.TimeStamp) < 0)
                {
                    perNodeLastDataReceived.Add(recentPower.Timestamp.ToString("MM/dd/yy hh:mm:ss tt"));
                }
                else
                {
                    perNodeLastDataReceived.Add(recentUtilization.TimeStamp.ToString("MM/dd/yy hh:mm:ss tt"));
                }
            }

            ClusterAdvancedData clusterAdvancedData = new ClusterAdvancedData();
            clusterAdvancedData.ClusterID = cluster.Id;

            clusterAdvancedData.PerNodeVoltageUtilization = perNodeVoltageUtilization;
            clusterAdvancedData.NodeIDs = nodeIDs;
            clusterAdvancedData.NodeIpAddresses = nodeIpAddresses;
            clusterAdvancedData.NodeOperatingSystems = nodeOperatingSystems;
            clusterAdvancedData.NodeUpTimes = nodeUpTimes;
            clusterAdvancedData.TimeStamp = timeStamp.ToShortDateString() + " " + timeStamp.ToLongTimeString();
            clusterAdvancedData.PerNodeLastDataReceived = perNodeLastDataReceived;
            return clusterAdvancedData;
        }
    }
}
