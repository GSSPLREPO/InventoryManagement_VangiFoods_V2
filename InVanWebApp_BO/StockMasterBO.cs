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

        //Added for dashboard
        public float MinimumStock { get; set; }


    }
}
