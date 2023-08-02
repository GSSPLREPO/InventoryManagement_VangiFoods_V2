using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class IntermediateRejectionNoteBO
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Enter Intermediate rej note no!")]
        public string Intermediate_Rej_NoteNo { get; set; } 
        [Required(ErrorMessage = "Select date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Intermediate_Rej_NoteDate { get; set; }         
        public string WorkOrderNumber { get; set; }
        public string QCNumber { get; set; }
        public Nullable<int> SalesOrderId { get; set; }
        public string SONumber { get; set; }
        [Required(ErrorMessage = "Select issue by!")]
        public int IssueBy { get; set; }
        public string IssueByName { get; set; }
        [Required(ErrorMessage = "Select location!")]
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedby { get; set; }
        public string txtItemDetails { get; set; } 
        public List<IntermediateRejectionNoteDetailsBO> IntermediateRej_NoteDetails { get; set; }

        //Used this fileds for Intermediate Rejction Note Report 
        [Required(ErrorMessage = "Please Select From Date!")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Please Select To Date!")]
        public DateTime toDate { get; set; }

        public int SrNo { get; set; }

        public string ItemName { get; set; }

        public Nullable<double> AvailableStockQuantity { get; set; } 
        public Nullable<double> RejectedQuantity { get; set; }     
        public Nullable<double> BalanceQuantity { get; set; } 
        //public string Intermediate_RejNote_Date { get; set; }   
    }

    public class IntermediateRejectionNoteDetailsBO 
    { 
        public int ID { get; set; }
        public Nullable<int> Intermediate_Rej_NoteId { get; set; } 
        public Nullable<int> ItemId { get; set; }
        public string Item_Code { get; set; }
        public string Item_Name { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public string CurrencyName { get; set; }
        public int CurrencyId { get; set; }
        public decimal CurrencyPrice { get; set; }
        public Nullable<int> UnitId { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<double> AvailableStockQuantity { get; set; } 
        public Nullable<double> RejectedQuantity { get; set; }
        public Nullable<double> BalanceQuantity { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedby { get; set; }
    }

}
