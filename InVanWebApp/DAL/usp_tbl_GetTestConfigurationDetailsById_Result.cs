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
    
    public partial class usp_tbl_GetTestConfigurationDetailsById_Result
    {
        public int TestConfigurationId { get; set; }
        public Nullable<int> DataCapturingRate { get; set; }
        public Nullable<int> TestDuration { get; set; }
        public string TestName { get; set; }
        public string TestDescription { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string Remarks { get; set; }
    }
}
