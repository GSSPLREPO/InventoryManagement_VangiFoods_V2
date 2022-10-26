using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using log4net;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace InVanWebApp.Repository
{
    public class InwardQCSortingRepository : IInwardQCSortingRepository
    {
        //private readonly InVanDBContext _context;
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(InwardQCSortingRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of inward QC data.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InwardQCBO> GetAll()
        {
            List<InwardQCBO> resultList = new List<InwardQCBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InwardQCSorting_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new InwardQCBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            InwardNumber = reader["InwardNumber"].ToString(),
                            InwardQCDate = Convert.ToDateTime(reader["InwardQCDate"]),
                            InwardQCNo = reader["InwardQCNo"].ToString(),
                            //Item_Name = reader["Item_Name"].ToString(),
                            //Item_Code = reader["Item_Code"].ToString(),
                            //ItemUnitPrice = Convert.ToDecimal(reader["ItemUnitPrice"]),
                            InwardQuantity = Convert.ToInt32(reader["InwardQuantity"]),
                            RejectedQuantity = Convert.ToInt32(reader["RejectedQuantity"])
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
        public IEnumerable<InwardNoteBO> GetInwNumberForDropdown()
        {
            List<InwardNoteBO> resultList = new List<InwardNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InwardNote_GetAll", con);
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

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(InwardQCBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InwardQCSorting_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InwardQCNo", model.InwardQCNo);
                    cmd.Parameters.AddWithValue("@InwardNote_Id", model.InwardNote_Id);
                    //cmd.Parameters.AddWithValue("@InwardNumber", model.InwardNumber);
                    cmd.Parameters.AddWithValue("@InwardQCDate", model.InwardQCDate);
                    cmd.Parameters.AddWithValue("@SupplierName", model.SupplierName);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@QuantitiesForSorting", model.QuantitiesForSorting);
                    cmd.Parameters.AddWithValue("@BalanceQuantities", model.BalanceQuantities);
                    cmd.Parameters.AddWithValue("@RejectedQuantities", model.RejectedQuantities);
                    cmd.Parameters.AddWithValue("@WastageQuantities", model.WastageQuantities);
                    cmd.Parameters.AddWithValue("@ReasonsForRejection", model.ReasonsForRejection);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();

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
                // return false;
            }
            return response;
        }

        #endregion

        #region Function for viewing the perticular QC and download the same
        /// <summary>
        /// Farheen: This function is for fetch data for viewing by ID and for downloading pdf
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        public InwardQCBO GetById(int ID)
        {
            var result = new InwardQCBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InwardQC_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = new InwardQCBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            InwardQCNo = reader["InwardQCNo"].ToString(),
                            InwardNumber = reader["InwardNumber"].ToString(),
                            InwardQCDate = Convert.ToDateTime(reader["InwardQCDate"]),
                            InwardNote_Id = Convert.ToInt32(reader["InwardNote_Id"])
                        };
                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return result;
        }

        #endregion

        #region Bind all Inward note details 
        public IEnumerable<InwardNoteBO> GetInwDetailsById(int InwQCId, int InwdNote_Id)
        {
            List<InwardNoteBO> resultList = new List<InwardNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {

                    if (InwdNote_Id == 0)
                    {
                        SqlCommand cmd = new SqlCommand("usp_tbl_InwNoteForInwQC_GetByID", con);
                        cmd.Parameters.AddWithValue("@ID", InwQCId);
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader dataReader = cmd.ExecuteReader();

                        while (dataReader.Read())
                        {
                            var result = new InwardNoteBO()
                            {
                                ID = Convert.ToInt32(dataReader["ID"]),
                                InwardDate = Convert.ToDateTime(dataReader["InwardDate"]),
                                PONumber = dataReader["PONumber"].ToString(),
                                SupplierDetails = dataReader["SupplierDetails"].ToString()
                            };
                            resultList.Add(result);
                        }
                        con.Close();

                        SqlCommand cmd2 = new SqlCommand("usp_tbl_InwardQCItemDetails_GetByInwId", con);
                        cmd2.Parameters.AddWithValue("@ID", InwQCId);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader dataReader2 = cmd2.ExecuteReader();

                        while (dataReader2.Read())
                        {
                            var result = new InwardNoteBO()
                            {
                                Item_Name = dataReader2["Item_Name"].ToString(),
                                Item_Code = dataReader2["Item_Code"].ToString(),
                                ItemUnitPrice = Convert.ToDecimal(dataReader2["ItemUnitPrice"]),
                                InwardQuantity = Convert.ToDouble(dataReader2["InwardQuantity"]),
                                QuantityTookForSorting = float.Parse(dataReader2["QuantityTookForSorting"].ToString()),
                                BalanceQuantity = float.Parse(dataReader2["BalanceQuantity"].ToString()),
                                RejectedQuantity = float.Parse(dataReader2["RejectedQuantity"].ToString()),
                                WastageQuantityInPercentage = float.Parse(dataReader2["WastageQuantityInPercentage"].ToString()),
                                Remarks = dataReader2["Remarks"].ToString()

                            };
                            resultList.Add(result);
                        }
                        con.Close();
                    }
                    else
                    {
                        SqlCommand cmd3 = new SqlCommand("usp_tbl_InwardQCItemDetailsForView_GetByID", con);
                        cmd3.Parameters.AddWithValue("@ID", InwQCId);
                        cmd3.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader dataReader3 = cmd3.ExecuteReader();

                        while (dataReader3.Read())
                        {
                            var result = new InwardNoteBO()
                            {
                                Item_Name = dataReader3["Item_Name"].ToString(),
                                Item_Code = dataReader3["Item_Code"].ToString(),
                                ItemUnitPrice = Convert.ToDecimal(dataReader3["ItemUnitPrice"]),
                                InwardQuantity = Convert.ToDouble(dataReader3["InwardQuantity"]),
                                QuantityTookForSorting = float.Parse(dataReader3["QuantityTookForSorting"].ToString()),
                                RejectedQuantity = float.Parse(dataReader3["RejectedQuantity"].ToString()),
                                Remarks = dataReader3["Remarks"].ToString()

                            };
                            resultList.Add(result);
                        }
                        con.Close();
                    }

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

        #region Delete function

        /// <summary>
        /// Delete record by ID
        /// </summary>
        /// <param name="ID"></param>
        public void Delete(int ID, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InwardQC_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
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

    }
}