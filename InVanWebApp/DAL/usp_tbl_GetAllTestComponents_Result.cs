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
    
    public partial class usp_tbl_GetAllTestComponents_Result
    {
        public Nullable<int> TestComponentCategoryId { get; set; }
        public string TestComponentCategoryName { get; set; }
        public string Description { get; set; }
        public int TestComponentId { get; set; }
        public string Make { get; set; }
        public string SerialNumber { get; set; }
        public string Range { get; set; }
        public Nullable<System.DateTime> CalibrationOn { get; set; }
        public Nullable<System.DateTime> CalibrationDueOn { get; set; }
        public string PressureGauge { get; set; }
    }
}