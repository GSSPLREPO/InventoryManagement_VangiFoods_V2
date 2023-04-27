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
        public string QCType { get; set; }
        public string NoteType { get; set; }
        [Required(ErrorMessage = "Please select the Rejection Note date!")]
        [DataType(DataType.Date)]
        public DateTime NoteDate { get; set; }
        public Nullable<int> PO_Id { get; set; } //
        public string PONumber { get; set; } // 
        public Nullable<int> SupplierID { get; set; }
        //[Required(ErrorMessage = "Select supplier name!")]
        public string SupplierName { get; set; }
        [Required(ErrorMessage = "Please enter the Rejection Note number!")] 
        public string RejectionNoteNo { get; set; }
        //---------------
       // [Required(ErrorMessage = "Please select Inward QC number!")]
        public Nullable<int> InwardQCId { get; set; }
        public string InwardQCNumber { get; set; }
       // [Required(ErrorMessage = "Select inward number!")]
        public Nullable<int> InwardNote_Id { get; set; }
        public string InwardNoteNumber { get; set; }     ///Rahul updated InwardNoteNumber 13-01-2023.     
        public Nullable<double> TotalQuantity { get; set; } // 
        public Nullable<double> InwardQuantity { get; set; } // 
        public Nullable<double> RejectedQuantity { get; set; } //
        //[Required(ErrorMessage = "Select PO number!")]
        public string Remarks { get; set; }

        //Additional fields for Inward QC
        public string QuantitiesForSorting { get; set; }
        public string BalanceQuantities { get; set; }
        public string RejectedQuantities { get; set; }
        public string WastageQuantities { get; set; }
        public string InwardQuantities { get; set; } //
        public string ReasonsForRejection { get; set; }

        public string PreProductionQCNumber { get; set; }  //Rahul added 18-04-2023. 
        public string ProductionMaterialIssueNoteNo { get; set; }  //Rahul added 18-04-2023. 
        //[Required(ErrorMessage = "Enter Production Indent Number!")]
        public string ProductionIndentNo { get; set; }  //Rahul added 18-04-2023.  
        //[Required(ErrorMessage ="Select Production QC!")]
        public int? PreProductionQCId { get; set; }  //Rahul added 18-04-2023. 
        public int? ProductionIndentId { get; set; }  //Rahul added 18-04-2023.  
        public int? ProductionMaterialIssueNoteId { get; set; }  //Rahul added 19-04-2023.  

        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        //Added the below field for Rejection note Report

        [Required(ErrorMessage = "Please Select From Date!")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Please Select To Date!")]
        public DateTime toDate { get; set; }

        public bool FlagDebitNote { get; set; } //Rahul added 24-04-23. 

        //Added the below property for saving the item table details. 
        public List<RejectionNoteItemDetailsBO> itemDetails { get; set; } 
    }
    public class RejectionNoteItemDetailsBO 
    {
        public int RejectionID { get; set; }
        public DateTime RejectionDate { get; set; }
        public int PO_Id { get; set; } //
        public string PONumber { get; set; } // 
        public Nullable<int> SupplierID { get; set; }
        [Required(ErrorMessage = "Select supplier name!")]
        public string SupplierName { get; set; }                
        public DateTime DateOfReceving  { get; set; }
        public string NameOfMaterial  { get; set; }
        public DateTime ManufecturingDate { get; set; }
        public DateTime BestBefore { get; set; }
        public Nullable<int> ItemCategoryID { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string ItemName { get; set; }//view pdf
        public string ItemCode { get; set; }// view pdf
        public string Item_Name { get; set; }//
        public string Item_Code { get; set; }//
        public Nullable<decimal> ItemUnitPrice { get; set; }//
        public string ItemUnit { get; set; }//
        public Nullable<double> TotalQuantity { get; set; } //
        public Nullable<double> InwardQuantity { get; set; } //
        public Nullable<double> RejectedQuantity { get; set; } //
        public Nullable<double> QuantityTookForSorting { get; set; } // 
        public Nullable<double> WastageQuantityInPercentage { get; set; } // 
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
        public string InwardQCNumber { get; set; }
        public string ReasonForRR { get; set; }

        //Added the below feild for Debit note
        public int? CurrencyID { get; set; }
        public Nullable<double> CurrencyPrice { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string LocationName { get; set; }
        public string DeliveryAddress { get; set; }
        public string SupplierAddress { get; set; }
        public Nullable<int> TermsAndConditionID { get; set; }
        public string Terms { get; set; }
        public decimal OtherTax { get; set; }
        public Nullable<decimal> TotalItemCost { get; set; }
        public Nullable<int> flag { get; set; }//Rahul added 21-04-23.  

    }

}
