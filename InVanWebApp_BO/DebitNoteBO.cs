using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class DebitNoteBO
    {
        public int ID { get; set; }
        public string DebitNoteNo { get; set; }
        [DataType(DataType.Date)]
        public DateTime DebitNoteDate { get; set; }
        public Nullable<int> GRNId { get; set; }
        public string GRN_No { get; set; }
        [Required(ErrorMessage = "Select PO number!")]
        public Nullable<int> PO_ID { get; set; }
        public string PO_Number { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string LocationName { get; set; }
        public string DeliveryAddress { get; set; }
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public int TermsAndConditionID { get; set; }
        public string Terms { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public float CurrencyPrice { get; set; }
        public decimal TotalBeforeTax { get; set; }
        public decimal TotalTax { get; set; }
        public decimal OtherTax { get; set; }
        public decimal GrandTotal { get; set; }
        public string Signature { get; set; }
        public string Remarks { get; set; }

        [Required(ErrorMessage = "Select Rejection number!")]  //Rahul added 20-04-23. 
        public Nullable<int> RejectionId { get; set; } //Rahul added 20-04-23.
        public string RejectionNoteNo { get; set; } //Rahul added 20-04-23. 

        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedby { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string txtItemDetails { get; set; }
        public List<DebitNoteDetailsBO> debitNoteDetails { get; set; }
        public string UserName { get; set; }
    }

    public class DebitNoteDetailsBO
    {
        public int ID { get; set; }
        public Nullable<int> DebitNoteId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Item_Name { get; set; }
        public string Item_Code { get; set; }
        public string ItemTaxValue { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
        public Nullable<double> POQuantity { get; set; }
        public Nullable<double> DebitedQuantity { get; set; }
        public Nullable<double> ItemTotalAmount { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public float CurrencyPrice { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
