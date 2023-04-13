using Dapper;
using InVanWebApp.Common;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace InVanWebApp.Repository
{
    public class RecipeMaterRepository : IRecipeMaterRepository
    {
        //private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ToString();
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(RecipeMaterRepository));

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
                            ProductID = reader["ProductID"] is DBNull ? 0 : Convert.ToInt32(reader["ProductID"]),
                            ProductName = reader["ProductName"] is DBNull ? "" : reader["ProductName"].ToString(),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
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

        #region Get list of Recipe Items from Recipe Master 
        /// <summary>
        /// Rahul: This function is for fecthing list of Recipe item from Recipe Master.
        /// </summary>
        public List<ItemBO> GetItemDetailsForRecipe()
        {
            List<ItemBO> RecipeList = new List<ItemBO>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_GetItemListForRecipe", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var recipe = new ItemBO()
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Item_Code = reader["Item_Code"].ToString(),
                        UnitOfMeasurement_ID = Convert.ToInt32(reader["UOM_Id"]), 
                        UnitCode = reader["UnitCode"].ToString(),
                    };
                    RecipeList.Add(recipe);
                }
                con.Close();
                return RecipeList;
            }
        }
        #endregion

        ///usp_tbl_GetRecipeMasterItemDetailsById
        #region Get details of Recipe Items by ID 
        /// <summary>
        /// Rahul: This function is for fecthing list of items by ID. 
        /// </summary>
        public ItemBO GetRecipeDetails(int itemID)  
        {
            try
            {
                ItemBO ItemDetails = new ItemBO();
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_GetItemDetailsByIdForPOandOC", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemId", itemID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        ItemDetails = new ItemBO()
                        {
                            Item_Name = reader["Item_Name"].ToString(),
                            Item_Code = reader["Item_Code"].ToString(),
                            UnitCode = reader["UnitID"].ToString(),
                            UnitPrice = Convert.ToDouble(reader["UnitPrice"]),
                            ItemTaxValue = float.Parse(reader["ItemTaxValue"].ToString()),
                            IndianCurrencyValue = reader["IndianCurrencyValue"].ToString()
                        };
                    }
                    con.Close();
                    return ItemDetails;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Rahul: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(RecipeMasterBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RecipeMaster_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;                    
                    cmd.Parameters.AddWithValue("@RecipeName", model.RecipeName);
                    cmd.Parameters.AddWithValue("@Description", model.Description);                                     
                    cmd.Parameters.AddWithValue("@ProductID", model.ProductID);                    
                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);                    
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    //cmd.ExecuteNonQuery();
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    var RecipeID = 0;
                    while (dataReader.Read())
                    {
                        response.ItemName = dataReader["RecipeName"].ToString();
                        response.ItemCode = dataReader["Description"].ToString();
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                        RecipeID = Convert.ToInt32(dataReader["RecipeID"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.TxtItemDetails); 

                    List<Recipe_DetailsBO> itemDetails = new List<Recipe_DetailsBO>();

                    foreach (var item in data)
                    {
                        Recipe_DetailsBO objItemDetails = new Recipe_DetailsBO(); 
                        objItemDetails.RecipeID = RecipeID;
                        objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(0).Value);
                        objItemDetails.ItemCode = item.ElementAt(1).Value.ToString();
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.UnitName = item.ElementAt(3).Value.ToString();
                        objItemDetails.Ratio = float.Parse(item.ElementAt(4).Value);
                        objItemDetails.BatchSize = float.Parse(item.ElementAt(5).Value);
                        objItemDetails.Description = item.ElementAt(6).Value.ToString();
                        
                        itemDetails.Add(objItemDetails);
                    }

                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_Recipe_Details_Insert", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@RecipeID", item.RecipeID);
                        cmdNew.Parameters.AddWithValue("@RecipeName", model.RecipeName);
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.ItemId);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.ItemCode);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmdNew.Parameters.AddWithValue("@Ratio", item.Ratio);
                        cmdNew.Parameters.AddWithValue("@BatchSize", item.BatchSize);                        
                        cmdNew.Parameters.AddWithValue("@UnitName", item.UnitName);
                        cmdNew.Parameters.AddWithValue("@Description", item.Description);
                        cmdNew.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                        cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));

                        SqlDataReader dataReaderNew = cmdNew.ExecuteReader();

                        while (dataReaderNew.Read())
                        {
                            response.Status = Convert.ToBoolean(dataReaderNew["Status"]);
                        }
                        con.Close();
                    }
                };
            }
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
            }
            return response;
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
        public List<Recipe_DetailsBO> GetItemDetailsByRecipeId(int Recipe_ID)
        {
            string queryString = "select * From RecipeIngredientsDetails where RecipeID = @RecipeID AND IsDeleted = 0";
            using (SqlConnection con = new SqlConnection(conString))
            {
                var result = con.Query<Recipe_DetailsBO>(queryString, new { @RecipeID = Recipe_ID }).ToList();
                return result;
            }
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
                    cmd.Parameters.AddWithValue("@ProductID", item.ProductID);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", item.LastModifiedBy);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(item.TxtItemDetails);

                    List<Recipe_DetailsBO> itemDetails = new List<Recipe_DetailsBO>();

                    foreach (var items in data)
                    {
                        Recipe_DetailsBO objItemDetails = new Recipe_DetailsBO();
                        objItemDetails.RecipeID = item.RecipeID;
                        objItemDetails.RecipeName = items.ElementAt(0).Value.ToString();
                        objItemDetails.ItemId = Convert.ToInt32(items.ElementAt(0).Value);
                        objItemDetails.ItemCode = items.ElementAt(1).Value.ToString();
                        objItemDetails.ItemName = items.ElementAt(2).Value.ToString();
                        objItemDetails.Ratio = float.Parse(items.ElementAt(4).Value);
                        objItemDetails.BatchSize = float.Parse(items.ElementAt(5).Value);
                        objItemDetails.UnitName = items.ElementAt(3).Value.ToString();
                        objItemDetails.Description = items.ElementAt(6).Value.ToString();

                        itemDetails.Add(objItemDetails);
                    }
                    var count = itemDetails.Count;
                    var i = 1;
                    foreach (var items in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_Recipe_Details_Update", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@RecipeID", items.RecipeID);
                        cmdNew.Parameters.AddWithValue("@RecipeName", item.RecipeName);
                        cmdNew.Parameters.AddWithValue("@Item_ID", items.ItemId);
                        cmdNew.Parameters.AddWithValue("@Item_Code", items.ItemCode);
                        cmdNew.Parameters.AddWithValue("@ItemName", items.ItemName);
                        cmdNew.Parameters.AddWithValue("@Ratio", items.Ratio);
                        cmdNew.Parameters.AddWithValue("@BatchSize", items.BatchSize);                        
                        cmdNew.Parameters.AddWithValue("@UnitName", items.UnitName);
                        cmdNew.Parameters.AddWithValue("@Description", items.Description);
                        cmdNew.Parameters.AddWithValue("@LastModifiedBy", item.LastModifiedBy);
                        cmdNew.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                        if (count == 1)
                            cmdNew.Parameters.AddWithValue("@OneItemIdentifier", 1);
                        else
                        {
                            cmdNew.Parameters.AddWithValue("@OneItemIdentifier", 0);
                            cmdNew.Parameters.AddWithValue("@flagCheck", i);
                        }
                        i++;

                        SqlDataReader dataReaderNew = cmdNew.ExecuteReader();

                        while (dataReaderNew.Read())
                        {
                            response.Status = Convert.ToBoolean(dataReaderNew["Status"]);
                        }
                        con.Close();
                    }                    
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);                
                response.Status = false;
            }
            return response;
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