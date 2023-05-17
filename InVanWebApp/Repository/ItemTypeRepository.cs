using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
//using InVanWebApp.DAL;
using InVanWebApp.Repository;
using log4net;
using InVanWebApp.Common;

namespace InVanWebApp.Repository
{
    public class ItemTypeRepository : IItemTypeRepository
    {
        //private readonly InVanDBContext _context;
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(ItemTypeRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of item master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemTypeBO> GetAll()
        {
            List<ItemTypeBO> itemMastersList = new List<ItemTypeBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ItemType_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var ItemMasters = new ItemTypeBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            ItemType = reader["ItemType"].ToString(),
                            Description = reader["Description"].ToString()
                        };
                        itemMastersList.Add(ItemMasters);
                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return itemMastersList;
            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Update functions
        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="ItemId"></param>
        /// <returns></returns>

        public ItemTypeBO GetById(int ItemTypeId)
        {
            var itemMaster = new ItemTypeBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ItemType_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemTypeID", ItemTypeId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        itemMaster = new ItemTypeBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            ItemType = reader["ItemType"].ToString(),
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
            return itemMaster;
            //return _context.UnitMasters.Find(UnitID);
        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="itemMaster"></param>
        public ResponseMessageBO Update(ItemTypeBO itemMaster)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ItemType_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemTypeId", itemMaster.ID);
                    cmd.Parameters.AddWithValue("@ItemType", itemMaster.ItemType);
                    cmd.Parameters.AddWithValue("@Description", itemMaster.Description);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
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

            //_context.Entry(unitMaster).State = EntityState.Modified;
        }

        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="itemMaster"></param>
        public ResponseMessageBO Insert(ItemTypeBO itemMaster)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ItemType_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemType", itemMaster.ItemType);
                    cmd.Parameters.AddWithValue("@Description", itemMaster.Description);
                    cmd.Parameters.AddWithValue("@CreatedBy", 1);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    //cmd.ExecuteNonQuery();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.ItemType = dataReader["ItemType"].ToString();
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
                }
                //return true;

            }
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
               // return false;
            }
            return response;
        }

        #endregion

        #region Delete function

        /// <summary>
        /// Delete record by ID
        /// </summary>
        /// <param name="ItemTypeID"></param>
        public void Delete(int ItemTypeID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ItemType_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemTypeID", ItemTypeID);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
        #endregion
    }
}