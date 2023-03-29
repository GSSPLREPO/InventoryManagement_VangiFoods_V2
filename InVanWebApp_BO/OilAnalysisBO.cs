using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class OilAnalysisBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Select Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }

        /*adding this flied*/
        public string Time { get; set; }

        [Required(ErrorMessage ="Enter Lot Number!")]
       public string LotNo { get; set; }

        [Required(ErrorMessage ="Enter the Name of Sample!")]
        public string SampleName { get; set;}

        [Required(ErrorMessage ="Enter the Value of ACID!")]
        public decimal? ACIDValue { get; set; }

        [Required(ErrorMessage = "Enter the Value of Peroxide!")]
        public string PeroxideValue { get; set; }

        [Required(ErrorMessage ="Enter the Color of Oil!")]
        public string Color { get; set; }

        [Required(ErrorMessage ="Enter the Flavour of Oil!")]
        public string Flavour { get; set; }

        [Required(ErrorMessage ="Enter the Odour of Oil!")]
        public string Odour { get; set; }

        [Required(ErrorMessage = "Enter name of User!")]
        public string VerifyByName { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        //Added the below prop for binding data in grid
        //[DataType(DataType.Date)]
        //public DateTime dateGridBinding { get; set; }

        //Date:15 March'23
        //Author: Yatri
        //Added:Below fileds are for report.

        [Required(ErrorMessage = "Invalid date selection!")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "You are selecting greater from date than to date!")]
        public DateTime toDate { get; set; }
    }
}
