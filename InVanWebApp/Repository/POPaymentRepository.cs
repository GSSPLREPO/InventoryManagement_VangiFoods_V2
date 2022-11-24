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
    public class POPaymentRepository : IPOPaymentRepository
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(PurchaseOrderRepository));

        #region Bind Grid
        /// <summary>
        /// Raj : This function is for fecthing list of PONumbers from Purchase Order.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetPONumbers()
        {
            Dictionary<int, string> PONumbers = new Dictionary<int, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PurchaseOrder_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        int PurchaseOrderId = Convert.ToInt32(reader["PurchaseOrderId"]);
                        string PONumber = reader["PONumber"].ToString();
                        PONumbers.Add(PurchaseOrderId, PONumber);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return PONumbers;
        }

        /// <summary>
        /// Raj: This function is for fecthing list of Purchase Order Payment data.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<POPaymentBO> GetAll()
        {
            List<POPaymentBO> resultList = new List<POPaymentBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PurchaseOrderPaymentDetails_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new POPaymentBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            PurchaseOrderId = Convert.ToInt32(reader["PurchaseOrderId"]),
                            PONumber = reader["PONumber"].ToString(),
                            VendorName = reader["CompanyName"].ToString(),
                            TotalPOAmount = Convert.ToDecimal(reader["GrandTotal"]),
                            InvoiceNumber = reader["InvoiceNo"].ToString(),
                            PaymentAmount = Convert.ToDecimal(reader["InvoiceAmount"]),
                            TotalPaybleAmount = Convert.ToInt32(reader["AmountPaid"]),
                            BalanceAmount = Convert.ToDecimal(reader["BalancePay"]),
                            PaymentDate = Convert.ToDateTime(reader["PaymentDate"]),
                            PaymentMode = reader["PaymentMode"].ToString(),
                            IsPaid = reader["PaymentStatus"].ToString()
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
        /// Raj : This function is for inserting the Purchase Order payment records.
        /// </summary>
        /// <param name="purchaseOrderMaster"></param>
        /// <returns></returns>
        public ResponseMessageBO Insert(POPaymentBO POPaymentDetails)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PurchaseOrderPaymentDetails_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PurchaseOrderId", POPaymentDetails.PurchaseOrderId);
                    cmd.Parameters.AddWithValue("@InvoiceNo", POPaymentDetails.InvoiceNumber);
                    cmd.Parameters.AddWithValue("@InvoiceAmount", POPaymentDetails.PaymentAmount);
                    cmd.Parameters.AddWithValue("@AmountPaid", POPaymentDetails.TotalPaybleAmount);
                    cmd.Parameters.AddWithValue("@BalancePay", POPaymentDetails.BalanceAmount);
                    cmd.Parameters.AddWithValue("@AdvancedPayment", 0);
                    cmd.Parameters.AddWithValue("@PaymentDate", POPaymentDetails.PaymentDate);
                    cmd.Parameters.AddWithValue("@PaymentMode", POPaymentDetails.PaymentMode);
                    cmd.Parameters.AddWithValue("@ChequeNo", POPaymentDetails.ChequeNumber);
                    cmd.Parameters.AddWithValue("@BankName", POPaymentDetails.BankName);
                    cmd.Parameters.AddWithValue("@AccountNo", POPaymentDetails.AccountNumber);
                    cmd.Parameters.AddWithValue("@PaymentDetails", POPaymentDetails.Remarks);
                    cmd.Parameters.AddWithValue("@PaymentStatus", POPaymentDetails.IsPaid);
                    cmd.Parameters.AddWithValue("@CreatedBy", 1);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", DateTime.Now);

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

        #region Get Purchase Order
        /// <summary>
        /// Raj: This function is used to get the single purchase order using PurchaseOrderId using dapper
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        public PurchaseOrder GetPurchaseOrderById(int purchaseOrderId)
        {
            string purchaseOrderQuery = "SELECT * FROM PurchaseOrder WITH(NOLOCK) WHERE PurchaseOrderId = @purchaseOrderId AND IsDeleted = 0";
            using (SqlConnection con = new SqlConnection(connString))
            {
                var purchaseOrder = con.Query<PurchaseOrder>(purchaseOrderQuery, new { @purchaseOrderId = purchaseOrderId }).FirstOrDefault();
                return purchaseOrder;
            }
        }

        /// <summary>
        /// Raj: This function is used to get the list of purchase order Items againts PurchaseOrderId using dapper.
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        public List<PurchaseOrderItemsDetail> GetPOItemsByPurchaseOrderId(int purchaseOrderId)
        {
            string purchaseOrderQuery = "select * From PurchaseOrderItemsDetails where PurchaseOrderId = @purchaseOrderId AND IsDeleted = 0";
            using (SqlConnection con = new SqlConnection(connString))
            {
                var purchaseOrder = con.Query<PurchaseOrderItemsDetail>(purchaseOrderQuery, new { @purchaseOrderId = purchaseOrderId }).ToList();
                return purchaseOrder;
            }
        }
        #endregion

        #region Get Payment Details
        /// <summary>
        /// Raj: This method returns payment Details on behalf of PaymentId. 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PurchaseOrderPaymentDetails GetPOPaymentDetailsById(int Id)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                String query = "SELECT * FROM PurchaseOrderPaymentDetails WITH(NOLOCK) WHERE ID = @Id AND IsDeleted = 0";
                var POPaymentDetail = con.Query<PurchaseOrderPaymentDetails>(query, new { @Id = Id }).FirstOrDefault();
                return POPaymentDetail;
            }
        }
        #endregion


        #region Update Payment Details
        public ResponseMessageBO Update(POPaymentBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PurchaseOrderPaymentDetails_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@PaymentMode", model.PaymentMode);
                    cmd.Parameters.AddWithValue("@ChequeNo", model.ChequeNumber);
                    cmd.Parameters.AddWithValue("@BankName", model.BankName);
                    cmd.Parameters.AddWithValue("@AccountNo", model.AccountNumber);
                    cmd.Parameters.AddWithValue("@AmountPaid", model.TotalPaybleAmount);
                    cmd.Parameters.AddWithValue("@BalancePay", model.BalanceAmount);
                    cmd.Parameters.AddWithValue("@PaymentStatus", model.IsPaid);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@PaymentDueDate", model.PaymentDueDate);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);

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

        #region Delete Purchase Order Payment Details
        public void Delete(int ID, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PurchaseOrderPaymentDetails_Delete", con);
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