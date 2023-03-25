using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class BatchPlanningMasterBO 
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Select the SO Number!")] //added
        public int SO_Id { get; set; }
        public string SONumber { get; set; }
        public string WorkOrderNumber { get; set; }
        [Required(ErrorMessage = "Select product name!")]
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Enter packing size!")]
        public decimal PackingSize { get; set; }
        [Required(ErrorMessage = "Enter packing size unit!")]
        public string PackingSizeUnit { get; set; }
        [Required(ErrorMessage = "Enter packing type!")]
        public string PackingType { get; set; }
        public decimal OrderQuantity { get; set; }
        [Required(ErrorMessage = "Select unit of masurement!")]
        public Nullable<int> UOM_Id { get; set; }        
        public string OrderQuantityUnit { get; set; }
        public decimal RequiredQuantityInKG { get; set; }
        public decimal TotalBatchSize { get; set; }
        public decimal TotalNoBatches { get; set; }
        [Required(ErrorMessage = "Enter Remarks!")]
        [StringLength(100, ErrorMessage = "Legth of Remarks is exceeded!")]
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; } 
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

    }

}
