﻿using System;
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
    public class LocationRepository : ILocationRepository
    {
        //private readonly InVanDBContext _context;
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;

        #region  Bind grid
        /// <summary>
        /// Date: 26 may'22
        /// Farheen: This function is for fecthing list of location master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LocationMasterBO> GetAll()
        {
            List<LocationMasterBO> LocationList = new List<LocationMasterBO>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_LocationMaster_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var location = new LocationMasterBO()
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        LocationName = reader["LocationName"].ToString(),
                        Remark = reader["Remark"].ToString()
                    };
                    LocationList.Add(location);

                }
                con.Close();
                return LocationList;
            }
            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 26 may'22
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="locationMaster"></param>
        public void Insert(LocationMasterBO locationMaster)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_LocationMaster_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LocationName", locationMaster.LocationName);
                cmd.Parameters.AddWithValue("@Remark", locationMaster.Remark);
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
        /// Date: 26 may'22
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="Location_ID"></param>
        /// <returns></returns>
        public LocationMasterBO GetById(int Location_ID)
        {
            var location = new LocationMasterBO();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_LocationMaster_GetByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LocationID", Location_ID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    location = new LocationMasterBO()
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        LocationName = reader["LocationName"].ToString(),
                        Remark = reader["Remark"].ToString()
                    };
                }
                con.Close();
                return location;
            }

        }

        /// <summary>
        /// Date: 26 may'22
        /// Farheen: Update record
        /// </summary>
        /// <param name="locationMaster"></param>
        public void Update(LocationMasterBO locationMaster)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_LocationMaster_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LocationID", locationMaster.ID);
                cmd.Parameters.AddWithValue("@LocationName", locationMaster.LocationName);
                cmd.Parameters.AddWithValue("@Remark", locationMaster.Remark);
                cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        #endregion

        #region Delete function
        public void Delete(int Location_ID)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_LocationMaster_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LocationID", Location_ID);
                cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            };
        }

        #endregion

        //#region  Bind drop-down of Country
        //public IEnumerable<CountryMaster> GetCountryForDropDown()
        //{
        //    List<CountryMaster> CountryList = new List<CountryMaster>();
        //    using (SqlConnection con = new SqlConnection(conString))
        //    {
        //        SqlCommand cmd = new SqlCommand("usp_tbl_Country_GetAll", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        con.Open();
        //        SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
        //        while (reader.Read())
        //        {
        //            var Country = new CountryMaster()
        //            {
        //                CountryID = Convert.ToInt32(reader["CountryID"]),
        //                CountryName = reader["CountryName"].ToString()
        //            };
        //            CountryList.Add(Country);
        //        }
        //        con.Close();
        //        return CountryList;
        //    }
        //}
        //#endregion

        //#region  Bind drop-down of state list
        //public IEnumerable<StateMaster> GetStateForDropdown(int CountryID)
        //{
        //    List<StateMaster> StateList = new List<StateMaster>();
        //    using (SqlConnection con = new SqlConnection(conString))
        //    {
        //        SqlCommand cmd = new SqlCommand("usp_tbl_State_GetAll", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@CountryID", CountryID);
        //        con.Open();
        //        SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
        //        while (reader.Read())
        //        {
        //            var stateMaster = new StateMaster()
        //            {
        //                StateID = Convert.ToInt32(reader["StateID"]),
        //                StateName = reader["StateName"].ToString()
        //            };
        //            StateList.Add(stateMaster);
        //        }
        //        con.Close();
        //        return StateList;
        //    }
        //}
        //#endregion

        //#region  Bind drop-down of city list
        //public IEnumerable<CityMaster> GetCityForDropdown(int StateID)
        //{
        //    List<CityMaster> CityList = new List<CityMaster>();
        //    using (SqlConnection con = new SqlConnection(conString))
        //    {
        //        SqlCommand cmd = new SqlCommand("usp_tbl_City_GetAll", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@StateID", StateID);
        //        con.Open();
        //        SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
        //        while (reader.Read())
        //        {
        //            var cityMaster = new CityMaster()
        //            {
        //                CityID = Convert.ToInt32(reader["CityID"]),
        //                CityName = reader["CityName"].ToString()
        //            };
        //            CityList.Add(cityMaster);
        //        }
        //        con.Close();
        //        return CityList;
        //    }
        //}
        //#endregion

    }
}