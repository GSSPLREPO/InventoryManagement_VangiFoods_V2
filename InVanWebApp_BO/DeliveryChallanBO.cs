using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class DeliveryChallanBO
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="Enter the delivery challan number!")]
        public string DeliveryChallanNumber { get; set; }

        [Required(ErrorMessage ="Select delivery challan date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DeliveryChallanDate { get; set; }
        [Required(ErrorMessage ="Select SO number!")]
        public Nullable<int> SO_Id { get; set; }
        public string SONumber { get; set; }
        public Nullable<System.DateTime> SODate { get; set; }
        public int IndentID { get; set; }
        public string IndentNo { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string SupplierAddress { get; set; }
        public int VendorsID { get; set; }
        public string CompanyName { get; set; }
        public string ShippingAddress { get; set; }
        public string Signature { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public float CurrencyPrice { get; set; }
        public float GrandTotal { get; set; }
        public float TotalAfterTax { get; set; }
        public float OtherTax { get; set; }
        public float DiscountPercentage { get; set; }
        public int TermsAndCondition_ID { get; set; }
        public string Terms { get; set; }
        public string Remarks { get; set; }
        [Required(ErrorMessage ="Select one returnable/not-returnable!")]
        public bool IsReturnable { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        public string txtItemDetails { get; set; }
        public List<DeliveryChallanItemDetailsBO> deliveryChallanItemDetails{ get; set; }

        //Added the below field for Outward report
        public string OutwardNoteDate { get; set; }

        [Required(ErrorMessage = "Please Select From Date!")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Please Select To Date!")]
        public DateTime toDate { get; set; }

    }

    public class DeliveryChallanItemDetailsBO
    {
        public int ID { get; set; }
        public int DeliveryChallanID { get; set; }
        public int Item_ID { get; set; }
        public string ItemName { get; set; }
        public string Item_Code { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public string ItemUnit { get; set; }
        public decimal OutwardQuantity { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal BalanceQuantity { get; set; }
        public string ItemTaxValue { get; set; }
        public decimal TotalItemCost { get; set; }
        public string HSN_Code { get; set; }
        public string Remarks { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public float CurrencyPrice { get; set; }
        public decimal AvailableStockBeforeDelivery { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        //Added the below field for Outward report
        public int SrNo { get; set; }
        public string DeliveryChallanNumber { get; set; }
        public string OutwardDate { get; set; }
        public float DispatchQuantity { get; set; }
        public string DeliveryAddress { get; set; }
        public string ApprovedBy { get; set; }

    }
}
