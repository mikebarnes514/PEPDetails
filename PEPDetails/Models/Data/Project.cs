using System;
using System.Collections.Generic;

namespace PEPDetails.Models.Data
{
    public partial class Project
    {
        public Project()
        {
            BillingHistory = new HashSet<BillingHistory>();
            ProjectActivity = new HashSet<ProjectActivity>();
            ProjectTask = new HashSet<ProjectTask>();
        }

        public int Id { get; set; }
        public int ProjectTypeId { get; set; }
        public int Cmimid { get; set; }
        public int ResponsibleAttyId { get; set; }
        public int BillingAttyId { get; set; }
        public int? CurrentTaskId { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? InitialMeetingDate { get; set; }
        public DateTime? SigningDeadline { get; set; }
        public DateTime? SigningDate { get; set; }
        public DateTime? MailingDate { get; set; }
        public DateTime? LastTaskDate { get; set; }
        public string Notes { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? CompletionDate { get; set; }

        public Staff BillingAtty { get; set; }
        public Cmimdata Cmim { get; set; }
        public ProjectType ProjectType { get; set; }
        public Staff ResponsibleAtty { get; set; }
        public Billing Billing { get; set; }
        public ICollection<BillingHistory> BillingHistory { get; set; }
        public ICollection<ProjectActivity> ProjectActivity { get; set; }
        public ICollection<ProjectTask> ProjectTask { get; set; }
    }
}
