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
    
    public partial class SOItem
    {
        public int SOItemId { get; set; }
        public Nullable<int> SalesOrderId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<int> UnitId { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public string IsSchedule { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<int> TaxId { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedById { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedById { get; set; }
        public string Remarks { get; set; }
        public string JoNo { get; set; }
    }
}
