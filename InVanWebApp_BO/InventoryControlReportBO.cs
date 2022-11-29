using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
   public class InventoryControlReportBO
    {
        public int ID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public decimal StockQuantity { get; set; }
        public int RowNumber { get; set; }
        public string PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public int PurchaseQuantity { get; set; }
        public string UsedDate { get; set; }
        public decimal UsedPrice { get; set; }
        public int UsedQuantity { get; set; }
        public decimal AvailablePrice { get; set; }
        public int AvailableQuantity { get; set; }
        [Required(ErrorMessage = "Please Select From Date ")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Please Select From Date ")]
        public DateTime toDate { get; set; }
        public DateTime IssueNoteDate { get; set; }

        public string PONumber { get; set; }
        public string Title { get; set; }
        public float POQuantity { get; set; }
        public float QuantityIssued { get; set; }
        public int PO_ID { get; set; }

    }
}
