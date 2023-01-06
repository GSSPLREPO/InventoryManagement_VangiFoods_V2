using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class InquiryFormItemDetailsBO 
    {
        public int InquiryDetailsId { get; set; }  
        public int InquiryID { get; set; }
        [Required(ErrorMessage = "Please select the Delivery Start date!")] 
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        
        public Nullable<decimal> ItemQuantity { get; set; }  
        [Required(ErrorMessage = "Enter remakrs!")] ////added 
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; } 
        public Nullable<System.DateTime> CreatedDate { get; set; } 
        public Nullable<int> LastModifiedBy { get; set; } 
        public Nullable<System.DateTime> LastModifiedDate { get; set; } 
        //==============Rahul: These fields are for Request For Quotation details 03/01/2023.==============//
        public Nullable<int> Item_ID { get; set; }
        public string ItemName { get; set; }        
        public string Item_Code { get; set; }        
        public string ItemUnit { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public Nullable<double> QuotedPrice { get; set; }
        public Nullable<double> ExpectedPrice { get; set; }
        public Nullable<double> CloserPrice { get; set; }
        public Nullable<decimal> ItemTaxValue { get; set; } 
        public Nullable<decimal> TotalItemCost { get; set; }
        public string HSN_Code { get; set; }        
        public int CurrencyID { get; set; } 
        public string CurrencyName { get; set; }
        

    }
}
