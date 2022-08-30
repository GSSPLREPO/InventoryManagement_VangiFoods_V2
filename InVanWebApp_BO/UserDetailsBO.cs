using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class UserDetailsBO
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Enter name of employee!")]
        [StringLength(100, ErrorMessage = "Legth of name is exceeded!")]
        public string EmployeeName { get; set; }
        public Nullable<int> DepartmentID { get; set; }

        [Required(ErrorMessage = "Select designation!")]
        public Nullable<int> DesignationID { get; set; }

        [Required(ErrorMessage = "Enter Contact number!")]
        public string EmployeeMobileNo { get; set; }
        public Nullable<System.DateTime> EmployeeBirthDate { get; set; }

        [Required(ErrorMessage = "Select DOJ!")]
        public Nullable<System.DateTime> EmployeeJoingDate { get; set; }
        
        [Required(ErrorMessage = "Select gender!")]
        public string EmployeeGender { get; set; }
        
        [Required(ErrorMessage = "Enter address!")]
        [StringLength(300, ErrorMessage = "Legth of address is exceeded!")]
        public string EmployeeAddress { get; set; }
        public string PinNumber { get; set; }
        public string EmployeePic { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        [Required(ErrorMessage = "Select organisation!")]
        public Nullable<int> OrganizationID { get; set; }

        [Required(ErrorMessage = "Enter email id!")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("[a-z0-9._%+-]+@[a-z0-9.-]+.[a-z]{2,4}", 
            ErrorMessage = "Please enter correct email")]
        public string EmailId { get; set; }
        public string Designation{ get; set; }

        [Required(ErrorMessage = "Enter user name!")]
        public string UserName{ get; set; }

        [Required(ErrorMessage = "Enter password!")]
        [StringLength(50, ErrorMessage = "Legth of password is exceeded!")]
        public string Password{ get; set; }

        [Required(ErrorMessage = "Select role!")]
        public int RoleId{ get; set; }
        public bool IsActive { get; set; }
    }
}
