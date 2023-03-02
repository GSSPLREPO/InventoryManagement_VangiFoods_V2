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
    public class ProductionRecipeRepository : IProductionRecipeRepository
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ToString();
        private static ILog log = LogManager.GetLogger(typeof(ProductionRecipeRepository));

        #region  Bind grid
        /// <summary>
        /// Rahul: This function is for fecthing list of item category master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RecipeMasterBO> GetAll()
        {
            List<RecipeMasterBO> ItemList = new List<RecipeMasterBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RecipeMaster_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var item = new RecipeMasterBO()
                        {
                            RecipeID = Convert.ToInt32(reader["RecipeID"]),
                            RecipeName = reader["RecipeName"].ToString(),
                            Description = reader["Description"].ToString(),
                            PackingSize = reader["PackingSize"] is DBNull ? 0 : float.Parse(reader["PackingSize"].ToString()),
                            PackingSizeUnit = reader["PackingSizeUnit"] is DBNull ? 0 : Convert.ToInt32(reader["PackingSizeUnit"]),
                            ProductID = reader["ProductID"] is DBNull ? 0 : Convert.ToInt32(reader["ProductID"]),
                            ProductName = reader["ProductName"] is DBNull ? "" : reader["ProductName"].ToString()  
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

        #region Insert function
        /// <summary>
        /// Rahul: Insert record.
        /// </summary>
        /// <param name="item"></param>
        public ResponseMessageBO Insert(RecipeMasterBO item)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RecipeMaster_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;                    
                    cmd.Parameters.AddWithValue("@RecipeName", item.RecipeName);
                    cmd.Parameters.AddWithValue("@Description", item.Description);                    
                    cmd.Parameters.AddWithValue("@PackingSize", item.PackingSize);                    
                    cmd.Parameters.AddWithValue("@PackingSizeUnit", item.PackingSizeUnit);                    
                    cmd.Parameters.AddWithValue("@ProductID", item.ProductID);                    
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);                    
                    cmd.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    //cmd.ExecuteNonQuery();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.ItemName = dataReader["RecipeName"].ToString(); 
                        response.ItemCode = dataReader["Description"].ToString();
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
                };

            }
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
            }

            return response;
        }

        /// <summary>
        /// Date: 27 Feb 2023
        /// Rahul: This function is for uploading Recipe item list (Bulk upload)
        /// </summary>
        /// <param name="model"></param>
        public List<ResponseMessageBO> SaveRecipeItemData(List<RecipeMasterBO> model) 
        {
            try
            {
                //var success = false;
                int cnt = 0;
                List<ResponseMessageBO> responsesList = new List<ResponseMessageBO>();
                if (model != null && model.Count > 0)
                {

                    for (int i = 0; i < model.Count; i++)
                    {
                        RecipeMasterBO material = new RecipeMasterBO();

                        material.RecipeName = model[i].RecipeName;
                        material.Description = model[i].Description;
                        material.PackingSize = model[i].PackingSize;
                        material.PackingSizeUnit = model[i].PackingSizeUnit;
                        material.ProductName = model[i].ProductName;
                        material.IsDeleted = false;
                        material.CreatedBy = model[i].CreatedBy;
                        material.CreatedDate = Convert.ToDateTime(model[i].CreatedDate);

                        using (SqlConnection con = new SqlConnection(conString))
                        {
                            SqlCommand cmd = new SqlCommand("usp_tbl_RecipeMaster_Insert", con); 
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@RecipeName", material.RecipeName);
                            cmd.Parameters.AddWithValue("@Description", material.Description);
                            cmd.Parameters.AddWithValue("@PackingSize", material.PackingSize);
                            cmd.Parameters.AddWithValue("@PackingSizeUnit", material.PackingSizeUnit);
                            //cmd.Parameters.AddWithValue("@ProductID", material.ProductID);
                            cmd.Parameters.AddWithValue("@ProductName", material.ProductName); 
                            cmd.Parameters.AddWithValue("@CreatedBy", material.CreatedBy);
                            cmd.Parameters.AddWithValue("@CreatedDate", material.CreatedDate);
                            con.Open();
                            //cmd.ExecuteNonQuery();
                            SqlDataReader dataReader = cmd.ExecuteReader();

                            while (dataReader.Read())
                            {
                                ResponseMessageBO response = new ResponseMessageBO();

                                response.ItemName = dataReader["RecipeName"].ToString();
                                response.ItemCode = dataReader["Description"].ToString();
                                response.Status = Convert.ToBoolean(dataReader["Status"]);

                                responsesList.Add(response);
                            }
                            con.Close();
                        };

                        cnt += 1;
                    }
                }                
                return responsesList;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        #endregion

        #region Update functions

        /// <summary>
        /// Rahul: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="Recipe_ID"></param>
        /// <returns></returns>
        public RecipeMasterBO GetById(int Recipe_ID) 
        {
            var item = new RecipeMasterBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RecipeMaster_GetByID", con); 
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Recipe_ID", Recipe_ID); 
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        item = new RecipeMasterBO()
                        {
                            RecipeID = Convert.ToInt32(reader["RecipeID"]),
                            RecipeName = reader["RecipeName"].ToString(),
                            Description = reader["Description"].ToString(),
                            PackingSize = reader["PackingSize"] is DBNull ? 0 : float.Parse(reader["PackingSize"].ToString()),
                            PackingSizeUnit = reader["PackingSizeUnit"] is DBNull ? 0 : Convert.ToInt32(reader["PackingSizeUnit"]),
                            ProductID = reader["ProductID"] is DBNull ? 0 : Convert.ToInt32(reader["ProductID"]),
                            ProductName = reader["ProductName"] is DBNull ? "" : reader["ProductName"].ToString()
                        };
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return item;    
        }

        /// <summary>
        /// Rahul: Update record Recipe Master
        /// </summary>
        /// <param name="item"></param>
        public ResponseMessageBO Update(RecipeMasterBO item)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RecipeMaster_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Recipe_ID", item.RecipeID); 
                    cmd.Parameters.AddWithValue("@RecipeName", item.RecipeName);
                    cmd.Parameters.AddWithValue("@Description", item.Description);
                    cmd.Parameters.AddWithValue("@PackingSize", item.PackingSize);
                    cmd.Parameters.AddWithValue("@PackingSizeUnit", item.PackingSizeUnit);
                    cmd.Parameters.AddWithValue("@ProductID", item.ProductID);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", item.LastModifiedBy);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    //cmd.ExecuteNonQuery();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
                return response;
            }
        }
        #endregion

        #region Delete function 
        public void Delete(int Recipe_ID, int userId) 
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RecipeMaster_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Recipe_ID", Recipe_ID);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", userId);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                };
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
        #endregion


    }
}