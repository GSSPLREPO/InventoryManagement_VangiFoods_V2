
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
    
public partial class usp_rpt_PostProductionRejection_Report_Result
{

    public Nullable<long> SrNo { get; set; }

    public Nullable<System.DateTime> RNDate { get; set; }

    public string WONumber { get; set; }

    public string BatchNumber { get; set; }

    public string PostProductionRejectionNoteNo { get; set; }

    public string Stage { get; set; }

    public string ItemName { get; set; }

    public string ItemCode { get; set; }

    public Nullable<double> ItemUnitPrice { get; set; }

    public Nullable<double> TotalQty { get; set; }

    public Nullable<double> RejectedQty { get; set; }

    public string Remarks { get; set; }

    public string ApprovedBy { get; set; }

}

}