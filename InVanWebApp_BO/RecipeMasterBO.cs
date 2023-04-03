using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class RecipeMasterBO
    {
        public int RecipeID { get; set; }
        [Required(ErrorMessage = "Enter recipe name!")]
        [StringLength(100, ErrorMessage = "Legth of recipe name is exceeded!")]
        public string RecipeName { get; set; }
        [Required(ErrorMessage = "Enter recipe description!")]
        [StringLength(100, ErrorMessage = "Legth of recipe description is exceeded!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Select product name!")] 
        public Nullable<int> ProductID { get; set; }  
        public string ProductName { get; set; }
        public Nullable<bool> IsDeleted { get; set; } 
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
       
        //This is for inserting the recipe_Details 
        public string UnitName { get; set; }
        public Nullable<int> Item_ID { get; set; }
        public string ItemCode { get; set; }
        //public string ItemUnit { get; set; }
        public string ItemName { get; set; }
        public float Ratio { get; set; }
        public float BatchSize { get; set; } //Rahul added 15-03-2023. 
        public decimal ItemQuantity { get; set; }
        public decimal FinalQuantity { get; set; }
        public string TxtItemDetails { get; set; } 
        public List<Recipe_DetailsBO> recipe_Details { get; set; }
    }
    public class Recipe_DetailsBO 
    {
        public int RecipeIngredientsDetailID { get; set; }
        public Nullable<int> RecipeID { get; set; }
        public string RecipeName { get; set; }
        public Nullable<int> ItemId { get; set; }        
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> UOM_Id { get; set; }
        public decimal ItemQuantity { get; set; }
        public decimal FinalQuantity { get; set; } 
        public string UnitName { get; set; }
        public float Ratio { get; set; }
        public float BatchSize { get; set; } //Rahul added 15-03-2023. 
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        //Added the below fields for batch planning
        public decimal? RoundedRatio { get; set; }
        public decimal? TotalBatches { get; set; }
        public decimal? Yield { get; set; }
        public decimal? ActualRequirement { get; set; }
        public decimal? StockInHand { get; set; }
        public decimal? ToBeProcured { get; set; }

        //Added the below field for batch planning
        public decimal? SalesOrderQty { get; set; }

    }

}
