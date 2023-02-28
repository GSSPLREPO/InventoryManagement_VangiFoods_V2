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
        [Required(ErrorMessage = "Enter Personal Hygine!")]
        public string PersonalHygine { get; set; }
        [Required(ErrorMessage = "Enter Cleaning & Sanitation!")]
        public string CleaningAndSanitation { get; set; }
        [Required(ErrorMessage = "Enter Cleaning of Equipment!")]
        public string CleaningOfEquipment { get; set; }
        [Required(ErrorMessage = "Enter Water Potability!")]
        public string WaterPotability { get; set; }
        [Required(ErrorMessage = "Enter Allergic!")]
        public string Allergic { get; set; }
        [Required(ErrorMessage = "Enter Non Allergic!")]
        public string NonAllergic { get; set; }
        [Required(ErrorMessage = "Enter Vegetable Processing Area!")]
        public string VegetableProcessingArea { get; set; }
        [Required(ErrorMessage = "Enter Packaging & Labelling Area!")]
        public string PackagingLabellingArea { get; set; }
        [Required(ErrorMessage = "Enter FGS Area!")]
        public string FgsArea { get; set; }
        [Required(ErrorMessage = "Enter Inside!")]
        public string Inside { get; set; }
        [Required(ErrorMessage = "Enter Out Side!")]
        public string OutSide { get; set; }
        [Required(ErrorMessage = "Enter Dry!")]
        public string Dry { get; set; }
        [Required(ErrorMessage = "Enter Wet!")]
        public string Wet { get; set; }
        [Required(ErrorMessage = "Enter Out Siders!")]
        public string OutSiders { get; set; }
        [Required(ErrorMessage = "Enter Production Area!")]
        public string ProductionArea { get; set; }
        [Required(ErrorMessage = "Enter Office Staff!")]
        public string OfficeStaff { get; set; }

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
