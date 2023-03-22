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
        [Required(ErrorMessage = "Enter     date!")]
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
        [Required(ErrorMessage = "Enter batch weight!")]
        public string BatchWeight { get; set; }

        [Required(ErrorMessage = "Enter monitoring parameter!")]
        [StringLength(150, ErrorMessage = "Legth of monitoring parameter is exceeded!")]
        public string MonitoringParameter { get; set; }

        [Required(ErrorMessage = "Enter batch release time of RQ!")]
        public string BatchReleaseTimeOfRQ { get; set; }

        [Required(ErrorMessage = "Enter mandatory temperature!")]
        public string MandatoryTemp { get; set; }

        [Required(ErrorMessage = "Enter frequency!")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "Enter responsible position!")]
        public string Responsibility { get; set; }

        public string Remarks { get; set; }

        [Required(ErrorMessage = "Enter verification!")]
        public string Verification { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

    }
}
