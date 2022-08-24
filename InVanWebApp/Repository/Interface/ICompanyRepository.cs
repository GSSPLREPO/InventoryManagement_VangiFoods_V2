using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface ICompanyRepository
    {
        //Define function for fetching details of Item master.
        IEnumerable<CompanyBO> GetAll();

        //Define function for fetching details of company master by ID.
        CompanyBO GetById(int ID);

        //Function define for: Insert record.
        bool Insert(CompanyBO model);
        //Function define for: Delete record of company using it's ID
        void Delete(int ID);

    }
}
