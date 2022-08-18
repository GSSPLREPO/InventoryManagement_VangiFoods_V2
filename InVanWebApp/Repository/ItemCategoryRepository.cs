using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp.Repository;
//using InVanWebApp.DAL;
using InVanWebApp_BO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace InVanWebApp.Repository
{
    public class ItemCategoryRepository:IItemCategoryRepository
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
               
        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of item category master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemCategoryMasterBO> GetAll()
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
                        ItemCategoryName = reader["ItemCategoryName"].ToString(),
                        ItemTypeName = reader["ItemTypeName"].ToString(),
                        Description = reader["Description"].ToString()
                    };
                    ItemCategoryList.Add(ItemCategory);
                }
                con.Close();
                return ItemCategoryList;
            }
            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="itemCategoryMaster"></param>
        public void Insert(ItemCategoryMasterBO itemCategoryMaster)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_ItemCategory_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemTypeId", itemCategoryMaster.ItemTypeId);
                cmd.Parameters.AddWithValue("@ItemCategoryName", itemCategoryMaster.ItemCategoryName);
                cmd.Parameters.AddWithValue("@Description", itemCategoryMaster.Description);
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
        /// <param name="ItemCategoryId"></param>
        /// <returns></returns>
        public ItemCategoryMasterBO GetById(int ItemCategoryId)
        {
            var ItemCategory = new ItemCategoryMasterBO();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_ItemCategory_GetByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemCategoryID", ItemCategoryId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ItemCategory = new ItemCategoryMasterBO()
                    {
                        ItemCategoryID = Convert.ToInt32(reader["ItemCategoryID"]),
                        ItemTypeId = Convert.ToInt32(reader["ItemTypeId"]),
                        ItemCategoryName = reader["ItemCategoryName"].ToString(),
                        Description = reader["Description"].ToString()
                    };
                }
                con.Close();
                return ItemCategory;
            }

        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="itemCategoryMaster"></param>
        public void Udate(ItemCategoryMasterBO itemCategoryMaster)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_ItemCategory_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemCategoryId", itemCategoryMaster.ItemCategoryID);
                cmd.Parameters.AddWithValue("@ItemTypeId", itemCategoryMaster.ItemTypeId);
                cmd.Parameters.AddWithValue("@ItemCategoryName", itemCategoryMaster.ItemCategoryName);
                cmd.Parameters.AddWithValue("@Description", itemCategoryMaster.Description);
                cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        #endregion

        #region Delete function
        public void Delete(int ItemCategoryId)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_ItemCategory_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemCategoryID", ItemCategoryId);
                cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            };
        }

        #endregion

        #region  Bind drop-down of Item type
        public IEnumerable<ItemTypeBO> GetItemTypeForDropDown()
        {
            List<ItemTypeBO> ItemMasterList = new List<ItemTypeBO>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_ItemType_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var ItemMaster = new ItemTypeBO()
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        ItemType = reader["ItemType"].ToString()
                    };
                    ItemMasterList.Add(ItemMaster);
                }
                con.Close();
                return ItemMasterList;
            }
        }
        #endregion
    }
}