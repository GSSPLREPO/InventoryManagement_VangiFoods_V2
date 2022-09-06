using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class OrganisationGroupBO
    {
        public int OrganisationGroupId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Logo { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
