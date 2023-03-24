//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InVanWebApp.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class CreditNote
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CreditNote()
        {
            this.CreditNoteDetails = new HashSet<CreditNoteDetail>();
        }
    
        public int ID { get; set; }
        public string CreditNoteNo { get; set; }
        public Nullable<System.DateTime> CreditNoteDate { get; set; }
        public Nullable<int> GRNId { get; set; }
        public string GRN_No { get; set; }
        public Nullable<int> PO_ID { get; set; }
        public string PO_Number { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string DeliveryAddress { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> VendorID { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public Nullable<int> TermsAndConditionID { get; set; }
        public string Terms { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<decimal> CurrencyPrice { get; set; }
        public Nullable<decimal> TotalBeforeTax { get; set; }
        public Nullable<decimal> TotalTax { get; set; }
        public Nullable<decimal> OtherTax { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual CurrencyMaster CurrencyMaster { get; set; }
        public virtual GRN_Master GRN_Master { get; set; }
        public virtual LocationMaster LocationMaster { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual TermsAndConditionMaster TermsAndConditionMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CreditNoteDetail> CreditNoteDetails { get; set; }
    }
}