using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class Weighment_ProductionIndentBO
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Enter production indent no.!")]
        public string WeighmentNo { get; set; } 
        [Required(ErrorMessage = "Enter production indent no.!")]
        public Nullable<int> ProductionIndentID { get; set; } 
        public string ProductionIndentNo { get; set; }
        [Required(ErrorMessage = "Select the weighment date!")] 
        public Nullable<System.DateTime> WeighmentDate { get; set; } 
        //[Required(ErrorMessage ="Select user!")] 
        public Nullable<int> RaisedBy { get; set; }
        public string Description { get; set; }
        public Nullable<int> Status { get; set; }
        [Required(ErrorMessage = "Select the Product Name!")]
        public Nullable<int> RecipeID { get; set; } 
        public string RecipeName { get; set; }

        [Required(ErrorMessage = "Select the SO Number!")]
        public Nullable<int> SalesOrderId { get; set; }
        public string SONo { get; set; }
        public string WorkOrderNo { get; set; }
        [Required(ErrorMessage = "Enter Total Batches!")]
        public Nullable<int> TotalBatches { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string UserName { get; set; }
        public string IndentStatus { get; set; }

        //This is for inserting the itemdetails
        public Nullable<int> Item_ID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public Nullable<double> ItemQuantity { get; set; }
        public Nullable<double> FinalQuantity { get; set; }
        public string UnitName { get; set; }
        public float Ratio { get; set; }
        public string TxtItemDetails { get; set; }
        public int IndentCount { get; set; }        
        public string BatchNumber { get; set; }
        public Nullable<int> BatchPlanningDocId { get; set; }
        public string BatchPlanningDocumentNo { get; set; }
        public List<Weighment_ProductionIndent_DetailsBO> indent_Details { get; set; }
        public int WeightDataLenth { get; set; } //// Rahul added 20-07-23.   
        public int ItemId { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public decimal AvailableStock { get; set; }
        public string CurrencyName { get; set; }
        public string ItemUnit { get; set; }
        [Required(ErrorMessage = "Select the SO Number!")]
        public Nullable<int> SO_Id { get; set; }
    }

    //public class BatchNumberMasterBO
    //{
    //    public int ID { get; set; }
    //    public string BatchNumber { get; set; }
    //}
}
