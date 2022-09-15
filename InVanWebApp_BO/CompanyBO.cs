using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class CompanyBO
    {
        public int ID { get; set; }
        public string CompanyType { get; set; }

        [Required(ErrorMessage = "Enter name of company!")]
        [StringLength(100, ErrorMessage = "Legth of name is exceeded!")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Enter name of contact person!")]
        [StringLength(30, ErrorMessage = "Legth of name is exceeded!")]
        public string ContactPersonName { get; set; }

        [Required(ErrorMessage = "Enter Contact number!")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string ContactPersonNo { get; set; }

        [Required(ErrorMessage = "Enter email id!")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("[a-z0-9._%+-]+@[a-z0-9.-]+.[a-z]{2,4}",
            ErrorMessage = "Please enter correct email")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Enter address!")]
        [StringLength(150, ErrorMessage = "Legth of address is exceeded!")]
        public string Address { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<int> StateID { get; set; }
        public Nullable<int> CountryID { get; set; }
        public Nullable<int> PinCode { get; set; }

        [Required(ErrorMessage = "Enter GST number!")]
        [StringLength(15, ErrorMessage = "Legth of GST is exceeded!")]
        [RegularExpression("^[a-zA-Z0-9]*$",
            ErrorMessage = "Invalid GST No. !")]
        public string GSTNumber{ get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlackListed { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        //Added: Below fields are for supplier and buyer address in OC form.
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
    }
}
