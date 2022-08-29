using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp.Repository;
using InVanWebApp_BO;
//using InVanWebApp.DAL;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using log4net;

namespace InVanWebApp.Repository
{
    public class ItemRepository : IItemRepository
    {
        //private readonly InVanDBContext _context;
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(ItemRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of item category master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemBO> GetAll()
        {
            List<ItemBO> ItemList = new List<ItemBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Item_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var item = new ItemBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            ItemCategoryName = reader["ItemCategoryName"].ToString(),
                            ItemTypeName = reader["ItemTypeName"].ToString(),
                            Item_Code = reader["Item_Code"].ToString(),
                            Item_Name = reader["Item_Name"].ToString(),
                            HSN_Code = reader["HSN_Code"].ToString(),
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

            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="item"></param>
        public ResponseMessageBO Insert(ItemBO item)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Item_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemTypeID", item.ItemTypeID);
                    cmd.Parameters.AddWithValue("@ItemCategory_ID", item.ItemCategory_ID);
                    cmd.Parameters.AddWithValue("@ItemTypeName", null);
                    cmd.Parameters.AddWithValue("@ItemCategory_Name", null);
                    cmd.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                    cmd.Parameters.AddWithValue("@Item_Name", item.Item_Name);
                    cmd.Parameters.AddWithValue("@HSN_Code", item.HSN_Code);
                    cmd.Parameters.AddWithValue("@MinStock", item.MinStock);
                    cmd.Parameters.AddWithValue("@Description", item.Description);
                    cmd.Parameters.AddWithValue("@CreatedBy", 1);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    //cmd.ExecuteNonQuery();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.ItemName = dataReader["ItemName"].ToString();
                        response.ItemCode = dataReader["ItemCode"].ToString();
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
        /// Date: 22 Aug 2022
        /// Farheen: This function is for uploading item list (Bulk upload)
        /// </summary>
        /// <param name="model"></param>
        public List<ResponseMessageBO> SaveItemData(List<ItemBO> model)
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
                        ItemBO material = new ItemBO();

                        material.Item_Name = model[i].Item_Name;
                        material.ItemTypeName = model[i].ItemTypeName;
                        material.ItemCategoryName = model[i].ItemCategoryName;
                        material.Item_Code = model[i].Item_Code;
                        material.HSN_Code = model[i].HSN_Code;
                        material.MinStock = model[i].MinStock;
                        material.Description = model[i].Description;

                        material.IsDeleted = false;
                        material.CreatedBy = model[i].CreatedBy;
                        material.CreatedDate = Convert.ToDateTime(model[i].CreatedDate);

                        using (SqlConnection con = new SqlConnection(conString))
                        {
                            SqlCommand cmd = new SqlCommand("usp_tbl_Item_Insert", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            //cmd.Parameters.AddWithValue("@ItemTypeID", 0);
                            //cmd.Parameters.AddWithValue("@ItemCategory_ID", 0);
                            cmd.Parameters.AddWithValue("@ItemTypeName", material.ItemTypeName);
                            cmd.Parameters.AddWithValue("@ItemCategory_Name", material.ItemCategoryName);
                            cmd.Parameters.AddWithValue("@Item_Code", material.Item_Code);
                            cmd.Parameters.AddWithValue("@Item_Name", material.Item_Name);
                            cmd.Parameters.AddWithValue("@HSN_Code", material.HSN_Code);
                            cmd.Parameters.AddWithValue("@MinStock", material.MinStock);
                            cmd.Parameters.AddWithValue("@Description", material.Description);
                            cmd.Parameters.AddWithValue("@CreatedBy", material.CreatedBy);
                            cmd.Parameters.AddWithValue("@CreatedDate", material.CreatedDate);
                            con.Open();
                            //cmd.ExecuteNonQuery();
                            SqlDataReader dataReader = cmd.ExecuteReader();

                            while (dataReader.Read())
                            {
                                ResponseMessageBO response = new ResponseMessageBO();

                                response.ItemName = dataReader["ItemName"].ToString();
                                response.ItemCode = dataReader["ItemCode"].ToString();
                                response.Status = Convert.ToBoolean(dataReader["Status"]);

                                responsesList.Add(response);
                            }
                            con.Close();
                        };

                        //success = true;

                        cnt += 1;
                    }
                }
                //return success;
                return responsesList;
            }
            catch (Exception ex)
            {
                //dbContextTransaction.Dispose();
                throw ex;
            }
        }

        #endregion

        #region Update functions

        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="Item_ID"></param>
        /// <returns></returns>
        public ItemBO GetById(int Item_ID)
        {
            var item = new ItemBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Item_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Item_ID", Item_ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        item = new ItemBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            ItemTypeID = Convert.ToInt32(reader["ItemTypeID"]),
                            ItemCategory_ID = Convert.ToInt32(reader["ItemCategory_ID"]),
                            Item_Code = reader["Item_Code"].ToString(),
                            Item_Name = reader["Item_Name"].ToString(),
                            HSN_Code = reader["HSN_Code"].ToString(),
                            MinStock = Convert.ToInt32(reader["MinStock"]),
                            Description = reader["Description"].ToString()
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
        /// Farheen: Update record
        /// </summary>
        /// <param name="item"></param>
        public ResponseMessageBO Update(ItemBO item)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Item_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Item_ID", item.ID);
                    cmd.Parameters.AddWithValue("@ItemTypeID", item.ItemTypeID);
                    cmd.Parameters.AddWithValue("@ItemCategory_ID", item.ItemCategory_ID);
                    cmd.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                    cmd.Parameters.AddWithValue("@Item_Name", item.Item_Name);
                    cmd.Parameters.AddWithValue("@HSN_Code", item.HSN_Code);
                    cmd.Parameters.AddWithValue("@MinStock", item.MinStock);
                    cmd.Parameters.AddWithValue("@Description", item.Description);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
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
        public void Delete(int Item_ID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Item_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Item_ID", Item_ID);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
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

        #region  Bind drop-down of Item category
        public IEnumerable<ItemCategoryMasterBO> GetItemCategoryForDropDown()
        {
            List<ItemCategoryMasterBO> ItemCategoryList = new List<ItemCategoryMasterBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ItemCategory_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var ItemCategory = new ItemCategoryMasterBO()
                        {
                            ItemCategoryID = Convert.ToInt32(reader["ItemCategoryID"]),
                            ItemCategoryName = reader["ItemCategoryName"].ToString()
                        };
                        ItemCategoryList.Add(ItemCategory);
                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return ItemCategoryList;
        }
        #endregion

        #region  Bind drop-down of item type list
        public IEnumerable<ItemTypeBO> GetItemTypeForDropdown()
        {
            List<ItemTypeBO> ItemTypeList = new List<ItemTypeBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ItemType_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var itemType = new ItemTypeBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            ItemType = reader["ItemType"].ToString()
                        };
                        ItemTypeList.Add(itemType);
                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return ItemTypeList;
        }
        #endregion

    }
}