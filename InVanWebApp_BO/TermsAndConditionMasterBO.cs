using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace InVanWebApp_BO
{
    public class TermsAndConditionMasterBO
    {
        public int TermsConditionID { get; set; }

        [Required(ErrorMessage ="Please enter term!")]
        public string TermName { get; set; }

        [Required(ErrorMessage ="Please describe terms and condition!")]
        public string TermDescription { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public int SrNo { get; set; }

    }
}
