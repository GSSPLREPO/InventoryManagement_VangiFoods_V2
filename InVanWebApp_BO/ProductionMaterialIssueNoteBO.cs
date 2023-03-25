using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class ProductionMaterialIssueNoteBO
    {
        public int ID { get; set; }
        public string ProductionMaterialIssueNoteNo { get; set; }

        [Required(ErrorMessage = "Select PO number!")]
        public int PO_Id { get; set; }

        public string PONumber{ get; set; }
        public Nullable<System.DateTime> PODate { get; set; }

        [Required(ErrorMessage = "Please enter QC number!")]
        public string QCNumber { get; set; }

        [Required(ErrorMessage = "Select QC date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> QCDate { get; set; }
        public Nullable<int> LocationStockID { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Item_Name { get; set; }
        public string Item_Code { get; set; }
        public Nullable<double> IssuedQuantity { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
        public string Signature { get; set; }
        public string Remarks { get; set; }
        public string ChallanNo{ get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        //Additional fields for Po details.

        [Required(ErrorMessage = "Enter shipping details!")]
        public string ShippingDetails { get; set; }

        [Required(ErrorMessage = "Enter Production Indent No!")]
        public string ProductionIndentNo { get; set; }

        public string InwardQuantities { get; set; }
        public string BalanceQuantities { get; set; }


        //Additional fields for Inward QC
        public float QuantityTookForSorting { get; set; }
        public float BalanceQuantity { get; set; }
        public float RejectedQuantity { get; set; }
        public float WastageQuantityInPercentage { get; set; }

        //Added below fields for GRN module
        public string DeliveryAddress { get; set; }
        public string SupplierAddress { get; set; }
        public string ItemUnit { get; set; }
        public float TotalQuantity { get; set; }
        public float ReceivedQty { get; set; }

        //Added below fields for Currency module
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public double CurrencyPrice { get; set; }
        public List<InwardNoteDetailBO> itemDetails { get; set; }
        
        //This field is used in Inward QC.
        public string ItemTaxValue { get; set; }

        //Below field is added for Rejection note.
        public Nullable<int> ProductionIndentID { get; set; } 
    }

    public class ProductionMaterialIssueNoteDetailBO
    {
        public int ID { get; set; }
        public Nullable<int> InwardNoteId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Item_Name { get; set; }
        public string Item_Code { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
        public string ItemTaxValue { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<decimal> POQuantity { get; set; }
        public Nullable<double> InwardQuantity { get; set; }
        public Nullable<double> BalanceQuantity { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        //Added below fields for Currency module
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public double CurrencyPrice { get; set; }
        public int PO_ID { get; set; }

    }
}
