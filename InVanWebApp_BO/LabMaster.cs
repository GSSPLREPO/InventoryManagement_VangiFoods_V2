using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class LabMaster
    {
        public int LabID { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<int> FGSID { get; set; }
        public string LabName { get; set; }
        public string LabReport { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        public virtual FinishGoodSery FinishGoodSery { get; set; }
        public virtual LocationMasterBO LocationMaster { get; set; }
    }
}
