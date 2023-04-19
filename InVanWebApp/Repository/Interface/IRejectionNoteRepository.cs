using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IRejectionNoteRepository 
    {        
        //Define function for fetching details of Item type.
        IEnumerable<RejectionNoteBO> GetAll();
        //Get Rejection note details by ID. 
        IEnumerable<RejectionNoteItemDetailsBO> GetInwardDetailsById(int Id);  
        //Function define for: Insert record. 
        ResponseMessageBO Insert(RejectionNoteBO model);
        //Pass the data to the repository for view that record.
        RejectionNoteBO GetRejectionNoteById(int ID);  
        //Function define for: Delete record of item type using it's RejectionID  
        void Delete(int RejectionID, int userId);
        //Bind dropdown of Pre-Production QC Number 
        IEnumerable<PreProduction_QCBO> GetPreProductionQCNumberForDropdown();
        //Bind all Pre Production note details  
        IEnumerable<ProductionMaterialIssueNoteBO> GetProdIndent_NoDeatils(int PPQCId, int PPNote_Id = 0);
    }
}
