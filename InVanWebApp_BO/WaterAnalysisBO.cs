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
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Time { get; set; }

        [Required(ErrorMessage = "Enter the PH of RO water!")]
        [Range(typeof(decimal), "0", "14", ErrorMessage = "Enter PH Number between 0 to 14")]
        public decimal? PAPH { get; set; }

        [Required(ErrorMessage = "Enter the TDS of RO water!")]
        public string PATDS { get; set; }

        [Required(ErrorMessage = "Enter the Hardness of RO Water!")]

        public string PAHardness { get; set; }

        [Required(ErrorMessage = "Enter the amount of salt added in RO Water!")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)*$", ErrorMessage = "RO water Salt Added must be alphanumeric.")]
        public string PASaltAdded { get; set; }


        [Required(ErrorMessage = "Enter the PH of  Soft Water!")]
        [Range(typeof(decimal), "0", "14", ErrorMessage = "Enter PH Number between 0 to 14")]
        public decimal? SWPH { get; set; }

        [Required(ErrorMessage = "Enter the TDS of Soft Water!")]
        public string SWTDS { get; set; }

        [Required(ErrorMessage = "Enter the Hardness of Soft Water!")]
        public string SWHardness { get; set; }

        [Required(ErrorMessage = "Enter the Temperature of ETP Plant!")]
        public string ETPTEM { get; set; }

        [Required(ErrorMessage = "Enter the PH of ETP Plant!")]
        [Range(typeof(decimal), "0", "14", ErrorMessage = "Enter PH Number between 0 to 14")]
        public decimal? ETPPH { get; set; }

        [Required(ErrorMessage = "Enter the TDS of ETP Plant!")]
        public string ETPTDS { get; set; }

        [Required(ErrorMessage = "Enter the Temperature!")]
        public string TEM { get; set; }

        
        [Required(ErrorMessage = "Enter the Reading of Gas in ETP Plant!")]
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
        
        //public DateTime dateGridBinding { get; set; }

        public int SrNo { get; set; }
        //Date:14 Apirl23
        //Author: Yatri
        //Added:Below fileds are for report.
        [Required(ErrorMessage = "Invalid date selection!")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "You are selecting greater from date than to date!")]
        public DateTime toDate { get; set; }
    }
}
