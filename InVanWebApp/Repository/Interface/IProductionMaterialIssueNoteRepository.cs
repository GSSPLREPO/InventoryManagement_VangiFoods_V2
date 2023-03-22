using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
   public interface IProductionMaterialIssueNoteRepository
    {
        //This function is for fecthing list of Production Material Issue Note data.
        IEnumerable<ProductionMaterialIssueNoteBO> GetAll();
        //Insert record.
        ResponseMessageBO Insert(ProductionMaterialIssueNoteBO model);
        //This function is for fetch data for editing by ID and for downloading pdf
        ProductionMaterialIssueNoteBO GetById(int ID);
        //Delete record by ID
        ResponseMessageBO Delete(int Id, int userId);
    }

}
