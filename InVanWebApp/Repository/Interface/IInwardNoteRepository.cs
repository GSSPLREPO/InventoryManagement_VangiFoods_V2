using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IInwardNoteRepository
    {
        //Define function for fetching details of Inward note.
        IEnumerable<InwardNoteBO> GetAll();

        //Get purchase order list for dropdown.
        IEnumerable<PurchaseOrderBO> GetPONumberForDropdown();

        //Get purchase order details by ID.
        IEnumerable<PurchaseOrderBO> GetPODetailsById(int PO_Id, int InwId);

        //Function define for: Insert record.
        ResponseMessageBO Insert(InwardNoteBO model);

        //Function define for: Update master record.
        ResponseMessageBO Update(InwardNoteBO model);

        //Function define for: Get inward record by it's ID.
        InwardNoteBO GetById(int ID);

        //Function define for: Delete record of item type using it's ID
        void Delete(int ID, int userId);

        //Rahul: this function id calling from PO for timeline of PO
        InwardNoteBO GetPOById(int PO_Id);
    }
}
