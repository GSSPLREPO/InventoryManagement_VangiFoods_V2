using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class LocationMasterBO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Enter location name!")]
        [StringLength(50, ErrorMessage = "Legth of location name is exceeded!")]
        public string LocationName { get; set; }
        public Nullable<int> Levels { get; set; }
        public Nullable<int> ParentId { get; set; }

        [StringLength(150, ErrorMessage = "Legth of remarks is exceeded!")]
        public string Remark { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
