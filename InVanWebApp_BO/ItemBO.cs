﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class ItemBO
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="Select category name!")]
        public Nullable<int> ItemCategory_ID { get; set; }

        [Required(ErrorMessage ="Select type name!")]
        public Nullable<int> ItemTypeID { get; set; }
        
        [Required(ErrorMessage ="Enter item code!")]
        public string Item_Code { get; set; }

        [Required(ErrorMessage ="Enter item name!")]
        [StringLength(100, ErrorMessage = "Legth of item name is exceeded!")]
        public string Item_Name { get; set; }
        public string HSN_Code { get; set; }

        [Required(ErrorMessage ="Enter minimum stock value!")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid stock value!")]
        public Nullable<double> MinStock { get; set; }

        [StringLength(150, ErrorMessage = "Legth of description is exceeded!")]
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        //Added below fields for dropdowns
        public string ItemCategoryName { get; set; }
        public string ItemTypeName { get; set; }
    }
}