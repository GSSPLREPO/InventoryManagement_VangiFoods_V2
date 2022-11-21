using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class StockReportBO
    {
        public int ID { get; set; }
        public string Item_Code { get; set; }
        public string ItemName { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public decimal  StockQuantity { get; set; }
        public decimal InventoryValue { get; set; }
        public decimal ReOrderLevel { get; set; }
        public decimal ItemReOrderQuantity { get; set; }

        //Added the below fields for report generation by Bhandavi.
        public string Reorder { get; set; }
        public int RowNumber { get; set; }
    }
}
