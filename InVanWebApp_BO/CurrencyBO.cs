using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class CurrencyBO
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Enter the Currency Name!")]
        public string CurrencyName { get; set; }
        [Required(ErrorMessage = "Enter the Value!")]
        public Nullable<double> Value { get; set; }
        [Required(ErrorMessage = "Enter the  Indian Currency Value!")]
        public Nullable<double> IndianCurrencyValue { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

       
    }
}
