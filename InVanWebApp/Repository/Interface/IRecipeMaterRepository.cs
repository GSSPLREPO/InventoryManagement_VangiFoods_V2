using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IRecipeMaterRepository
    {
        //Define function for fetching details of Recipe Master. 
        IEnumerable<RecipeMasterBO> GetAll();
        //Get list of Items for Recipe Master 
        List<ItemBO> GetItemDetailsForRecipe();
        //Get Recipe item details by ID. 
        ItemBO GetRecipeDetails(int itemID);  

        //Function define for: Insert record.
        ResponseMessageBO Insert(RecipeMasterBO model); 
        //List<ResponseMessageBO> SaveRecipeItemData(List<RecipeMasterBO> model);
        //This function is for fetch data for editing by Recipe_ID 
        RecipeMasterBO GetById(int Recipe_ID);
        //This function is for fetch data for editing by ID.
        List<Recipe_DetailsBO> GetItemDetailsByRecipeId(int Recipe_ID);
        //Function define for: Update Recipe master record. 
        ResponseMessageBO Update(RecipeMasterBO item);
        //Function define for: Delete record of item using it's ID
        void Delete(int Recipe_ID, int userId);

    }
}
