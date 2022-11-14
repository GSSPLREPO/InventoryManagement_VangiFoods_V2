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
    
    public partial class DebitNoteDetail
    {
        public int ID { get; set; }
        public Nullable<int> DebitNoteId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Item_Name { get; set; }
        public string Item_Code { get; set; }
        public string ItemTaxValue { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
        public Nullable<double> QuantityDebited { get; set; }
        public Nullable<double> QuantityRejected { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedby { get; set; }
    
        public virtual DebitNote DebitNote { get; set; }
        public virtual Item Item { get; set; }
    }
}
