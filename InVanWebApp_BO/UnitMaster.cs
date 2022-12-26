using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class UnitMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UnitMaster()
        {
            this.Items = new HashSet<ItemBO>();
            this.RequestForQuotations = new HashSet<RequestForQuotation>();
        }

        public int UnitID { get; set; }

        [Required(ErrorMessage = "Enter unit name!")]
        [StringLength(50, ErrorMessage = "Legth of unit name is exceeded!")]
        public string UnitName { get; set; }

        [Required(ErrorMessage = "Enter unit code!")]
        public string UnitCode { get; set; }
        
        [StringLength(150, ErrorMessage = "Legth of description is exceeded!")]
        public string Description { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemBO> Items { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestForQuotation> RequestForQuotations { get; set; }
    }
}
