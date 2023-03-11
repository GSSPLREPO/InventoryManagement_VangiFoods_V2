using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class WaterAnalysisBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Select Date!")]
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> Date { get; set; }

        /*adding this flied*/
        public string Time { get; set; }

        [Required(ErrorMessage = "Enter RO water PH!")]
        [Range(0, 14, ErrorMessage = "Enter RO water PH number between 0 to 14")]
        public string PAPH { get; set; }

        [Required(ErrorMessage = "Enter RO water TDS!")]
        public string PATDS { get; set; }

        [Required(ErrorMessage = "Enter Hardness!")]
        public string PAHardness { get; set; }

        [Required(ErrorMessage = "Enter RO water Salt Added!")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)*$", ErrorMessage = "RO water Salt Added must be alphanumeric.")]
        public string PASaltAdded { get; set; }


        [Required(ErrorMessage = "Enter Soft Water PH!")]
        [Range(0, 14, ErrorMessage = "Enter Soft Water PH number between 0 to 14")]
        public string SWPH { get; set; }

        [Required(ErrorMessage = "Enter Soft Water TDS!")]
        public string SWTDS { get; set; }

        [Required(ErrorMessage = "Enter  Hardness!")]
        public string SWHardness { get; set; }

        [Required(ErrorMessage = "Enter ETP Plant TEMP!")]
        public string ETPTEM { get; set; }

        [Required(ErrorMessage = "Enter ETP Plant PH!")]
        [Range(0, 14, ErrorMessage = "Enter number between 0 to 14")]
        public string ETPPH { get; set; }

        [Required(ErrorMessage = "Enter ETP Plant TDS!")]
        public string ETPTDS { get; set; }

        [Required(ErrorMessage = "Enter TEMP!")]
        public string TEM { get; set; }

        
        [Required(ErrorMessage = "Enter Gas Reading!")]
        //[RegularExpression(@"[\d]{1,5}([.,][\d]{1,2})?", ErrorMessage = "Gas Reading must be a Decimal Number.")]
        public decimal? GasReading { get; set; }

        [Required(ErrorMessage = "Enter name of User!")]
        public string VerifyByName { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        //Added the below prop for binding data in grid
        [DataType(DataType.Date)]
        public DateTime dateGridBinding { get; set; }
    }
}
