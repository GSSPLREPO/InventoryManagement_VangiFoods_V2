//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InVanWebApp.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class RoleRight
    {
        public int RoleRightId { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<int> ScreenId { get; set; }
        public Nullable<int> UserId { get; set; }
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
    
        public virtual Role Role { get; set; }
        public virtual ScreenName ScreenName { get; set; }
        public virtual User User { get; set; }
    }
}
