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

        //Additional fields for Po details.

        [Required(ErrorMessage = "Enter shipping details!")]
        public string ShippingDetails { get; set; }
        public string ProductionIndentNo { get; set; }
        public string InwardQuantities { get; set; }
        public string BalanceQuantities { get; set; }


        //Additional fields for Inward QC
        public float QuantityTookForSorting { get; set; }
        public float BalanceQuantity { get; set; }
        public float RejectedQuantity { get; set; }
        public float WastageQuantityInPercentage { get; set; }

        //Added below fields for GRN module
        public string DeliveryAddress { get; set; }
        public string SupplierAddress { get; set; }
        public string ItemUnit { get; set; }
        public float TotalQuantity { get; set; }
        public float ReceivedQty { get; set; }

        //Added below fields for Currency module
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public double CurrencyPrice { get; set; }
        public List<InwardNoteDetailBO> itemDetails { get; set; }

        //This field is used in Inward QC.
        public string ItemTaxValue { get; set; }

        //Below field is added for Rejection note.
        public Nullable<int> ProductionIndentID { get; set; }
        [Required(ErrorMessage = "Enter Production Indent Number!")]
        public Nullable<int> ProductionIndentId { get; set; }

        //Added the below fields for Pre-QC prod
        public Nullable<int> ItemId { get; set; }
        public string Item_Name { get; set; }
        public string Item_Code { get; set; }
        public Nullable<double> IssuedQuantity { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
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
        public Nullable<double> IssuingQty { get; set; }
        public Nullable<double> BalanceQty { get; set; }
        public Nullable<double> StockAfterIssuing { get; set; }
        public Nullable<double> AvailableStockBeforeIssue { get; set; }
        public Nullable<int> UnitId { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedby { get; set; }

        //Added below fields for Currency module
        public int CurrencyID { get; set; }
        public int PO_ID { get; set; }
    }

}
