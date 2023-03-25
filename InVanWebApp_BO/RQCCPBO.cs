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
        public int RQCCPID { get; set; }
        [Required(ErrorMessage = "Enter Date!")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Enter activity!")]
        [StringLength(100, ErrorMessage = "Legth of activity is exceeded!")]
        public string Activity { get; set; }

        [Required(ErrorMessage = "Enter name of item!")]
        [StringLength(30, ErrorMessage = "Legth of name is exceeded!")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Enter number of batches!")]
        
        public string NoBatches { get; set; }

        //[Required(ErrorMessage = "Enter Batch Weight!")]
        [Required(ErrorMessage = "Enter Batch Weight!")]
        public string BatchWeight { get; set; }

        [Required(ErrorMessage = "Enter Monitoring Parameter!")]
        [StringLength(150, ErrorMessage = "Legth of monitoring parameter is exceeded!")]
        public string MonitoringParameter { get; set; }

        [Required(ErrorMessage = "Enter Batch release time of RQ!")]
        public string BatchReleaseTimeOfRQ { get; set; }

        [Required(ErrorMessage = "Enter Mandatory Temperature!")]
        public string MandatoryTemp { get; set; }

        [Required(ErrorMessage = "Enter Frequency!")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "Enter Responsible Position!")]
        public string Responsibility { get; set; }

        public string Remarks { get; set; }

        [Required(ErrorMessage = "Enter Verification!")]
        public string Verification { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

    }
}
