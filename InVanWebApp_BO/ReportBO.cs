using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class ReportBO
    {
        //This BO Fileds are used for Batchwise Production Cost Report

        [Required(ErrorMessage = "Please Select From Date!")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Please Select To Date!")]
        public DateTime toDate { get; set; }

        public int ID { get; set; }
        public int SrNo { get; set; }
        public string WorkOrderNumber { get; set; }

        public string ProductionMaterailIssueNoteNumber { get; set; }

        public string ProductName { get; set; }

        public string BatchNumber { get; set; }

        public decimal RawMaterialCost { get; set; }


        //This BO Fileds are used for Yeild Report
        public decimal ExpectedYeild { get; set; }

        public decimal ActualYeild { get; set; }

        //This BO Fileds are used for FG Locationwise Report

        public string LocationName { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string ItemUnit { get; set; }

        public string ItemUnitPrice { get; set; }

        public string Quantity { get; set; }

        //Thius BO used for Raw Material Cost Analysis Report

        public string QuantityUsed { get; set; }

        //this fileds are used for YeildDashboard

        public int BatchNumberId { get; set; }
        public int WorkOrderNumberId { get; set; }

        //This BO Fileds are used for UtilityConsumptionProduction Dashboard
        public decimal ConsumeQty { get; set; }
        public decimal ProQty { get; set; }


        //This BO used for Delivery Challan (Against SO) Report

        public string SONumber { get; set; }

        public string SODate { get; set; }

        public string DeliveryChallanNo { get; set; }

        public string DeliveryChallanDate { get; set; }

        public string ClientName { get; set; }


        //This BO used for Sales Report

        public string InquiryNo { get; set; }

        public string Status { get; set; }

        //This BO used for Sales InvoiceReport

        public string InvoiceNo { get; set; }

        public string InvoiceAmount { get; set; }

        public string AmountRecived { get; set; }

        public string BalanceRecivable { get; set; }

        //This BO used for Debit Note Report

        public string DebitNoteNo { get; set; }

        public string DebitNoteDate { get; set; }

        public string PONo { get; set; }

        public string POQuantity { get; set; }

        public string DebitedQuantity { get; set; }
    }
}
