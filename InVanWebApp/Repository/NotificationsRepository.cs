using InVanWebApp.Common;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InVanWebApp.Repository
{
    public class NotificationsRepository : INotificationsRepository
    {
        //private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(NotificationsRepository));

        #region Function for reorder point On MinStock of available total stock
        public List<StockMasterBO> GetReorderPointOnMinStock(int ItemId = 0) 
        {
            List<StockMasterBO> resultList = new List<StockMasterBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_dashb_ReorderPoint_OnMinStock", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemId", ItemId);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = new StockMasterBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            ItemName = reader["ItemName"].ToString(),
                            ItemUnit = reader["ItemUnit"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(reader["ItemUnitPrice"]),
                            StockQuantity = Convert.ToDouble(reader["StockQuantity"]),
                            CurrencyName = (reader["CurrencyName"].ToString()),
                            MinimumStock = float.Parse(reader["MinStock"].ToString())
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

        #region Function for for displaying NameOfEquipment and CalibrationDueDate in notification.
        public List<CalibrationLogBO> GetCalibrationDueDateData()  
        {
            List<CalibrationLogBO> resultList = new List<CalibrationLogBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_dashb_ReorderPoint_CalibrationLog", con);
                    cmd.CommandType = CommandType.StoredProcedure;                    

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = new CalibrationLogBO() 
                        {                            
                            NameOfEquipment = reader["NameOfEquipment"].ToString(),
                            CalibrationLogDueDate = Convert.ToDateTime(reader["CalibrationDueDate"]).ToString("dd-MM-yyyy")  
                            //CalibrationDueDate = Convert.ToDateTime(reader["CalibrationDueDate"]) 
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

        #region Function for for displaying PurchaseOrderId and PaymentDueDate in notification.
        public List<POPaymentBO> GetPOPaymentDueDateData() 
        {
            List<POPaymentBO> resultList = new List<POPaymentBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_dashb_ReorderPoint_PurchaseOrderPaymentDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = new POPaymentBO()
                        {
                            PurchaseOrderId = Convert.ToInt32(reader["PurchaseOrderId"]),
                            POPaymentDueDate = Convert.ToDateTime(reader["POPaymentDueDate"]).ToString("dd-MM-yyyy")
                            //PaymentDueDate = Convert.ToDateTime(reader["POPaymentDueDate"])
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

        #region Function for for displaying SalesOrderId and PaymentDueDate in notification.
        public List<SOPaymentBO> GetSOPaymentDueDateData() 
        {
            List<SOPaymentBO> resultList = new List<SOPaymentBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_dashb_ReorderPoint_SalesOrderPaymentDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = new SOPaymentBO()
                        {
                            SalesOrderId = Convert.ToInt32(reader["SalesOrderId"]),
                            SOPaymentDueDate = Convert.ToDateTime(reader["SOPaymentDueDate"]).ToString("dd-MM-yyyy") 
                            //PaymentDueDate = Convert.ToDateTime(reader["SOPaymentDueDate"]) 
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