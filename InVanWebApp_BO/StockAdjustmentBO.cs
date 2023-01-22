using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class StockAdjustmentBO
    {
        public int ID { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentNo { get; set; }
        [Required(ErrorMessage ="Select Location!")]
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        [StringLength(95, ErrorMessage ="Length is exceeding!")]
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModificationDate { get; set; }
        public Nullable<int> LastModificationBy { get; set; }
        public string UserName { get; set; }
        //Added for insertion
        public string TxtItemDetails { get; set; }
        public List<StockAdjustmentDetailsBO> stockAdjustmentDetails { get; set; }
        
        //Added the below fields for report Stock Reconciliation
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
    }

    public class StockAdjustmentDetailsBO
    {
        public int ID { get; set; }
        public int StockAdjustmentID { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Item_Name { get; set; }
        public string Item_Code { get; set; }
        public string ItemUnit { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public decimal AvailableStock { get; set; }
        public decimal PhysicalStock { get; set; }
        public decimal DifferenceInStock { get; set; }
        public decimal TransferPrice { get; set; }

        [StringLength(100,ErrorMessage ="Length is exceeding!")]
        public string Remarks { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public float CurrencyPrice { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModificationDate { get; set; }
        public Nullable<int> LastModificationBy { get; set; }

        //Added the below fields for report Stock Reconciliation
        public string ItemUnitPriceWithCurrency { get; set; }
        public int SrNo { get; set; }
        public string UserName { get; set; }
        public string StockAdjustedDate { get; set; }

    }
}
