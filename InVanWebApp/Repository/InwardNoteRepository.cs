using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Dapper;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;
using InVanWebApp.Common;

namespace InVanWebApp.Repository
{
    public class InwardNoteRepository : IInwardNoteRepository
    {
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(InwardNoteRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of organisation.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InwardNoteBO> GetAll()
        {
            List<InwardNoteBO> resultList = new List<InwardNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InwardNote_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new InwardNoteBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            PONumber = reader["PONumber"].ToString(),
                            InwardNumber = reader["InwardNumber"].ToString(),
                            InwardDate = Convert.ToDateTime(reader["InwardDate"]),
                            ChallanNo=reader["ChallanNo"].ToString(),
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

        #region Bind dropdown of PO Number
        public IEnumerable<PurchaseOrderBO> GetPONumberForDropdown()
        {
            List<PurchaseOrderBO> resultList = new List<PurchaseOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PurchaseOrder_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new PurchaseOrderBO()
                        {
                            PurchaseOrderId = Convert.ToInt32(dataReader["PurchaseOrderId"]),
                            PONumber = dataReader["PONumber"].ToString()
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

        #region Bind all PO details 
        public IEnumerable<PurchaseOrderBO> GetPODetailsById(int PO_Id, int InwId)
        {
            List<PurchaseOrderBO> resultList = new List<PurchaseOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PODetails_GetByID", con);
                    cmd.Parameters.AddWithValue("@ID", PO_Id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new PurchaseOrderBO()
                        {
                            PurchaseOrderId = Convert.ToInt32(dataReader["PurchaseOrderId"]),
                            PODate = Convert.ToDateTime(dataReader["PODate"]),
                            DeliveryAddress = dataReader["DeliveryAddress"].ToString(),
                            SupplierAddress = dataReader["SupplierAddress"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();

                    //==========This condition is for edit functionality "InwId == 0".===========///
                    if (InwId == 0)
                    {
                        //SqlCommand cmd2 = new SqlCommand("usp_tbl_POItemsDetails_GetByID", con);
                        SqlCommand cmd2 = new SqlCommand("usp_tbl_InwardItemDetails_GetByID", con);
                        cmd2.Parameters.AddWithValue("@PO_Id", PO_Id);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader dataReader2 = cmd2.ExecuteReader();

                        while (dataReader2.Read())
                        {
                            var result = new PurchaseOrderBO()
                            {
                                Item_ID = Convert.ToInt32(dataReader2["ItemID"]),
                                ItemName = dataReader2["ItemName"].ToString(),
                                Item_Code = dataReader2["Item_Code"].ToString(),
                                ItemUnitPrice = Convert.ToDecimal(dataReader2["ItemUnitPrice"]),
                                ItemUnit = dataReader2["ItemUnit"].ToString(),
                                ItemQuantity = Convert.ToDecimal(dataReader2["ItemQuantity"]),
                                //ItemTaxValue = dataReader2["ItemTaxValue"].ToString(), rahul updated String to Decimal
                                ItemTaxValue = Convert.ToDecimal(dataReader2["ItemTaxValue"]),
                                InwardQuantity = ((dataReader2["InwardQuantity"] != null) ? Convert.ToDecimal(dataReader2["InwardQuantity"]) : 0),
                                BalanceQuantity = ((dataReader2["BalanceQuantity"] != null) ? Convert.ToDecimal(dataReader2["BalanceQuantity"]) : 0),
                                CurrencyName = dataReader2["CurrencyName"].ToString(),
                                CurrencyID=Convert.ToInt32(dataReader2["CurrencyID"])
                            };
                            resultList.Add(result);
                        }
                        con.Close();
                    }

                    else //==========This else will execute for generating view.===============//
                    {
                        SqlCommand cmd1 = new SqlCommand("usp_tbl_InwardItemDetailsForView_GetByID", con);
                        cmd1.Parameters.AddWithValue("@ID", InwId);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader dataReader1 = cmd1.ExecuteReader();

                        while (dataReader1.Read())
                        {
                            var result = new PurchaseOrderBO()
                            {
                                ItemName = dataReader1["ItemName"].ToString(),
                                Item_Code = dataReader1["Item_Code"].ToString(),
                                ItemUnitPrice = Convert.ToDecimal(dataReader1["ItemUnitPrice"]),
                                ItemUnit = dataReader1["ItemUnit"].ToString(),
                                ItemQuantity = (dataReader1["ItemQuantity"] != null ? Convert.ToDecimal(dataReader1["ItemQuantity"]) : 0),
                                //ItemTaxValue = dataReader1["ItemTaxValue"].ToString(),  rahul updated String to Decimal 
                                ItemTaxValue = Convert.ToDecimal(dataReader1["ItemTaxValue"]),
                                InwardQuantity = ((dataReader1["InwardQuantity"] != null) ? Convert.ToDecimal(dataReader1["InwardQuantity"]) : 0),
                                CurrencyName=dataReader1["CurrencyName"].ToString()
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

        #region Update functions
        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID and for downloading pdf
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        public InwardNoteBO GetById(int ID)
        {
            var result = new InwardNoteBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InwardNote_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = new InwardNoteBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            InwardNumber = reader["InwardNumber"].ToString(),
                            InwardDate = Convert.ToDateTime(reader["InwardDate"]),
                            PODate = Convert.ToDateTime(reader["PODate"]),
                            PO_Id = Convert.ToInt32(reader["PO_Id"]),
                            PONumber = reader["PONumber"].ToString(),
                            ChallanNo=reader["ChallanNo"].ToString(),
                            Signature = reader["Signature"].ToString(),
                            Remarks = reader["Remarks"].ToString()
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

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(InwardNoteBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InwardNote_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@PO_Id", model.PO_Id);
                    //cmd.Parameters.AddWithValue("@PONumber", model.PONumber);
                    cmd.Parameters.AddWithValue("@InwardNumber", model.InwardNumber);
                    cmd.Parameters.AddWithValue("@InwardDate", model.InwardDate);
                    cmd.Parameters.AddWithValue("@Signature", model.Signature);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedBy);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
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
            }
            return response;

        }

        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(InwardNoteBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                //var userId = Session[ApplicationSession.USERID];
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InwardNote_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PO_Id", model.PO_Id);
                    //cmd.Parameters.AddWithValue("@PONumber", model.PONumber);
                    cmd.Parameters.AddWithValue("@InwardNumber", model.InwardNumber);
                    cmd.Parameters.AddWithValue("@InwardDate", model.InwardDate);
                    //cmd.Parameters.AddWithValue("@InwardQuantities", model.InwardQuantities);
                    //cmd.Parameters.AddWithValue("@BalanceQuantities", model.BalanceQuantities);
                    cmd.Parameters.AddWithValue("@Signature", model.Signature);
                    cmd.Parameters.AddWithValue("@ChallanNo", model.ChallanNo);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    //cmd.ExecuteNonQuery();
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int InwardID=0;
                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                        InwardID = Convert.ToInt32(dataReader["InwardNoteId"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.InwardQuantities);

                    List<InwardNoteDetailBO> itemDetails = new List<InwardNoteDetailBO>();

                    foreach (var item in data)
                    {
                        InwardNoteDetailBO objItemDetails = new InwardNoteDetailBO();
                        objItemDetails.PO_ID = model.PO_Id;
                        objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(1).Value);
                        objItemDetails.Item_Name = item.ElementAt(4).Value.ToString();
                        objItemDetails.Item_Code = item.ElementAt(5).Value.ToString();
                        objItemDetails.POQuantity = Convert.ToDecimal(item.ElementAt(6).Value);
                        objItemDetails.ItemTaxValue = item.ElementAt(8).Value.ToString();
                        objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(2).Value);
                        objItemDetails.ItemUnit = (item.ElementAt(7).Value).ToString();
                        objItemDetails.InwardQuantity = Convert.ToDouble(item.ElementAt(0).Value);
                        objItemDetails.BalanceQuantity = Convert.ToDouble(item.ElementAt(3).Value);
                        objItemDetails.CurrencyName = (item.ElementAt(9).Value).ToString();
                        objItemDetails.CreatedBy = model.CreatedBy;
                        objItemDetails.CreatedDate = Convert.ToDateTime(System.DateTime.Now);

                        itemDetails.Add(objItemDetails);
                    }

                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_InwardItemDetails_Insert", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@PurchaseOrderId", item.PO_ID);
                        cmdNew.Parameters.AddWithValue("@InwardNoteId", InwardID);
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.ItemId);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.Item_Name);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                        cmdNew.Parameters.AddWithValue("@POQuantity", item.POQuantity);
                        cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                        cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        cmdNew.Parameters.AddWithValue("@InwardQuantity", item.InwardQuantity);
                        cmdNew.Parameters.AddWithValue("@BalanceQuantity", item.BalanceQuantity);
                        cmdNew.Parameters.AddWithValue("@CurrencyName", item.CurrencyName);
                        cmdNew.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                        cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));

                        SqlDataReader dataReaderNew = cmdNew.ExecuteReader();

                        while (dataReaderNew.Read())
                        {
                            response.Status = Convert.ToBoolean(dataReaderNew["Status"]);
                        }
                        con.Close();
                    }
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
        public void Delete(int ID, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InwardNote_Delete", con);
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

        //Rahul: this function id calling from PO for timeline of PO
        #region GetPOById function for timeline view 

        /// <summary>
        /// GetPOById record by ID, Rahul 08/12/2022. 
        /// </summary>
        /// <param name="ID"></param>
        public InwardNoteBO GetPOById(int PO_Id)
        {

            string inwardNoteQuery = "SELECT TOP(1) * FROM InwardNote WHERE PO_Id = @purchaseOrderId AND IsDeleted = 0 order by CreatedDate desc";
            try
            {

                using (SqlConnection con = new SqlConnection(connString))
                {
                    var inwardNote = con.Query<InwardNoteBO>(inwardNoteQuery, new { @purchaseOrderId = PO_Id }).FirstOrDefault();

                    return inwardNote;
                }
            }
            catch (Exception ex)
            {
                InwardNoteBO inwardNoteBO = new InwardNoteBO();
                log.Error(ex.Message, ex);
                return inwardNoteBO;
            }
        }
        #endregion

    }
}