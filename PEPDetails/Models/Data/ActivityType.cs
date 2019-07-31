using System;
using System.Collections.Generic;

namespace PEPDetails.Models.Data
{
    public partial class ActivityType
    {
        public ActivityType()
        {
            ProjectActivity = new HashSet<ProjectActivity>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public ICollection<ProjectActivity> ProjectActivity { get; set; }
    }
}
