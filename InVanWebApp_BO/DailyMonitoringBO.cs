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
        [Required(ErrorMessage = "Select Whether the Personal Hygiene Is Ok?")]
        public string PersonalHygine { get; set; }
        [Required(ErrorMessage = "Select Whether the Cleaning & Sanitation Is Ok?")]
        public string CleaningAndSanitation { get; set; }
        [Required(ErrorMessage = "Select Whether the Cleaning of Equipment Is Ok?")]
        public string CleaningOfEquipment { get; set; }
        [Required(ErrorMessage = "Select Potability of Water!")]
        public string WaterPotability { get; set; }
        [Required(ErrorMessage = "Select Whether Allergic?")]
        public string Allergic { get; set; }
        [Required(ErrorMessage = "Select Whether Non Allergic?")]
        public string NonAllergic { get; set; }
        [Required(ErrorMessage = "Select Cleaniness Of Vegetable Processing Area!")]
        public string VegetableProcessingArea { get; set; }
        [Required(ErrorMessage = "Select Whether Packaging & Labelling Area Is Ok?")]
        public string PackagingLabellingArea { get; set; }
        [Required(ErrorMessage = "Select Whether FGS Area Is Ok?")]
        public string FgsArea { get; set; }
        [Required(ErrorMessage = "Select Whether the Pest Control is done inside?")]
        public string Inside { get; set; }
        [Required(ErrorMessage = "Select Whether the Pest Control is done outside?!")]
        public string OutSide { get; set; }
        [Required(ErrorMessage = "Select Whether the waste is dry?")]
        public string Dry { get; set; }
        [Required(ErrorMessage = "Select Whether the waste is wet?")]
        public string Wet { get; set; }
        [Required(ErrorMessage = "Select Whether an Outsider?")]
        public string OutSiders { get; set; }
        [Required(ErrorMessage = "Select Whether Production Area Is Ok?")]
        public string ProductionArea { get; set; }
        [Required(ErrorMessage = "Select Whether an Office Staff?")]
        public string OfficeStaff { get; set; }
        [Required(ErrorMessage = "Select Name of User!")]
        public string VerifyByName { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        //Added the below field for report

        [Required(ErrorMessage = "Please Select From Date!")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Please Select To Date!")]
        public DateTime toDate { get; set; }
    }
}
