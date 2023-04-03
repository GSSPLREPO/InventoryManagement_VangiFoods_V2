using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class BatchPlanningMasterBO
    {
        public int ID { get; set; }
        public string BatchPlanningDocumentNo { get; set; }
        public string WorkOrderNumber { get; set; }
        public Nullable<int> SO_Id { get; set; }
        public string SONumber { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> PackingSize { get; set; }
        public string PackingSizeUnit { get; set; }
        public string PackingType { get; set; }
        public Nullable<decimal> OrderQuantity { get; set; }
        public string OrderQuantityUnit { get; set; }
        public Nullable<decimal> RequiredQuantityInKG { get; set; }
        public Nullable<decimal> TotalBatchSize { get; set; }
        public Nullable<decimal> TotalNoBatches { get; set; }
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
        public decimal TotalRawMaterialYeild { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public IEnumerable<BatchPlanning_DetailsBO> batchPlanningItemDetails{ get; set; }
        public string txtItemDetails { get; set; }
    }

    public class BatchPlanning_DetailsBO
    {
        public int ID { get; set; }
        public Nullable<int> BatchPlanningId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<decimal> QuantityPercentage { get; set; }
        public Nullable<decimal> BatchSize { get; set; }
        public Nullable<decimal> TotalQuantityInBatch { get; set; }
        public Nullable<decimal> YieldPercentage { get; set; }
        public Nullable<decimal> ActualRequirement { get; set; }
        public Nullable<decimal> StockInHand { get; set; }
        public Nullable<decimal> ToBeProcured { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
