using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IPestControlLogRepository
    {
        //Define function for fetching details of PestControlLog.
        IEnumerable<PestControlLogBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(PestControlLogBO model);

        //Function define for: Delete record of  PestControlLog using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of  PestControlLog by Id.
        PestControlLogBO GetById(int Id);

        //Function define for: Update  PestControlLog record.
        ResponseMessageBO Update(PestControlLogBO model);
        List<PestControlLogBO> GetAllPestCntrolLogList(int flagdate, DateTime? FromDate = null, DateTime? ToDate = null);

    }
}
