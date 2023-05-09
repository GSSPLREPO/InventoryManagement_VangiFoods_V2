using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class SalesOrderBO
    {
        public int SalesOrderId { get; set; }

        [Required(ErrorMessage = "Enter sales order number!")]
        public string SONo { get; set; }

        [Required(ErrorMessage = "Select document date!")]
        public Nullable<System.DateTime> SODate { get; set; }

        [Required(ErrorMessage = "Select delivery date!")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }

        [Required(ErrorMessage = "Select client name!")]
        public int ClientID { get; set; }
        public string CompanyName { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        [Required(ErrorMessage = "Select terms!")]
        public Nullable<int> TermsAndConditionID { get; set; }
        public string Terms { get; set; }
        public string SalesOrderStatus { get; set; }
        public Nullable<bool> Cancelled { get; set; }
        public string ReasonForCancellation { get; set; }
        public Nullable<bool> DraftFlag { get; set; }
        [Required(ErrorMessage = "Enter amendment no.!")]
        public Nullable<int> Amendment { get; set; }

        [Required(ErrorMessage = "Enter delivery address!")]
        public string DeliveryAddress { get; set; }

        [Required(ErrorMessage = "Enter supplier address!")]
        public string SupplierAddress { get; set; }
        public decimal AdvancedPayment { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal TotalAfterTax { get; set; }

        [Required(ErrorMessage = "Select location!")]
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Attachment { get; set; }
        public string QuotationRef { get; set; }

        [Required(ErrorMessage = "Select currency!")]
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public decimal? CurrencyPrice { get; set; }
        public decimal OtherTax { get; set; }
        public string Signature { get; set; }
        public Nullable<int> ApprovedById { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<int> CheckedById { get; set; }
        public Nullable<System.DateTime> CheckedDate { get; set; }
        public string Remarks { get; set; }
        public string DispatchMode { get; set; }
        [Required(ErrorMessage = "Select inquiry number!")]
        public int? InquiryID { get; set; }
        public string InquiryNumber { get; set; }
        [Required(ErrorMessage = "Enter work order number!")]
        public string WorkOrderNo { get; set; }
        [Required(ErrorMessage = "Select work order type!")]
        public string WorkOrderType { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedById { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedById { get; set; }
        public string txtItemDetails { get; set; }
        public List<SalesOrderItemsDetail> salesOrderItemsDetails { get; set; }

        //Added below fields for fetching items details in Outward note/delivery challan
        public Nullable<int> Item_ID { get; set; }
        public string ItemName { get; set; }
        public string Item_Code { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
        public Nullable<decimal> ItemQuantity { get; set; }
        public decimal ItemTaxValue { get; set; }
        public string ItemUnit { get; set; }
        public decimal TotalItemCost { get; set; }
        public decimal BalanceQuantity { get; set; }
        public int OutwardCount { get; set; }
        public decimal OutwardQuantity { get; set; }

        //Below field is added for finding amendment entry or simple insert entry
        public int IsAmendmentFlag { get; set; }

        //Added the below field for production indent.
        public string SONumber { get; set; }

        //Added the below field for batch planning count
        public int? IsBatchDone { get; set; }

        //[Required(ErrorMessage = "Select vendors name!")]
        public Nullable<int> VendorsID { get; set; }
        //Rahul Added the below field for SO payment module 
        public float AmountPaid { get; set; }
        //Added the below the field for Delivery Challan including the Finished goods quantity in it.
        public decimal FinishedGoodQuantity { get; set; }   
    }

    public class SalesOrderItemsDetail
    {
        public int ID { get; set; }
        public Nullable<int> SalesOrderId { get; set; }
        public Nullable<int> Item_ID { get; set; }
        public string ItemName { get; set; }
        public string Item_Code { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
        public Nullable<decimal> ItemQuantity { get; set; }
        public decimal ItemTaxValue { get; set; }
        public string ItemUnit { get; set; }
        public decimal TotalItemCost { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public decimal? CurrencyPrice { get; set; }
        public decimal BalanceQuantity { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        //Added the below field for finding the batch planning is done or not
        public int ItemCount { get; set; }
    }
}