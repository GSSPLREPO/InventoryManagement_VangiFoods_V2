using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class StockMasterBO 
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string Item_Code { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<double> StockQuantity { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<int> InwardNoteId { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> Unit { get; set; }
        public Nullable<int> GRNId { get; set; }
        public Nullable<int> PO_Id { get; set; }
        public string PO_Number { get; set; }
        public string SaledOrder_Number { get; set; }
        public Nullable<int> SO_Id { get; set; }
        public string CurrencyName { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public int ItemCategoryNameID { get; set; } //Use in Stock Report
        //Added for dashboard
        public float MinimumStock { get; set; }

        //Added below field for Inventory FIFO report
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string ItemUnitPriceWithCurrency { get; set; }
        public int SrNo { get; set; }
        public string InwardDate { get; set; }

        //Added below field for Inventory Analysis FIFO report 22-02-23. 
        public string CompanyName { get; set; }
        public string GRNCode { get; set; }
        public string Outward_No { get; set; }
        public string GRNDate { get; set; }
        public float StockInQty { get; set; }
        public decimal StockInTotalPrice { get; set; }
        public string DeliveryChallanDate { get; set; }
        public float StockOutQty { get; set; }
        public decimal StockOutUnitPrice { get; set; }
        public decimal StockOutTotalPrice { get; set; }
        public string StockOutCurrency { get; set; }
        public string AvlDate { get; set; }
        public float AvlQty { get; set; }
        public float AvlUnitPrice { get; set; }
        public float AvlTotalPrice { get; set; }
        public string AvlCurrency { get; set; }
    }
}
