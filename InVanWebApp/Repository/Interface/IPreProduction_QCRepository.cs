using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IPreProduction_QCRepository
    {
        //Define function for fetching details of Item type.
        IEnumerable<PreProduction_QCBO> GetAll();

        //Get inward note list for dropdown.
        IEnumerable<ProductionMaterialIssueNoteBO> GetQCNumberForDropdown();

        //Function define for: Insert record.
        ResponseMessageBO Insert(PreProduction_QCBO model);

        //Get purchase order details by ID.
        IEnumerable<ProductionMaterialIssueNoteBO> GetProdIndent_NoDeatils(int pQCId, int pMNote_Id = 0);


        //Function define for: Get inward record by it's ID.
        PreProduction_QCBO GetById(int ID);

        //Function define for: Delete record of item type using it's ID
        void Delete(int ID, int userId);
    }
}
