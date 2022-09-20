using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class InwardQCBO
    {
        public int ID { get; set; }
        public int InwardNote_Id { get; set; }
        public string InwardQCNo { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Item_Name { get; set; }
        public string Item_Code { get; set; }
        public decimal ItemPrice { get; set; }
        public string ItemTaxValue { get; set; }
        public Nullable<float> InwardQuantity { get; set; }
        public Nullable<float> RejectedQuantity { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
