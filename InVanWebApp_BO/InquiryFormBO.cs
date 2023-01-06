using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class InquiryFormBO
    {
        public int InquiryID { get; set; }
        public string InquiryNumber { get; set; } //Rahul added 02-01-2023. 
        public Nullable<int> InquiryStatusID { get; set; }
        public Nullable<System.DateTime> DateOfInquiry { get; set; }
        public string ContactPersonName { get; set; }
        public string ClientEmail { get; set; }
        [Required(ErrorMessage = "Select vendors name!")]
        public Nullable<int> VendorsID { get; set; }
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Select the location name!")]
        public Nullable<int> LocationId { get; set; }
        public string LocationName { get; set; }
        [Required(ErrorMessage = "Enter delivery address!")]
        public string DeliveryAddress { get; set; }

        [Required(ErrorMessage = "Enter supplier address!")]
        public string SupplierAddress { get; set; }

        public Nullable<int> ItemCategoryID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<int> Item_ID { get; set; } 
        public string ItemCode { get; set; }
        public Nullable<double> ItemQuantity { get; set; }
        public Nullable<double> QuotedPrice { get; set; }
        public Nullable<double> ExpectedPrice { get; set; }
        public Nullable<double> CloserPrice { get; set; }
        public float CGST { get; set; }
        public float SGST { get; set; }
        public float IGST { get; set; }
        public Nullable<decimal> TotalAfterTax { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public float AdvancedPayment { get; set; } ///Rahul added 04-01-2023. 
        public Nullable<decimal> TotalItemCost { get; set; }
        public string SONumber { get; set; } 
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string InquiryStatus { get; set; } ///Rahul added 02-01-2023. 
                //==============Rahul: These fields are for PO Currency details 02/01/2023==============//
        [Required(ErrorMessage = "Select Currency!")] 
        public int CurrencyID { get; set; }
        public int CountryID { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<double> CurrencyPrice { get; set; }
        public Nullable<double> IndianCurrencyValue { get; set; } 

        [Required(ErrorMessage = "Please select the Delivery date!")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string HSN_Code { get; set; }
        public string Remarks { get; set; }
        public string TxtItemDetails { get; set; } 
        public List<InquiryFormItemDetailsBO> itemDetails { get; set; }  ///Rahul added 03-01-2023. 

    }

}
