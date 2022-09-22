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
    
    public partial class UserDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserDetail()
        {
            this.Users = new HashSet<User>();
        }
    
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> DesignationID { get; set; }
        public string EmployeeMobileNo { get; set; }
        public Nullable<System.DateTime> EmployeeBirthDate { get; set; }
        public Nullable<System.DateTime> EmployeeJoingDate { get; set; }
        public string EmployeeGender { get; set; }
        public Nullable<int> CountryID { get; set; }
        public Nullable<int> StateID { get; set; }
        public Nullable<int> CityID { get; set; }
        public string EmployeeAddress { get; set; }
        public string PinNumber { get; set; }
        public string EmployeePic { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> OrganizationID { get; set; }
        public string EmailId { get; set; }
        public string UserName { get; set; }
        public string Signature { get; set; }
    
        public virtual CityMaster CityMaster { get; set; }
        public virtual CountryMaster CountryMaster { get; set; }
        public virtual Department Department { get; set; }
        public virtual DesignationMaster DesignationMaster { get; set; }
        public virtual StateMaster StateMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
        public virtual Organisation Organisation { get; set; }
    }
}
