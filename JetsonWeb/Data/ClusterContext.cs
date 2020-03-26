using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JetsonModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
            options
               // .UseLoggerFactory(_myLoggerFactory)
                .UseSqlite("DataSource=data.db");
        }

        /// <inheritdoc/>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.SetupPrimaryKeys(modelBuilder);
            this.SetupValueConversions(modelBuilder);
            this.SetupIndexes(modelBuilder);
        }

        /// <summary>
        /// Assigns primary keys and associated database generation strategies to the storage types defined in <see cref="JetsonModels"/>.
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void SetupPrimaryKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cluster>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Node>()
                .HasKey(x => x.GlobalId);
            modelBuilder.Entity<Node>()
                .Property(x => x.GlobalId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<NodePower>()
              .HasKey(x => x.Id);
            modelBuilder.Entity<NodePower>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<NodeUtilization>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<NodeUtilization>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }

        /// <summary>
        /// Assigns <see cref="ValueConverter"/>s to properties in <see cref="JetsonModels"/> to optimize storage in SQLite.
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void SetupValueConversions(ModelBuilder modelBuilder)
        {

            // Setup value conversion for CpuCore since CPU cores don't need primary keys
            modelBuilder.Entity<NodeUtilization>()
                .Property(x => x.Cores)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<ICollection<CpuCore>>(v));

            // Store Timestamps as Integer type in SQLite, not TEXT.
            // Human readability is not critical, but ordering and lookup speed by relative time is.
            modelBuilder.Entity<NodeUtilization>()
                .Property(x => x.TimeStamp)
                .HasConversion(new DateTimeToBinaryConverter());

            modelBuilder.Entity<NodePower>()
                .Property(x => x.Timestamp)
                .HasConversion(new DateTimeToBinaryConverter());
        }

        /// <summary>
        /// Creates database indexes to speedup lookups on frequently used properties in <see cref="JetsonModels"/>.
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void SetupIndexes(ModelBuilder modelBuilder)
        {
            // Index on NodeUtilization GlobalNodeId to speedup lookups
            modelBuilder.Entity<NodeUtilization>()
                .HasIndex(x => x.GlobalNodeId);

            // Index on NodePower GlobalNodeId to speedup lookups
            modelBuilder.Entity<NodePower>()
                .HasIndex(x => x.GlobalNodeId);

            // Index on NodeUtilization TimeStamp to speedup lookups
            modelBuilder.Entity<NodeUtilization>()
                .HasIndex(x => x.TimeStamp);

            // Index on NodePower TimeStamp to speedup lookups
            modelBuilder.Entity<NodePower>()
                .HasIndex(x => x.Timestamp);
        }
    }
}
