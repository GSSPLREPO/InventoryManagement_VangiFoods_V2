using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IIssueNoteRepository
    {
        IEnumerable<IssueNoteBO> GetAll();
        ResponseMessageBO Insert(IssueNoteBO model);
        ResponseMessageBO Delete(int Id, int userId);
        IssueNoteBO GetById(int ID);
        IEnumerable<UsersBO> GetUserNameList();
        IEnumerable<IssueNoteBO> GetIssueNoteNumber();
    }
}
