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
    public class AddItemRepository : IAddItemRepository
    {
        private readonly InVanDBContext _context;
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;

        #region Initializing constructor.
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public AddItemRepository()
        {
            //Define the DbContext object.
            _context = new InVanDBContext();
        }

        //Constructor with parameter for initializing the DbContext object.
        public AddItemRepository(InVanDBContext context)
        {
            _context = context;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of item category master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Item> GetAll()
        {
            List<Item> ItemList = new List<Item>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_AddItem_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var item = new Item()
                    {
                        Item_ID = Convert.ToInt32(reader["Item_ID"]),
                        ItemCategoryName = reader["ItemCategoryName"].ToString(),
                        Item_Code = reader["Item_Code"].ToString(),
                        Item_Name = reader["Item_Name"].ToString(),
                        UnitName = reader["UnitName"].ToString(),
                        HSN_Code = reader["HSN_Code"].ToString(),
                        Current_Stock = Convert.ToInt32(reader["Current_Stock"]),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Tax = reader["Tax"].ToString(),
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
        public void Insert(Item item)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_AddItem_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemCategory_ID", item.ItemCategory_ID);
                cmd.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                cmd.Parameters.AddWithValue("@Item_Name", item.Item_Name);
                cmd.Parameters.AddWithValue("@UnitOfMeasurement_ID", item.UnitOfMeasurement_ID);
                cmd.Parameters.AddWithValue("@HSN_Code", item.HSN_Code);
                cmd.Parameters.AddWithValue("@Current_Stock", item.Current_Stock);
                cmd.Parameters.AddWithValue("@Price", item.Price);
                cmd.Parameters.AddWithValue("@Tax", item.Tax);
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
        /// <param name="ItemCategoryId"></param>
        /// <returns></returns>
        public Item GetById(int Item_ID)
        {
            var item = new Item();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_AddItem_GetByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Item_ID", Item_ID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item = new Item()
                    {
                        Item_ID = Convert.ToInt32(reader["Item_ID"]),
                        ItemCategory_ID = Convert.ToInt32(reader["ItemCategory_ID"]),
                        Item_Code = reader["Item_Code"].ToString(),
                        Item_Name = reader["Item_Name"].ToString(),
                        UnitOfMeasurement_ID = Convert.ToInt32(reader["UnitOfMeasurement_ID"]),
                        HSN_Code = reader["HSN_Code"].ToString(),
                        Current_Stock = Convert.ToInt32(reader["Current_Stock"]),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Tax = reader["Tax"].ToString(),
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
        public void Udate(Item item)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_AddItem_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Item_ID", item.Item_ID);
                cmd.Parameters.AddWithValue("@ItemCategory_ID", item.ItemCategory_ID);
                cmd.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                cmd.Parameters.AddWithValue("@Item_Name", item.Item_Name);
                cmd.Parameters.AddWithValue("@UnitOfMeasurement_ID", item.UnitOfMeasurement_ID);
                cmd.Parameters.AddWithValue("@HSN_Code", item.HSN_Code);
                cmd.Parameters.AddWithValue("@Current_Stock", item.Current_Stock);
                cmd.Parameters.AddWithValue("@Price", item.Price);
                cmd.Parameters.AddWithValue("@Tax", item.Tax);
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
                SqlCommand cmd = new SqlCommand("usp_tbl_AddItem_Delete", con);
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
        public IEnumerable<ItemCategoryMaster> GetItemCategoryForDropDown()
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
                        ItemCategoryName = reader["ItemCategoryName"].ToString()
                    };
                    ItemCategoryList.Add(ItemCategory);
                }
                con.Close();
                return ItemCategoryList;
            }
        }
        #endregion

        #region  Bind drop-down of unit list
        public IEnumerable<UnitMaster> GetUnitForDropdown()
        {
            List<UnitMaster> UnitList = new List<UnitMaster>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Unit_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var unitMaster = new UnitMaster()
                    {
                        UnitID = Convert.ToInt32(reader["UnitID"]),
                        UnitName = reader["UnitName"].ToString()
                    };
                    UnitList.Add(unitMaster);
                }
                con.Close();
                return UnitList;
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