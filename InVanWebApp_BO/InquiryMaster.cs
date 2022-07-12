using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class InquiryMaster
    {
        public int InquiryID { get; set; }
        public Nullable<int> InquiryStatusID { get; set; }
        public string CompanyName { get; set; }
        public string ContactPersonName { get; set; }
        public Nullable<System.DateTime> DateOfInquiry { get; set; }
        public string ClientEmail { get; set; }
        public string ClientAddress { get; set; }
        public Nullable<int> ClientCountry { get; set; }
        public Nullable<int> ClientState { get; set; }
        public Nullable<int> ClientCity { get; set; }
        public string ClientZipCode { get; set; }
        public Nullable<int> ItemCategoryID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public string ItemCode { get; set; }
        public Nullable<double> ItemQuantity { get; set; }
        public Nullable<double> QuotedPrice { get; set; }
        public Nullable<double> ExpectedPrice { get; set; }
        public Nullable<double> CloserPrice { get; set; }
        public string PONumber { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        public virtual ItemCategoryMaster ItemCategoryMaster { get; set; }
        public virtual ItemMaster ItemMaster { get; set; }
    }
}
