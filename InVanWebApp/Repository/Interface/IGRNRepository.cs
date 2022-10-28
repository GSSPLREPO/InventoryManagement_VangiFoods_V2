using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IGRNRepository
    {
        //Define function for fetching details of Item type.
        IEnumerable<GRN_BO> GetAll();

    }
}
