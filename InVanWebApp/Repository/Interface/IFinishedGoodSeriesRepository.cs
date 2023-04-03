using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IFinishedGoodSeriesRepository
    {
        //Define function for fetching details of FinishedGoodSeries.
        //IEnumerable<FinishedGoodSeriesBO> GetAll();
        //List<FinishedGoodSeriesBO> GetAllFinishedGoodSeriesList(int flag, DateTime? fromDate = null, DateTime? toDate = null);
        List<FinishedGoodSeriesBO> GetAllFinishedGoodSeriesList();

        //Function define for: Insert details of FinishedGoodSeries.
        //record.
        ResponseMessageBO Insert(FinishedGoodSeriesBO model);

        //Function define for: Delete record of FinishedGoodSeries using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of FinishedGoodSeries by Id.
        FinishedGoodSeriesBO GetById(int Id);

        //Function define for: Update FinishedGoodSeries record.
        ResponseMessageBO Update(FinishedGoodSeriesBO model);


        IEnumerable<SalesOrderBO> GetSONUmberForDropDown();

        IEnumerable<SalesOrderBO > GetBindWorkOrderNo(int id);

        IEnumerable<BatchNumberMasterBO> GetBatchNo(int SOId);

    }
}
