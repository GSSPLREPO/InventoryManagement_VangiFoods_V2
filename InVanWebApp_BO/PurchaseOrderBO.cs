using System;
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
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public float DiscountValue { get; set; }
        public float CGST { get; set; }
        public float SGST { get; set; }
        public float IGST { get; set; }
        public Nullable<int> TermsAndConditionID { get; set; }
        public string Terms { get; set; }
        public string OrderStatus { get; set; }
        public Nullable<int> Cancelled { get; set; }
        public string ReasonForCancellation { get; set; }
        public Nullable<float> TotalAmount { get; set; }
        public Nullable<bool> DraftFlag { get; set; }
        public Nullable<int> Amendment { get; set; }

        [Required(ErrorMessage = "Enter buyer address!")]
        public string BuyerAddress { get; set; }
        
        [Required(ErrorMessage = "Enter supplier address")]
        public string SupplierAddress { get; set; }
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
        public string ItemTaxValue { get; set; }
        public decimal InwardQuantity { get; set; }
        public decimal BalanceQuantity { get; set; }

    }
}
