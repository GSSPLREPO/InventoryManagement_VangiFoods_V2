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
    
    public partial class RQCCP
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Activity { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> NoBatches { get; set; }
        public Nullable<decimal> BatchWeight { get; set; }
        public string MonitoringParameter { get; set; }
        public Nullable<System.TimeSpan> BatchReleaseTimeOfRQ { get; set; }
        public Nullable<decimal> MandatoryTemp { get; set; }
        public string Frequency { get; set; }
        public string Responsibility { get; set; }
        public string Remarks { get; set; }
        public string Verification { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
