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
    
    public partial class DebitNote
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DebitNote()
        {
            this.DebitNoteDetails = new HashSet<DebitNoteDetail>();
        }
    
        public int ID { get; set; }
        public string DebitNoteNo { get; set; }
        public Nullable<System.DateTime> DebitNoteDate { get; set; }
        public Nullable<int> GRNId { get; set; }
        public Nullable<int> InwardNoteId { get; set; }
        public Nullable<int> PO_ID { get; set; }
        public string PONumber { get; set; }
        public string Remarks { get; set; }
        public string Signature { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedby { get; set; }
        public string DebitRejectionNoteNo { get; set; }
        public Nullable<System.DateTime> DebitRejectionNoteDate { get; set; }
        public string GRNCode { get; set; }
        public Nullable<System.DateTime> GRNDate { get; set; }
        public string FlagDebitRejectionNotes { get; set; }
        public string Attachment { get; set; }
    
        public virtual GRN_Master GRN_Master { get; set; }
        public virtual InwardNote InwardNote { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DebitNoteDetail> DebitNoteDetails { get; set; }
    }
}
