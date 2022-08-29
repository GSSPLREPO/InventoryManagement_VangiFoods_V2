using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository
{
    public interface IUnitRepository
    {
        //Define function for fetching details of Unit master.
        IEnumerable<UnitMaster> GetAll();

        //Define function for fetching details of Unit master by ID.
        UnitMaster GetById(int UnitID);

        //Function define for: Insert record.
        ResponseMessageBO Insert(UnitMaster unitMaster);

        //Function define for: Update master record.
        ResponseMessageBO Update(UnitMaster unitMaster);

        //Function define for: Delete record of unit using it's ID
        void Delete(int UnitID);
       // void Save();

    }
}
