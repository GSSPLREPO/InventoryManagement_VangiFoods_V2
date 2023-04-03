using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IProductEvaluationLogRepository
    {
        //Define function for fetching details of ProductEvaluationLog.
        //IEnumerable<ProductEvaluationLogBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(ProductEvaluationLogBO model);

        //Function define for: Delete record of ProductEvaluationLog using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of ProductEvaluationLog by Id.
        ProductEvaluationLogBO GetById(int Id);

        //Function define for: Update ProductEvaluationLog record.
        ResponseMessageBO Update(ProductEvaluationLogBO model);
        //Define function for fetching details of ProductEvaluationLog in between date.
        List<ProductEvaluationLogBO> GetAllProductEvaluationLogList(int flag, DateTime? fromDate = null, DateTime? toDate = null);



    }
}
