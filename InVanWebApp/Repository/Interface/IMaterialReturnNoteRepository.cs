using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IMaterialReturnNoteRepository
    {
        // This function is for fecthing list of Material Return Note data.
        IEnumerable<MaterialReturnNoteBO> GetAll();
        /// Insert record Material Return Note and Material Return Note Details. 
        ResponseMessageBO Insert(MaterialReturnNoteBO model);
        ///This function is for Material Return Note pdf export/view   
        MaterialReturnNoteBO GetById(int ID);
        /// Delete Material Return Note record by ID
        ResponseMessageBO Delete(int Id, int userId);

    }
}
