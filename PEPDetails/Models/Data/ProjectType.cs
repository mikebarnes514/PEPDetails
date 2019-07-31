using System;
using System.Collections.Generic;

namespace PEPDetails.Models.Data
{
    public partial class ProjectType
    {
        public ProjectType()
        {
            Project = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Project> Project { get; set; }
    }
}
