using System;
using System.Collections.Generic;

namespace PEPDetails.Models.Data
{
    public partial class BillingHistory
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public decimal BillingYear { get; set; }
        public decimal BillingMonth { get; set; }
        public decimal BilledTime { get; set; }
        public decimal BilledCost { get; set; }
        public decimal? BilledTotal { get; set; }
        public DateTime BillingDate { get; set; }

        public Project Project { get; set; }
    }
}
