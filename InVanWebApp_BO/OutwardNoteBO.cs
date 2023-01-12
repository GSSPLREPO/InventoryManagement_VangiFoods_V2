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
        public string OutwardNoteNumber { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> OutwardDate { get; set; }
        public Nullable<int> PO_ID { get; set; }
        public string PONumber { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public int IndentID { get; set; }
        public string IndentNo { get; set; }
        public int LocationId { get; set; }
        public string DeliveryAddress { get; set; }
        public int VendorsID { get; set; }
        public string CompanyName { get; set; }
        public string Signature { get; set; }
        public string Remarks { get; set; }
        public string TransporterName { get; set; }
        public string TransporterGSTNumber { get; set; }
        public string TransportationDocumentNumber { get; set; }
        public string VehicleNumber { get; set; }
        public float GrandTotal { get; set; }
        public float TotalAfterTax { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        //public float CurrencyPrice { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }


        //Added the below field for Outward report
        public string OutwardNoteDate { get; set; }

        [Required(ErrorMessage = "Please Select From Date ")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Please Select From Date ")]
        public DateTime toDate { get; set; }

    }

    public class OutwardNoteItemDetailsBO
    {
        public int OutwardNoteDetailsId { get; set; }
        public int OutwardNoteID { get; set; }
        public int Item_ID { get; set; }
        public string ItemName { get; set; }
        public string Item_Code { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public decimal ItemQuantity { get; set; }
        public string ItemTaxValue { get; set; }
        public float QuotedPrice { get; set; }
        public float ExpectedPrice { get; set; }
        public float CloserPrice { get; set; }
        public string ItemUnit { get; set; }
        public float TotalItemCost { get; set; }
        public string HSN_Code { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Remarks { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        //public float CurrencyPrice { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        //Added the below field for Outward report
        public int SrNo { get; set; }
        public string OutwardNoteNumber { get; set; }
        public string OutwardDate { get; set; }
        public float DispatchQuantity { get; set; }

    }
}
