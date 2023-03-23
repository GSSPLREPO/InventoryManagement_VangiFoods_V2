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

    }
}
