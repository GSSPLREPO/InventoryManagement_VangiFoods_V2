using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IMicroAnalysisRepository
    {
        //Define function for fetching details of SanitizationAndHygine.
        IEnumerable<MicroAnalysisBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(MicroAnalysisBO model);

        //Function define for: Delete record of SanitizationAndHygine using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of SanitizationAndHygine by Id.
        MicroAnalysisBO GetById(int Id);

        //Function define for: Update SanitizationAndHygine record.
        ResponseMessageBO Update(MicroAnalysisBO model);
        //Report Method
        List<MicroAnalysisBO> GetAllMicroAnalysisList(int flag, DateTime? fromDate = null, DateTime? toDate = null);

    }
}
