using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class FinishedGoodSeriesBO
    {
        public int FGSID { get; set; }
        [Required(ErrorMessage = "Select Product Name")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Enter Package Size of Product")]
        public string PackageSize { get; set; }
        [Required(ErrorMessage = "Select Manufacture Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> MfgDate { get; set; }
        //[Required(ErrorMessage = "Enter No of Carton Box of Product!")]
        public Nullable<int> NoOfCartonBox { get; set; }
        [Required(ErrorMessage = "Enter Quantity of Product!")]
        public Nullable<double> QuantityInKG { get; set; }
        [Required(ErrorMessage = "Enter Batch No of Product!")]
        public string BatchNo { get; set; }
        [Required(ErrorMessage = "Select Sales Order Number!")]
        public Nullable<int> SalesOrderId { get; set; }
        public string SONo { get; set; }
        public string PONumber { get; set; }
        [Required(ErrorMessage = "Product Packaged or Not?")]
        public string Packaging { get; set; }
        [Required(ErrorMessage = "Select Product Sealed or Not?")]
        public string Sealing { get; set; }
        [Required(ErrorMessage = "Select Product Labeled or Not?")]
        public string Labelling { get; set; }
        [Required(ErrorMessage = "Select Product Quality Control Check or Not?")]
        public string QCCheck { get; set; }
        [Required(ErrorMessage = "Enter Actual Packets of Product!")]
        public string ActualPackets { get; set; }
        [Required(ErrorMessage = "Enter Expected Packets of Product!")]
        public string ExpectedPackets { get; set; }
        [Required(ErrorMessage = "Enter Expected Yield of Product!")]
        public Nullable<decimal> ExpectedYield { get; set; }
        [Required(ErrorMessage = "Enter Actual Yield of Product!")]
        public Nullable<decimal> ActualYield { get; set; }
        public string WorkOrderNo { get; set; }
        //public string VerifyByName { get; set; }
        [Required(ErrorMessage = "Select Location!")]
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public int ItemId { get; set; }
        public string Item_Code { get; set; }
        public decimal OrderQty { get; set; }

        //Added the below fields for Post-production rejection note
        public string WorkOrderAndBN { get; set; }
        public Nullable<double> CurrentFGSQty { get; set; } 
        public Nullable<double> CurrentRejectedQty { get; set; }  
         
        //Added the below field for report
        //[Required(ErrorMessage = "Please Select From Date!")]
        //public DateTime fromDate { get; set; }
        //[Required(ErrorMessage = "Please Select To Date!")]
        //public DateTime toDate { get; set; }
    }
}
