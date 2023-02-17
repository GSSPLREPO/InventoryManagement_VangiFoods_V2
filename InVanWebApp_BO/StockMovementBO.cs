using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class StockMovementBO
    {
        public int ID { get; set; }
        public Nullable<int> FromLocationId { get; set; }
        public Nullable<int> ToLocationId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Item_Name { get; set; }//ST.Item_Name
        public string Item_Code { get; set; }//ST.Item_Code
        public Nullable<double> TransferQuantity { get; set; }//ST.TransferQuantity,
        public Nullable<double> RequiredQuantity { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }//Date
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedby { get; set; }

        //Added fields for Stock movement report.
        public int SrNo { get; set; }  //SrNo
        public float TotalQty{ get; set; }
        public float ItemUnitPrice { get; set; }
        public string Action{ get; set; }//Action
        public string FromLocationName { get; set; }//FromLocationName
        public string ToLocationName { get; set; }//ToLocationName
        public float ValueOut { get; set; }//ValueOut
        public float ValueIn { get; set; }//ValueIn
        public float QtyIn { get; set; }
        public float FromLocation_BeforeTransferQty { get; set; }
        public int BalanceQty_FromLocation { get; set; }
        public float ToLocation_FinalQty { get; set; }
        public float UnitPrice { get; set; }

        [Required(ErrorMessage = "Please Select From Date!")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Please Select To Date!")]
        public DateTime toDate { get; set; }
        public string Date { get; set; }

        //Added the below fields for reports
        public string InwardDateOfItem { get; set; }
    }
}
