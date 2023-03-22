using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class ChillerCCPBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Select date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }

        [Required(ErrorMessage ="Enter the water clorinated!")]
        public string WaterClorinated { get; set; }

        [Required(ErrorMessage = "Enter totle time in chiller!")]
        public string TotleTimeInChiller { get; set; }

        [Required(ErrorMessage = "Enter quntity of packed product!")]
        public string QuntityOfPakedProduct { get; set; }

        [Required(ErrorMessage = "Enter no of crates!")]
        public string NoOfCrates { get; set; }

        [Required(ErrorMessage = "Enter mandatoty temperature!")]
        public string MandatotyTemperature { get; set; }
      
        [Required(ErrorMessage = "Enter name of user!")]
        public string VerifyByName { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
