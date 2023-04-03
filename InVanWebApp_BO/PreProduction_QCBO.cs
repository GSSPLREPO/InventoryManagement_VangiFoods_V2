using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class PreProduction_QCBO
    {
        public int ID { get; set; }
        
        [Required(ErrorMessage = "Select Material Issue number!")]
        public int MaterialIssue_Id { get; set; }
        public string MaterialIssue_No { get; set; }
        [Required(ErrorMessage = "Enter QC number!")]
        public string QCNumber { get; set; }
        
        public string ProdIndent_No { get; set; }
        public int? ProdIndent_Id { get; set; }

        [Required(ErrorMessage = "Select QC date!")]
        [DataType(DataType.Date)]
        public DateTime QCDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        //Fields name in reProduction_QC details table.

        //[Required(ErrorMessage = "Select supplier name!")]
        public string SupplierName { get; set; }
        public Nullable<float> IssuedQuantity { get; set; }
        public Nullable<float> RejectedQuantity { get; set; }
        public float BalanceQuantity { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Item_Name { get; set; }
        public string Item_Code { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public string ItemTaxValue { get; set; }
        public float WastageQuantityInPercentage { get; set; }
        public float QuantityTookForSorting { get; set; }

        //Additional fields for reProduction_QC 
        public string txtItemDetails { get; set; }

        //Added the below property for saving the item table details.
        public List<PreProduction_QC_Details> itemDetails { get; set; }
    }

    public class PreProduction_QC_Details
    {
        public int ID { get; set; }
        public Nullable<int> QC_Id { get; set; }
        public string SupplierName { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Item_Name { get; set; }
        public string Item_Code { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
        public string ItemUnit { get; set; }
        public string ItemTaxValue { get; set; }
        public Nullable<double> IssuedQuantity { get; set; }
        public Nullable<double> RejectedQuantity { get; set; }
        public Nullable<double> BalanceQuantity { get; set; }
        public Nullable<double> QuantityTookForSorting { get; set; }
        public Nullable<double> WastageQuantityInPercentage { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        //Added the below properties for Currency
        public string CurrencyName { get; set; }

    }
}
