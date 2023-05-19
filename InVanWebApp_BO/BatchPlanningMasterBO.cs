using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class BatchPlanningMasterBO
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="Enter document number!")]
        public string BatchPlanningDocumentNo { get; set; }
        //[Required(ErrorMessage ="Enter work order number!")]
        public string WorkOrderNumber { get; set; }
        [Required(ErrorMessage ="Select SO number!")]
        public Nullable<int> SO_Id { get; set; }
        public string SONumber { get; set; }
        [Required(ErrorMessage ="Select product name!")]
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        [Required(ErrorMessage ="Enter packaging size!")]
        public Nullable<decimal> PackingSize { get; set; }
        public string PackingSizeUnit { get; set; }
        [Required(ErrorMessage ="Enter packing type!")]
        public string PackingType { get; set; }
        [Required(ErrorMessage ="Enter order quantity!")]
        public Nullable<decimal> OrderQuantity { get; set; }
        [Required(ErrorMessage ="Enter order quantity unit!")]
        public string OrderQuantityUnit { get; set; }
        [Required(ErrorMessage ="Enter required quantity!")]
        public Nullable<decimal> RequiredQuantityInKG { get; set; }
        public Nullable<float> TotalBatchSize { get; set; }
        public Nullable<float> TotalNoBatches { get; set; }
        [Required(ErrorMessage ="Select location!")]
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
        [Required(ErrorMessage ="Enter total yield of raw material!")]
        public decimal? TotalRawMaterialYeild { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public IEnumerable<BatchPlanning_DetailsBO> batchPlanningItemDetails{ get; set; }
        public string txtItemDetails { get; set; }

        //Added the below field for finding the batch planning is done or not
        public int SOCount { get; set; }
    }

    public class BatchPlanning_DetailsBO
    {
        public int ID { get; set; }
        public Nullable<int> BatchPlanningId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<float> QuantityPercentage { get; set; }
        public Nullable<float> BatchSize { get; set; }
        public Nullable<float> TotalQuantityInBatch { get; set; }
        public Nullable<float> YieldPercentage { get; set; }
        public Nullable<float> ActualRequirement { get; set; }
        public Nullable<float> StockInHand { get; set; }
        public Nullable<float> ToBeProcured { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
