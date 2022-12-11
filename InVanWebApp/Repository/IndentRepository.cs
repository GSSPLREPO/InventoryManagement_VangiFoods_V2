using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;

namespace InVanWebApp.Repository
{
    public class IndentRepository:IIndentRepository
    {
        //private readonly InVanDBContext _context;
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(IndentRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of indents.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IndentBO> GetAll(string type=null)
        {
            List<IndentBO> resultList = new List<IndentBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Indent_GetAll", con);
                    cmd.Parameters.AddWithValue("@Type",type);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new IndentBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            IndentNo = reader["IndentNo"].ToString(),
                            IndentDate = Convert.ToDateTime(reader["IndentDate"]),
                            IndentDueDate = Convert.ToDateTime(reader["IndentDueDate"]),
                            IndentStatus = reader["IndentStatus"].ToString(),
                            Description = reader["Description"].ToString()
                        };
                        resultList.Add(result);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return resultList;

            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Update functions
        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public List<Indent_DetailsBO> GetById(int id)
        {
            var resultList = new List<Indent_DetailsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_IndentItemDetails_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = new Indent_DetailsBO()
                        {
                            IndentID = Convert.ToInt32(reader["IndentID"]),
                            ItemName = reader["ItemName"].ToString(),
                            ItemId= Convert.ToInt32(reader["ItemId"]),
                            RequiredQuantity= Convert.ToDouble(reader["RequiredQuantity"]),
                            SentQuantity= Convert.ToDouble(reader["SentQuantity"])
                            
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return resultList;
            //return _context.UnitMasters.Find(UnitID);
        }
        #endregion
    }
}