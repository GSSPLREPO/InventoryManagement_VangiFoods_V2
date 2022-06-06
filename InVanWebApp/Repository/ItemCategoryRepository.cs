using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp.Repository;
using InVanWebApp.DAL;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace InVanWebApp.Repository
{
    public class ItemCategoryRepository:IItemCategoryRepository
    {
        private readonly InVanDBContext _context;
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;

        #region Initializing constructor.
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public ItemCategoryRepository()
        {
            //Define the DbContext object.
            _context = new InVanDBContext();
        }

        //Constructor with parameter for initializing the DbContext object.
        public ItemCategoryRepository(InVanDBContext context)
        {
            _context = context;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of item category master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemCategoryMaster> GetAll()
        {
            List<ItemCategoryMaster> ItemCategoryList = new List<ItemCategoryMaster>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_ItemCategory_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var ItemCategory = new ItemCategoryMaster()
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
        public void Insert(ItemCategoryMaster itemCategoryMaster)
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
        public ItemCategoryMaster GetById(int ItemCategoryId)
        {
            var ItemCategory = new ItemCategoryMaster();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_ItemCategory_GetByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemCategoryID", ItemCategoryId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ItemCategory = new ItemCategoryMaster()
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
        public void Udate(ItemCategoryMaster itemCategoryMaster)
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
        public IEnumerable<ItemMaster> GetItemTypeForDropDown()
        {
            List<ItemMaster> ItemMasterList = new List<ItemMaster>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Item_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var ItemMaster = new ItemMaster()
                    {
                        ItemID = Convert.ToInt32(reader["ItemID"]),
                        ItemName = reader["ItemName"].ToString()
                    };
                    ItemMasterList.Add(ItemMaster);
                }
                con.Close();
                return ItemMasterList;
            }
        }
        #endregion

        #region Dispose function
        private bool disposed = false;

        /// <summary>
        /// For releasing unmanageable objects and scarce resources,
        /// like deallocating the controller instance.   
        ///And it get called when the view is rendered.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    _context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}