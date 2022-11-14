using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class PurchaseOrderAmendmentDetailBO
    {
        public int PurchaseOrderAmendmentDetailsId { get; set; }
        public Nullable<int> PurchaseOrderAmendmentId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string ItemName { get; set; }
        public string Item_Code { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> AmendQuantity { get; set; }
        public Nullable<decimal> AmendRate { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> CGST { get; set; }
        public Nullable<decimal> SGST { get; set; }
        public Nullable<decimal> IGST { get; set; }
        public Nullable<decimal> AmendDiscount { get; set; }
        public Nullable<decimal> AmendCgst { get; set; }
        public Nullable<decimal> AmendSgst { get; set; }
        public Nullable<decimal> AmendIgst { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<decimal> ItemTaxValue { get; set; }
        public Nullable<decimal> TotalItemCost { get; set; }
    }
}
