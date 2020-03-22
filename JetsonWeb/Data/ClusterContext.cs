using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetsonModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace JetsonWeb.Data
{
    /// <summary>
    /// <see cref="ClusterContext"/> provides an interface to the SQLite database with EntityFramework.
    /// </summary>
    public class ClusterContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterContext"/> class.
        /// </summary>
        public ClusterContext()
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        /// <summary>
        /// Gets or sets the list of clusters (<see cref="Cluster"/>) which constitute the system.
        /// </summary>
        public DbSet<Cluster> Clusters { get; set; }

        public DbSet<NodePower> PowerData { get; set; }

        public DbSet<NodeUtilization> UtilizationData { get; set; }

        public static readonly Microsoft.Extensions.Logging.LoggerFactory _myLoggerFactory =
            new Microsoft.Extensions.Logging.LoggerFactory(new[] {
                new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider(),
            });

        /// <inheritdoc/>
        /// <param name="options"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.
                UseSqlite("DataSource=data.db");
        }

        /// <inheritdoc/>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NodeUtilization>()
                .Property(x => x.Cores)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<ICollection<CpuCore>>(v));

            // Index on NodeUtilization to speedup lookups
            modelBuilder.Entity<NodeUtilization>()
                .HasIndex(x => x.GlobalNodeId);

            // Index on NodePower to speedup lookups
            modelBuilder.Entity<NodePower>()
                .HasIndex(x => x.GlobalNodeId);
        }
    }
}
