using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class OrganisationsBO
    {
        public int OrganisationId { get; set; }

        [Required(ErrorMessage = "Select organisation group!")]
        public int OrganisationGroupId { get; set; }

        [Required(ErrorMessage = "Enter organisation name!")]
        [StringLength(100, ErrorMessage = "Legth of name is exceeded!")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Legth of abbrevation is exceeded!")]
        public string Abbreviation { get; set; }
        public string Logo { get; set; }

        [StringLength(50, ErrorMessage = "Legth of name is exceeded!")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "Enter Contact number!")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Enter address!")]
        [StringLength(150, ErrorMessage = "Legth of address is exceeded!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Enter email id!")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("[a-z0-9._%+-]+@[a-z0-9.-]+.[a-z]{2,4}",
            ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }
        public string Website { get; set; }
        public int City { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> StateCode { get; set; }
        public string Country { get; set; }
        public string PANNo { get; set; }
        public string CINNo { get; set; }

        [Required(ErrorMessage = "Enter GST number!")]
        [StringLength(50, ErrorMessage = "Legth of GST is exceeded!")]
        public string GSTINNo { get; set; }

        [StringLength(100, ErrorMessage = "Legth of description is exceeded!")]
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        
        public string OrganisationGroupName { get; set; }
        public string CityName{ get; set; }
        public int UserId{ get; set; }

    }
}
