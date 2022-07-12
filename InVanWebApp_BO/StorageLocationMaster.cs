using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class StorageLocationMaster
    {
        public int StorageLocationID { get; set; }
        public int LocationID { get; set; }
        public string StorageLocationName { get; set; }
        public string StorageDescription { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        public virtual LocationMaster LocationMaster { get; set; }
    }
}
