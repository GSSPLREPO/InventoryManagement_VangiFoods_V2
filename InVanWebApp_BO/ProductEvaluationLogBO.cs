using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class ProductEvaluationLogBO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Select Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> PELDate { get; set; }
        [Required(ErrorMessage = "Enter Product Name!")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Enter Batch Code!")]
        public string BatchCode { get; set; }
        [Required(ErrorMessage = "Enter Ph!")]
        [Range(0, 14, ErrorMessage = "Enter PH Number between 0 to 14")]
        public string Ph { get; set; }
        [Required(ErrorMessage = "Enter Tex, Col & Taste!")]
        public string TexColTaste { get; set; }
        [Required(ErrorMessage = "Enter Acid!")]
        public string Acid { get; set; }
        [Required(ErrorMessage = "Enter Salt!")]
        public string Salt { get; set; }
        [Required(ErrorMessage = "Enter Viscosity!")]
        public string Viscosity { get; set; }

        //[Required(ErrorMessage = "Select Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> PELDateAfter7Days { get; set; }
        //[Required(ErrorMessage = "Enter Ph!")]
        public string PhAfter7Days { get; set; }
        //[Required(ErrorMessage = "Enter Tex, Col & Taste!")]
        public string TexColTasteAfter7Days { get; set; }
        //[Required(ErrorMessage = "Enter Acid!")]
        public string AcidAfter7Days { get; set; }
        //[Required(ErrorMessage = "Enter Salt!")]
        public string SaltAfter7Days { get; set; }
        //[Required(ErrorMessage = "Enter Viscosity!")]
        public string ViscosityAfter7Days { get; set; }
        //[Required(ErrorMessage = "Enter Work Order!")]
        public string WorkOrder { get; set; }
        public string Status { get; set; }
        //[Required(ErrorMessage = "Enter Name of User!")]
        public string VerifyByName { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
