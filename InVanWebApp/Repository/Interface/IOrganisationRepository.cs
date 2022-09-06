using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IOrganisationRepository
    {
        //Define function for fetching details of Item type.
        IEnumerable<OrganisationsBO> GetAll();

        //Define function for fetching details of Item type by ID.
        OrganisationsBO GetById(int ID);

        //Function define for: Insert record.
        ResponseMessageBO Insert(OrganisationsBO model);

        //Function define for: Update master record.
        ResponseMessageBO Update(OrganisationsBO model);

        //Function define for: Delete record of item type using it's ID
        void Delete(int ID, int userId);

        //Function for dropdown
        IEnumerable<OrganisationGroupBO> GetOrganisationGroupList();
    }
}
