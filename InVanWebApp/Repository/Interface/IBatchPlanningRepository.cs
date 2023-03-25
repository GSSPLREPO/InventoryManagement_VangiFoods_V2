using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IBatchPlanningRepository
    {
        //Define function for fetching details of batch planning.
        IEnumerable<BatchPlanningMasterBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(BatchPlanningMasterBO model);
        SalesOrderBO GetWorkOrderNumber(int id);
        IEnumerable<Recipe_DetailsBO> GetRecipe(int id, int locationId);

        //Function define for: Delete record of item type using it's sales order id 
        void Delete(int ID, int userId);

    }
}
