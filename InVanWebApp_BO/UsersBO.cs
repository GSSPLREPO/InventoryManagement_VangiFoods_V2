using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class UsersBO
    {
        public Nullable<int> UserId { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<int> EmployeeId { get; set; }

        [Required(ErrorMessage ="Enter username!")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Enter the password!")]
        public string Password { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public bool IsActive { get; set; }

        //Added the following for stopping the system after 3 months
        public Boolean flag { get; set; }
    }
}
