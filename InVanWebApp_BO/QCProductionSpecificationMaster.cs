using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class QCProductionSpecificationMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QCProductionSpecificationMaster()
        {
            this.QCProductioObservationMastes = new HashSet<QCProductioObservationMaste>();
            this.QCProductioObservationMasters = new HashSet<QCProductioObservationMaster>();
        }

        public int QCProductionSpecificationID { get; set; }
        public string ProductionSpecification { get; set; }
        public int ItemCategoryID { get; set; }
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        public virtual ItemCategoryMaster ItemCategoryMaster { get; set; }
        public virtual ItemMaster ItemMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QCProductioObservationMaste> QCProductioObservationMastes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QCProductioObservationMaster> QCProductioObservationMasters { get; set; }
    }
}
