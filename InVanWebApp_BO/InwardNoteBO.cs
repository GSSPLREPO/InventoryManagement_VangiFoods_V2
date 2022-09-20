using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class InwardNoteBO
    {
        public int ID { get; set; }
        public int PO_Id { get; set; }
        public string PONumber{ get; set; }
        public string InwardNumber { get; set; }
        public Nullable<System.DateTime> InwardDate { get; set; }
        public Nullable<int> LocationStockID { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Signature { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
       
        //Inward QC column
        public Nullable<double> InwardQuantity { get; set; }
        public Nullable<double> RejectedQuantity { get; set; }
    }
}
