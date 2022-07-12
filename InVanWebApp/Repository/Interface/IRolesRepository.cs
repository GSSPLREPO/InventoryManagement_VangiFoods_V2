using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp.DAL;

namespace InVanWebApp.Repository
{
    public interface IRolesRepository
    {
        //Define function for fetching details of Role master.
        IEnumerable<Role> GetAll();

        //Define function for fetching details of Role master by ID.
        Role GetById(int RoleId);

        //Function define for: Insert record.
        void Insert(Role roleMaster);

        //Function define for: Update master record.
        void Udate(Role roleMaster);

        //Function define for: Delete record of role using it's ID
        void Delete(int RoleId);
    }
}
