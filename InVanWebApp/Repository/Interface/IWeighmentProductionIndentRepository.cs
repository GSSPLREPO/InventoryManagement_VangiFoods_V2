using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IWeighmentProductionIndentRepository
    {
        //Bind Dropdown For Get Production Indent Number 
        IEnumerable<ProductionIndentBO> GetProductionIndentNumber();
        //Bind Get Production Indent Details
        ProductionIndentBO GetProductionIndentDetails(int id);

    }
}
