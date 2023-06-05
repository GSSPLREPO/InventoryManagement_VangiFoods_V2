using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
   public class Stage3BO
    {
        /* ID */
        public int ID { get; set; }
        [Required(ErrorMessage = "Select The Date!")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Select The Product Name!")]
        public int ItemId { get; set; }
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Enter The Lot Number!")]
        public string LotNumber { get; set; } //Rahul added 'LotNumber' 05-06-2023.
        public string RawBatchesNo { get; set; }
        public string PackingHopperTemp { get; set; }
        public string ChillerTemp { get; set; }
        public string Consistency { get; set; }
        public string PackingSize { get; set; }
        public string PackingUnit { get; set; }
        public string NoOfPackets { get; set; }
        public string FinalWeight { get; set; }
        public string FinalPackets { get; set; }
        public string RejectedPackets{ get; set; }

        /*Remarks*/
        public string Remarks { get; set; }

        /*Common Five*/
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

    }
}
