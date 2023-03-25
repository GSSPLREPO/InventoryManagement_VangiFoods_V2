using System;
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
        //[RegularExpression(@"\d{0,5}(\.\d{1,2})?", ErrorMessage = "Solution-A ml must be a Integer or Decimal Number.")]
        [Required(ErrorMessage = "Enter the Proportion of Solution-A in ml!")]
        public double? VegWasher1SolutionAMl { get; set; }
        [Required(ErrorMessage = "Enter the Proportion of Solution-B in ml!")]
        public double? VegWasher1SolutionBMl { get; set; }
        [Required(ErrorMessage = "Enter the Name of Item!")]
        public string NameOfItem1 { get; set; }
        [Required(ErrorMessage = "Enter the time Required for Washing!")]
        public string WashingTime1 { get; set; }
        [Required(ErrorMessage = "Enter PPM!")]
        public string Ppm1 { get; set; }
        [Required(ErrorMessage = "Enter the Proportion of Solution-A in ml!")]
        public double? VegWasher2SolutionAMl { get; set; }
        [Required(ErrorMessage = "Enter the Proportion of Solution-B in ml!")]
        public double? VegWasher2SolutionBMl { get; set; }
        [Required(ErrorMessage = "Enter the Name of Item!")]
        public string NameOfItem2 { get; set; }
        [Required(ErrorMessage = "Enter the time Required for Washing!")]
        public string WashingTime2 { get; set; }
        [Required(ErrorMessage = "Enter PPM!")]
        public string Ppm2 { get; set; }
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
