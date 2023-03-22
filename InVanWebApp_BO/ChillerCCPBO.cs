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

        [Required(ErrorMessage = "Select Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }

        [Required(ErrorMessage ="Enter the Water Clorinated!")]
        public string WaterClorinated { get; set; }

        [Required(ErrorMessage = "Enter Totle Time In Chiller!")]
        public string TotleTimeInChiller { get; set; }

        [Required(ErrorMessage = "Enter Quntity Of Packed Product!")]
        public string QuntityOfPakedProduct { get; set; }

        [Required(ErrorMessage = "Enter No Of Crates!")]
        public string NoOfCrates { get; set; }

        [Required(ErrorMessage = "Enter Mandatoty Temperature!")]
        public string MandatotyTemperature { get; set; }
      
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
