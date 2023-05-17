using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class CreditNoteBO
    {
        public int ID { get; set; }
        public string CreditNoteNo { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> CreditNoteDate { get; set; }
        public Nullable<int> GRNId { get; set; }
        public string GRN_No { get; set; }
        [Required(ErrorMessage ="Select SO number!")]
        public Nullable<int> PO_ID { get; set; }
        public string PO_Number { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string DeliveryAddress { get; set; }
        public string Remarks { get; set; }
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedby { get; set; }
        public string LocationName { get; set; }
        public int TermsAndConditionID { get; set; }
        public string Terms { get; set; }
        public decimal OtherTax { get; set; }

        public decimal TotalBeforeTax { get; set; }
        public decimal TotalTax { get; set; }
        public decimal GrandTotal { get; set; }

        //Added the below fields for insertion
        public Nullable<int> CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public float CurrencyPrice { get; set; }
        public string TxtItemDetails { get; set; }
        public List<CreditNoteDetailsBO> creditNoteDetails { get; set; }
        public string UserName { get; set; }
    }
}
