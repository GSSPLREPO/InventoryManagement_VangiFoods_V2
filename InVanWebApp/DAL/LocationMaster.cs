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
    using System.ComponentModel.DataAnnotations;
    public partial class LocationMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LocationMaster()
        {
            this.LabMasters = new HashSet<LabMaster>();
            this.MachineMasters = new HashSet<MachineMaster>();
            this.StorageLocationMasters = new HashSet<StorageLocationMaster>();
        }
    
        public int LocationID { get; set; }
        [Required(ErrorMessage ="Enter location")]
        public string LocationName { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string Address { get; set; }
        public Nullable<int> Country_ID { get; set; }
        public Nullable<int> State_ID { get; set; }
        public Nullable<int> City_ID { get; set; }
        public Nullable<int> Pincode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LabMaster> LabMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MachineMaster> MachineMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StorageLocationMaster> StorageLocationMasters { get; set; }
        public virtual CityMaster CityMaster { get; set; }
        public virtual CountryMaster CountryMaster { get; set; }
        public virtual StateMaster StateMaster { get; set; }

        //Fields for dropdown
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
    }
}
