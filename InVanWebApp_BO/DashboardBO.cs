using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class DashboardBO
    {
        /*-------------fromDate and toDate----------------------------*/

        /*From Date*/
        [DataType(DataType.Date)]
        public DateTime fromDate { get; set; }

        /*toDate*/
        [Required(ErrorMessage = "Please select the GRN date!")]
        [DataType(DataType.Date)]
        public DateTime toDate { get; set; }


        /*-----------Start FIFO System-------------------*/
        /*GRN Date*/
        [Required(ErrorMessage = "Please select the GRN date!")]
        [DataType(DataType.Date)]
        public String GRNDate { get; set; }

        /*Location*/
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        /*Item ID*/
        public int ItemID { get; set; }
        /*Item Name*/
        public string ItemName { get; set; }

        /*Item Price*/
        public decimal ItemUnitPrice { get; set; }

        /*Currency*/
        public string CurrencyName { get; set; }

        /* Received Qty*/
        public float ReceivedQty { get; set; }
        /*------------End FIFO Dashboard-------------------------*/
        



        /*-------------Start Yeild Report---------------------------*/
        public string WorkOrderNumber { get; set; }

        public string BatchNumber { get; set; }

        public string ProductName { get; set; }

        public decimal ExpectedYeild { get; set; }

        public decimal ActualYeild { get; set; }
        /*-------------End   Yeild Report---------------------------*/

        // This feild added for WorkOrderwiseProductionCost Dashboard
        [Required(ErrorMessage = "Please Select From Date!")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "Please Select To Date!")]
        public DateTime ToDate { get; set; }
        public string SalesOrderNumber { get; set; }
        public int SalesOrderId { get; set; }
        public decimal RawMatrialCost { get; set; }

        //Shweta added Main Dashboard 21-08-2023
        /*-------------Start Main Dashboard---------------------------*/
        public int PurchaseOpenCount { get; set; }
        public int PurchaseCloseCount { get; set; }

        public int PandingPayment { get; set; }
        public int CompletePayment { get; set; }

        public int InqConvToSales { get; set; }
        public int TotalInq { get; set; }

        /*-------------End Main Dashboard---------------------------*/

    }
}
