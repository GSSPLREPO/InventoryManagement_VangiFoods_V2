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

    }
}
