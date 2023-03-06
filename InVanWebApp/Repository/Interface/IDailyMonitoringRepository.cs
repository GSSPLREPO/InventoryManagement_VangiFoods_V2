using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IDailyMonitoringRepository
    {
        //Define function for fetching details of DailyOperation.
        IEnumerable<DailyMonitoringBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(DailyMonitoringBO model);

        //Function define for: Delete record of DailyOperation using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of DailyOperation by Id.
        DailyMonitoringBO GetById(int Id);

        //Function define for: Update DailyOperation record.
        ResponseMessageBO Update(DailyMonitoringBO model);

    }
}
