using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IClorinationLogRepository
    {
        //Define function for fetching details of ClorinationLog.
        IEnumerable<ClorinationLogBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(ClorinationLogBO model);

        //Function define for: Delete record of ClorinationLog using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of ClorinationLog by Id.
        ClorinationLogBO GetById(int Id);

        //Function define for: Update ClorinationLog record.
        ResponseMessageBO Update(ClorinationLogBO model);

    }
}
