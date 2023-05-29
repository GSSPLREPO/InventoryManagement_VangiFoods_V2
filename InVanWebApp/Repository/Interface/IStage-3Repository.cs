using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IStage3Repository
    {
        //Define function for fetching details of SanitizationAndHygine.
        IEnumerable<Stage3BO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(Stage3BO model);

        //Function define for: Delete record of SanitizationAndHygine using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of SanitizationAndHygine by Id.
        Stage3BO GetById(int Id);

        //Function define for: Update SanitizationAndHygine record.
        ResponseMessageBO Update(Stage3BO model);
        //Report Method
        //List<HotFillingPackingLineLogSheetCCPBO> GetAllMicroAnalysisList(int flag, DateTime? fromDate = null, DateTime? toDate = null);
    }
}
