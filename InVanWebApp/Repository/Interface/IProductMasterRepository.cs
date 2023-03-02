using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IProductMasterRepository
    {
        //Define function for fetching details of Recipe Master. 
        IEnumerable<ProductMasterBO> GetAll();

    }
}
