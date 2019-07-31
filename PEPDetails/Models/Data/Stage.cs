using System;
using System.Collections.Generic;

namespace PEPDetails.Models.Data
{
    public partial class Stage
    {
        public Stage()
        {
            Task = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }

        public ICollection<Task> Task { get; set; }
    }
}
