using System;
using System.Collections.Generic;

namespace PEPDetails.Models.Data
{
    public partial class ProjectTask
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Notes { get; set; }

        public Project Project { get; set; }
        public Task Task { get; set; }
    }
}
