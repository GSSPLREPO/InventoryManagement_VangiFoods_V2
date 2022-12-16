﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class InwardQCBO
    {
        public int ID { get; set; }
        
        [Required(ErrorMessage = "Select inward number!")]
        public int InwardNote_Id { get; set; }
        public string InwardNumber { get; set; }

        [Required(ErrorMessage = "Enter QC number!")]
        public string InwardQCNo { get; set; }

        [Required(ErrorMessage = "Select QC date!")]
        [DataType(DataType.Date)]
        public DateTime InwardQCDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        //Fields name in Inward QC details table.

        [Required(ErrorMessage = "Select supplier name!")]
        public string SupplierName { get; set; }
        public Nullable<float> InwardQuantity { get; set; }
        public Nullable<float> RejectedQuantity { get; set; }
        public float BalanceQuantity { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Item_Name { get; set; }
        public string Item_Code { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public string ItemTaxValue { get; set; }
        public float WastageQuantityInPercentage { get; set; }
        public float QuantityTookForSorting { get; set; }

        //Additional fields for Inward QC
        public string QuantitiesForSorting { get; set; }
        public string BalanceQuantities { get; set; }
        public string RejectedQuantities { get; set; }
        public string WastageQuantities { get; set; }
        public string ReasonsForRejection { get; set; }

        //Added the below property for saving the item table details.
        public List<InwardQCDetail> itemDetails { get; set; }
    }

    public class InwardQCDetail
    {
        public int ID { get; set; }
        public Nullable<int> InwardQC_Id { get; set; }
        public string SupplierName { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string Item_Name { get; set; }
        public string Item_Code { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
        public string ItemTaxValue { get; set; }
        public Nullable<double> InwardQuantity { get; set; }
        public Nullable<double> RejectedQuantity { get; set; }
        public Nullable<double> BalanceQuantity { get; set; }
        public Nullable<double> QuantityTookForSorting { get; set; }
        public Nullable<double> WastageQuantityInPercentage { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

    }
}
