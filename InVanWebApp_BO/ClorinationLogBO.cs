using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class ClorinationLogBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Select Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }
        [Required(ErrorMessage = "Enter Foot Washer Dosage!")]
        //[RegularExpression("^([a-zA-Z0-9 .&'-]+)*$", ErrorMessage = "Foot Washer must be alphanumeric.")]
        public string FootWasher { get; set; }
        [Required(ErrorMessage = "Enter RO Water Dosage!")]
        //[RegularExpression("^([a-zA-Z0-9 .&'-]+)*$", ErrorMessage = "RO Water must be alphanumeric.")]
        public string RoWater { get; set; }
        [Required(ErrorMessage = "Enter Soft Water Dosage!")]
        //[RegularExpression("^([a-zA-Z0-9 .&'-]+)*$", ErrorMessage = "Soft Water must be alphanumeric.")]
        public string SoftWater { get; set; }
        [Required(ErrorMessage = "Enter Cooling Water Tank Dosage!")]
        //[RegularExpression("^([a-zA-Z0-9 .&'-]+)*$", ErrorMessage = "Cooling Water Tank must be alphanumeric.")]
        public string CoolingWaterTank { get; set; }
        [Required(ErrorMessage = "Enter Portable Water Dosage!")]
        //[RegularExpression("^([a-zA-Z0-9 .&'-]+)*$", ErrorMessage = "Processing Watermust be alphanumeric.")]
        public string ProcessingWater { get; set; }
        [Required(ErrorMessage = "Enter CIP Water Tank!")]
        public string CIPWaterTank { get; set; }
        [Required(ErrorMessage = "Enter Name of User!")]
        public string VerifyByName { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
