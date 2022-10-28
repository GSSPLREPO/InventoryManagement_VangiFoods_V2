using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class GRN_BO
    {
        public int ID { get; set; }
        public Nullable<int> PO_ID { get; set; }
        public string PONumber { get; set; }
        public string GRNCode { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> GRNDate { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> InwardNoteId { get; set; }
        public string InwardNoteNumber { get; set; }
    }
}
