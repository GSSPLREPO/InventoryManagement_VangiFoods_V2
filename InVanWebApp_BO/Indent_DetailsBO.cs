using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class Indent_DetailsBO
    {
        public int ID { get; set; }
        public Nullable<int> IndentID { get; set; }
        public Nullable<int> ItemId { get; set; }
        public Nullable<double> RequiredQuantity { get; set; }
        public Nullable<double> SentQuantity { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        
        //Added for fetching Item details in PO
        public string ItemName { get; set; }
    }
}
