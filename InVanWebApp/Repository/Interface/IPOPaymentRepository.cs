using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IPOPaymentRepository
    {
        //Define function for fetching details of Purchase Order Number.
        Dictionary<int, string> GetPONumbers();
    }
}
