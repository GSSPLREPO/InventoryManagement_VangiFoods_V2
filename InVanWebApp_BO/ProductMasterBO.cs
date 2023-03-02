using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class ProductMasterBO 
    {
        public int ProductID { get; set; }  
        public string ProductCode { get; set; }   
        public string ProductName { get; set; }  
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; } 
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

    }
}
