using System;
using System.Collections.Generic;

namespace PEPDetails.Models.Data
{
    public partial class Billing
    {
        public int ProjectId { get; set; }
        public decimal Quote { get; set; }
        public decimal UnbilledTime { get; set; }
        public decimal UnbilledCost { get; set; }
        public decimal? UnbilledTotal { get; set; }
        public DateTime? LastBillDate { get; set; }

        public Project Project { get; set; }
    }
}
