using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class RejectionNoteBO 
    {
        public int ID { get; set; }
        public string NoteType { get; set; }
        [Required(ErrorMessage = "Please select the Rejection Note date!")]
        public DateTime NoteDate { get; set; }
        [Required(ErrorMessage = "Please enter the Rejection Note number!")] 
        public string RejectionNoteNo { get; set; }
        //---------------
     
        [Required(ErrorMessage = "Select inward number!")]
        public int InwardNote_Id { get; set; }
        public string InwardNumber { get; set; }
        public Nullable<int> InwardQC_Id { get; set; }

        [Required(ErrorMessage = "Enter QC number!")]
        public string InwardQCNo { get; set; }
        [Required(ErrorMessage = "Select QC date!")]
        [DataType(DataType.Date)]
        public DateTime InwardQCDate { get; set; }
        public Nullable<float> InwardQuantity { get; set; }
        public Nullable<float> RejectedQuantity { get; set; }
        [Required(ErrorMessage = "Select supplier name!")]
        public string SupplierName { get; set; }
        public string Remarks { get; set; }

        //Additional fields for Inward QC
        public string QuantitiesForSorting { get; set; }
        public string BalanceQuantities { get; set; }
        public string RejectedQuantities { get; set; }
        public string WastageQuantities { get; set; }
        public string ReasonsForRejection { get; set; }
        //Added the below property for saving the item table details. 
        public List<RejectionDataSheetMasterDetailBO> itemDetails { get; set; }

        //Added the below field for Rejection note Report

        [Required(ErrorMessage = "Please Select From Date ")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Please Select From Date ")]
        public DateTime toDate { get; set; }
    }
    public class RejectionDataSheetMasterDetailBO 
    {
        public int RejectionID { get; set; }
        public DateTime RejectionDate { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string PONumber { get; set; }
        public DateTime DateOfReceving  { get; set; }
        public string NameOfMaterial  { get; set; }
        public DateTime ManufecturingDate { get; set; }
        public DateTime BestBefore { get; set; }
        public Nullable<int> ItemCategoryID { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Item_Name { get; set; }//
        public string Item_Code { get; set; }//
        public Nullable<decimal> ItemUnitPrice { get; set; }//
        public string ItemUnit { get; set; }//
        public string ItemTaxValue { get; set; }//
        public Nullable<double> TotalRecevingQuantiy { get; set; }
        public Nullable<double> TotalRejectedQuantity { get; set; }
        public Nullable<double> PostRejectedQuantity { get; set; }
        public string ReasonForRejection { get; set; }
        public string CurrectiveAction { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        //Added the below properties for Currency
        public string CurrencyName { get; set; }

        //Added the below field for Rejection note Report
        public int SrNo { get; set; }
        public string RejectionNoteDate { get; set; }
        public string ApprovedBy { get; set; }
        public string RejectionNoteNo { get; set; }
        public string InwardNumber { get; set; }

    }

}
