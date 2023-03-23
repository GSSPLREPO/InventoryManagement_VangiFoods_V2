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
    
    public partial class BatchPlanningMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BatchPlanningMaster()
        {
            this.BatchNumberMasters = new HashSet<BatchNumberMaster>();
            this.BatchPlanning_Details = new HashSet<BatchPlanning_Details>();
        }
    
        public int ID { get; set; }
        public string WorkOrderNumber { get; set; }
        public Nullable<int> SO_Id { get; set; }
        public string SONumber { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> PackingSize { get; set; }
        public string PackingSizeUnit { get; set; }
        public string PackingType { get; set; }
        public Nullable<decimal> OrderQuantity { get; set; }
        public string OrderQuantityUnit { get; set; }
        public Nullable<decimal> RequiredQuantityInKG { get; set; }
        public Nullable<decimal> TotalBatchSize { get; set; }
        public Nullable<decimal> TotalNoBatches { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BatchNumberMaster> BatchNumberMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BatchPlanning_Details> BatchPlanning_Details { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }
    }
}
