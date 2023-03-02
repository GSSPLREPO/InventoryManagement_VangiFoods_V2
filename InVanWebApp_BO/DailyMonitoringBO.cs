using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class DailyMonitoringBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Select Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }
        public string PersonalHygine { get; set; }
        public string CleaningAndSanitation { get; set; }
        public string CleaningOfEquipment { get; set; }
        public string WaterPotability { get; set; }
        public string Allergic { get; set; }
        public string NonAllergic { get; set; }
        public string VegetableProcessingArea { get; set; }
        public string PackagingLabellingArea { get; set; }
        public string FgsArea { get; set; }
        public string Inside { get; set; }
        public string OutSide { get; set; }
        public string Dry { get; set; }
        public string Wet { get; set; }
        public string OutSiders { get; set; }
        public string ProductionArea { get; set; }
        public string OfficeStaff { get; set; }

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
