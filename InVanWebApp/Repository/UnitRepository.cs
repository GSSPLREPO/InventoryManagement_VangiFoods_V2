using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp.DAL;
using System.Data.Entity;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace InVanWebApp.Repository
{
    public class UnitRepository : IUnitRepository
    {
        private readonly InVanDBContext _context;
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;

        #region Initializing constructor.
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public UnitRepository()
        {
            //Define the DbContext object.
            _context = new InVanDBContext();
        }

        //Constructor with parameter for initializing the DbContext object.
        public UnitRepository(InVanDBContext context)
        {
            _context = context;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of unit master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UnitMaster> GetAll()
        {
            List<UnitMaster> unitMastersList = new List<UnitMaster>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Unit_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var UnitMasters = new UnitMaster()
                    {
                        UnitID =Convert.ToInt32(reader["UnitID"]),
                        UnitName = reader["UnitName"].ToString(),
                        UnitCode = reader["UnitCode"].ToString(),
                        Description=reader["Description"].ToString()
                    };
                    unitMastersList.Add(UnitMasters);
                }
                con.Close();
                return unitMastersList;
            }
            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Update functions
        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="UnitID"></param>
        /// <returns></returns>

        public UnitMaster GetById(int UnitID)
        {
            var unitMaster = new UnitMaster();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Unit_GetByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UnitID", UnitID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    unitMaster=new UnitMaster()
                    {
                        UnitID=Convert.ToInt32(reader["UnitID"]),
                        UnitName = reader["UnitName"].ToString(),
                        UnitCode = reader["UnitCode"].ToString(),
                        Description = reader["Description"].ToString()
                    };
                }
                con.Close();
                return unitMaster;
            }
                //return _context.UnitMasters.Find(UnitID);
        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="unitMaster"></param>
        public void Udate(UnitMaster unitMaster)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Unit_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UnitID", unitMaster.UnitID);
                cmd.Parameters.AddWithValue("@UnitName", unitMaster.UnitName);
                cmd.Parameters.AddWithValue("@UnitCode", unitMaster.UnitCode);
                cmd.Parameters.AddWithValue("@Description", unitMaster.Description);
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
        /// <param name="unitMaster"></param>
        public void Insert(UnitMaster unitMaster)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Unit_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UnitName",unitMaster.UnitName);
                cmd.Parameters.AddWithValue("@UnitCode", unitMaster.UnitCode);
                cmd.Parameters.AddWithValue("@Description", unitMaster.Description);
                cmd.Parameters.AddWithValue("@CreatedBy", 1);
                cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
              //  _context.UnitMasters.Add(unitMaster);
        }

        #endregion

        //public void Save()
        //{
        //    _context.SaveChanges();
        //}

        #region Delete function

        /// <summary>
        /// Delete record by ID
        /// </summary>
        /// <param name="UnitID"></param>
        public void Delete(int UnitID)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Unit_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UnitID", UnitID);
                cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            //    UnitMaster unitMaster = _context.UnitMasters.Find(UnitID);
            //_context.UnitMasters.Remove(unitMaster);
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