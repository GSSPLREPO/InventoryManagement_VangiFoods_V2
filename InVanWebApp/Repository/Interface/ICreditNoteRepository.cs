using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface ICreditNoteRepository
    {
        IEnumerable<CreditNoteBO> GetAll();
        ResponseMessageBO Insert(CreditNoteBO model);
        void Delete(int Id, int userId);
        List<CreditNoteDetailsBO> GetCreditNoteDetails(int Id);
        CreditNoteBO GetById(int ID);
        List<PurchaseOrderBO> GetPONumberForDropdown();
        IEnumerable<PurchaseOrderBO> GetPODetailsById(int POId);
    }
}
