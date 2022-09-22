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
    
    public partial class SalesOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SalesOrder()
        {
            this.SalesOrderItemsDetails = new HashSet<SalesOrderItemsDetail>();
            this.SalesOrderPaymentDetails = new HashSet<SalesOrderPaymentDetail>();
            this.StockMasters = new HashSet<StockMaster>();
        }
    
        public int SalesOrderId { get; set; }
        public Nullable<int> OrganisationId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string SONo { get; set; }
        public Nullable<System.DateTime> SODate { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string DispatchTo { get; set; }
        public string ServiceType { get; set; }
        public string PONo { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public string TransportationMode { get; set; }
        public string QuatationRef { get; set; }
        public Nullable<System.DateTime> DelivaryDate { get; set; }
        public Nullable<int> SOIncharge { get; set; }
        public Nullable<decimal> OtherCharges { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public string InspectionTerms { get; set; }
        public string InspectionTermsDesc { get; set; }
        public string PaymentTerms { get; set; }
        public string PaymentTermsDesc { get; set; }
        public string DeliveryTerms { get; set; }
        public string DeliveryTermsDesc { get; set; }
        public string DespatchMode { get; set; }
        public string Freight { get; set; }
        public string Payment { get; set; }
        public Nullable<int> TaxId { get; set; }
        public string TransitInsurance { get; set; }
        public string PackingAndForwarding { get; set; }
        public string TestToBeOffered { get; set; }
        public string SupervisionTerms { get; set; }
        public string SOYear { get; set; }
        public Nullable<int> ApprovalStatus { get; set; }
        public string Approvalremarks { get; set; }
        public Nullable<int> ApprovedById { get; set; }
        public Nullable<int> CheckedById { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedById { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedById { get; set; }
        public Nullable<decimal> FreightRs { get; set; }
        public Nullable<decimal> FreightGSTPercent { get; set; }
        public Nullable<bool> IsOpen { get; set; }
        public string SpecialRemarks { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesOrderItemsDetail> SalesOrderItemsDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesOrderPaymentDetail> SalesOrderPaymentDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockMaster> StockMasters { get; set; }
    }
}
