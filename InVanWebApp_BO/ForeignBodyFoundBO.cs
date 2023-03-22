using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class ForeignBodyFoundBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Select Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }

        [Required(ErrorMessage = "Enter RawMaterial!")]
        public string RawMaterial { get; set; }

        [Required(ErrorMessage = "Enter OnGoingProcessing!")]
        public string OnGoingProcessing { get; set; }

        [Required(ErrorMessage = "Enter Batching!")]
        public string Batching { get; set; }

        [Required(ErrorMessage = "Enter PostProcessing!")]
        public string PostProcessing { get; set; }

        [Required(ErrorMessage = "Enter CorrectiveAction!")]
        public string CorrectiveAction { get; set; }
        public string VerifyByName { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        //Date:17 March'23
        //Author: Yatri
        //Added:Below fileds are for report.

        [Required(ErrorMessage = "Invalid date selection!")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "You are selecting greater from date than to date!")]
        public DateTime toDate { get; set; }
    }
}
