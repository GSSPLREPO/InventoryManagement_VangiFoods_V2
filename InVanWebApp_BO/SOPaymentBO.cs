using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class SOPaymentBO 
    {
        public int ID { get; set; }
        public int SalesOrderId { get; set; } 
      //  [Required(ErrorMessage = "Select PO Number!")]
        public string SONumber { get; set; }        
        public int VendorID { get; set; }

        [Required(ErrorMessage = "Enter Vendor Name!")]
        public string VendorName { get; set; }

        [Required(ErrorMessage = "Enter Invoice Number!")]
        public string InvoiceNumber { get; set; }
        
        [Required(ErrorMessage = "Select payment date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> PaymentDate { get; set; }

        [Required(ErrorMessage = "Select Payment Due Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> PaymentDueDate { get; set; }

        [Required(ErrorMessage = "Enter invoice amount!")]
        public decimal PaymentAmount { get; set; }

        [Required(ErrorMessage = "Select Payment Mode!")]
        public string PaymentMode { get; set; }
        public string BankName { get; set; }
        public string ChequeNumber { get; set; }
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Enter Total PO Payment Amount!")]
        public decimal TotalPOAmount { get; set; }

        [Required(ErrorMessage = "Enter Total Payble Amount!")]
        public int TotalPaybleAmount { get; set; }

        public decimal BalanceAmount { get; set; }

        [Required(ErrorMessage = "Please Select Amount Is Paid or UnPaid!")]
        public string IsPaid { get; set; }
        public string Remarks { get; set; }

        public List<SalesOrderItemsDetailBO> SalesOrderItems { get; set; }
        public decimal AdvancedPayment { get; set; }
        public float AmountPaid{ get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        //Add below fields for IFSC
        public string IFSCCode { get; set; }
        public string BranchName { get; set; }
        public string UTRNo { get; set; }

        //Added the below field for view of PO payment
        public string CurrencyName { get; set; }

        //Added by Siddharth for Credit note.
        public string LocationName { get; set; }
        public Nullable<double> CurrencyPrice { get; set; }
        public int CurrencyID { get; set; }

        //[Required(ErrorMessage = "Select the location name!")]
        public Nullable<int> LocationId { get; set; }

        //[Required(ErrorMessage = "Select vendors name!")]
        public Nullable<int> VendorsID { get; set; }

        public string CompanyName { get; set; }

        //[Required(ErrorMessage = "Enter delivery address!")]
        //public string BuyerAddress { get; set; }
        public string DeliveryAddress { get; set; }

        //[Required(ErrorMessage = "Enter supplier address!")]
        public string SupplierAddress { get; set; }

        //[Required(ErrorMessage = "Select terms and condition!")]
        public Nullable<int> TermsAndConditionID { get; set; }
        public string Terms { get; set; }
        public decimal OtherTax { get; set; }

        public Nullable<int> Item_ID { get; set; }
        public string Item_Code { get; set; }
        public string ItemName { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<decimal> ItemTaxValue { get; set; }
        public decimal ItemQuantity { get; set; }
        public decimal RejectedQuantity { get; set; }
        public Nullable<decimal> TotalItemCost { get; set; }

    }
    public class SalesOrderItemsDetailBO 
     {
        public int ID { get; set; }
        public Nullable<int> SalesOrderId { get; set; } 
        public Nullable<int> Item_ID { get; set; }
        public string ItemName { get; set; }
        public string Item_Code { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
        public Nullable<decimal> ItemQuantity { get; set; }
        public string ItemTaxValue { get; set; }
        public string ItemUnit { get; set; }
        public Nullable<double> TotalItemCost { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

    }
}
