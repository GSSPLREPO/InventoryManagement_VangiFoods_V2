using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;

namespace InVanWebApp.Repository
{
    public class CreditNoteRepository : ICreditNoteRepository
    {

        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(GRNRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of credit note.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CreditNoteBO> GetAll()
        {
            List<CreditNoteBO> resultList = new List<CreditNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_CreditNote_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new CreditNoteBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            CreditNoteNo = reader["CreditNoteNo"].ToString(),
                            CreditNoteDate = Convert.ToDateTime(reader["CreditNoteDate"]),
                            GRN_No = reader["GRN_No"].ToString(),
                            Remarks = reader["Remarks"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return resultList;
        }

        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(CreditNoteBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_CreditNote_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CreditNoteNo", model.CreditNoteNo);
                    cmd.Parameters.AddWithValue("@CreditNoteDate", model.CreditNoteDate);
                    cmd.Parameters.AddWithValue("@GRNId", model.GRNId);
                    cmd.Parameters.AddWithValue("@GRN_No", model.GRN_No);
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", model.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    //cmd.ExecuteNonQuery();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
            }
            return response;
        }

        #endregion

        #region Delete function

        /// <summary>
        /// Delete record by ID
        /// </summary>
        /// <param name="ID"></param>
        public void Delete(int Id, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_CreditNote_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", Id);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", userId);
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

        #region This function is for pdf export/view
        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID and for downloading pdf
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        public CreditNoteBO GetById(int ID)
        {
            var result = new CreditNoteBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_CreditNote_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = new CreditNoteBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            CreditNoteNo=reader["CreditNoteNo"].ToString(),
                            CreditNoteDate = Convert.ToDateTime(reader["CreditNoteDate"]),
                            GRN_No = reader["GRN_No"].ToString(),
                            LocationName = reader["LocationName"].ToString(),
                            DeliveryAddress = reader["DeliveryAddress"].ToString()
                        };
                    }
                    con.Close();
                };

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return result;
        }

        public List<CreditNoteDetailsBO> GetCreditNoteDetails(int Id)
        {
            var resultList = new List<CreditNoteDetailsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {

                    SqlCommand cmd1 = new SqlCommand("usp_tbl_CreditNoteDetails_GetByID", con);
                    cmd1.Parameters.AddWithValue("@ID", Id);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader1 = cmd1.ExecuteReader();

                    while (dataReader1.Read())
                    {
                        var result = new CreditNoteDetailsBO()
                        {
                            Item_Name = dataReader1["Item_Name"].ToString(),
                            Item_Code = dataReader1["Item_Code"].ToString(),
                            ItemUnit = dataReader1["ItemUnit"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(dataReader1["ItemUnitPrice"]),
                            ItemQuantity = float.Parse(dataReader1["ItemQuantity"].ToString()),
                            ItemTaxValue = dataReader1["ItemTaxValue"].ToString(),
                            ItemTotalAmount = float.Parse(dataReader1["ItemTotalAmount"].ToString()),
                            CurrencyName = dataReader1["CurrencyName"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return resultList;
        }

        #endregion
    }
}