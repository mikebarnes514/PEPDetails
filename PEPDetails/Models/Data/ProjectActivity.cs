using System;
using System.Collections.Generic;

namespace PEPDetails.Models.Data
{
    public partial class ProjectActivity
    {
        public int Id { get; set; }
        public int ActivityTypeId { get; set; }
        public int ProjectId { get; set; }
        public DateTime ActivityDate { get; set; }
        public string Notes { get; set; }

        public ActivityType ActivityType { get; set; }
        public Project Project { get; set; }
    }
}
