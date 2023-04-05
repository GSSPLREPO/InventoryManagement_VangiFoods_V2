using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class OrderSummaryBO
    {
        /*-------------fromDate and toDate----------------------------*/

        /*From Date*/
        [DataType(DataType.Date)]
        public DateTime fromDate { get; set; }

        /*toDate*/
        [Required(ErrorMessage = "Please select the GRN date!")]
        [DataType(DataType.Date)]
        public DateTime toDate { get; set; }

        /*------------Start ORDER SUMMERY------------------------*/
   
        /*DateTime*/
        [Required(ErrorMessage = "Please select the  Date!")]
        [DataType(DataType.Date)]
        public string DateWise { get; set; }

        /*Flag*/
        [Required(ErrorMessage = "Please select Dueration")]
        public int Duration { get; set; }

        public float Open_GrandTotal { get; set; }

        public float Closing_GrandTotal { get; set; }
        /*------------End ORDER SUMMERY--------------------------*/

    }
}
