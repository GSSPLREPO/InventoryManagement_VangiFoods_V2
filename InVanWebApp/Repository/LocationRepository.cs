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
    public class LocationRepository : ILocationRepository
    {
        private readonly InVanDBContext _context;
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;

        #region Initializing constructor.
        /// <summary>
        /// Date: 26 may'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public LocationRepository()
        {
            //Define the DbContext object.
            _context = new InVanDBContext();
        }

        //Constructor with parameter for initializing the DbContext object.
        public LocationRepository(InVanDBContext context)
        {
            _context = context;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 26 may'22
        /// Farheen: This function is for fecthing list of location master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LocationMaster> GetAll()
        {
            List<LocationMaster> LocationList = new List<LocationMaster>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_LocationMaster_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var location = new LocationMaster()
                    {
                        LocationID = Convert.ToInt32(reader["LocationID"]),
                        LocationName = reader["LocationName"].ToString(),
                        Address = reader["Address"].ToString(),
                        CountryName = reader["CountryName"].ToString(),
                        StateName = reader["StateName"].ToString(),
                        CityName = reader["CityName"].ToString(),
                        Pincode = Convert.ToInt32(reader["Pincode"])
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
        public void Insert(LocationMaster locationMaster)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_LocationMaster_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LocationName", locationMaster.LocationName);
                cmd.Parameters.AddWithValue("@Address", locationMaster.Address);
                cmd.Parameters.AddWithValue("@Country_ID", locationMaster.Country_ID);
                cmd.Parameters.AddWithValue("@State_ID", locationMaster.State_ID);
                cmd.Parameters.AddWithValue("@City_ID", locationMaster.City_ID);
                cmd.Parameters.AddWithValue("@Pincode", locationMaster.Pincode);
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
        public LocationMaster GetById(int Location_ID)
        {
            var location = new LocationMaster();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_LocationMaster_GetByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LocationID", Location_ID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    location = new LocationMaster()
                    {
                        LocationID = Convert.ToInt32(reader["LocationID"]),
                        LocationName = reader["LocationName"].ToString(),
                        Address = reader["Address"].ToString(),
                        Country_ID = Convert.ToInt32(reader["Country_ID"]),
                        State_ID = Convert.ToInt32(reader["State_ID"]),
                        City_ID = Convert.ToInt32(reader["City_ID"]),
                        Pincode = Convert.ToInt32(reader["Pincode"])
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
        public void Udate(LocationMaster locationMaster)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_LocationMaster_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LocationID", locationMaster.LocationID);
                cmd.Parameters.AddWithValue("@LocationName", locationMaster.LocationName);
                cmd.Parameters.AddWithValue("@Address", locationMaster.Address);
                cmd.Parameters.AddWithValue("@Country_ID", locationMaster.Country_ID);
                cmd.Parameters.AddWithValue("@State_ID", locationMaster.State_ID);
                cmd.Parameters.AddWithValue("@City_ID", locationMaster.City_ID);
                cmd.Parameters.AddWithValue("@Pincode", locationMaster.Pincode);
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

        #region  Bind drop-down of Country
        public IEnumerable<CountryMaster> GetCountryForDropDown()
        {
            List<CountryMaster> CountryList = new List<CountryMaster>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Country_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var Country = new CountryMaster()
                    {
                        CountryID = Convert.ToInt32(reader["CountryID"]),
                        CountryName = reader["CountryName"].ToString()
                    };
                    CountryList.Add(Country);
                }
                con.Close();
                return CountryList;
            }
        }
        #endregion

        #region  Bind drop-down of state list
        public IEnumerable<StateMaster> GetStateForDropdown(int CountryID)
        {
            List<StateMaster> StateList = new List<StateMaster>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_State_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CountryID", CountryID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var stateMaster = new StateMaster()
                    {
                        StateID = Convert.ToInt32(reader["StateID"]),
                        StateName = reader["StateName"].ToString()
                    };
                    StateList.Add(stateMaster);
                }
                con.Close();
                return StateList;
            }
        }
        #endregion

        #region  Bind drop-down of city list
        public IEnumerable<CityMaster> GetCityForDropdown(int StateID)
        {
            List<CityMaster> CityList = new List<CityMaster>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_City_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StateID", StateID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var cityMaster = new CityMaster()
                    {
                        CityID = Convert.ToInt32(reader["CityID"]),
                        CityName = reader["CityName"].ToString()
                    };
                    CityList.Add(cityMaster);
                }
                con.Close();
                return CityList;
            }
        }
        #endregion

        #region Dispose function
        private bool disposed = false;

        /// <summary>
        /// Date: 26 may'22
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