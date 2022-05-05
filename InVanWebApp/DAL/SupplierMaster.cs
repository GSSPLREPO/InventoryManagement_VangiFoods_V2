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
    
    public partial class SupplierMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SupplierMaster()
        {
            this.RejectionDataSheetMasters = new HashSet<RejectionDataSheetMaster>();
        }
    
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierEmailID { get; set; }
        public string ContactPersonName { get; set; }
        public Nullable<int> CountryID { get; set; }
        public Nullable<int> StateID { get; set; }
        public Nullable<int> CityID { get; set; }
        public string Pincode { get; set; }
        public string StateCode { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string BankAccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string GSTINNumber { get; set; }
        public string PANNumber { get; set; }
        public string PaymentTerms { get; set; }
        public string DeliveryTerms { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> IsDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RejectionDataSheetMaster> RejectionDataSheetMasters { get; set; }
    }
}
