using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using InVanWebApp.Repository.Interface;
using log4net;
using InVanWebApp_BO;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;
using Dapper;

namespace InVanWebApp.Repository
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        //private readonly InVanDBContext _context;
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(SalesOrderRepository));

        #region  Bind grid
        /// <summary>
        /// Rahul : This function is for fecthing list of order master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SalesOrderBO> GetAll()
        {
            List<SalesOrderBO> resultList = new List<SalesOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SalesOrder_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new SalesOrderBO()
                        {
                            SalesOrderId = Convert.ToInt32(reader["SalesOrderId"]),
                            Amendment = Convert.ToInt32(reader["Amendment"]),
                            CompanyName = reader["CompanyName"].ToString(),
                            SODate = Convert.ToDateTime(reader["SODate"]),
                            SONo = reader["SONo"].ToString(),
                            SalesOrderStatus = reader["SalesOrderStatus"].ToString(),
                            DeliveryAddress = reader["DeliveryAddress"].ToString(),
                            LastModifiedById = Convert.ToInt32(reader["LastModifiedBy"]),
                            LastModifiedDate = Convert.ToDateTime(reader["LastModifiedDate"]),
                            DraftFlag = Convert.ToBoolean(reader["DraftFlag"]),
                            OutwardCount = Convert.ToInt32(reader["OutwardCount"])
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
        /// Rahul: Insert record.
        /// </summary>
        /// <param name="model"></param>                
        public ResponseMessageBO Insert(SalesOrderBO model)
        {

            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {

                    SqlCommand cmd = new SqlCommand("usp_tbl_SalesOrder_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@WorkOrderType", model.WorkOrderType);
                    cmd.Parameters.AddWithValue("@WorkOrderNo", model.WorkOrderNo);
                    cmd.Parameters.AddWithValue("@SONo", model.SONo);
                    cmd.Parameters.AddWithValue("@SODate", model.SODate);
                    cmd.Parameters.AddWithValue("@DeliveryDate", model.DeliveryDate);
                    cmd.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
                    cmd.Parameters.AddWithValue("@CurrencyName", model.CurrencyName);
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    cmd.Parameters.AddWithValue("@ClientID", model.ClientID);
                    cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
                    cmd.Parameters.AddWithValue("@SupplierAddress", model.SupplierAddress);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", model.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@Amendment", model.Amendment);
                    cmd.Parameters.AddWithValue("@InquiryID", model.InquiryID);
                    cmd.Parameters.AddWithValue("@InquiryNumber", model.InquiryNumber);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@DraftFlag", model.DraftFlag);
                    cmd.Parameters.AddWithValue("@TermsAndConditionID", model.TermsAndConditionID);
                    cmd.Parameters.AddWithValue("@TermDescription", model.Terms);
                    cmd.Parameters.AddWithValue("@CGST", model.CGST);
                    cmd.Parameters.AddWithValue("@SGST", model.SGST);
                    cmd.Parameters.AddWithValue("@IGST", model.IGST);
                    cmd.Parameters.AddWithValue("@SalesOrderStatus", "Open");
                    cmd.Parameters.AddWithValue("@Cancelled", model.Cancelled);
                    cmd.Parameters.AddWithValue("@ReasonForCancellation", model.ReasonForCancellation);
                    cmd.Parameters.AddWithValue("@AdvancedPayment", model.AdvancedPayment);
                    cmd.Parameters.AddWithValue("@GrandTotal", model.GrandTotal);
                    cmd.Parameters.AddWithValue("@TotalAfterTax", model.TotalAfterTax);
                    cmd.Parameters.AddWithValue("@OtherTax", model.OtherTax);
                    cmd.Parameters.AddWithValue("@Signature", model.Signature);
                    cmd.Parameters.AddWithValue("@ApprovedBy", model.CreatedById);
                    cmd.Parameters.AddWithValue("@ApprovedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@CheckedBy", model.CreatedById);
                    cmd.Parameters.AddWithValue("@CheckedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedById);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@LastModifiedBy", model.CreatedById);

                    //con.Open();

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int SalesOrderId = 0;
                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                        SalesOrderId = Convert.ToInt32(dataReader["SalesOrderId"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    if (SalesOrderId != 0)
                    {
                        var data = json.Deserialize<Dictionary<string, string>[]>(model.txtItemDetails);

                        List<SalesOrderItemsDetail> itemDetails = new List<SalesOrderItemsDetail>();
                        foreach (var item in data)
                        {
                            SalesOrderItemsDetail objItemDetails = new SalesOrderItemsDetail();
                            objItemDetails.SalesOrderId = SalesOrderId;
                            objItemDetails.Item_Code = item.ElementAt(0).Value.ToString();
                            objItemDetails.Item_ID = Convert.ToInt32(item.ElementAt(1).Value);
                            objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                            objItemDetails.ItemQuantity = Convert.ToDecimal(item.ElementAt(3).Value);
                            objItemDetails.ItemUnit = item.ElementAt(4).Value.ToString();
                            objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(5).Value);
                            objItemDetails.ItemTaxValue = Convert.ToDecimal(item.ElementAt(6).Value);
                            objItemDetails.TotalItemCost = Convert.ToDecimal(item.ElementAt(7).Value);
                            objItemDetails.CreatedBy = model.CreatedById;
                            //Added the below field for Currency
                            objItemDetails.CurrencyID = model.CurrencyID;
                            objItemDetails.CurrencyName = model.CurrencyName;

                            itemDetails.Add(objItemDetails);
                        }

                        foreach (var item in itemDetails)
                        {
                            con.Open();
                            SqlCommand cmdNew = new SqlCommand("usp_tbl_SalesOrderItemsDetails_Insert", con);
                            cmdNew.CommandType = CommandType.StoredProcedure;

                            cmdNew.Parameters.AddWithValue("@SalesOrderId", item.SalesOrderId);
                            cmdNew.Parameters.AddWithValue("@Item_ID", item.Item_ID);
                            cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                            cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                            cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                            cmdNew.Parameters.AddWithValue("@ItemQuantity", item.ItemQuantity);
                            cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                            cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                            cmdNew.Parameters.AddWithValue("@TotalItemCost", item.TotalItemCost);
                            cmdNew.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                            cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                            //Added the below field for Indent, currency and terms description
                            cmdNew.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
                            cmdNew.Parameters.AddWithValue("@CurrencyName", model.CurrencyName);
                            cmdNew.Parameters.AddWithValue("@InquiryID", model.InquiryID);

                            SqlDataReader dataReaderNew = cmdNew.ExecuteReader();

                            while (dataReaderNew.Read())
                            {
                                response.Status = Convert.ToBoolean(dataReaderNew["Status"]);
                            }
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return response;
        }

        #endregion

        #region Update functions
        /// <summary>
        /// Rahul: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SalesOrderBO GetSalesOrderById(int Id)
        {
            string purchaseOrderQuery = "SELECT * FROM SalesOrder WHERE SalesOrderId = @Id AND IsDeleted = 0";
            string purchaseOrderItemQuery = "SELECT * FROM SalesOrderItemsDetails WHERE SalesOrderId = @Id AND IsDeleted = 0";
            SalesOrderBO result = new SalesOrderBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    result = con.Query<SalesOrderBO>(purchaseOrderQuery, new { @Id = Id }).FirstOrDefault();
                    var purchaseOrderList = con.Query<SalesOrderItemsDetail>(purchaseOrderItemQuery, new { @Id = Id }).ToList();
                    result.salesOrderItemsDetails = purchaseOrderList;
                }

            }
            catch (Exception ex)
            {
                result = null;
                log.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(SalesOrderBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SalesOrder_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SalesOrderId", model.SalesOrderId);
                    cmd.Parameters.AddWithValue("@WorkOrderType", model.WorkOrderType);
                    cmd.Parameters.AddWithValue("@WorkOrderNo", model.WorkOrderNo);
                    cmd.Parameters.AddWithValue("@SONo", model.SONo);
                    cmd.Parameters.AddWithValue("@SODate", model.SODate);
                    cmd.Parameters.AddWithValue("@DeliveryDate", model.DeliveryDate);
                    cmd.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
                    cmd.Parameters.AddWithValue("@CurrencyName", model.CurrencyName);
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    cmd.Parameters.AddWithValue("@ClientID", model.ClientID);
                    cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
                    cmd.Parameters.AddWithValue("@SupplierAddress", model.SupplierAddress);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", model.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@Amendment", model.Amendment);
                    cmd.Parameters.AddWithValue("@InquiryID", model.InquiryID);
                    cmd.Parameters.AddWithValue("@InquiryNumber", model.InquiryNumber);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@DraftFlag", model.DraftFlag);
                    cmd.Parameters.AddWithValue("@TermsAndConditionID", model.TermsAndConditionID);
                    cmd.Parameters.AddWithValue("@TermDescription", model.Terms);
                    cmd.Parameters.AddWithValue("@AdvancedPayment", model.AdvancedPayment);
                    cmd.Parameters.AddWithValue("@GrandTotal", model.GrandTotal);
                    cmd.Parameters.AddWithValue("@TotalAfterTax", model.TotalAfterTax);
                    cmd.Parameters.AddWithValue("@OtherTax", model.OtherTax);
                    cmd.Parameters.AddWithValue("@Signature", model.Signature);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedById);                 


                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.txtItemDetails);

                    List<SalesOrderItemsDetail> itemDetails = new List<SalesOrderItemsDetail>();

                    foreach (var item in data)
                    {
                        SalesOrderItemsDetail objItemDetails = new SalesOrderItemsDetail();
                        objItemDetails.SalesOrderId = model.SalesOrderId;
                        objItemDetails.Item_Code = item.ElementAt(0).Value.ToString();
                        objItemDetails.Item_ID = Convert.ToInt32(item.ElementAt(1).Value);
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.ItemQuantity = Convert.ToDecimal(item.ElementAt(3).Value);
                        objItemDetails.ItemUnit = item.ElementAt(6).Value.ToString();
                        objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(5).Value);
                        objItemDetails.ItemTaxValue = Convert.ToDecimal(item.ElementAt(6).Value);
                        objItemDetails.TotalItemCost = Convert.ToDecimal(item.ElementAt(7).Value);
                        objItemDetails.LastModifiedBy = model.LastModifiedById;
                        //Added the below field for Currency
                        objItemDetails.CurrencyID = model.CurrencyID;
                        objItemDetails.CurrencyName = model.CurrencyName;                        

                        itemDetails.Add(objItemDetails);
                    }

                    var count = itemDetails.Count;
                    var i = 1;
                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_SalesOrderItemsDetails_Update", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@SalesOrderId", item.SalesOrderId);
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.Item_ID);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                        cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                        cmdNew.Parameters.AddWithValue("@ItemQuantity", item.ItemQuantity);
                        cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        cmdNew.Parameters.AddWithValue("@TotalItemCost", item.TotalItemCost);
                        cmdNew.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                        cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                        //Added the below field for Indent, currency and terms description
                        cmdNew.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
                        cmdNew.Parameters.AddWithValue("@CurrencyName", model.CurrencyName);
                        cmdNew.Parameters.AddWithValue("@InquiryID", model.InquiryID);

                        if (count == 1)
                            cmdNew.Parameters.AddWithValue("@OneItemIdentifier", 1);
                        else
                        {
                            cmdNew.Parameters.AddWithValue("@OneItemIdentifier", 0);
                            cmdNew.Parameters.AddWithValue("@flagCheck", i);
                        }
                        i++;
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
                    SqlCommand cmd = new SqlCommand("usp_tbl_SalesOrder_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SalesOrderId", ID);
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

        #region Bind dropdown function
        public IEnumerable<InquiryFormBO> GetInquiryList()
        {
            List<InquiryFormBO> inquiryFormMastersList = new List<InquiryFormBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_GetInquiryListForSO", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var inquiryFormMasters = new InquiryFormBO()
                        {
                            InquiryID = Convert.ToInt32(reader["InquiryID"]),
                            InquiryNumber = reader["InquiryNumber"].ToString(),
                            InquiryStatus = reader["InquiryStatus"].ToString()

                        };
                        inquiryFormMastersList.Add(inquiryFormMasters);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return inquiryFormMastersList;
        }


        #endregion

        #region Fecth Inquiry item details
        public List<InquiryFormItemDetailsBO> GetInquiryFormById(int id, int CurrencyId = 0)
        {
            var resultList = new List<InquiryFormItemDetailsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InquiryItemDetailsForSO_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@CurrencyId", CurrencyId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = new InquiryFormItemDetailsBO()
                        {
                            InquiryID = Convert.ToInt32(reader["InquiryID"]),
                            ItemName = reader["ItemName"].ToString(),
                            Item_ID = Convert.ToInt32(reader["Item_ID"]),
                            ItemQuantity = Convert.ToDecimal(reader["ItemQuantity"]),
                            //SentQuantity = Convert.ToDouble(reader["SentQuantity"]),

                            //Added the below fields for binding the items in PO
                            Item_Code = reader["Item_Code"].ToString(),
                            ItemUnit = reader["ItemUnit"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(reader["ItemUnitPrice"]),
                            ItemTaxValue = Convert.ToDecimal(reader["ItemTax"]),
                            TotalItemCost = Convert.ToDecimal(reader["TotalItemCost"])
                            //BalanceQuantity = Convert.ToDouble(reader["BalanceQuantity"])

                        };
                        resultList.Add(result);
                    }
                    con.Close();
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

    }
}