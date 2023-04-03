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
    }
}
