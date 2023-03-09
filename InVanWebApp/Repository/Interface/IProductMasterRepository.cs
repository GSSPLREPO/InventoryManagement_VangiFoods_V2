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
        //Function define for: Insert record.
        ResponseMessageBO Insert(ProductMasterBO item);
        //This function is for fetch data for editing by Product_ID  
        ProductMasterBO GetById(int Product_ID); 
        //Function define for: Update Product master record.  
        ResponseMessageBO Update(ProductMasterBO item);
        //Function define for: Delete record of item using it's ID 
        void Delete(int Product_ID, int userId); 
    }
}
