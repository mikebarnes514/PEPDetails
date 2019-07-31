using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PEPDetails.Models.Data;

namespace PEPDetails.Models
{
    public class ProjectDetailViewModel
    {
        public Project ProjectDetail { get; private set; }
        public List<ProjectType> ProjectTypes { get; set; }

        public List<ActivityType> ActivityTypes { get; set; }

        public string ResponsibleAttyImageURL { get { return String.Format("http://intranet.millerjohnson.com/firm%20directory%20images/{0}.jpg", ProjectDetail.ResponsibleAtty.Username); } }
        public string BillingAttyImageURL { get { return String.Format("http://intranet.millerjohnson.com/firm%20directory%20images/{0}.jpg", ProjectDetail.BillingAtty.Username); } }
        
        public decimal BilledAmount
        {
            get
            {
                decimal amount = 0;

                if (ProjectDetail.ProjectStartDate.HasValue)
                    amount = ProjectDetail.BillingHistory.Where(b => b.BillingDate.Ticks >= ProjectDetail.ProjectStartDate.Value.Ticks).Sum(b => b.BilledTotal.Value);
                else
                    amount = ProjectDetail.BillingHistory.Sum(b => b.BilledTotal.Value);

                return amount;
            }
        }

        public decimal RemainingBudget
        {
            get
            {
                decimal total = BilledAmount + (ProjectDetail.Billing.UnbilledTotal.HasValue ? ProjectDetail.Billing.UnbilledTotal.Value : 0);

                return Math.Max(ProjectDetail.Billing.Quote, total) - total;
            }
        }

        public decimal BudgetOverrun
        {
            get
            {
                decimal total = BilledAmount + (ProjectDetail.Billing.UnbilledTotal.HasValue ? ProjectDetail.Billing.UnbilledTotal.Value : 0);
                decimal overrun = 0;

                if (total > ProjectDetail.Billing.Quote)
                    overrun = total - ProjectDetail.Billing.Quote;

                return overrun;
            }
        }

        public List<Stage> ProjectStages
        {
            get
            {
                List<Stage> stages = new List<Stage>();

                foreach(ProjectTask t in ProjectDetail.ProjectTask)
                {
                    if (!stages.Any(s => s.Id == t.Task.StageId))
                        stages.Add(t.Task.Stage);
                }

                return stages.OrderBy(s => s.SortOrder).ToList();
            }
        }

        public ProjectDetailViewModel(Project prj)
        {
            ProjectDetail = prj;
            ProjectTypes = new List<ProjectType>();
            ActivityTypes = new List<ActivityType>();
        }

        public ProjectDetailViewModel()
        {
            ProjectDetail = null;
            ProjectTypes = new List<ProjectType>();
            ActivityTypes = new List<ActivityType>();
        }
    }
}
