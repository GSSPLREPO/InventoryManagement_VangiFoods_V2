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
        [Required(ErrorMessage = "Enter Foot Washer!")]
        public string FootWasher { get; set; }
        [Required(ErrorMessage = "Enter RO Washer!")]
        public string RoWater { get; set; }
        [Required(ErrorMessage = "Enter Soft Washer!")]
        public string SoftWater { get; set; }
        [Required(ErrorMessage = "Enter Cooling Water Tank!")]
        public string CoolingWaterTank { get; set; }
        [Required(ErrorMessage = "EnterProcessing Water!")]

        public string ProcessingWater { get; set; }
        [Required(ErrorMessage = "Enter name of User!")]
        public string VerifyByName { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
