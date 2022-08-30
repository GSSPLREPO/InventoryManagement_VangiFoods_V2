using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IUserDetailsRepository
    {
        //Define function for fetching details of Item type.
        IEnumerable<UserDetailsBO> GetAll();

        //Function for fetching lists for dropdowns.
        IEnumerable<OrganisationsBO> GetOrganisationForDropDown();
        IEnumerable<DesignationBO> GetDesignationForDropDown();
        IEnumerable<RoleBO> GetRoleForDropDown();
        
        //Function define for: Insert record.
        ResponseMessageBO Insert(UserDetailsBO model);

        //Define function for fetching details of user details by ID.
        UserDetailsBO GetById(int userId);

        //Function define for: Update master record.
        ResponseMessageBO Update(UserDetailsBO model);

        //Function define for: Delete record of item using it's ID
        void Delete(int userId);

    }
}
