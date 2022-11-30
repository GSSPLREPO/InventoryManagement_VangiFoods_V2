using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public Nullable<double> FromLocation_BeforeTransferQty { get; set; }  
        public Nullable<double> ToLocation_FinalQty { get; set; }   
        [Required(ErrorMessage = "Enter remakrs!")]
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

        //FromLocationBeforeTransferQty float Checked

        //public List<Item> Item { get; set; } 

        //public List<LocationMaster> LocationMaster { get; set; }

        public List<StockMaster> itemDetails { get; set; }
        public string QuantitiesForSorting { get; set; }
        public string BalanceQuantities { get; set; }
        public string RejectedQuantities { get; set; }
        public string WastageQuantities { get; set; }
        public string ReasonsForRejection { get; set; }

    }

    public class StockMaster
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


    }


}
