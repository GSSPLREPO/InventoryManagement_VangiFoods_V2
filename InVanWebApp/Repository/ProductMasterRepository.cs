using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InVanWebApp.Repository
{
    public class ProductMasterRepository : IProductMasterRepository 
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ToString();
        private static ILog log = LogManager.GetLogger(typeof(ProductionRecipeRepository));

        #region  Bind grid
        /// <summary>
        /// Rahul: This function is for fecthing list of Product Master. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductMasterBO> GetAll()
        {
            List<ProductMasterBO> ItemList = new List<ProductMasterBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ProductMaster_GetAll", con); 
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var item = new ProductMasterBO() 
                        {
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            ProductCode = reader["ProductCode"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            Description = reader["Description"].ToString()
                        };
                        ItemList.Add(item);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return ItemList;
        }
        #endregion


    }
}