using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IDebitNoteRepository
    {
        IEnumerable<DebitNoteBO> GetAll();
        ResponseMessageBO Insert(DebitNoteBO model);
        void Delete(int Id, int userId);
        DebitNoteBO GetById(int ID);

        IEnumerable<RejectionNoteBO> GetRejctionNoteNoForDD();
    }
}
