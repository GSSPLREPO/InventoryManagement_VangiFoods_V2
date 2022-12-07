using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface ITermsConditionRepository
    {
        //Define function for fetching details of terms and condition.
        IEnumerable<TermsAndConditionMasterBO> GetAll();

        //Define function for fetching details of terms and condition master by ID.
        TermsAndConditionMasterBO GetById(int ID);

        //Function define for: Insert record.
        ResponseMessageBO Insert(TermsAndConditionMasterBO taxMaster);

        //Function define for: Update master record.
        ResponseMessageBO Update(TermsAndConditionMasterBO taxMaster);

        //Function define for: Delete record of terms and condition using it's ID
        void Delete(int Id,int LastModifiedId);
    }
}
