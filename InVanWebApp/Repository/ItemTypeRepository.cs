﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
//using InVanWebApp.DAL;
using InVanWebApp.Repository;

namespace InVanWebApp.Repository
{
    public class ItemTypeRepository : IItemTypeRepository
    {
        //private readonly InVanDBContext _context;
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of item master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemTypeBO> GetAll()
        {
            List<ItemTypeBO> itemMastersList = new List<ItemTypeBO>();
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
                        ID = Convert.ToInt32(reader["SrNo"]),
                        ItemType = reader["ItemType"].ToString(),
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

        public ItemTypeBO GetById(int ItemTypeId)
        {
            var itemMaster = new ItemTypeBO();
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
                return itemMaster;
            }
            //return _context.UnitMasters.Find(UnitID);
        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="itemMaster"></param>
        public void Udate(ItemTypeBO itemMaster)
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
        public void Insert(ItemTypeBO itemMaster)
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
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        #endregion

        #region Delete function

        /// <summary>
        /// Delete record by ID
        /// </summary>
        /// <param name="ItemTypeID"></param>
        public void Delete(int ItemTypeID)
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
        #endregion
    }
}