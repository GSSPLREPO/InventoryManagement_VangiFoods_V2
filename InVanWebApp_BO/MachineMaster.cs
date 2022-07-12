using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class MachineMaster
    {
        public int MachineID { get; set; }
        public string MachineName { get; set; }
        public Nullable<System.DateTime> InstallationDate { get; set; }
        public Nullable<int> LocationID { get; set; }
        public string Description { get; set; }
        public string ManufacturerName { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonMobileNo { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        public virtual LocationMaster LocationMaster { get; set; }
    }
}
