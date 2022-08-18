using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{

    public class PurchaseOrder
    {
        public int PurchaseOrderId { get; set; }
        [Required(ErrorMessage = "Enter title!")]
        public string Tittle { get; set; }
        public string PONumber { get; set; }
        public Nullable<System.DateTime> DocumentDate { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string DocumentNumber { get; set; }
        [Required(ErrorMessage = "Enter buyer address!")]
        public string BuyerAddress { get; set; }
        [Required(ErrorMessage = "Enter supplier address")]
        public string SupplierAddress { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> Item_ID { get; set; }
        public Nullable<decimal> ItemQuantity { get; set; }
        public string Tax { get; set; }
        public Nullable<decimal> TotalItemCost { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public Nullable<decimal> AdvancedPAyment { get; set; }
        public string OtherChargesDescription { get; set; }
        public string OtherChargesTax { get; set; }
        public Nullable<decimal> OtherChargesCost { get; set; }
        public string Signature { get; set; }
        public Nullable<int> TermsAndConditionID { get; set; }
        public string Remark { get; set; }
        public Nullable<int> TransactionFlag { get; set; }
        public Nullable<bool> DraftFlag { get; set; }
        public Nullable<int> InvoiceStatus { get; set; }
        public Nullable<int> GoodsStatus { get; set; }
        public string WorkOrderNo { get; set; }
        public Nullable<int> Amendment { get; set; }
        public string IndentNumber { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public string OrderStatus { get; set; }

        public virtual Company Company { get; set; }
        public virtual ItemBO Item { get; set; }
        public virtual Status Status { get; set; }
        public virtual Status Status1 { get; set; }
        public virtual TermsAndConditionMaster TermsAndConditionMaster { get; set; }

        //Added: Fields added for dropdowns.
        public string CountryName { get; set; }
        public string InvoiceStat { get; set; }
        public string GoodsStat { get; set; }
        public string CompanyName { get; set; }

        //Below are added for update functionality of OC.
        public string ItemDescription { get; set; }
        public string ItemUnit { get; set; }
        public decimal ItemPrice { get; set; }
        public string ItemTax { get; set; }
        public string Item_Code { get; set; }
        public string Item_HSN_Code { get; set; }
    }
}
