using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
   public class HotFillingPackingLineLogSheetCCPBO
    {
        /* ID */
        public int ID { get; set; }

        /* Date*/
        [Required(ErrorMessage = "Enter date!")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        /*Item Name*/
        [Required(ErrorMessage = "Enter name of item!")]
        //[StringLength(30, ErrorMessage = "Legth of name is exceeded!")]
        public string ItemName { get; set; }

        /*RELEASE TIME FROM SILO*/
        //[Required(ErrorMessage = "Enter Release time From SILO!")]
        public string ReleaseTime { get; set; }

        /*Hot Line Temp*/
        [Required(ErrorMessage = "Enter hot line temperature(°C)!")]
        public string HotLineTemp { get; set; }

        /*Product Temp*/
        [Required(ErrorMessage = "Enter product temperature(°C)!")]
        public string ProductTemp { get; set; }

        /*CLEANING & HYGINE CHECKS*/
        [Required(ErrorMessage = " Select whether the cleaning & hygiene is Ok?!")]
        public string CleaningHygine { get; set; }

        /*RANDOM WEIGHT*/
        [Required(ErrorMessage = "Enter the weight of product!")]
        public decimal? RandomWeight { get; set; }

        /*MONITORING PARAMETERS(LEACKAGES, GAMAGES,SEAL STRENGTH)*/
        [Required(ErrorMessage = "Select monitoring parameters!")]
        public string MonitoringParameters { get; set; }

        /*MANDATORY FILLING TEMP-93  TO 95 DC(PRODUCT FILLING TEMP.)*/
        [Required(ErrorMessage = "Enter monitoring filling temperature(°C)!")]
        public string MonitoringFilling { get; set; }

        /*NO OF POUCHES */
        [Required(ErrorMessage = "Enter no of pouches!")]
        public string NoOfPouches{ get; set; }

        /*Remarks*/
        public string Remarks { get; set; }

        /*Corrective Actions*/
        [Required(ErrorMessage = "Enter corrective actions!")]
        public string CorrectiveActions { get; set; }

        /*Verification Name*/
        /*[Required(ErrorMessage = "Enter Verification!")]
        public string Verification { get; set; }*/

        /*Common Five*/
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

    }
}
