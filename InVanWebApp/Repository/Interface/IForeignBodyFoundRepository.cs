using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IForeignBodyFoundRepository
    {
        //Define function for fetching details of ForeignBodyFound.
        IEnumerable<ForeignBodyFoundBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(ForeignBodyFoundBO model);

        //Function define for: Delete record of ForeignBodyFound using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of ForeignBodyFound by Id.
        ForeignBodyFoundBO GetById(int Id);

        //Function define for: Update ForeignBodyFound record.
        ResponseMessageBO Update(ForeignBodyFoundBO model);

        List<ForeignBodyFoundBO> GetAllForeignBodyFoundList(int flagdate, DateTime? FromDate = null, DateTime? ToDate = null);

    }
}
