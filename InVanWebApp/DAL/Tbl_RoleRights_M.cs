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
    
    public partial class Tbl_RoleRights_M
    {
        public int RoleRightsId { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<int> ScreenId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    
        public virtual Tbl_Roles_M Tbl_Roles_M { get; set; }
        public virtual Tbl_Screens_M Tbl_Screens_M { get; set; }
    }
}
