using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JetsonWeb.Models
{
    /// <summary>
    /// Constructor for <see cref="Utilization"/> model
    /// </summary>
    /// <remarks>
    /// Contents based on default report contents as listed in: https://slurm.schedmd.com/sreport.html
    /// </remarks>
    public class Utilization
    {
        /// <summary>
        /// Gets the cluster for which the utilization report is generated.
        /// </summary>
        public string Cluster { get; }

        /// <summary>
        /// Gets the time (as a percentage) nodes were in use with active jobs or an active reservation.
        /// </summary>
        public float Allocated { get; }

        /// <summary>
        /// Gets the time (as a percentage) that nodes were marked Down or time that clurmctld was not responding.
        /// (assuming TrackSlurmctldDown is set in slurmdbd.conf)
        /// </summary>
        public float Down { get; }

        /// <summary>
        /// Gets the time (as a percentage) that nodes were in use by a reservation created with the MAINT.
        /// flag but not the IGNORE_JOBS flag.
        /// </summary>
        public float PlannedDown { get; }

        /// <summary>
        /// Gets the time (as a percentage) where nodes had no active jobs or reservations.
        /// </summary>
        public float Idle { get; }

        /// <summary>
        /// Gets the time (as a percentage) that a node spent idle with eligible jobs in the queue that were unable
        /// to start due to time or size constraints.
        /// </summary>
        public float Reserved { get; }

        /// <summary>
        /// Gets the total time (as a percentage) reported by all fields in the utilization report.
        /// </summary>
        public float Reported { get; }

        /// <summary>
        /// Gets the start time of the report in DateTime format.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; }

        /// <summary>
        /// Gets the end time of the report in DateTime format.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; }
    }
}
