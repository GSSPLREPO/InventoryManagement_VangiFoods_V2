using InVanWebApp.Repository.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
using Dapper;
using System.Web.Script.Serialization;

namespace InVanWebApp.Repository
{
    public class DebitNoteRepository : IDebitNoteRepository
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(DebitNoteRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of debit note.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DebitNoteBO> GetAll()
        {
            List<DebitNoteBO> resultList = new List<DebitNoteBO>();
            try
            {
                string queryString = "Select * from DebitNote where IsDeleted=0";
                using (SqlConnection con = new SqlConnection(connString))
                {
                    resultList = con.Query<DebitNoteBO>(queryString).ToList();
                }
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
        public ResponseMessageBO Insert(DebitNoteBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_DebitNote_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DebitNoteNo", model.DebitNoteNo);
                    cmd.Parameters.AddWithValue("@DebitNoteDate", model.DebitNoteDate);
                    cmd.Parameters.AddWithValue("@PO_ID", model.PO_ID);
                    cmd.Parameters.AddWithValue("@PO_Number", model.PO_Number);
                    cmd.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
                    cmd.Parameters.AddWithValue("@CurrencyName", model.CurrencyName);
                    cmd.Parameters.AddWithValue("@CurrencyPrice", model.CurrencyPrice);
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", model.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@VendorID", model.VendorID);
                    cmd.Parameters.AddWithValue("@VendorName", model.VendorName);
                    cmd.Parameters.AddWithValue("@VendorAddress", model.VendorAddress);
                    cmd.Parameters.AddWithValue("@TotalBeforeTax", model.TotalBeforeTax);
                    cmd.Parameters.AddWithValue("@TotalTax", model.TotalTax);
                    cmd.Parameters.AddWithValue("@OtherTax", model.OtherTax);
                    cmd.Parameters.AddWithValue("@GrandTotal", model.GrandTotal);
                    cmd.Parameters.AddWithValue("@TermsAndConditionID", model.TermsAndConditionID);
                    cmd.Parameters.AddWithValue("@Terms", model.Terms);

                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    model.CreatedDate = Convert.ToDateTime(System.DateTime.Now);
                    con.Open();

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int DebitNoteID = 0;

                    while (dataReader.Read())
                    {
                        DebitNoteID = Convert.ToInt32(dataReader["ID"]);
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.txtItemDetails);

                    List<DebitNoteDetailsBO> itemDetails = new List<DebitNoteDetailsBO>();

                    foreach (var item in data)
                    {
                        DebitNoteDetailsBO objItemDetails = new DebitNoteDetailsBO();
                        objItemDetails.DebitNoteId = DebitNoteID;
                        objItemDetails.Item_Code = item.ElementAt(0).Value.ToString();
                        objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(1).Value);
                        objItemDetails.Item_Name = item.ElementAt(2).Value.ToString();
                        objItemDetails.POQuantity = float.Parse(item.ElementAt(3).Value.ToString());
                        objItemDetails.DebitedQuantity = Convert.ToDouble(item.ElementAt(4).Value);
                        objItemDetails.ItemUnit = item.ElementAt(5).Value.ToString();
                        objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(6).Value);
                        objItemDetails.CurrencyName = item.ElementAt(7).Value.ToString();
                        objItemDetails.ItemTaxValue = item.ElementAt(8).Value.ToString();
                        objItemDetails.ItemTotalAmount = float.Parse(item.ElementAt(9).Value.ToString());
                        objItemDetails.Remarks = item.ElementAt(10).Value.ToString();
                        objItemDetails.CreatedBy = model.CreatedBy;

                        //Added the below field for Currency
                        objItemDetails.CurrencyID = model.CurrencyID;
                        objItemDetails.CurrencyPrice = model.CurrencyPrice;

                        itemDetails.Add(objItemDetails);
                    }

                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_CreditNoteDetails_Insert", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@DebitNoteId", item.DebitNoteId);
                        cmdNew.Parameters.AddWithValue("@ItemId", item.ItemId);
                        cmdNew.Parameters.AddWithValue("@Item_Name", item.Item_Name);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                        cmdNew.Parameters.AddWithValue("@POQuantity", item.POQuantity);
                        cmdNew.Parameters.AddWithValue("@DebitedQuantity", item.DebitedQuantity);
                        cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                        cmdNew.Parameters.AddWithValue("@ItemTotalAmount", item.ItemTotalAmount);
                        cmdNew.Parameters.AddWithValue("@Remarks", item.Remarks);
                        cmdNew.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                        cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));

                        //Added the below field for Indent, currency and terms description
                        cmdNew.Parameters.AddWithValue("@CurrencyID", item.CurrencyID);
                        cmdNew.Parameters.AddWithValue("@CurrencyName", item.CurrencyName);
                        cmdNew.Parameters.AddWithValue("@CurrencyPrice", item.CurrencyPrice);


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
        public void Delete(int Id, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_DebitNote_Delete", con);
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

        public DebitNoteBO GetById(int ID)
        {
            DebitNoteBO result = new DebitNoteBO();
            try
            {
                string stringQuery = "Select * from DebitNote where IsDeleted=0 and ID=@ID";
                string stringItemQuery = "Select * from DebitNoteDetails where IsDeleted=0 and DebitNoteId=@ID";
                using (SqlConnection con = new SqlConnection(connString))
                {
                    result = con.Query<DebitNoteBO>(stringQuery, new { @ID = ID }).FirstOrDefault();
                    var ItemList = con.Query<DebitNoteDetailsBO>(stringItemQuery, new { @ID = ID }).ToList();
                    result.debitNoteDetails = ItemList;
                }
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
                    SqlDataReader dataReader2 = cmd1.ExecuteReader();

                    while (dataReader2.Read())
                    {
                        var result = new CreditNoteDetailsBO()
                        {
                            ItemId = Convert.ToInt32(dataReader2["ItemId"]),
                            Item_Code = dataReader2["Item_Code"].ToString(),
                            Item_Name = dataReader2["Item_Name"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(dataReader2["ItemUnitPrice"]),
                            ItemUnit = dataReader2["ItemUnit"].ToString(),
                            ItemTaxValue = dataReader2["ItemTaxValue"].ToString(),
                            POQuantity = Convert.ToDouble(dataReader2["POQuantity"]),
                            RejectedQuantity = ((dataReader2["RejectedQuantity"] != null) ? Convert.ToDecimal(dataReader2["RejectedQuantity"]) : 0),
                            CurrencyName = dataReader2["CurrencyName"].ToString(),
                            CurrencyID = Convert.ToInt32(dataReader2["CurrencyID"]),
                            Remarks = dataReader2["Remarks"].ToString(),
                            ItemTotalAmount = Convert.ToDouble(dataReader2["ItemTotalAmount"])
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