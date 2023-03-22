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
                            ItemTaxValue = Convert.ToDecimal(reader["ItemTaxValue"])
                            //BalanceQuantity = Convert.ToDouble(reader["BalanceQuantity"])

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