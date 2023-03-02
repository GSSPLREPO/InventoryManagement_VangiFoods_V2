using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IProductionRecipeRepository
    {
        //Define function for fetching details of Recipe Master. 
        IEnumerable<RecipeMasterBO> GetAll(); 

        //Function define for: Insert record.
        ResponseMessageBO Insert(RecipeMasterBO item);
        List<ResponseMessageBO> SaveRecipeItemData(List<RecipeMasterBO> model);
        //This function is for fetch data for editing by Recipe_ID 
        RecipeMasterBO GetById(int Recipe_ID);
        //Function define for: Update Recipe master record. 
        ResponseMessageBO Update(RecipeMasterBO item);
        //Function define for: Delete record of item using it's ID
        void Delete(int Recipe_ID, int userId);

    }
}
