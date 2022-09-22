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
    
    public partial class GRNDetail
    {
        public int ID { get; set; }
        public Nullable<int> GRN_ID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<double> OrderQuantity { get; set; }
        public Nullable<double> ReceivedQuantity { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string Item_Name { get; set; }
        public string Item_Code { get; set; }
        public Nullable<decimal> ItemPrice { get; set; }
        public string ItemTaxValue { get; set; }
    
        public virtual GRN_Master GRN_Master { get; set; }
        public virtual Item Item { get; set; }
    }
}
