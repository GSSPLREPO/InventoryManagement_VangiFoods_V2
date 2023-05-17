using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IIndentRepository
    {
        //Define function for fetching details of Item master, edited the passing parameter for PO.
        IEnumerable<IndentBO> GetAll();

        //Get by Id Indent details
        List<Indent_DetailsBO> GetItemDetailsById(int id, int CurrencyId=0);

        //Function define for the update of Indent.
        ResponseMessageBO Update(IndentBO model);

        //Function define for the delete Indent details
        void Delete(int ID, int userId);

        //Define function for inserting data into Indent
        ResponseMessageBO Insert(IndentBO model);
        IndentBO GetById(int id);
        List<Indent_DetailsBO> GetItemDetailsByIndentId(int IndentId);

        List<ItemBO> GetItemDetailsForDD();
        //This function is for pdf export/view
        IndentBO GetItemDetailsByIndentById(int ID);

    }
}
