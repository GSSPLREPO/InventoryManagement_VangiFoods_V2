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
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> Date { get; set; }

        [Required(ErrorMessage ="Enter Type of Pest!")]
        public string TypeOfPest { get; set; }

        [Required(ErrorMessage = "Enter Method For Pest Control!")]
        public string MethodForPestControl { get; set; }

        [Required(ErrorMessage = "Enter Area!")]
        public string Area { get; set; }

        [Required(ErrorMessage = "Enter Frequency!")]
        public string Frequncy { get; set; }

        [Required(ErrorMessage = "Enter COA Received From Pest Company!")]
        public string COARecivedFromPestControl { get; set; }

        [Required(ErrorMessage = "Enter Effective Or Not!")]
        public string EffectiveOrNot { get; set; }

        [Required(ErrorMessage = "Enter Any Hazard Detected After Pest!")]
        public string AnyHazardDetectedAfterPest { get; set; }

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
