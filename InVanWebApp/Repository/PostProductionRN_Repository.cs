using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using InVanWebApp.Common;
using InVanWebApp.Repository.Interface;
using log4net;
using InVanWebApp_BO;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace InVanWebApp.Repository
{
    public class PostProductionRN_Repository : IPostProductionRN_Repository
    {
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(SalesOrderRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen : This function is for fecthing list of order master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PostProductionRejectionNoteBO> GetAll()
        {
            List<PostProductionRejectionNoteBO> resultList = new List<PostProductionRejectionNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PostProdRN_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new PostProductionRejectionNoteBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            PostProdRejectionNoteNo = reader["PostProdRejectionNoteNo"].ToString(),
                            PostProdRejectionNoteDate = Convert.ToDateTime(reader["PostProdRejectionNoteDate"]),
                            PostProdRejectionType = reader["PostProdRejectionType"].ToString(),
                            BatchNumber = reader["BatchNumber"].ToString(),
                            WorkOrderNo = reader["WorkOrderNo"].ToString(),
                            SO_No = reader["SO_No"].ToString()
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

        #region Bind Item details
        public FinishedGoodSeriesBO GetItemDetails(int FGSID, int FGSStage, string RNType)
        {
            var result = new FinishedGoodSeriesBO();

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PostProRNSO_BindItemDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FSID",FGSID);
                    cmd.Parameters.AddWithValue("@Stage", FGSStage);
                    cmd.Parameters.AddWithValue("@Type", RNType);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        result = new FinishedGoodSeriesBO()
                        {
                            BatchNo = reader["BatchNo"].ToString(),
                            SalesOrderId = reader["SalesOrderId"] is DBNull ? 0: Convert.ToInt32(reader["SalesOrderId"]),
                            SONo = reader["SONumber"].ToString(),
                            ItemId = reader["ItemId"] is DBNull?0: Convert.ToInt32(reader["ItemId"]),
                            ProductName = reader["ProductName"].ToString(),
                            Item_Code = reader["Item_Code"].ToString(),
                            OrderQty = reader["OrderQty"] is DBNull?0: Convert.ToDecimal(reader["OrderQty"]),
                            QuantityInKG = reader["QuantityInKG"] is DBNull?0: Convert.ToDouble(reader["QuantityInKG"])
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

        #region Bind work order dropdown
        public IEnumerable<FinishedGoodSeriesBO> BindWorkOrderDD()
        {
            List<FinishedGoodSeriesBO> resultList = new List<FinishedGoodSeriesBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PostProRNSO_DD", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new FinishedGoodSeriesBO()
                        {
                            FGSID = Convert.ToInt32(reader["FGSID"]),
                            SalesOrderId = Convert.ToInt32(reader["SalesOrderId"]),
                            SONo = reader["SONumber"].ToString(),
                            WorkOrderNo = reader["WorkOrderNo"].ToString(),
                            BatchNo = reader["BatchNo"].ToString()
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