using InVanWebApp_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IProductionIndentRepository
    {
        //Define function for fetching details of Item master, edited the passing parameter for Production Indent. 
        IEnumerable<ProductionIndentBO> GetAll();
        //Get Recipe details by ID. 
        IEnumerable<RecipeMasterBO> GetRecipeDetailsById(int ProductId, int Recipe_Id);
        //Define function for inserting data into Production Indent 
        ResponseMessageBO Insert(ProductionIndentBO model);
        //Bind dropdown of Sales Order Number 
        IEnumerable<SalesOrderBO> GetSONumberForDropdown();
    }

}
