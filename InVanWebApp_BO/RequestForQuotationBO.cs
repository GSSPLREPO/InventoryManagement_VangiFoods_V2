using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class RequestForQuotationBO
    {
        public int RequestForQuotationId { get; set; }
        ///public Nullable<int> OrganisationId { get; set; } ///Rahul removed 'OrganisationId' 17-12-2022.  
        ///public Nullable<int> BranchId { get; set; } ///Rahul removed 'BranchId' 17-12-2022.  
        public string RFQNO { get; set; }
        //public Nullable<int> CompanyId { get; set; } ///Rahul updated 'CompanyId' to 'VendorsID' 14-12-2022. 
        public Nullable<int> LocationId { get; set; }
        ///public Nullable<int> UnitId { get; set; } ///Rahul removed 'UnitId' 17-12-2022.  
        ///public Nullable<int> ItemId { get; set; } ///Rahul removed 'ItemId' 17-12-2022.  
        [Required(ErrorMessage = "Please select the RFQ Date!")]
        public Nullable<System.DateTime> Date { get; set; }
        [Required(ErrorMessage = "Please select the Delivery date!")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        [Required(ErrorMessage = "Please select the Bidding Start date!")]
        public Nullable<System.DateTime> BiddingStartDate { get; set; }
        [Required(ErrorMessage = "Please select the Bidding End date!")]
        public Nullable<System.DateTime> BiddingEndDate { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public string Signature { get; set; }
        [Required(ErrorMessage = "Enter remakrs!")] ////added 
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedByID { get; set; }
        public Nullable<System.DateTime> CreatedByDate { get; set; }
        public Nullable<int> LastModifiedByID { get; set; }
        public Nullable<System.DateTime> LastModifiedByDate { get; set; }
        //==============Rahul: These fields are for Request For Quotation details 14/12/2022==============//
        public string LocationName { get; set; }
        public Nullable<int> VendorsID { get; set; } 
        public string VendorIDs { get; set; }         
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Enter delivery address!")]
        //public string BuyerAddress { get; set; } 
        public string DeliveryAddress { get; set; }

        //[Required(ErrorMessage = "Enter supplier address!")]
        //public string SupplierAddress { get; set; }
        public Nullable<int> Item_ID { get; set; } ///Rahul added 19-12-2022.   
        public string ItemName { get; set; }        
        public string Item_Code { get; set; }        
        public string ItemUnit { get; set; }
        public Nullable<decimal> TotalItemCost { get; set; }
        public string HSN_Code { get; set; }         
        public string TxtItemDetails { get; set; }
        public List<RequestForQuotationItemDetailsBO> itemDetails { get; set; } ///Rahul added 17-12-2022.  
        public string Attachment { get; set; } 
    }
}
