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

namespace InVanWebApp.Repository
{
    public class ItemRepository : IItemRepository
    {
        //private readonly InVanDBContext _context;
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
                
        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of item category master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemBO> GetAll()
        {
            List<ItemBO> ItemList = new List<ItemBO>();
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
                return ItemList;
            }
            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="item"></param>
        public void Insert(ItemBO item)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Item_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemTypeID", item.ItemTypeID);
                cmd.Parameters.AddWithValue("@ItemCategory_ID", item.ItemCategory_ID);
                cmd.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                cmd.Parameters.AddWithValue("@Item_Name", item.Item_Name);
                cmd.Parameters.AddWithValue("@HSN_Code", item.HSN_Code);
                cmd.Parameters.AddWithValue("@MinStock", item.MinStock);
                cmd.Parameters.AddWithValue("@Description", item.Description);
                cmd.Parameters.AddWithValue("@CreatedBy", 1);
                cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            };
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
                return item;
            }

        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="item"></param>
        public void Udate(ItemBO item)
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
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        #endregion

        #region Delete function
        public void Delete(int Item_ID)
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

        #endregion

        #region  Bind drop-down of Item category
        public IEnumerable<ItemCategoryMasterBO> GetItemCategoryForDropDown()
        {
            List<ItemCategoryMasterBO> ItemCategoryList = new List<ItemCategoryMasterBO>();
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
                return ItemCategoryList;
            }
        }
        #endregion

        #region  Bind drop-down of item type list
        public IEnumerable<ItemTypeBO> GetItemTypeForDropdown()
        {
            List<ItemTypeBO> ItemTypeList = new List<ItemTypeBO>();
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
                        ItemType= reader["ItemType"].ToString()
                    };
                    ItemTypeList.Add(itemType);
                }
                con.Close();
                return ItemTypeList;
            }
        }
        #endregion

    }
}