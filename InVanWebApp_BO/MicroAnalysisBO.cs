using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class MicroAnalysisBO
    {
        /* ID */
        public int Id { get; set; }

        /*Source*/
        [Required(ErrorMessage = "Enter Source!")]
        public string Source { get; set; }

        /*Date*/
        [Required(ErrorMessage = "Select Date!")]

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }

        /*Wo po*/
        [Required(ErrorMessage = "Enter Wo/Po!")]
        public string WOPO { get; set; }

        /*Product Name*/
        [Required(ErrorMessage = "Enter the Name of Product!")]
        public string ProductName { get; set; }

        /*Batch No*/
        [Required(ErrorMessage = "Enter Number of Batch!")]
        public string BatchNo { get; set; }

        /*Packing Size*/
        [Required(ErrorMessage = "Enter the Size of Packing!")]
        public string PackingSize { get; set; }


        /*Date*/
        [Required(ErrorMessage = "Select the Best Before Date of Product!")]

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> BestBeforeDate { get; set; }

        /*Clostridium Perfringens*/
        [Required(ErrorMessage = "Select Whether Clostridium Perfringens is present!")]
        public string ClostridiumPerfringens { get; set; }

        /*EscherichiaColi*/
        [Required(ErrorMessage = "Enter Escherichia Coli!")]
        public string EscherichiaColi { get; set; }

        /*Salmonella */
        [Required(ErrorMessage = "Enter Salmonella !")]
        public string Salmonella { get; set; }

        /*Total Plate Count Number*/
        [Required(ErrorMessage = "Enter Total Plate Count!")]
        public string TotalPlateCountNumber { get; set; }
        
        /*Total Plate Count Special*/
        /*
        [Required(ErrorMessage = "Enter Total Plate Count special!")]
        public string TotalPlateCountSpecial { get; set; }
        */

        /*Yeast & Mould*/
        [Required(ErrorMessage = "Enter Yeast & Mould!")]
        public string YeastandMould { get; set; }

        /*Coliform*/
        [Required(ErrorMessage = "Enter Coliform!")]
        public string Coliform { get; set; }

        /*Verified by*/
        [Required(ErrorMessage = "Enter the Name of Verifier!")]
        public string VerifyByName { get; set; }

        /*Mandentory*/
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }


        //Added the below field for report

        [Required(ErrorMessage = "Please Select From Date!")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Please Select To Date!")]
        public DateTime toDate { get; set; }


    }
}
