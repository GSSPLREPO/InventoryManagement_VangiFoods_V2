using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IIntermediateRejectionNoteRepository
    {
        //// This function is for fecthing list of Intermediate Rejection Note data.
        IEnumerable<IntermediateRejectionNoteBO> GetAll();
        /// Insert record Intermediate Rejection Note and Intermediate Rejection Note Details. 
        ResponseMessageBO Insert(IntermediateRejectionNoteBO model);

    }
}
