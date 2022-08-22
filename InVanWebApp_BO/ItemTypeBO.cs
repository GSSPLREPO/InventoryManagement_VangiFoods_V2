using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class ItemTypeBO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Enter type of item!")]
        [StringLength(50, ErrorMessage = "Legth of category name is exceeded!")]
        public string ItemType { get; set; }

        [StringLength(150, ErrorMessage = "Legth of description is exceeded!")]
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
