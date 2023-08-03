using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class MaterialReturnNoteBO 
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Enter Material return note no!")] 
        public string MaterialReturnNoteNo { get; set; } 
        [Required(ErrorMessage = "Select date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> MaterialReturnNoteDate { get; set; }          
        public string WorkOrderNumber { get; set; }
        public string QCNumber { get; set; }
        public Nullable<int> SalesOrderId { get; set; }
        public string SONumber { get; set; }
        [Required(ErrorMessage = "Select return by!")] 
        public int ReturnBy { get; set; } 
        public string ReturnByName { get; set; }
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
        public List<MaterialReturnNoteDetailsBO> Material_ReturnNote_Details { get; set; }

        //Used this fileds for Material return Note Report 
        [Required(ErrorMessage = "Please Select From Date!")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Please Select To Date!")]
        public DateTime toDate { get; set; }

        public int SrNo { get; set; }

        public string ItemName { get; set; }

        public Nullable<double> CurrentStockQuantity { get; set; } 
        public Nullable<double> ReturnQuantity { get; set; }     
        public Nullable<double> FinalQuantity { get; set; }
        //public string MaterialReturnNote_Date { get; set; }   
    }

    public class MaterialReturnNoteDetailsBO 
    { 
        public int ID { get; set; }
        public Nullable<int> MaterialReturnNoteId { get; set; }  
        public Nullable<int> ItemId { get; set; }
        public string Item_Code { get; set; }
        public string Item_Name { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public string CurrencyName { get; set; }
        public int CurrencyId { get; set; }
        public decimal CurrencyPrice { get; set; }
        public Nullable<int> UnitId { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<double> CurrentStockQuantity { get; set; } 
        public Nullable<double> ReturnQuantity { get; set; }
        public Nullable<double> FinalQuantity { get; set; } 
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedby { get; set; }
    }

}
