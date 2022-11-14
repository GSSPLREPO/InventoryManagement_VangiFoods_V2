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
    
    public partial class InwardNote
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InwardNote()
        {
            this.DebitNotes = new HashSet<DebitNote>();
            this.GRN_Master = new HashSet<GRN_Master>();
            this.InwardNoteDetails = new HashSet<InwardNoteDetail>();
            this.InwardQCs = new HashSet<InwardQC>();
        }
    
        public int ID { get; set; }
        public Nullable<int> PO_Id { get; set; }
        public string PONumber { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public string InwardNumber { get; set; }
        public Nullable<System.DateTime> InwardDate { get; set; }
        public string Signature { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DebitNote> DebitNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GRN_Master> GRN_Master { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InwardNoteDetail> InwardNoteDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InwardQC> InwardQCs { get; set; }
    }
}
