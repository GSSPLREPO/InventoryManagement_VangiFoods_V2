﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class SalesOrderBO
    {
        public int SalesOrderId { get; set; }
        public string SONo { get; set; }
        public Nullable<System.DateTime> SODate { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public int ClientID { get; set; }
        public string CompanyName { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public Nullable<int> TermsAndConditionID { get; set; }
        public string Terms { get; set; }
        public string SalesOrderStatus { get; set; }
        public Nullable<bool> Cancelled { get; set; }
        public string ReasonForCancellation { get; set; }
        public Nullable<bool> DraftFlag { get; set; }
        public Nullable<int> Amendment { get; set; }
        public string DeliveryAddress { get; set; }
        public string SupplierAddress { get; set; }
        public decimal AdvancedPayment { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal TotalAfterTax { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Attachment { get; set; }
        public string QuotationRef { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public decimal CurrencyPrice { get; set; }
        public decimal OtherTax { get; set; }
        public string Signature { get; set; }
        public Nullable<int> ApprovedById { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<int> CheckedById { get; set; }
        public Nullable<System.DateTime> CheckedDate { get; set; }
        public string Remarks { get; set; }
        public string DispatchMode { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedById { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedById { get; set; }
        public string txtItemDetails { get; set; }
        public List<SalesOrderItemsDetail> salesOrderItemsDetails { get; set; }

        //Added below fields for fetching items details in Outward note
        public Nullable<int> Item_ID { get; set; }
        public string ItemName { get; set; }
        public string Item_Code { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
        public Nullable<decimal> ItemQuantity { get; set; }
        public decimal ItemTaxValue { get; set; }
        public string ItemUnit { get; set; }
        public decimal TotalItemCost { get; set; }
        public decimal BalanceQuantity { get; set; }
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
        public decimal CurrencyPrice { get; set; }
        public decimal BalanceQuantity { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}