using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IOilAnalysisRepository
    {
        //Define function for fetching details of OilAnalysis.
        IEnumerable<OilAnalysisBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(OilAnalysisBO model);

        //Function define for: Delete record of OilAnalysis using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of OilAnalysis by Id.
        OilAnalysisBO GetById(int Id);

        //Function define for: Update OilAnalysis record.
        ResponseMessageBO Update(OilAnalysisBO model);

    }
}
