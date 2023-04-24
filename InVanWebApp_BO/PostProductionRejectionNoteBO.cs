using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class PostProductionRejectionNoteBO 
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please select the Type!")]
        public string Type { get; set; }   
        [Required(ErrorMessage = "Please select the Rejection Note date!")]
        public DateTime RejectionNoteDate { get; set; }
        public string RejectionNoteNo { get; set; }
        [Required(ErrorMessage = "Please select the WorkOrderNo!")]
        public string WorkOrderNo { get; set; }   
        public string Item_Code { get; set; }
        public string ItemName { get; set; }
        public Nullable<double> TotalQuantity { get; set; }  
        public Nullable<double> RejectedQuantity { get; set; } 
        public string Stage { get; set; }
        public bool RejectBatch { get; set; }
        public string Remarks { get; set; }
        public string VerifyBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
       
    }

}
