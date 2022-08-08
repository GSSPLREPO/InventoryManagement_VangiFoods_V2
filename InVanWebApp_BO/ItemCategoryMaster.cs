using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class ItemCategoryMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ItemCategoryMaster()
        {
            this.InquiryMasters = new HashSet<InquiryMaster>();
            this.Items = new HashSet<Item>();
            this.QCProductionSpecificationMasters = new HashSet<QCProductionSpecificationMaster>();
        }

        public int ItemCategoryID { get; set; }

        [Required(ErrorMessage = "Select item type!")]
        public Nullable<int> ItemTypeId { get; set; }

        [Required(ErrorMessage = "Enter category name!")]
        [StringLength(50, ErrorMessage = "Legth of category name is exceeded!")]
        public string ItemCategoryName { get; set; }

        [StringLength(150, ErrorMessage = "Legth of description is exceeded!")]
        public string Description { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InquiryMaster> InquiryMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item> Items { get; set; }
        public virtual ItemMaster ItemMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QCProductionSpecificationMaster> QCProductionSpecificationMasters { get; set; }

        //Below type name is added for dropdown.
        public string ItemTypeName { get; set; }
    }
}
