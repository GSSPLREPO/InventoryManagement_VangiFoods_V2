using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class Weighment_ProductionIndent_DetailsBO
    {
        public int ID { get; set; }
        public Nullable<int> WeighmentID { get; set; } 
        public Nullable<int> ProductionIndentID { get; set; }
        public Nullable<int> QCcheck_1 { get; set; }
        public Nullable<int> QCcheck_2 { get; set; }
        public Nullable<int> QCcheck_3 { get; set; }
        public string ProductionCheck { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        //Added for fetching Item details in Recipe Master
        public Nullable<int> ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double BatchQuantity { get; set; }
        public double FinalQuantity { get; set; }
        public string ItemUnit { get; set; }
        public double Percentage { get; set; }

        //Added the below fields for Production material Issue note
        public decimal? RequestedQty { get; set; }
        public decimal? IssuedQty { get; set; }
        public decimal? IssuingQty { get; set; }
        public decimal? BalanceQty { get; set; }
        public decimal? AvailableStock { get; set; }
        public decimal? ItemUnitPrice { get; set; }
        public string CurrencyName { get; set; }
        public decimal? FinalStock { get; set; }
        public string WorkOrderNo { get; set; }
        public Nullable<int> BatchPlanningDocId { get; set; } //Rahul added 'BatchPlanningDocId' 13-06-23.  
        public string BatchPlanningDocumentNo { get; set; } //Rahul added 'BatchPlanningDocumentNo' 13-06-23.   
        public decimal? Weight { get; set; }
        public decimal? Difference { get; set; }

    }
}
