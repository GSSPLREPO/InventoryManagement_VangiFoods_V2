using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using InVanWebApp.Common;
using InVanWebApp.DAL;
using System.Globalization;
using System.Data.Entity.Validation;
using Newtonsoft.Json;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using Dapper;

namespace InVanWebApp.Repository
{
    public class SOPaymentRepository : ISOPaymentRepository
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(PurchaseOrderRepository));

        #region Bind Grid
        /// <summary>
        /// Rahul : This function is for fecthing list of SONumbers from Sales Order.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetSONumbers()
        {
            Dictionary<int, string> SONumbers = new Dictionary<int, string>();
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
                        int SalesOrderId = Convert.ToInt32(reader["SalesOrderId"]);
                        string SONo = reader["SONo"].ToString();
                        SONumbers.Add(SalesOrderId, SONo);  
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return SONumbers; 
        }

        /// <summary>
        /// Rahul: This function is for fecthing list of Sales Order Payment data.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SOPaymentBO> GetAll()
        {
            List<SOPaymentBO> resultList = new List<SOPaymentBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SalesOrderPaymentDetails_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new SOPaymentBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            SalesOrderId = Convert.ToInt32(reader["SalesOrderId"]),
                            SONumber = reader["SONumber"].ToString(),
                            VendorName = reader["CompanyName"].ToString(),
                            TotalPOAmount = Convert.ToDecimal(reader["GrandTotal"]),
                            InvoiceNumber = reader["InvoiceNo"].ToString(),
                            PaymentAmount = Convert.ToDecimal(reader["InvoiceAmount"]),
                            TotalPaybleAmount = Convert.ToInt32(reader["AmountPaid"]),
                            BalanceAmount = Convert.ToDecimal(reader["BalancePay"]),
                            PaymentDate = Convert.ToDateTime(reader["PaymentDate"]),
                            PaymentMode = reader["PaymentMode"].ToString(),
                            IsPaid = reader["PaymentStatus"].ToString(),
                            PaymentDueDate= Convert.ToDateTime(reader["PaymentDueDate"])
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

        #region Insert Data
        /// <summary>
        /// Rahul : This function is for inserting the Sales Order payment records.
        /// </summary>
        /// <param name="salesOrderMaster"></param>
        /// <returns></returns>
        public ResponseMessageBO Insert(SOPaymentBO SOPaymentDetails)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SalesOrderPaymentDetails_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SalesOrderId", SOPaymentDetails.SalesOrderId);
                    cmd.Parameters.AddWithValue("@InvoiceNo", SOPaymentDetails.InvoiceNumber);
                    cmd.Parameters.AddWithValue("@InvoiceAmount", SOPaymentDetails.PaymentAmount);
                    cmd.Parameters.AddWithValue("@AmountPaid", SOPaymentDetails.TotalPaybleAmount);
                    cmd.Parameters.AddWithValue("@BalancePay", SOPaymentDetails.BalanceAmount);
                    cmd.Parameters.AddWithValue("@AdvancedPayment", SOPaymentDetails.AdvancedPayment);
                    cmd.Parameters.AddWithValue("@PaymentDate", SOPaymentDetails.PaymentDate);
                    cmd.Parameters.AddWithValue("@PaymentDueDate", SOPaymentDetails.PaymentDueDate);
                    cmd.Parameters.AddWithValue("@PaymentMode", SOPaymentDetails.PaymentMode);
                    cmd.Parameters.AddWithValue("@ChequeNo", SOPaymentDetails.ChequeNumber);
                    cmd.Parameters.AddWithValue("@BankName", SOPaymentDetails.BankName);
                    cmd.Parameters.AddWithValue("@AccountNo", SOPaymentDetails.AccountNumber);
                    cmd.Parameters.AddWithValue("@Remarks", SOPaymentDetails.Remarks);
                    cmd.Parameters.AddWithValue("@PaymentStatus", SOPaymentDetails.IsPaid);
                    cmd.Parameters.AddWithValue("@CreatedBy", SOPaymentDetails.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@BranchName", SOPaymentDetails.BranchName);
                    cmd.Parameters.AddWithValue("@IFSCCode", SOPaymentDetails.IFSCCode);
                    cmd.Parameters.AddWithValue("@UTRNo", SOPaymentDetails.UTRNo);

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

        #region Get Sales Order
        /// <summary>
        /// Raj: This function is used to get the single purchase order using PurchaseOrderId using dapper
        /// </summary>
        /// <param name="salesOrderId"></param>
        /// <returns></returns>
        public SalesOrderBO GetSalesOrderById(int salesOrderId) 
        {
            SalesOrderBO result = new SalesOrderBO(); 
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SalesOrderPaymentDetails_GetBySOId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SO_ID", salesOrderId);

                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        result.SalesOrderId = Convert.ToInt32(dataReader["SalesOrderId"]);
                        result.SONo = dataReader["SONo"].ToString();
                        result.GrandTotal = Convert.ToDecimal(dataReader["GrandTotal"]);
                        result.AdvancedPayment = Convert.ToDecimal(dataReader["AdvancedPayment"].ToString());
                        result.VendorsID = Convert.ToInt32(dataReader["VendorsID"]);
                        result.AmountPaid = float.Parse(dataReader["AmountPaid"].ToString());
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
        /// Rahul: This function is used to get the list of sales order Items againts SalesOrderId using dapper. 
        /// </summary>
        /// <param name="salesOrderId"></param>
        /// <returns></returns>
        public List<SalesOrderItemsDetailBO> GetSOItemsBySalesOrderId(int salesOrderId)  
        {
            string salesOrderQuery = "select * From SalesOrderItemsDetails where SalesOrderId = @salesOrderId AND IsDeleted = 0";
            using (SqlConnection con = new SqlConnection(connString))
            {
                var salesOrder = con.Query<SalesOrderItemsDetailBO>(salesOrderQuery, new { @salesOrderId = salesOrderId }).ToList();
                return salesOrder;
            }
        }
        #endregion

        #region Get Payment Details
        /// <summary>
        /// Raj: This method returns payment Details on behalf of PaymentId. 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SalesOrderPaymentDetail GetSOPaymentDetailsById(int Id) 
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                String query = "SELECT * FROM SalesOrderPaymentDetails WHERE ID = @Id AND IsDeleted = 0";                
                var SOPaymentDetail = con.Query<SalesOrderPaymentDetail>(query, new { @Id = Id }).FirstOrDefault();
                return SOPaymentDetail;
            }
        }
        #endregion

        #region Update Payment Details
        /// <summary>
        /// Created By: Farheen 
        /// Date: 20 Dec'22
        /// Description: Fetch the details of Payment for edit by PO payment id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SOPaymentBO GetByID(int Id)
        {
            var result = new SOPaymentBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SOPayment_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);

                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        result.ID = Convert.ToInt32(dataReader["ID"]);
                        result.SalesOrderId = Convert.ToInt32(dataReader["SalesOrderId"]);
                        result.SONumber = dataReader["SONumber"].ToString();
                        result.VendorName = dataReader["VendorName"].ToString();
                        result.PaymentDate = Convert.ToDateTime(dataReader["PaymentDate"]);
                        result.InvoiceNumber = dataReader["InvoiceNo"].ToString();
                        result.PaymentAmount = Convert.ToDecimal(dataReader["InvoiceAmount"]);
                        result.PaymentDueDate = Convert.ToDateTime(dataReader["PaymentDueDate"]);
                        result.PaymentMode = dataReader["PaymentMode"].ToString();
                        result.BankName = dataReader["BankName"].ToString();
                        result.BranchName = dataReader["BranchName"].ToString();
                        result.AccountNumber = dataReader["AccountNo"].ToString();
                        result.IFSCCode = dataReader["IFSCCode"].ToString();
                        result.ChequeNumber = dataReader["ChequeNo"].ToString();
                        result.TotalPOAmount = Convert.ToDecimal(dataReader["SO_Amount"]);
                        result.AdvancedPayment = Convert.ToDecimal(dataReader["AdvancePayment"]);
                        result.AmountPaid = float.Parse(dataReader["AmountPaid"].ToString());
                        result.BalanceAmount = Convert.ToDecimal(dataReader["BalancePay"]);
                        result.IsPaid = dataReader["PaymentStatus"].ToString();
                        result.Remarks = dataReader["Remarks"].ToString();
                        result.UTRNo = dataReader["UTRNo"].ToString();
                        result.CurrencyName = dataReader["CurrencyName"].ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                log.Error(ex.Message, ex);
            }
            return result;
        }
        public ResponseMessageBO Update(SOPaymentBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SalesOrderPaymentDetails_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@SalesOrderId", model.SalesOrderId);
                    cmd.Parameters.AddWithValue("@InvoiceNo", model.InvoiceNumber);
                    cmd.Parameters.AddWithValue("@InvoiceAmount", model.PaymentAmount);
                    cmd.Parameters.AddWithValue("@PaymentDueDate", model.PaymentDueDate);
                    cmd.Parameters.AddWithValue("@PaymentMode", model.PaymentMode);
                    cmd.Parameters.AddWithValue("@ChequeNo", model.ChequeNumber);
                    cmd.Parameters.AddWithValue("@BankName", model.BankName);
                    cmd.Parameters.AddWithValue("@BranchName", model.BranchName);
                    cmd.Parameters.AddWithValue("@AccountNo", model.AccountNumber);
                    cmd.Parameters.AddWithValue("@IFSCCode", model.IFSCCode);
                    cmd.Parameters.AddWithValue("@PaymentStatus", model.IsPaid);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedBy);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@UTRNo", model.UTRNo);
                    //cmd.Parameters.AddWithValue("@BalancePay", model.BalanceAmount);
                    //cmd.Parameters.AddWithValue("@PaymentDate", model.PaymentDate);
                    //cmd.Parameters.AddWithValue("@AdvancedPayment", model.AdvancedPayment);
                    //cmd.Parameters.AddWithValue("@AmountPaid", model.TotalPaybleAmount);

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

        #region Delete Sales Order Payment Details
        public void Delete(int ID, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SalesOrderPaymentDetails_Delete", con);
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