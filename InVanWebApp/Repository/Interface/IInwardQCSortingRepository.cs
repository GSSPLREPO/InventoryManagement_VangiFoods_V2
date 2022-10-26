using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IInwardQCSortingRepository
    {
        //Define function for fetching details of Item type.
        IEnumerable<InwardQCBO> GetAll();
       
        //Get inward note list for dropdown.
        IEnumerable<InwardNoteBO> GetInwNumberForDropdown();
        
        //Function define for: Insert record.
        ResponseMessageBO Insert(InwardQCBO model);

        //Get purchase order details by ID.
        IEnumerable<InwardNoteBO> GetInwDetailsById(int InwQCId, int InwdNote_Id);


        //Function define for: Get inward record by it's ID.
        InwardQCBO GetById(int ID);

        //Function define for: Delete record of item type using it's ID
        void Delete(int ID, int userId);
    }
}
