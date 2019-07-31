using System;
using System.Collections.Generic;

namespace PEPDetails.Models.Data
{
    public partial class Task
    {
        public Task()
        {
            ProjectTask = new HashSet<ProjectTask>();
        }

        public int Id { get; set; }
        public int StageId { get; set; }
        public string Name { get; set; }
        public string MatterTypeCode { get; set; }
        public bool IsAttorney { get; set; }
        public int StageOrder { get; set; }

        public Stage Stage { get; set; }
        public ICollection<ProjectTask> ProjectTask { get; set; }
    }
}
