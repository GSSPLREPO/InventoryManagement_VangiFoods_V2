using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class CreditNoteDetailsBO
    {
        public int ID { get; set; }
        public Nullable<int> CreditNote_ID { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Item_Name { get; set; }
        public string Item_Code { get; set; }
        public string ItemTaxValue { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
        public Nullable<double> ItemQuantity { get; set; }
        public Nullable<double> ItemTotalAmount { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public float CurrencyPrice { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
    }
}
