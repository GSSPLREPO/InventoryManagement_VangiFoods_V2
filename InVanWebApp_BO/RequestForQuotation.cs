using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class RequestForQuotation
    {
        public int RequestForQuotationId { get; set; }
        public Nullable<int> OrganisationId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string RFQNO { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<int> UnitId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<System.DateTime> BiddingStartDate { get; set; }
        public Nullable<System.DateTime> BiddingEndDate { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public string Signature { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedByID { get; set; }
        public Nullable<System.DateTime> CreatedByDate { get; set; }
        public Nullable<int> LastModifiedByID { get; set; }
        public Nullable<System.DateTime> LastModifiedByDate { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Company Company { get; set; }
        public virtual Item Item { get; set; }
        public virtual LocationMaster LocationMaster { get; set; }
        public virtual RequestForQuotation RequestForQuotation1 { get; set; }
        public virtual RequestForQuotation RequestForQuotation2 { get; set; }
        public virtual UnitMaster UnitMaster { get; set; }
    }
}
