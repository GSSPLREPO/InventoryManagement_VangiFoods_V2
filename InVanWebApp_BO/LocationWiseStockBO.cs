using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class LocationWiseStockBO 
    {
        public int ID { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<int> ItemId { get; set; } 
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        //Added below fields for Grid dropdowns
        public string Item_Code { get; set; }
        public string Item_Name { get; set; }        
        public string UnitCode { get; set; }
        public float ItemTaxValue { get; set; }        
        public double UnitPrice { get; set; }
        
    }
}
