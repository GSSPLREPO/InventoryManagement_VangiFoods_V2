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
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> Date { get; set; }

        /*adding this flied*/
        public string Time { get; set; }

        [Required(ErrorMessage ="Enter Lot No!")]
       public string LotNo { get; set; }

        [Required(ErrorMessage ="Enter Sample Name!")]
        public string SampleName { get; set;}

        [Required(ErrorMessage ="Enter ACID Value!")]
        public decimal? ACIDValue { get; set; }

        [Required(ErrorMessage ="Enter Peroxide Value!")]
        public string PeroxideValue { get; set; }

        [Required(ErrorMessage ="Enter Color!")]
        public string Color { get; set; }

        [Required(ErrorMessage ="Enter Flavour!")]
        public string Flavour { get; set; }

        [Required(ErrorMessage ="Enter Odour!")]
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
        [DataType(DataType.Date)]
        public DateTime dateGridBinding { get; set; }
    }
}
