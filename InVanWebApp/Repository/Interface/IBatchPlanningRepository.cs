using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IBatchPlanningRepository
    {
        //This function is for fecthing list of Production Batch Planning. 
        IEnumerable<BatchPlanningMasterBO> GetAll();

    }
}
