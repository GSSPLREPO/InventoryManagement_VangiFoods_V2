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
    
    public partial class Branch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Branch()
        {
            this.Departments = new HashSet<Department>();
            this.RequestForQuotations = new HashSet<RequestForQuotation>();
        }
    
        public int BranchId { get; set; }
        public Nullable<int> OrganisationID { get; set; }
        public string BranchName { get; set; }
        public string Abbreviation { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNo { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<int> CountryID { get; set; }
        public Nullable<int> StateID { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    
        public virtual Organisation Organisation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Department> Departments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestForQuotation> RequestForQuotations { get; set; }
    }
}
