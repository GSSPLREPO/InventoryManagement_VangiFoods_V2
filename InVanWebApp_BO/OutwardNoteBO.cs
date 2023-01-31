using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class OutwardNoteBO
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="Enter outward number!")]
        public string OutwardNumber { get; set; }

        [Required(ErrorMessage ="Select outward date!")]
        public DateTime OutwardDate { get; set; }

        [Required(ErrorMessage ="Select location!")]
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }

        [Required(ErrorMessage ="Enter delivery address!")]
        public string DeliveryAddress { get; set; }
        public string DispatchThrough { get; set; }
        public string DocketNumber { get; set; }
        public string ContactPerson { get; set; }

        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid contact number.")]
        public string ContactInformation { get; set; }
        public string Signature { get; set; }

        [Required(ErrorMessage ="Select verified by field!")]
        public int VerifiedBy { get; set; }
        public string VerifiedByName { get; set; }
        public bool IsReturnable { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string txtItemDetails { get; set; }
        public List<OutwardNoteDetailsBO> outwardNoteDetails { get; set; }

        //This is for inserting the itemdetails
        public Nullable<int> Item_ID { get; set; }
        public string ItemCode { get; set; }
        public string ItemUnit { get; set; }
        public string ItemName { get; set; }
        public Nullable<double> ShippedQuantity { get; set; }
        public string Comments { get; set; }
    }

    public class OutwardNoteDetailsBO
    {
        public int ID { get; set; }
        public int OutwardNoteID { get; set; }
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public decimal OutwardQuantity { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
