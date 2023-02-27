using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IVegWasherDosageLogRepository
    {
        //Define function for fetching details of VegWasherDosageLog.
        IEnumerable<VegWasherDosageLogBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(VegWasherDosageLogBO model);

        //Define function for fetching details of VegWasherDosageLog by Id.
        VegWasherDosageLogBO GetById(int Id);

        //Function define for: Update VegWasherDosageLog record.
        ResponseMessageBO Update(VegWasherDosageLogBO model);

        //Function define for: Delete record of VegWasherDosageLog using it's Id
        void Delete(int Id, int userId);
    }
}
