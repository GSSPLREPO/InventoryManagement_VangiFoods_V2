﻿using System;
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
        [Required(ErrorMessage = "Select Personal Hygine!")]
        public string PersonalHygine { get; set; }
        [Required(ErrorMessage = "Select Cleaning & Sanitation!")]
        public string CleaningAndSanitation { get; set; }
        [Required(ErrorMessage = "Select Cleaning of Equipment!")]
        public string CleaningOfEquipment { get; set; }
        [Required(ErrorMessage = "Select Water Potability!")]
        public string WaterPotability { get; set; }
        [Required(ErrorMessage = "Select Allergic!")]
        public string Allergic { get; set; }
        [Required(ErrorMessage = "Select Non Allergic!")]
        public string NonAllergic { get; set; }
        [Required(ErrorMessage = "Select Vegetable Processing Area!")]
        public string VegetableProcessingArea { get; set; }
        [Required(ErrorMessage = "Select Packaging & Labelling Area!")]
        public string PackagingLabellingArea { get; set; }
        [Required(ErrorMessage = "Select FGS Area!")]
        public string FgsArea { get; set; }
        [Required(ErrorMessage = "Select Inside!")]
        public string Inside { get; set; }
        [Required(ErrorMessage = "Select Out Side!")]
        public string OutSide { get; set; }
        [Required(ErrorMessage = "Select Dry!")]
        public string Dry { get; set; }
        [Required(ErrorMessage = "Select Wet!")]
        public string Wet { get; set; }
        [Required(ErrorMessage = "Select Out Siders!")]
        public string OutSiders { get; set; }
        [Required(ErrorMessage = "Select Production Area!")]
        public string ProductionArea { get; set; }
        [Required(ErrorMessage = "Select Office Staff!")]
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
