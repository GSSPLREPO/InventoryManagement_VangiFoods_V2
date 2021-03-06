using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class QCProductioObservationMaster
    {
        public int QCProductionObservationID { get; set; }
        public int QCProductionSpecificationID { get; set; }
        public string ProductionObservation { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        public virtual QCProductionSpecificationMaster QCProductionSpecificationMaster { get; set; }
    }
}
