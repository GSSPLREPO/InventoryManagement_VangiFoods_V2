using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IWaterAnalysisRepository
    {
        //Define function for fetching details of WaterAnalysis.
        IEnumerable<WaterAnalysisBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(WaterAnalysisBO model);

        //Function define for: Delete record of WaterAnalysis using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of WaterAnalysis by Id.
        WaterAnalysisBO GetById(int Id);

        //Function define for: Update WaterAnalysis record.
        ResponseMessageBO Update(WaterAnalysisBO model);

        List<WaterAnalysisBO> GetAllWaterAnalysisList(int flagdate, DateTime? FromDate = null, DateTime? ToDate = null);
    }
}
