
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
    
public partial class usp_tbl_CompanyDetails_GetByID_Result
{

    public int ID { get; set; }

    public string CompanyName { get; set; }

    public string CompanyType { get; set; }

    public string EmailId { get; set; }

    public string ContactPersonName { get; set; }

    public string ContactPersonNo { get; set; }

    public string Address { get; set; }

    public string GSTNumber { get; set; }

    public string Remarks { get; set; }

    public Nullable<bool> IsActive { get; set; }

    public Nullable<bool> IsBlackListed { get; set; }

}

}