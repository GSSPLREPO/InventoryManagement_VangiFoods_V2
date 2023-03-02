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
    public class ProductionIndentRepository : IProductionIndentRepository 
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(IProductionIndentRepository));

        #region Bind grid
        ///<summary>
        ///Rahul:  This function is for fecthing list of Production Indents. 
        ///</summary>
        ///<returns></returns>
        public IEnumerable<ProductionIndentBO> GetAll()
        {
            List<ProductionIndentBO> productionIndentList = new List<ProductionIndentBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Indent_GetAllForIndent", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ProductionIndentBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            ProductionIndentNo = reader["IndentNo"].ToString(),
                            IssueDate = Convert.ToDateTime(reader["IndentDate"]),                            
                            IndentStatus = reader["IndentStatus"].ToString(),
                            Description = reader["Description"].ToString(),
                            IndentCount = Convert.ToInt32(reader["IndentCount"])
                        };
                        productionIndentList.Add(result); 
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return productionIndentList;
        }
        #endregion
    }
}