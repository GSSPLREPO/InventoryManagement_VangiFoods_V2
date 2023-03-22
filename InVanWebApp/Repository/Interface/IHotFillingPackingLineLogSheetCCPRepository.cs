using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IHotFillingPackingLineLogSheetCCPRepository
    {
        //Define function for fetching details of SanitizationAndHygine.
        IEnumerable<HotFillingPackingLineLogSheetCCPBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(HotFillingPackingLineLogSheetCCPBO model);

        //Function define for: Delete record of SanitizationAndHygine using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of SanitizationAndHygine by Id.
        HotFillingPackingLineLogSheetCCPBO GetById(int Id);

        //Function define for: Update SanitizationAndHygine record.
        ResponseMessageBO Update(HotFillingPackingLineLogSheetCCPBO model);
        //Report Method
        //List<HotFillingPackingLineLogSheetCCPBO> GetAllMicroAnalysisList(int flag, DateTime? fromDate = null, DateTime? toDate = null);
    }
}
