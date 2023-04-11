using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class RoleRightBO
    {
        public int RoleRightId { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<int> ScreenId { get; set; }
        public Nullable<int> LastModifiedUserId { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> CheckedRole { get; set; }
        public Nullable<bool> ApprovedRole { get; set; }
        public Nullable<bool> PreparedRole { get; set; }
        public Nullable<bool> AddRight { get; set; }
        public Nullable<bool> UpdateRight { get; set; }
        public Nullable<bool> DeleteRight { get; set; }
        public Nullable<bool> ViewScreen { get; set; }

        public string ScreenName{ get; set; }
        public int[] ScreensForRole { get; set; }
        public string DisplayName { get; set; }
    }
}
