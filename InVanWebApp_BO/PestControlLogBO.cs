using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class PestControlLogBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Select Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }

        /*adding this flied*/
        //public string Time { get; set; }

        [Required(ErrorMessage ="Enter the Type of Pest!")]
        public string TypeOfPest { get; set; }

        [Required(ErrorMessage = "Enter Method For Pest Control!")]
        public string MethodForPestControl { get; set; }

        [Required(ErrorMessage = "Enter Area of Pest Control!")]
        public string Area { get; set; }

        [Required(ErrorMessage = "Enter Frequency!")]
        public string Frequncy { get; set; }

        [Required(ErrorMessage = "Enter COA Received From Pest Company!")]
        public string COARecivedFromPestControl { get; set; }

        [Required(ErrorMessage = "Enter Effective Or Not!")]
        public string EffectiveOrNot { get; set; }

        [Required(ErrorMessage = "Enter the Name of Hazard Detected After Pest!")]
        public string AnyHazardDetectedAfterPest { get; set; }

        [Required(ErrorMessage = "Enter name of User!")]
        public string VerifyByName { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        //Date:14 Apirl'23
        //Author: Yatri
        //Added:Below fileds are for report.

        [Required(ErrorMessage = "Invalid date selection!")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "You are selecting greater from date than to date!")]
        public DateTime toDate { get; set; }
    }
}
