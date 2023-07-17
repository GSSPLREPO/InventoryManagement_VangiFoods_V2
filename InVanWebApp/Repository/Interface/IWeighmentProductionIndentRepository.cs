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
        //Method used to insert the values Weighment data from weighing machine 
        bool InsertValuesFromDevices(int[] TempValues, int RecordId, DateTime capturedDateTime);
    }
}
