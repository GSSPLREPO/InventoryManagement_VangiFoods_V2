using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using InVanWebApp.DAL;
using InVanWebApp.Repository;

namespace InVanWebApp.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly InVanDBContext _context;
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;

        #region Initializing constructor.
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public ItemRepository()
        {
            //Define the DbContext object.
            _context = new InVanDBContext();
        }

        //Constructor with parameter for initializing the DbContext object.
        public ItemRepository(InVanDBContext context)
        {
            _context = context;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of item master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemMaster> GetAll()
        {
            List<ItemMaster> itemMastersList = new List<ItemMaster>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Item_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var ItemMasters = new ItemMaster()
                    {
                        ItemID = Convert.ToInt32(reader["ItemID"]),
                        ItemName = reader["ItemName"].ToString(),
                        Description = reader["Description"].ToString()
                    };
                    itemMastersList.Add(ItemMasters);
                }
                con.Close();
                return itemMastersList;
            }
            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Update functions
        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="ItemId"></param>
        /// <returns></returns>

        public ItemMaster GetById(int ItemId)
        {
            var itemMaster = new ItemMaster();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Item_GetByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemID", ItemId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    itemMaster = new ItemMaster()
                    {
                        ItemID = Convert.ToInt32(reader["ItemID"]),
                        ItemName = reader["ItemName"].ToString(),
                        Description = reader["Description"].ToString()
                    };
                }
                con.Close();
                return itemMaster;
            }
            //return _context.UnitMasters.Find(UnitID);
        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="itemMaster"></param>
        public void Udate(ItemMaster itemMaster)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Item_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemID", itemMaster.ItemID);
                cmd.Parameters.AddWithValue("@ItemName", itemMaster.ItemName);
                cmd.Parameters.AddWithValue("@Description", itemMaster.Description);
                cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            //_context.Entry(unitMaster).State = EntityState.Modified;
        }

        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="itemMaster"></param>
        public void Insert(ItemMaster itemMaster)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Item_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemName", itemMaster.ItemName);
                cmd.Parameters.AddWithValue("@Description", itemMaster.Description);
                cmd.Parameters.AddWithValue("@CreatedBy", 1);
                cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        #endregion

        #region Delete function

        /// <summary>
        /// Delete record by ID
        /// </summary>
        /// <param name="ItemID"></param>
        public void Delete(int ItemID)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Item_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemID", ItemID);
                cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
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