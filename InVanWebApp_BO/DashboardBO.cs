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
        /*GRN Date*/
        [Required(ErrorMessage = "Please select the GRN date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> GRNDate { get; set; }

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

        /*From Date*/
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> fromDate { get; set; }

        /*toDate*/
        [Required(ErrorMessage = "Please select the GRN date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> toDate { get; set; }
    }
}
