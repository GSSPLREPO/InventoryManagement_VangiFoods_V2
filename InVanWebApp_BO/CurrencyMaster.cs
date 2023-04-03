using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class CurrencyMaster
    {
        public int CurrencyID { get; set; }
        public int CountryID { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<double> CurrencyPrice { get; set; }
        public Nullable<double> IndianCurrencyPrice { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        public virtual CountryMaster CountryMaster { get; set; }
    }
}
