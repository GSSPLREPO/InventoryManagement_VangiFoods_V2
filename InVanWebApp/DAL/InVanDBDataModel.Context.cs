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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class InVanDBContext : DbContext
    {
        public InVanDBContext()
            : base("name=InVanDBContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<CityMaster> CityMasters { get; set; }
        public virtual DbSet<ClientMaster> ClientMasters { get; set; }
        public virtual DbSet<COAMaster> COAMasters { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CountryMaster> CountryMasters { get; set; }
        public virtual DbSet<CurrencyMaster> CurrencyMasters { get; set; }
        public virtual DbSet<DebitNote> DebitNotes { get; set; }
        public virtual DbSet<DebitNoteDetail> DebitNoteDetails { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<DesignationMaster> DesignationMasters { get; set; }
        public virtual DbSet<EmployeeMaster> EmployeeMasters { get; set; }
        public virtual DbSet<FinishGoodSery> FinishGoodSeries { get; set; }
        public virtual DbSet<GSTMaster> GSTMasters { get; set; }
        public virtual DbSet<InquiryMaster> InquiryMasters { get; set; }
        public virtual DbSet<InvoiceMaster> InvoiceMasters { get; set; }
        public virtual DbSet<InwardNote> InwardNotes { get; set; }
        public virtual DbSet<IssueNote> IssueNotes { get; set; }
        public virtual DbSet<IssueNoteDetail> IssueNoteDetails { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemCategoryMaster> ItemCategoryMasters { get; set; }
        public virtual DbSet<ItemMaster> ItemMasters { get; set; }
        public virtual DbSet<LabMaster> LabMasters { get; set; }
        public virtual DbSet<LocationMaster> LocationMasters { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<MachineMaster> MachineMasters { get; set; }
        public virtual DbSet<OrganisationGroup> OrganisationGroups { get; set; }
        public virtual DbSet<Organisation> Organisations { get; set; }
        public virtual DbSet<ProcessMaster> ProcessMasters { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<PurchaseOrderAmendment> PurchaseOrderAmendments { get; set; }
        public virtual DbSet<PurchaseOrderAmendmentDetail> PurchaseOrderAmendmentDetails { get; set; }
        public virtual DbSet<QCProductionSpecificationMaster> QCProductionSpecificationMasters { get; set; }
        public virtual DbSet<QCProductioObservationMaste> QCProductioObservationMastes { get; set; }
        public virtual DbSet<QCProductioObservationMaster> QCProductioObservationMasters { get; set; }
        public virtual DbSet<RecipeMaster> RecipeMasters { get; set; }
        public virtual DbSet<RejectionDataSheetMaster> RejectionDataSheetMasters { get; set; }
        public virtual DbSet<RequestForQuotation> RequestForQuotations { get; set; }
        public virtual DbSet<RoleRight> RoleRights { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SalesOrder> SalesOrders { get; set; }
        public virtual DbSet<ScreenName> ScreenNames { get; set; }
        public virtual DbSet<SOItem> SOItems { get; set; }
        public virtual DbSet<SOItemSchedule> SOItemSchedules { get; set; }
        public virtual DbSet<StateMaster> StateMasters { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<StockAdjustment_M> StockAdjustment_M { get; set; }
        public virtual DbSet<StockAdjustment_T> StockAdjustment_T { get; set; }
        public virtual DbSet<StockMaster> StockMasters { get; set; }
        public virtual DbSet<StockTransfer_M> StockTransfer_M { get; set; }
        public virtual DbSet<StockTransfer_T> StockTransfer_T { get; set; }
        public virtual DbSet<StorageItemMapping> StorageItemMappings { get; set; }
        public virtual DbSet<StorageLocationMaster> StorageLocationMasters { get; set; }
        public virtual DbSet<SupplierMaster> SupplierMasters { get; set; }
        public virtual DbSet<TermsAndConditionMaster> TermsAndConditionMasters { get; set; }
        public virtual DbSet<UnitMaster> UnitMasters { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WorkOrder> WorkOrders { get; set; }
        public virtual DbSet<WorkOrderDetail> WorkOrderDetails { get; set; }
        public virtual DbSet<YearMaster> YearMasters { get; set; }
    }
}
