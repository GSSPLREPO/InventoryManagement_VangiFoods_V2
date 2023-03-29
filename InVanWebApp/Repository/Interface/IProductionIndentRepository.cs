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
        IEnumerable<Recipe_DetailsBO> GetRecipeDetailsById(int ProductId, int Recipe_Id);
        //Bind all Batch Number details by SO_Id and Total_Batches 
        IEnumerable<BatchNumberMasterBO> GetBatchNumberById(int SO_Id, int Total_Batches);
        //Define function for inserting data into Production Indent 
        ResponseMessageBO Insert(ProductionIndentBO model);
        //Bind dropdown of Sales Order Number 
        IEnumerable<SalesOrderBO> GetSONumberForDropdown();
        //This function is for fetch data for editing by ID for Production Indent
        ProductionIndentBO GetById(int id);
        //This function is used to get the list of Items againts Production Indent ID 
        List<ProductionIndent_DetailsBO> GetItemDetailsByProductionIndentId(int ProductionIndentID);
        //Function define for the update of Production Indent.
        ResponseMessageBO Update(ProductionIndentBO model); 
        //Function define for the delete Production Indent details
        void Delete(int ID, int userId);

        //IEnumerable<SalesOrderBO> BindWorkOrderNo(int wONId);

        SalesOrderBO GetWorkOrderNumber(int id);
       // IEnumerable<BatchNumberMasterBO> GetBatchNumber(int SO_Id, int Total_Batches);
        
    }

}
