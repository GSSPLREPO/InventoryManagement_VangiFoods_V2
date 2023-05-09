using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using InVanWebApp.Common;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;

namespace InVanWebApp.Repository
{
    public class CreditNoteRepository : ICreditNoteRepository
    {

        //private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(CreditNoteRepository));

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
                            PO_Number = reader["PO_Number"].ToString(),
                            VendorName = reader["VendorName"].ToString(),
                            Remarks = reader["Remarks"].ToString(),
                            UserName=reader["UserName"].ToString()
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
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_CreditNote_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CreditNoteNo", model.CreditNoteNo);
                    cmd.Parameters.AddWithValue("@CreditNoteDate", model.CreditNoteDate);
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
                    int CreditNoteId = 0;

                    while (dataReader.Read())
                    {
                        CreditNoteId = Convert.ToInt32(dataReader["ID"]);
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.TxtItemDetails);

                    List<CreditNoteDetailsBO> itemDetails = new List<CreditNoteDetailsBO>();

                    foreach (var item in data)
                    {
                        CreditNoteDetailsBO objItemDetails = new CreditNoteDetailsBO();
                        objItemDetails.CreditNote_ID = CreditNoteId;
                        objItemDetails.Item_Code = item.ElementAt(0).Value.ToString();
                        objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(1).Value);
                        objItemDetails.Item_Name = item.ElementAt(2).Value.ToString();
                        objItemDetails.POQuantity= float.Parse(item.ElementAt(3).Value.ToString());
                        objItemDetails.RejectedQuantity = Convert.ToDecimal(item.ElementAt(4).Value);
                        objItemDetails.ItemUnit = item.ElementAt(5).Value.ToString();
                        objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(6).Value);
                        objItemDetails.CurrencyName = item.ElementAt(7).Value.ToString();
                        objItemDetails.ItemTaxValue = item.ElementAt(8).Value.ToString();
                        objItemDetails.ItemTotalAmount = float.Parse(item.ElementAt(9).Value.ToString());
                        objItemDetails.Remarks= item.ElementAt(10).Value.ToString();
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

                        cmdNew.Parameters.AddWithValue("@CreditNote_ID", item.CreditNote_ID);
                        cmdNew.Parameters.AddWithValue("@ItemId", item.ItemId);
                        cmdNew.Parameters.AddWithValue("@Item_Name", item.Item_Name);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                        cmdNew.Parameters.AddWithValue("@POQuantity", item.POQuantity);
                        cmdNew.Parameters.AddWithValue("@RejectedQuantity", item.RejectedQuantity);
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

                    ////dt.Columns.Add("ID");
                    ////dt.Columns.Add("PrimaryTableId");
                    ////dt.Columns.Add("ItemId");
                    ////dt.Columns.Add("Item_Code");
                    ////dt.Columns.Add("Item_Name");
                    ////dt.Columns.Add("HSN_Code");
                    ////dt.Columns.Add("ItemTaxValue");
                    ////dt.Columns.Add("ItemUnit");
                    ////dt.Columns.Add("ItemUnitPrice");
                    ////dt.Columns.Add("ItemQuantity");
                    ////dt.Columns.Add("POQuantity");
                    ////dt.Columns.Add("RejectedQuantity");
                    ////dt.Columns.Add("ItemTotalAmount");
                    ////dt.Columns.Add("CurrencyID");
                    ////dt.Columns.Add("CurrencyName");
                    ////dt.Columns.Add("CurrencyPrice");
                    ////dt.Columns.Add("Remarks");
                    ////dt.Columns.Add("IsDeleted");
                    ////dt.Columns.Add("CreatedBy");
                    ////dt.Columns.Add("CreatedDate");
                    ////dt.Columns.Add("LastModifiedBy");
                    ////dt.Columns.Add("LastModifiedDate");
                    ////int i = 1;
                    ////foreach (var item in data)
                    ////{
                    ////    DataRow dataRow = dt.NewRow();
                    ////    dataRow["ID"] = i;
                    ////    dataRow["PrimaryTableId"] = CreditNoteId;
                    ////    dataRow["Item_Code"] = item.ElementAt(0).Value.ToString();
                    ////    dataRow["ItemId"] = Convert.ToInt32(item.ElementAt(1).Value.ToString());
                    ////    dataRow["Item_Name"] = item.ElementAt(2).Value.ToString();
                    ////    dataRow["POQuantity"] = float.Parse(item.ElementAt(3).Value.ToString());
                    ////    dataRow["RejectedQuantity"] = float.Parse(item.ElementAt(4).Value.ToString());
                    ////    dataRow["ItemUnit"] = item.ElementAt(5).Value.ToString();
                    ////    dataRow["ItemUnitPrice"] = Convert.ToDecimal(item.ElementAt(6).Value.ToString());
                    ////    dataRow["CurrencyName"] = item.ElementAt(7).Value.ToString();
                    ////    dataRow["ItemTaxValue"] = item.ElementAt(8).Value.ToString();
                    ////    dataRow["ItemTotalAmount"] = float.Parse(item.ElementAt(9).Value.ToString());
                    ////    dataRow["Remarks"] = item.ElementAt(10).Value.ToString();
                    ////    dataRow["IsDeleted"] = false;
                    ////    dataRow["CreatedBy"] = model.CreatedBy;
                    ////    dataRow["LastModifiedBy"] = model.CreatedBy;
                    ////    dataRow["CreatedDate"] = model.CreatedDate;
                    ////    dataRow["LastModifiedDate"] = model.CreatedDate;
                    ////    dt.Rows.Add(dataRow);
                    ////    i++;
                    ////}

