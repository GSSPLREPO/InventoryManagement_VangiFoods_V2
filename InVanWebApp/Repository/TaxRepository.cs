using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;

namespace InVanWebApp.Repository
{
    public class TaxRepository:ITaxRepository
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;

        #region  Bind grid
        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen: This function is for fecthing list of tax master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TaxBO> GetAll()
        {
            List<TaxBO> TaxList = new List<TaxBO>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Tax_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var tax = new TaxBO()
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        TaxName = reader["TaxName"].ToString(),
                        Description = reader["Description"].ToString()
                    };
                    TaxList.Add(tax);

                }
                con.Close();
                return TaxList;
            }
            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="tax"></param>
        public void Insert(TaxBO tax)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_TaxMaster_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TaxName", tax.TaxName);
                cmd.Parameters.AddWithValue("@Description", tax.Description);
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
        /// Date: 19 Aug'22
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public TaxBO GetById(int ID)
        {
            var tax = new TaxBO();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_TaxMaster_GetByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", ID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tax = new TaxBO()
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        TaxName = reader["TaxName"].ToString(),
                        Description = reader["Description"].ToString()
                    };
                }
                con.Close();
                return tax;
            }

        }

        /// <summary>
        /// Date: 19 Aug'22
        /// Farheen: Update record
        /// </summary>
        /// <param name="tax"></param>
        public void Update(TaxBO tax)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_TaxMaster_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", tax.Id);
                cmd.Parameters.AddWithValue("@TaxName", tax.TaxName);
                cmd.Parameters.AddWithValue("@Description", tax.Description);
                cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        #endregion

        #region Delete function
        public void Delete(int taxId)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_TaxMaster_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", taxId);
                cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            };
        }

        #endregion
    }
}