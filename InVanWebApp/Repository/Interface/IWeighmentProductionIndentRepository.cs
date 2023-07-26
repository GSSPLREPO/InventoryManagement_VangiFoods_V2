using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Data;
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
        //To Convert Array To DataTable 
        DataTable ConvertArrayToDataTable(string[] array);
        //Bind Get Captured Weight Indent Details 
        WeighmentReceivedDataBO GetCapturedWeightIndentDetails();
        ////This function is for delete all the temp records of Clear Captured Weight temp Data    
        void ClearCapturedWeightDataDelete();
        //Define function for inserting data into Weighment Production Indent 
        ResponseMessageBO Insert(Weighment_ProductionIndentBO model);
        ///This function is for fecthing list of Weighment Production Indents. 
        IEnumerable<Weighment_ProductionIndentBO> GetAll();
        ///Fetch Weighment Production by it's ID    
        Weighment_ProductionIndentBO GetById(int id);
        ///This function is used to get the list of Items againts Weighment ID  
        List<Weighment_ProductionIndent_DetailsBO> GetItemDetailsByProductionWeighmentID(int WeighmentID);
        ///Delete function Weighment Production and Ingredients details by using Weighment ID.  
        void Delete(int ID, int userId);

    }
}