                    //SqlCommand cmd1 = new SqlCommand("usp_tbl_CreditNoteDetails_Insert", con);

                    //cmd1.CommandType = CommandType.StoredProcedure;
                    //cmd1.Parameters.AddWithValue("@ItemDetails", dt);

                    //con.Open();
                    //SqlDataReader dataReader1 = cmd1.ExecuteReader();

                    //while (dataReader1.Read())
                    //{
                    //    response.Status = Convert.ToBoolean(dataReader1["Status"]);
                    //}
                    //con.Close();
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
                    cmd.Parameters.AddWithValue("@CreditNoteId", Id);
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
                            CreditNoteNo = reader["CreditNoteNo"].ToString(),
                            CreditNoteDate = Convert.ToDateTime(reader["CreditNoteDate"]),
                            PO_Number = reader["PO_Number"].ToString(),
                            CurrencyName = reader["CurrencyName"].ToString(),
                            LocationName = reader["LocationName"].ToString(),
                            VendorName = reader["VendorName"].ToString(),
                            DeliveryAddress = reader["DeliveryAddress"].ToString(),
                            VendorAddress = reader["VendorAddress"].ToString(),
                            Terms = reader["Terms"].ToString(),
                            OtherTax = Convert.ToDecimal(reader["OtherTax"]),
                            TotalBeforeTax = Convert.ToDecimal(reader["TotalBeforeTax"]),
                            TotalTax = Convert.ToDecimal(reader["TotalTax"]),
                            GrandTotal = Convert.ToDecimal(reader["GrandTotal"]),
                            Remarks = reader["Remarks"].ToString(),
                            UserName = reader["UserName"].ToString()
                        };
                    }
                    con.Close();
                };

                var CreditNoteDetails = GetCreditNoteDetails(ID);
                result.creditNoteDetails = CreditNoteDetails.ToList();
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

        #region Function for dropdown binding
        public List<PurchaseOrderBO> GetPONumberForDropdown()
        {
            List<PurchaseOrderBO> resultList = new List<PurchaseOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PONumberForCreditNote_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        PurchaseOrderBO result = new PurchaseOrderBO()
                        {
                            PurchaseOrderId = Convert.ToInt32(reader["PurchaseOrderId"]),
                            PONumber = reader["PONumber"].ToString()
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

        #region Bind all PO details 
        public IEnumerable<PurchaseOrderBO> GetPODetailsById(int PO_Id)
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
                            PONumber = dataReader["PONumber"].ToString(),
                            CurrencyID = Convert.ToInt32(dataReader["CurrencyID"]),
                            CurrencyName = dataReader["CurrencyName"].ToString(),
                            CurrencyPrice = Convert.ToDouble(dataReader["CurrencyPrice"]),
                            LocationId = Convert.ToInt32(dataReader["LocationId"]),
                            LocationName = dataReader["LocationName"].ToString(),
                            VendorsID = Convert.ToInt32(dataReader["VendorsID"]),
                            CompanyName = dataReader["CompanyName"].ToString(),
                            DeliveryAddress = dataReader["DeliveryAddress"].ToString(),
                            SupplierAddress = dataReader["SupplierAddress"].ToString(),
                            TermsAndConditionID = Convert.ToInt32(dataReader["TermsAndConditionID"]),
                            Terms = dataReader["Terms"].ToString(),
                            OtherTax = Convert.ToDecimal(dataReader["OtherTax"])
                        };
                        resultList.Add(result);
                    }
                    con.Close();

                    //==========This is for fetching Item details".===========///

                    SqlCommand cmd2 = new SqlCommand("usp_tbl_POItemDetailsForCreditNote_GetByID", con);
                    cmd2.Parameters.AddWithValue("@PO_Id", PO_Id);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader2 = cmd2.ExecuteReader();

                    while (dataReader2.Read())
                    {
                        var result = new PurchaseOrderBO()
                        {
                            Item_ID = Convert.ToInt32(dataReader2["Item_ID"]),
                            Item_Code = dataReader2["Item_Code"].ToString(),
                            ItemName = dataReader2["ItemName"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(dataReader2["ItemUnitPrice"]),
                            ItemUnit = dataReader2["ItemUnit"].ToString(),
                            ItemTaxValue = Convert.ToDecimal(dataReader2["ItemTaxValue"]),
                            ItemQuantity = Convert.ToDecimal(dataReader2["POQty"]),
                            RejectedQuantity = ((dataReader2["RejectedQuantity"] != null) ? Convert.ToDecimal(dataReader2["RejectedQuantity"]) : 0),
                            CurrencyName = dataReader2["CurrencyName"].ToString(),
                            CurrencyID = Convert.ToInt32(dataReader2["CurrencyID"]),
                            Remarks = dataReader2["Remarks"].ToString(),
                            TotalItemCost = Convert.ToDecimal(dataReader2["TotalItemCost"])
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

        #region Bind all SO details 
        public IEnumerable<SOPaymentBO> GetSODetailsById(int SO_Id)
        {
            List<SOPaymentBO> resultList = new List<SOPaymentBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SODetails_GetByIDFor_CeditNote", con);
                    cmd.Parameters.AddWithValue("@ID", SO_Id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new SOPaymentBO()
                        {
                            SalesOrderId = Convert.ToInt32(dataReader["SalesOrderId"]),
                            SONumber = dataReader["SONumber"].ToString(),
                            CurrencyID = Convert.ToInt32(dataReader["CurrencyID"]),
                            CurrencyName = dataReader["CurrencyName"].ToString(),
                            CurrencyPrice = Convert.ToDouble(dataReader["CurrencyPrice"]),
                            LocationId = Convert.ToInt32(dataReader["LocationId"]),
                            LocationName = dataReader["LocationName"].ToString(),
                            VendorsID = Convert.ToInt32(dataReader["VendorsID"]),
                            CompanyName = dataReader["CompanyName"].ToString(),
                            DeliveryAddress = dataReader["DeliveryAddress"].ToString(),
                            SupplierAddress = dataReader["SupplierAddress"].ToString(),
                            TermsAndConditionID = Convert.ToInt32(dataReader["TermsAndConditionID"]),
                            Terms = dataReader["Terms"].ToString(),
                            OtherTax = Convert.ToDecimal(dataReader["OtherTax"])
                        };
                        resultList.Add(result);
                    }
                    con.Close();

                    //==========This is for fetching Item details".===========///

                    SqlCommand cmd2 = new SqlCommand("usp_tbl_SOItemDetailsForCreditNote_GetByID", con);
                    cmd2.Parameters.AddWithValue("@SO_Id", SO_Id);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader2 = cmd2.ExecuteReader();

                    while (dataReader2.Read())
                    {
                        var result = new SOPaymentBO()
                        {
                            Item_ID = Convert.ToInt32(dataReader2["Item_ID"]),
                            Item_Code = dataReader2["Item_Code"].ToString(),
                            ItemName = dataReader2["ItemName"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(dataReader2["ItemUnitPrice"]),
                            ItemUnit = dataReader2["ItemUnit"].ToString(),
                            ItemTaxValue = Convert.ToDecimal(dataReader2["ItemTaxValue"]),
                            ItemQuantity = Convert.ToDecimal(dataReader2["SOQty"]),
                            RejectedQuantity = ((dataReader2["RejectedQuantity"] != null) ? Convert.ToDecimal(dataReader2["RejectedQuantity"]) : 0),
                            CurrencyName = dataReader2["CurrencyName"].ToString(),
                            CurrencyID = Convert.ToInt32(dataReader2["CurrencyID"]),
                            Remarks = dataReader2["Remarks"].ToString(),
                            TotalItemCost = Convert.ToDecimal(dataReader2["TotalItemCost"])
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

        #region Function for dropdown binding
        public List<SOPaymentBO> GetSONumberForDropdown()
        {
            List<SOPaymentBO> resultList = new List<SOPaymentBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SONumber_Get", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SOPaymentBO result = new SOPaymentBO()
                        {
                            SalesOrderId = Convert.ToInt32(reader["ID"]),
                            SONumber = reader["SONumber"].ToString()
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