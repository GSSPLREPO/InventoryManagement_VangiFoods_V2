using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class SILOCCPBO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Enter date!")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Enter name of item!")]
        //[StringLength(30, ErrorMessage = "Legth of name is exceeded!")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Enter activity!")]
        //[StringLength(100, ErrorMessage = "Legth of activity is exceeded!")]
        public string Activity { get; set; }

        [Required(ErrorMessage = "Enter monitoring parameter!")]
        //[StringLength(150, ErrorMessage = "Legth of monitoring parameter is exceeded!")]
        public string MonitoringParameter { get; set; }

        [Required(ErrorMessage = "Select transefered time from RQS!")]
        public string TranseferedTimeFromRQS { get; set; }

        [Required(ErrorMessage = "Enter mandatory range!")]
        public string MandatoryRange { get; set; }


        [Required(ErrorMessage = "Enter frequency!")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "Enter weight of holding material!")]
        public string WeightOfHoldingMaterial { get; set; }

        [Required(ErrorMessage = "Select time!")]
        public string Time { get; set; }

        [Required(ErrorMessage = "Enter corrective actions!")]
        public string CorrectiveActions { get; set; }

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