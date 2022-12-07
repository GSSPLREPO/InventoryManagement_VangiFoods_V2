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

        [Required(ErrorMessage ="Please enter the PO number!")]
        public string PONumber { get; set; }

        [Required(ErrorMessage ="Please enter the GRN number!")]
        public string GRNCode { get; set; }

        [Required(ErrorMessage ="Please select the GRN date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> GRNDate { get; set; }
        public string Remark { get; set; }

        [Required(ErrorMessage ="Please select location!")]
        public int LocationId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        [Required(ErrorMessage ="Please select Inward number!")]
        public Nullable<int> InwardNoteId { get; set; }
        public string InwardNoteNumber { get; set; }
        [Required(ErrorMessage ="Please enter delivery address!")]
        public string DeliveryAddress { get; set; }

        //Added the field for GRN View
        public string SupplierAddress { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public string ItemUnit { get; set; }

        public float OrderQty{ get; set; }
        public float ReceivedQty{ get; set; }
        public float InwardQty{ get; set; }
        public string LocationName { get; set; }
    }
}
