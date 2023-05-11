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
        public string RFQNO { get; set; }

        [Required(ErrorMessage = "Select Location Name!")]
        public Nullable<int> LocationId { get; set; }
        ///public Nullable<int> UnitId { get; set; } ///Rahul removed 'UnitId' 17-12-2022.  
        ///public Nullable<int> ItemId { get; set; } ///Rahul removed 'ItemId' 17-12-2022.  
        [Required(ErrorMessage = "Please select the RFQ Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }
        [Required(ErrorMessage = "Please select the Delivery date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        [Required(ErrorMessage = "Please select the Bidding Start date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> BiddingStartDate { get; set; }
        [Required(ErrorMessage = "Please select the Bidding End date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> BiddingEndDate { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public string Signature { get; set; }

        [Required(ErrorMessage = "Enter remakrs!")]
        public string Remarks { get; set; }

        public string UserName { get; set; }

        //Added below fields for Indent dropdown in RFQ 02-02-2023. 
        [Required(ErrorMessage = "Select indent number!")] ////added 
        public int IndentID { get; set; }
        public string IndentNumber { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedByID { get; set; }
        public Nullable<System.DateTime> CreatedByDate { get; set; }
        public Nullable<int> LastModifiedByID { get; set; }
        public Nullable<System.DateTime> LastModifiedByDate { get; set; }

        //Added the below field for enabling and disabling the edit and delete button
        public int RFQCount { get; set; }

        //==============Rahul: These fields are for Request For Quotation details 14/12/2022==============//
        public string LocationName { get; set; }
        public Nullable<int> VendorsID { get; set; }
        public string VendorIDs { get; set; }
        [Required(ErrorMessage = "Select Vendors!")] ////added 
        public string CompanyName { get; set; }

        //public string BuyerAddress { get; set; } 
        [Required(ErrorMessage = "Select Location Name to enter delivery address!")]
        public string DeliveryAddress { get; set; }
        public string LocationAddress { get; set; } //added         
        public Nullable<int> Item_ID { get; set; } ///Rahul added 19-12-2022.   
        public string ItemName { get; set; }
        public string Item_Code { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<decimal> TotalItemCost { get; set; }
        public string HSN_Code { get; set; }
        public string TxtItemDetails { get; set; }
        public List<RequestForQuotationItemDetailsBO> itemDetails { get; set; } ///Rahul added 17-12-2022.          
        public int vendorIdLength { get; set; }  /// 
        //==============Rahul: These fields are for PO Currency details 23/01/2023==============//
        [Required(ErrorMessage = "Select Currency!")]
        public int CurrencyID { get; set; }
        public int CountryID { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<double> CurrencyPrice { get; set; }
        public Nullable<double> IndianCurrencyValue { get; set; }
        public float CGST { get; set; }
        public float SGST { get; set; }
        public float IGST { get; set; }
        public Nullable<decimal> TotalAfterTax { get; set; } ///Rahul added 24-01-2023. 
        public Nullable<decimal> GrandTotal { get; set; } ///Rahul added 24-01-2023. 
        public float AdvancedPayment { get; set; } ///Rahul added 24-01-2023. 
        public string SupplierAddress { get; set; } //Rahul added 25-01-2023 for edit.
        public List<CompanyBO> companyDetails { get; set; } ///Rahul added 25-01-2023 for edit.   
        public List<RFQ_VendorDetailsBO> rfqVendorDetails { get; set; }     ///Rahul added 23-01-2023.      

        //Added the below field for enabling the view button in punch quotation modal.
        public int countQuotation { get; set; }

    }

    public class RFQ_VendorDetailsBO
    {
        public int RFQ_VendorDetailsId { get; set; }
        public Nullable<int> RequestForQuotationId { get; set; }
        public string RFQNO { get; set; }
        [Required(ErrorMessage = "Please select the RFQ Date!")]
        public Nullable<System.DateTime> Date { get; set; }
        [Required(ErrorMessage = "Please select the Delivery date!")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<int> VendorsID { get; set; }
        public string CompanyName { get; set; }
        //[Required(ErrorMessage = "Please enter client address!")]
        public string Address { get; set; }
        public string SupplierAddress { get; set; } //Rahul added 31-01-2023 for post data. 
        public Nullable<int> LocationId { get; set; }
        public string LocationName { get; set; }
        public string DeliveryAddress { get; set; }
        //[Required(ErrorMessage = "Enter delivery address!")]
        public string LocationAddress { get; set; } //added
        public float CGST { get; set; }
        public float SGST { get; set; }
        public float IGST { get; set; }
        public Nullable<int> TermsAndConditionID { get; set; }
        public string Terms { get; set; }
        public Nullable<decimal> TotalAfterTax { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public float AdvancedPayment { get; set; } ///Rahul added 28-01-2023. 

        //==============Rahul: These fields are for PO Currency details 28/01/2023==============//
        //[Required(ErrorMessage = "Select Currency!")]
        public int CurrencyID { get; set; }
        public int CountryID { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<double> CurrencyPrice { get; set; }
        public Nullable<double> IndianCurrencyValue { get; set; }

        [Required(ErrorMessage = "Enter remakrs!")] ////added 
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedByID { get; set; }
        public Nullable<System.DateTime> CreatedByDate { get; set; }
        public Nullable<int> LastModifiedByID { get; set; }
        public Nullable<System.DateTime> LastModifiedByDate { get; set; }
        public string TxtItemDetails { get; set; }
        public List<RFQ_Vendor_ItemDetailsBO> rfqVendorItemDetails { get; set; }     ///Rahul added 28-01-2023.      

        //Added below fields for generating PO
        public string Tittle { get; set; }
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }
        public int Amendment { get; set; }
        public string Signature { get; set; }
        public decimal OtherTax { get; set; }
        public int IndentID { get; set; }
        public string IndentNumber { get; set; }

    }

    public class RFQ_Vendor_ItemDetailsBO
    {
        public int RFQ_Vendor_ItemDetailsId { get; set; }
        public Nullable<int> RFQ_VendorDetailsId { get; set; }
        public Nullable<int> RequestForQuotationId { get; set; }
        public Nullable<int> Item_ID { get; set; } ///Rahul added 19-12-2022.   
        public string ItemName { get; set; }
        public string Item_Code { get; set; }
        public decimal ItemUnitPrice { get; set; } //added 
        public Nullable<decimal> ItemQuantity { get; set; } //added         
        public string ItemTaxValue { get; set; } //added 
        public string ItemUnit { get; set; }
        public Nullable<decimal> TotalItemCost { get; set; }
        public string HSN_Code { get; set; }
        [Required(ErrorMessage = "Please select the Delivery Start date!")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        //Added the below field for currency details in RFQ Vendor Item details 
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<double> CurrencyPrice { get; set; }
        [Required(ErrorMessage = "Enter remakrs!")] ////added 
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedByID { get; set; }
        public Nullable<System.DateTime> CreatedByDate { get; set; }
        public Nullable<int> LastModifiedByID { get; set; }
        public Nullable<System.DateTime> LastModifiedByDate { get; set; }
        public string TxtItemDetails { get; set; }
        //Added the below field for generating PO
        public int PurchaseOrderId { get; set; }

    }


}
