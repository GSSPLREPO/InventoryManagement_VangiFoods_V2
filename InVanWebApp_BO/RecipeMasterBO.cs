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
        [Required(ErrorMessage = "Enter packing size!")]
        public float PackingSize { get; set; }
        [Required(ErrorMessage = "Enter packing size unit!")]
        public Nullable<int> PackingSizeUnit { get; set; }
        [Required(ErrorMessage = "Select product name!")] 
        public Nullable<int> ProductID { get; set; }  
        public string ProductName { get; set; }
        public Nullable<bool> IsDeleted { get; set; } 
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        [Required(ErrorMessage = "Select unit of masurement!")]
        public Nullable<int> UOM_Id { get; set; }
        //This is for inserting the recipe_Details 
        public string UnitName { get; set; }
        public Nullable<int> Item_ID { get; set; }
        public string ItemCode { get; set; }
        //public string ItemUnit { get; set; }
        public string ItemName { get; set; }
        public float Ratio { get; set; }
        public string TxtItemDetails { get; set; } 
        public List<Recipe_DetailsBO> recipe_Details { get; set; }
    }
    public class Recipe_DetailsBO 
    {
        public int RecipeIngredientsDetailID { get; set; }
        public Nullable<int> RecipeID { get; set; }
        public string RecipeName { get; set; }
        public Nullable<int> ItemId { get; set; }
        //public Nullable<int> Item_ID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> UOM_Id { get; set; }
        public string UnitName { get; set; }
        public float Ratio { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

    }

}
