using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class RequestForQuotationItemDetailsBO
    {
        public int RFQdetailsId { get; set; } 
        public int RequestForQuotationId { get; set; }
        [Required(ErrorMessage = "Please select the Delivery Start date!")] 
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        
        public Nullable<decimal> Quantity { get; set; }
        [Required(ErrorMessage = "Enter remakrs!")] ////added 
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedByID { get; set; }
        public Nullable<System.DateTime> CreatedByDate { get; set; }
        public Nullable<int> LastModifiedByID { get; set; }
        public Nullable<System.DateTime> LastModifiedByDate { get; set; }
        //==============Rahul: These fields are for Request For Quotation details 14/12/2022==============//
        public Nullable<int> Item_ID { get; set; }
        public string ItemName { get; set; }        
        public string Item_Code { get; set; }        
        public string ItemUnit { get; set; }
        public Nullable<decimal> TotalItemCost { get; set; }
        public string HSN_Code { get; set; }            
        
        
        
    }
}
