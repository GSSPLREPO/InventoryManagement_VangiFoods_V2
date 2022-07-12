using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class FinishGoodSery
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FinishGoodSery()
        {
            this.LabMasters = new HashSet<LabMaster>();
        }

        public int FGSID { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> PackageSize { get; set; }
        public Nullable<System.DateTime> MfgDate { get; set; }
        public Nullable<int> NoOfCartonBox { get; set; }
        public Nullable<double> QuantityInKG { get; set; }
        public string BatchNo { get; set; }
        public string PONumber { get; set; }
        public string Packaging { get; set; }
        public string Sealing { get; set; }
        public string Labeling { get; set; }
        public string QCCheck { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LabMaster> LabMasters { get; set; }
    }
}
