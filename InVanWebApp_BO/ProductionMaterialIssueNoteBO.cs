using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class ProductionMaterialIssueNoteBO 
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Enter production material issue note no.!")]
        public string ProductionMaterialIssueNoteNo { get; set; }
        [Required(ErrorMessage = "Select production material issue date!")]
        public Nullable<System.DateTime> ProductionMaterialIssueNoteDate { get; set; } 
        
        [Required(ErrorMessage = "Select Purpose!")]
        public string Purpose { get; set; }
        public string WorkOrderNumber { get; set; }
        public string QCNumber { get; set; }
        public Nullable<int> SalesOrderId { get; set; }
        public string SONumber { get; set; }
        public string OtherPurpose { get; set; }
        [Required(ErrorMessage = "Select issue by!")]
        public int IssueBy { get; set; }
        public string IssueByName { get; set; }
        [Required(ErrorMessage = "Select location!")]
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedby { get; set; }
        public string txtItemDetails { get; set; }
        public List<ProductionMaterialIssueNoteDetailsBO> ProductionMaterialIssueNoteDetails { get; set; }

    }

    public class ProductionMaterialIssueNoteDetailsBO 
    {
        public int ID { get; set; }
        public Nullable<int> IssueNoteId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Item_Code { get; set; }
        public string Item_Name { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public string CurrencyName { get; set; }
        public int CurrencyId { get; set; }
        public decimal CurrencyPrice { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<double> QuantityRequested { get; set; }
        public Nullable<double> QuantityIssued { get; set; }
        public Nullable<double> StockAfterIssuing { get; set; }
        public Nullable<double> AvailableStockBeforeIssue { get; set; }
        public Nullable<int> UnitId { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedby { get; set; }
    }

}
