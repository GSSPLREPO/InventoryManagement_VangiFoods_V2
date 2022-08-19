using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class ItemBO
    {
        public int ID { get; set; }
        public Nullable<int> ItemCategory_ID { get; set; }
        public Nullable<int> ItemTypeID { get; set; }
        public string Item_Code { get; set; }
        public string Item_Name { get; set; }
        public string HSN_Code { get; set; }
        public Nullable<double> MinStock { get; set; }
        public string Description { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        //Added below fields for dropdowns
        public string ItemCategoryName { get; set; }
        public string ItemTypeName { get; set; }
        public string UnitName { get; set; }
    }
}
