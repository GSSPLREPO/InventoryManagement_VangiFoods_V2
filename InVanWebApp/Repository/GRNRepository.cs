using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using log4net;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using InVanWebApp.Common;

namespace InVanWebApp.Repository
{
    public class GRNRepository : IGRNRepository
    {
        //private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(GRNRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of organisation.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GRN_BO> GetAll()
        {
            List<GRN_BO> resultList = new List<GRN_BO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_GRN_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new GRN_BO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            GRNCode = reader["GRNCode"].ToString(),
                            InwardQCNumber = reader["InwardQCNumber"].ToString(),
                            InwardNoteNumber = reader["InwardNoteNumber"].ToString(),
                            GRNDate = Convert.ToDateTime(reader["GRNDate"]),
                            Remark = reader["Remark"].ToString()
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

        #region Bind dropdown of Inward Number
        public IEnumerable<InwardNoteBO> GetInwardNumberForDropdown()
        {
            List<InwardNoteBO> resultList = new List<InwardNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InwardNoteForGRN_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new InwardNoteBO()
                        {
                            ID = Convert.ToInt32(dataReader["ID"]),
                            InwardNumber = dataReader["InwardNumber"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                };
            }
            catch (Exception ex)
            {
                resultList = null;
                log.Error(ex.Message, ex);
            }
            return resultList;
        }
        #endregion

        #region Bind all inward note details 
        public IEnumerable<InwardNoteBO> GetInwardDetailsById(int Id)
        {
            List<InwardNoteBO> resultList = new List<InwardNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InwdDetailsForGRN_GetByID", con);
                    cmd.Parameters.AddWithValue("@ID", Id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new InwardNoteBO()
                        {
                            PO_Id = Convert.ToInt32(dataReader["PO_Id"]),
                            PONumber = dataReader["PONumber"].ToString(),
                            LocationStockID = Convert.ToInt32(dataReader["LocationId"]),
                            DeliveryAddress = dataReader["DeliveryAddress"].ToString(),
                            SupplierAddress = dataReader["SupplierAddress"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();

                    SqlCommand cmd1 = new SqlCommand("usp_tbl_InwardItemDetailsForGRN_GetByID", con);
                    cmd1.Parameters.AddWithValue("@ID", Id);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader1 = cmd1.ExecuteReader();

                    while (dataReader1.Read())
                    {
                        var result = new InwardNoteBO()
                        {
                            Item_Name = dataReader1["Item_Name"].ToString(),
                            Item_Code = dataReader1["Item_Code"].ToString(),
                            ItemUnit = dataReader1["ItemUnit"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(dataReader1["ItemUnitPrice"]),
                            TotalQuantity = float.Parse(dataReader1["TotalQuantity"].ToString()),
                            InwardQuantity = (dataReader1["InwardQuantity"] != null ? Convert.ToDouble(dataReader1["InwardQuantity"]) : 0),
                            ReceivedQty = float.Parse(dataReader1["ReceivedQty"].ToString()),
                            CurrencyName = dataReader1["CurrencyName"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                    // }
                };
            }
            catch (Exception ex)
            {
                resultList = null;
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
        public ResponseMessageBO Insert(GRN_BO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                //var userId = Session[ApplicationSession.USERID];
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_GRN_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GRNCode", model.GRNCode);
                    cmd.Parameters.AddWithValue("@GRNDate", model.GRNDate);
                    cmd.Parameters.AddWithValue("@InwardQCId", model.InwardQCId);
                    cmd.Parameters.AddWithValue("@PO_Id", model.PO_ID);
                    cmd.Parameters.AddWithValue("@PONumber", model.PONumber);
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", model.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@Remark", model.Remark);
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
        /// <param name="ID"></param>
        public void Delete(int Id, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_GRN_Delete", con);
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

        public GRN_BO GetById(int ID)
        {
            var result = new GRN_BO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_GRN_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = new GRN_BO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            GRNCode=reader["GRNCode"].ToString(),
                            GRNDate=Convert.ToDateTime(reader["GRNDate"]),
                            PO_ID = Convert.ToInt32(reader["PO_Id"]),
                            PONumber = reader["PONumber"].ToString(),
                            InwardQCNumber =reader["InwardQCNumber"].ToString(),
                            InwardNoteNumber = reader["InwardNoteNumber"].ToString(),
                            LocationName = reader["LocationName"].ToString(),
                            DeliveryAddress = reader["DeliveryAddress"].ToString(),
                            SupplierAddress = reader["SupplierAddress"].ToString(),
                            Remark = reader["Remark"].ToString()
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

        public List<GRN_BO> GetGRNItemDetails(int Id)
        {
            var resultList = new List<GRN_BO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {

                    SqlCommand cmd1 = new SqlCommand("usp_tbl_GRNItemDetails_GetByID", con);
                    cmd1.Parameters.AddWithValue("@ID", Id);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader1 = cmd1.ExecuteReader();

                    while (dataReader1.Read())
                    {
                        var result = new GRN_BO()
                        {
                            ItemName = dataReader1["Item_Name"].ToString(),
                            ItemCode = dataReader1["Item_Code"].ToString(),
                            ItemUnit = dataReader1["ItemUnit"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(dataReader1["ItemUnitPrice"]),
                            OrderQty = float.Parse(dataReader1["OrderQuantity"].ToString()),
                            InwardQty = float.Parse(dataReader1["InwardQty"].ToString()),
                            ReceivedQty = float.Parse(dataReader1["ReceivedQuantity"].ToString()),
                            CurrencyName=dataReader1["CurrencyName"].ToString()
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