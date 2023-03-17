using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class SiloCCPBO
    {
        public int SiloCCPID { get; set; }
        [Required(ErrorMessage = "Enter Date!")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Enter name of item!")]
        [StringLength(30, ErrorMessage = "Legth of name is exceeded!")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Enter activity!")]
        [StringLength(100, ErrorMessage = "Legth of activity is exceeded!")]
        public string Activity { get; set; }

        [Required(ErrorMessage = "Enter Monitoring Parameter!")]
        [StringLength(150, ErrorMessage = "Legth of monitoring parameter is exceeded!")]
        public string MonitoringParameter { get; set; }

        [Required(ErrorMessage = "Enter Transefered time from RQS!")]
        public string TranseferedTimeFromRQS { get; set; }

        [Required(ErrorMessage = "Enter Mandatory Range!")]
        public string MandatoryRange { get; set; }

        [Required(ErrorMessage = "Enter Holding Pressure!")]
        public string HoldingPressure { get; set; }

        [Required(ErrorMessage = "Enter Frequency!")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "Enter Weight of Holding Material!")]
        public string WeightOfHoldingMaterial { get; set; }

        [Required(ErrorMessage = "Enter Time!")]
        public string Time { get; set; }

        [Required(ErrorMessage = "Enter Corrective Actions!")]
        public string CorrectiveActions { get; set; }

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