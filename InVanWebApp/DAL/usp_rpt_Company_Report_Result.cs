
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
    
public partial class usp_rpt_Company_Report_Result
{

    public int ID { get; set; }

    public string CompanyType { get; set; }

    public string CompanyName { get; set; }

    public string ContactPersonName { get; set; }

    public string ContactPersonNo { get; set; }

    public string EmailId { get; set; }

    public string Address { get; set; }

    public Nullable<int> CityID { get; set; }

    public Nullable<int> StateID { get; set; }

    public Nullable<int> CountryID { get; set; }

    public Nullable<int> PinCode { get; set; }

    public string GSTNumber { get; set; }

    public string Remarks { get; set; }

    public Nullable<bool> IsActive { get; set; }

    public Nullable<bool> IsBlackListed { get; set; }

    public Nullable<bool> IsDeleted { get; set; }

    public Nullable<int> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

    public Nullable<int> LastModifiedBy { get; set; }

    public Nullable<System.DateTime> LastModifiedDate { get; set; }

}

}