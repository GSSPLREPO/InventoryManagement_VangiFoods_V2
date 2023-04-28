using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class CalibrationLogBO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter the Number of Id!")]
        public string IdNo { get; set; }
        [Required(ErrorMessage = "Enter Name of Equipment!")]
        public string NameOfEquipment { get; set; }
        [Required(ErrorMessage = "Enter the Name of Department!")]
        public string Department { get; set; }
        [Required(ErrorMessage = "Enter Range!")]
        public string Range { get; set; }
        //[Required(ErrorMessage = "Enter Required Range[With Unit]!")]
        //public string RequiredRange { get; set; }

        [Required(ErrorMessage = "Enter Required from range (With Unit)!")]
        public string RangeFrom { get; set; }

        [Required(ErrorMessage = "Enter Required to range (With Unit)!")]
        public string RangeTo { get; set; }

        [Required(ErrorMessage = "Select Frequency of Calibration!")]
        public string FrequencyOfCalibration { get; set; }
        [Required(ErrorMessage = "Select Done Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> CalibrationDoneDate { get; set; }
        [Required(ErrorMessage = "Select Due Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> CalibrationDueDate { get; set; }
        public string VerifyByName { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public int SrNo { get; set; }

        //Added the below the fields for notification
        public string CalibrationLogDueDate { get; set; }
    }
}
