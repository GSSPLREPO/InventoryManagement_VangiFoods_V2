
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
    
public partial class usp_dashb_ReorderPoint_OnMinStock_Result
{

    public int ID { get; set; }

    public string ItemName { get; set; }

    public string ItemUnit { get; set; }

    public Nullable<decimal> ItemUnitPrice { get; set; }

    public Nullable<double> StockQuantity { get; set; }

    public string CurrencyName { get; set; }

    public Nullable<double> MinStock { get; set; }

}

}