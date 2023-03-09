using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class ProductMasterBO 
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Enter Product Code!")]
        [StringLength(40, ErrorMessage = "Legth of Product Code is exceeded!")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage = "Enter Product Name!")]
        [StringLength(40, ErrorMessage = "Legth of Product Name is exceeded!")]
        public string ProductName { get; set; }
        //[Required(ErrorMessage = "Enter recipe description!")]
        [StringLength(100, ErrorMessage = "Legth of recipe description is exceeded!")]
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; } 
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

    }
}
