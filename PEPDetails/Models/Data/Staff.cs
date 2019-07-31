using System;
using System.Collections.Generic;

namespace PEPDetails.Models.Data
{
    public partial class Staff
    {
        public Staff()
        {
            ProjectBillingAtty = new HashSet<Project>();
            ProjectResponsibleAtty = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string FullName { get; set; }

        public ICollection<Project> ProjectBillingAtty { get; set; }
        public ICollection<Project> ProjectResponsibleAtty { get; set; }
    }
}
