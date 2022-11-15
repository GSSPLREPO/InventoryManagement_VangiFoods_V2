﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{

    public class PurchaseOrderBO
    {
        public int PurchaseOrderId { get; set; }
        [Required(ErrorMessage = "Enter title!")]
        public string Tittle { get; set; }
        //[Required]
        public string PONumber { get; set; }
        //[Required]
        public DateTime PODate { get; set; }
        //[Required]
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        //public Nullable<int> CompanyID { get; set; }
        public Nullable<int> VendorsID { get; set; }
        public float DiscountValue { get; set; }
        public float CGST { get; set; }
        public float SGST { get; set; }
        public float IGST { get; set; }
        //[Required]
        public Nullable<int> TermsAndConditionID { get; set; }
        public string Terms { get; set; }
        public string PurchaseOrderStatus { get; set; }
        public Nullable<int> Cancelled { get; set; }
        public string ReasonForCancellation { get; set; }
        public Nullable<float> TotalAmount { get; set; }
        public Nullable<bool> DraftFlag { get; set; }
       // [Required]
        public Nullable<int> Amendment { get; set; }

        [Required(ErrorMessage = "Enter delivery address!")]
        //public string BuyerAddress { get; set; }
        public string DeliveryAddress { get; set; }

        [Required(ErrorMessage = "Enter supplier address")]
        public string SupplierAddress { get; set; }
        //public float TotalPOAmount { get; set; }
        public float AdvancedPayment { get; set; }
        public string Attachment { get; set; }
        public string Signature { get; set; }
        public string IndentNumber { get; set; }
        public string Remarks { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int CheckedBy { get; set; }
        public DateTime CheckedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }

        //==============These fields are for PO item details==============//
        public Nullable<int> Item_ID { get; set; }
        public string ItemName { get; set; }
        public string Item_Code { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public decimal ItemQuantity { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<decimal> ItemTaxValue { get; set; }
        public decimal InwardQuantity { get; set; }
        public decimal BalanceQuantity { get; set; }

        //==============These fields are added by Rahul for PO==============//
        public string CompanyName { get; set; }
        public Nullable<decimal> TotalItemCost { get; set; }
        public Nullable<decimal> TotalAfterTax { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string LocationName { get; set; }
        // public int UserId { get; set; }
        public string TxtItemDetails { get; set; }
        public List<PurchaseOrderItemsDetails> itemDetails { get; set; }

        //===The below field is add by Raj ======
        public int InwardCount { get; set; }

    }

    public class PurchaseOrderItemsDetails
    {
        public int ID { get; set; }
        public Nullable<int> PurchaseOrderId { get; set; }
        public Nullable<int> Item_ID { get; set; }
        public string ItemName { get; set; }
        public string Item_Code { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
        public Nullable<decimal> ItemQuantity { get; set; }
        public string ItemTaxValue { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<double> TotalItemCost { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

    }
}
