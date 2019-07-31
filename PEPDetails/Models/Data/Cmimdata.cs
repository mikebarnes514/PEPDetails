using System;
using System.Collections.Generic;

namespace PEPDetails.Models.Data
{
    public partial class Cmimdata
    {
        public Cmimdata()
        {
            Project = new HashSet<Project>();
        }

        public int Id { get; set; }
        public int ClientId { get; set; }
        public int MatterId { get; set; }
        public string MatterTypeCode { get; set; }
        public string ClientName { get; set; }
        public string MatterName { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactAddressLine1 { get; set; }
        public string ContactAddressLine2 { get; set; }
        public string ContactAddressCity { get; set; }
        public string ContactAddressState { get; set; }
        public string ContactAddressPostalCode { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ClientMatterId { get; set; }

        public ICollection<Project> Project { get; set; }
    }
}
