using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IPostProductionRN_Repository
    {
        //Define function for fetching details of Sales Order.
        IEnumerable<PostProductionRejectionNoteBO> GetAll();
    }
}
