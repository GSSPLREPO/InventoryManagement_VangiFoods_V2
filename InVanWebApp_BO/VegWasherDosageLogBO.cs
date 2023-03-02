﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class VegWasherDosageLogBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Select Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }
                
        [Required(ErrorMessage = "Enter Solution A ml!")]
        public string VegWasher1SolutionAMl { get; set; }
        [Required(ErrorMessage = "Enter Solution B ml!")]
        public string VegWasher1SolutionBMl { get; set; }
        [Required(ErrorMessage = "Enter Name of Item!")]
        public string NameOfItem1 { get; set; }
        [Required(ErrorMessage = "Enter Washing Time!")]
        public string WashingTime1 { get; set; }
        [Required(ErrorMessage = "Enter PPM!")]
        public string Ppm1 { get; set; }
        [Required(ErrorMessage = "Enter Solution A ml!")]
        public string VegWasher2SolutionAMl { get; set; }
        [Required(ErrorMessage = "Enter Solution B ml!")]
        public string VegWasher2SolutionBMl { get; set; }
        [Required(ErrorMessage = "Enter Name of Item!")]
        public string NameOfItem2 { get; set; }
        [Required(ErrorMessage = "Enter Washing Time!")]
        public string WashingTime2 { get; set; }
        [Required(ErrorMessage = "Enter PPM!")]
        public string Ppm2 { get; set; }
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