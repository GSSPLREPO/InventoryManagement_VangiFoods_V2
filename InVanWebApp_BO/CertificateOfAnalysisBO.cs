
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace InVanWebApp_BO
{
    public class CertificateOfAnalysisBO
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
        
        /*Yeast & Mould*/
        [Required(ErrorMessage = "Enter Yeast & Mould!")]
        public string YeastandMould { get; set; }

        /*Coliform*/
        [Required(ErrorMessage = "Enter Coliform!")]
        public string Coliform { get; set; }
        [Range(typeof(decimal), "0", "14")]
        [Required(ErrorMessage = "Enter PH!")]
        public decimal? PH { get; set; }
        [Required(ErrorMessage = "Enter Acidity!")]
        public string Acidity { get; set; }
        [Required(ErrorMessage = "Enter Total Soluble Solids!")]
        public string TotalSolubleSolids { get; set; }
        [Required(ErrorMessage = "Enter Peroxide Value!")]
        public string PeroxideValue { get; set; }
        [Required(ErrorMessage = "Enter Salt Content!")]
        public string SaltContent { get; set; }
        /*Verified by*/
        [Required(ErrorMessage = "Enter the Name of Verifier!")]
        public string VerifyByName { get; set; }

        public string LabAnalysisReportNo { get; set; }
        public string LabAnalysisReport { get; set; }
        //[Required(ErrorMessage = "Please Select File.")]
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.xlsx|.xls|.pdf)$", ErrorMessage = "Only PDF, Excel or PNG file allowed.")]
        //public HttpPostedFileBase LabAnalysisReportFile { get; set; }


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
