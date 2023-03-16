using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class ProductionIndent_DetailsBO 
    {
        public int ID { get; set; }
        public Nullable<int> ProductionIndentID { get; set; } 
        public Nullable<int> QCcheck_1 { get; set; } 
        public Nullable<int> QCcheck_2 { get; set; } 
        public Nullable<int> QCcheck_3 { get; set; }  
        public string ProductionCheck { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        //Added for fetching Item details in Recipe Master
        public Nullable<int> ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }        
        public double BatchQty { get; set; } 
        public double FinalQty { get; set; }  
        public string ItemUnit { get; set; }        
        public double Percentage { get; set; } 
    }
}
