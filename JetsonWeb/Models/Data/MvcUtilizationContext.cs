using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JetsonWeb.Models.Data
{
    public class MvcUtilizationContext : DbContext
    {
        public MvcUtilizationContext(DbContextOptions<MvcUtilizationContext> options)
            : base(options)
        {
        }

        // create DbSet<Utilization> property for entity set
        public DbSet<Utilization> UtilizationInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Utilization>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
