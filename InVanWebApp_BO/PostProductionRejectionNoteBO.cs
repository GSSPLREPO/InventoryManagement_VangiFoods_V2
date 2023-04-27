using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class PostProductionRejectionNoteBO
    {
        public int ID { get; set; }
        public string PostProdRejectionNoteNo { get; set; }
        public Boolean DraftFlag { get; set; }

        [Required(ErrorMessage = "Select the Rejection Note date!")]
        [DataType(DataType.Date)]
        public DateTime PostProdRejectionNoteDate { get; set; }

        [Required(ErrorMessage = "Select the rejection type!")]
        public string PostProdRejectionType { get; set; }

        [Required(ErrorMessage = "Select the work order!")]
        public int FGS_ID { get; set; }
        public int SO_Id { get; set; }
        public string SO_No { get; set; }
        public string WorkOrderNo { get; set; }
        public string Stage { get; set; }
        public bool WholeBatchRejection { get; set; }

        [Required(ErrorMessage = "Enter the batch number!")]
        public string BatchNumber { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string txtItemDetails { get; set; }
        public List<PostProductionRejectionNote_DetailsBO> note_DetailsBOs { get; set; }
    }


    public class PostProductionRejectionNote_DetailsBO
    {
        public int ID { get; set; }
        public int PostProdRejectionID { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public string ItemUnit { get; set; }
        public decimal ItemTaxValue { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public decimal CurrencyPrice { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal RejectedQuantity { get; set; }
        public decimal OrderedQuantity { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
