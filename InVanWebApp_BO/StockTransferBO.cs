using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class StockTransferBO 
    {
        public int ID { get; set; }
        public Nullable<int> FromLocationId { get; set; }
        public Nullable<int> ToLocationId { get; set; }
        public Nullable<int> ItemId { get; set; }        
        public Nullable<double> TransferQuantity { get; set; }
        public Nullable<double> RequiredQuantity { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedby { get; set; }
        public string Item_Name { get; set; }
        public string Item_Code { get; set; }

        public string ItemUnit { get; set; } //Rahul added on 09/11/2022
        public Nullable<double> FinalQuantity { get; set; }  //Rahul added on 09/11/2022 
        public string FromLocationName { get; set; }  //Rahul added on 09/11/2022  
        public string ToLocationName { get; set; }  //Rahul added on 09/11/2022  
        public Nullable<double> StockQuantity { get; set; } //Rahul added on 10/11/2022 
        public string TxtItemDetails { get; set; } //Rahul added on 10/11/2022 

        //public List<Item> Item { get; set; } 

        //public List<LocationMaster> LocationMaster { get; set; }



    }
}
