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

        [Required(ErrorMessage = "Enter Production Area PH!")]
        [RegularExpression(@"\d{0,2}(\.\d{1,2})?", ErrorMessage = " must be a Decimal Number.")]
        [Range(0, 14, ErrorMessage = "Enter Production Area PH number between 0 to 14")]
        public string PAPH { get; set; }

        [Required(ErrorMessage = "Enter Production Area TDS!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Production Area TDS must be numeric.")]
        public string PATDS { get; set; }

        [Required(ErrorMessage = "Enter Production Area Hardness!")]
        [RegularExpression("^[0-9]*$", ErrorMessage ="Proudction Area Hradness must be numeric")]
        public string PAHardness { get; set; }

        [Required(ErrorMessage = "Enter Production Area Salt Added!")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)*$", ErrorMessage = "Production Area Salt Added must be alphanumeric.")]
        public string PASaltAdded { get; set; }


        [Required(ErrorMessage = "Enter Soft Water PH!")]
        [Range(0, 14, ErrorMessage = "Enter Soft Water PH number between 0 to 14")]
        public string SWPH { get; set; }

        [Required(ErrorMessage = "Enter Soft Water TDS!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Soft Water TDS must be numeric.")]
        public string SWTDS { get; set; }

        [Required(ErrorMessage = "Enter Soft Water Hardness!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = " Soft Water Hardness must be numeric.")]
        public string SWHardness { get; set; }

        [Required(ErrorMessage = "Enter ETP Plant TEMP!")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)*$", ErrorMessage = "ETP Plant TEMP must be alphanumeric.")]
        public string ETPTEM { get; set; }

        [Required(ErrorMessage = "Enter ETP Plant PH!")]
        [Range(0, 14, ErrorMessage = "Enter number between 0 to 14")]
        public string ETPPH { get; set; }

        [Required(ErrorMessage = "Enter ETP Plant TDS!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "ETP Plant TDS must be numeric.")]
        public string ETPTDS { get; set; }

        [Required(ErrorMessage = "Enter TEMP!")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)*$", ErrorMessage = "TEMP must be alphanumeric.")]
        public string TEM { get; set; }

        //[RegularExpression(@"\d{0,2}(\.\d{1,2})?", ErrorMessage = "{0} must be a Decimal Number.")]
        [Required(ErrorMessage = "Enter Gas Reading!")]
        [RegularExpression(@"[\d]{1,5}([.,][\d]{1,2})?", ErrorMessage = "Gas Reading must be a Decimal Number.")]
        public decimal  GasReading { get; set; }

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
