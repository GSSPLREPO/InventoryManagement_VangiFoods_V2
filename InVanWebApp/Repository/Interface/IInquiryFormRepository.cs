using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IInquiryFormRepository
    {
        //Define function for fetching details of Inquiry Form. 
        IEnumerable<InquiryFormBO> GetAll();
        //Insert records details of Inquiry Form. 
        ResponseMessageBO Insert(InquiryFormBO inquiryFormBOMaster);        
        //Pass the data to the repository for updating that record.
        InquiryFormBO GetInquiryFormById(int InquiryID);
        //Function define for: Delete record of item type using it's Inquiry Form details. 
        void Delete(int InquiryID, int userId);
        //Function define for: Update master record.
        ResponseMessageBO Update(InquiryFormBO model); 

    }

}
