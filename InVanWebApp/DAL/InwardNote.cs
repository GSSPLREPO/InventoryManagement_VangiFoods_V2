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
        public int InwardNoteID { get; set; }
        public string InwardNumber { get; set; }
        public Nullable<System.DateTime> InwardDate { get; set; }
        public Nullable<int> PurchaseOrderId { get; set; }
        public string PONumber { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public Nullable<int> LocationID { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedby { get; set; }
    }
}
