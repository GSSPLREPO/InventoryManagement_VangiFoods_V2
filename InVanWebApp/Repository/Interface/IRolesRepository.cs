using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository
{
    public interface IRolesRepository
    {
        //Define function for fetching details of Role master.
        IEnumerable<RoleBO> GetAll();

        //Define function for fetching details of Role master by ID.
        RoleBO GetById(int RoleId);

        //Function define for: Insert record.
        ResponseMessageBO Insert(RoleBO roleMaster);

        //Function define for: Update master record.
        ResponseMessageBO Update(RoleBO roleMaster);

        //Function define for: Delete record of role using it's ID
        void Delete(int RoleId);

        //---------------Below functions are for Role and Right module---------------//

        //Get screens list
        IEnumerable<RoleRightBO> GetAllScreens();
        List<RoleRightBO> GetRightsOfScreenRole(int roleId);
        bool InsertRoleRights(string[] screenNames,int roleId, int LastModifiedBy);

        List<ScreenNameBO> GetRoleRightList(int roleId);
    }
}
