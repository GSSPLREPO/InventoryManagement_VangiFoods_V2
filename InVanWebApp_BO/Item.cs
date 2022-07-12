using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            this.PurchaseOrders = new HashSet<PurchaseOrder>();
            this.RequestForQuotations = new HashSet<RequestForQuotation>();
        }

        public int Item_ID { get; set; }
        public Nullable<int> ItemCategory_ID { get; set; }
        public Nullable<int> ItemTypeID { get; set; }
        public string Item_Code { get; set; }
        public string Item_Name { get; set; }
        public Nullable<int> UnitOfMeasurement_ID { get; set; }
        public string HSN_Code { get; set; }
        public Nullable<int> Current_Stock { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Tax { get; set; }
        public string Description { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        public virtual ItemCategoryMaster ItemCategoryMaster { get; set; }
        public virtual ItemMaster ItemMaster { get; set; }
        public virtual UnitMaster UnitMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestForQuotation> RequestForQuotations { get; set; }
        //Added below fields for dropdowns
        public string ItemCategoryName { get; set; }
        public string UnitName { get; set; }
    }
}
