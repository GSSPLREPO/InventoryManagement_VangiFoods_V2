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
    
    public partial class GRN_Master
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GRN_Master()
        {
            this.DebitNotes = new HashSet<DebitNote>();
            this.GRNDetails = new HashSet<GRNDetail>();
            this.StockMasters = new HashSet<StockMaster>();
        }
    
        public int ID { get; set; }
        public Nullable<int> PO_ID { get; set; }
        public string PONumber { get; set; }
        public string GRNCode { get; set; }
        public Nullable<System.DateTime> GRNDate { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> InwardNoteId { get; set; }
        public string InwardNoteNumber { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DebitNote> DebitNotes { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GRNDetail> GRNDetails { get; set; }
        public virtual InwardNote InwardNote { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockMaster> StockMasters { get; set; }
    }
}
