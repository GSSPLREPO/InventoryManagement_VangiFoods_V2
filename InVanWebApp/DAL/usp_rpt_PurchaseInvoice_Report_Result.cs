
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
    
public partial class usp_rpt_PurchaseInvoice_Report_Result
{

    public Nullable<long> SrNo { get; set; }

    public string PONumber { get; set; }

    public Nullable<System.DateTime> PurchaseOrderDate { get; set; }

    public Nullable<System.DateTime> DeliveryDate { get; set; }

    public string CompanyName { get; set; }

    public string InvoiceNumber { get; set; }

    public Nullable<double> AdvancedPayment { get; set; }

    public Nullable<double> AmountPaid { get; set; }

    public Nullable<double> BalancePayment { get; set; }

    public Nullable<System.DateTime> PaymentDate { get; set; }

    public string PaymentStatus { get; set; }

}

}