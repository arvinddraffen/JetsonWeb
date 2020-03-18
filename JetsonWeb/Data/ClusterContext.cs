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
        }

        /// <summary>
        /// Gets or sets the list of clusters (<see cref="Cluster"/>) which constitute the system.
        /// </summary>
        public DbSet<Cluster> Clusters { get; set; }

        /// <inheritdoc/>
        /// <param name="options"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("DataSource=data.db");

        /// <inheritdoc/>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NodeUtilization>()
                .Property(x => x.Cores)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<ICollection<CpuCore>>(v));
        }
    }
}
