using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
   public class RQCCPBO
    {
        public int Id { get; set; }

        
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Select The Date!")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Select The Product Name!")]
        public int ItemId { get; set; } 
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Enter The Lot Number!")]
        public string LotNumber { get; set; } //Rahul added 'LotNumber' 05-06-2023.
        public string RawBatchesNo { get; set; }
        public string WeightofRawBatches { get; set; }
        public string TansferTimeintoHoldingSilo { get; set; }
        public string Weight { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        //This BO Fileds are used for Consolidated Production Stages 1to3 Report 06-06-23.   
        [Required(ErrorMessage = "Please Select From Date!")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Please Select To Date!")]
        public DateTime toDate { get; set; } 
        public int SrNo { get; set; }
        public string RCCPDate { get; set; }  
        public string Time { get; set; }
        public string Temperature { get; set; }
        public string Pressure { get; set; }
        public string PackingHopperTemp { get; set; }
        public string ChillerTemp { get; set; }
        public string Consistency { get; set; }
        public string NoOfPackets { get; set; }
        public string RejectedPackets { get; set; }
        public string FinalPackets { get; set; } 

    }
}
