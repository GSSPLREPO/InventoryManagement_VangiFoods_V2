using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using System.Data.SqlClient;
using log4net;
using System.Configuration;
using System.Data;

namespace InVanWebApp.Repository
{
    public class DashboardRepository:IDashboardRepository
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(ItemTypeRepository));

        #region Function for real time warehouse wise stock
        public List<LocationWiseStockBO> GetDashboardData(int id, int ItemId = 0)
        {
            List<LocationWiseStockBO> resultList = new List<LocationWiseStockBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_dashb_RealTimeWarehouseWiseStock", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LocationId", id);
                    cmd.Parameters.AddWithValue("@ItemId", ItemId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new LocationWiseStockBO()
                        {
                            //ID = Convert.ToInt32(reader["ID"]),
                            LocationName = reader["LocationName"].ToString(),
                            ItemName = reader["Item_Name"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(reader["ItemUnitPrice"]),
                            ItemId= Convert.ToInt32(reader["ItemID"]),
                            Quantity = Convert.ToDouble(reader["Quantity"]),
                            ItemUnit = (reader["ItemUnit"].ToString()),
                            CurrencyName = (reader["CurrencyName"].ToString())
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

        #region Function for reorder point of available total stock
        public List<StockMasterBO> GetReorderPointDashboardData(int ItemId = 0)
        {
            List<StockMasterBO> resultList = new List<StockMasterBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_dashb_ReorderPointOnAvailableStock", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemId",ItemId);

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
                            MinimumStock=float.Parse(reader["MinStock"].ToString())
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

        #region Function for Bind Total Production Cost by Batch Number
        /// <summary>
        /// Snehal: This function is for fatching the batch number
        /// Also Use for Dashboard for production
        /// </summary>
        /// <returns></returns>
        public List<ProductionMaterialIssueNoteBO> GetDashboardProductionCostByBatchNumber(int SOID,string BatchNumber = "", DateTime? fromDate = null, DateTime? toDate = null)
        {
            List<ProductionMaterialIssueNoteBO> resultList = new List<ProductionMaterialIssueNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_dashb_BatchwiseProductionCost]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SOID", SOID);
                    cmd.Parameters.AddWithValue("@BatchNumber", BatchNumber);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = new ProductionMaterialIssueNoteBO()
                        {
                            ID = Convert.ToInt32(reader["SrNo"]),
                            WorkOrderNumber = reader["WorkOrderNumber"].ToString(),
                            ProductionMaterialIssueNoteNo = reader["POMaterialIssueNoteNumber"].ToString(),
                            Item_Name = reader["ProductName"].ToString(),
                            BatchNumber = reader["BatchNumber"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(reader["Rawmaterialcost"])
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

        #region Function For Production Utility Consumption By Batch Wise Data 
        public List<ReportBO> GetProductionUtilityConsumptionByBatchDashboardData(DateTime fromDate, DateTime toDate, int BatchNumberId = 0, int WorkOrderNumberId = 0)
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_dashb_UtilityConsumptionbybatchDashboard", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@BatchNumber", BatchNumberId);
                    cmd.Parameters.AddWithValue("@WorkOrderNumber", WorkOrderNumberId);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            WorkOrderNumber = (reader["WorkOrderNumber"].ToString()),
                            ProductionMaterailIssueNoteNumber = (reader["POMaterialIssueNoteNumber"].ToString()),
                            ItemName = (reader["RawMaterial"].ToString()),
                            BatchNumber = (reader["BatchNumber"].ToString()),
                            RawMaterialCost = Convert.ToDecimal(reader["RawMaterialConsumption"])
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

        #region Function Utility Consumption V/S Production By Worl Order Wise Data 
        public List<ReportBO> GetDashboardUtilityConsumptionProduction(int SO_ID, DateTime? fromDate = null, DateTime? toDate = null)
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_dash_UtilityConsumptionVsProduction]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@SO_Id", SO_ID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            WorkOrderNumber = (reader["WorkOrderNumber"].ToString()),
                            ConsumeQty = Convert.ToDecimal(reader["consumedQty"]),
                            ProQty = Convert.ToDecimal(reader["proQty"])
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

        #region  Bind Work Order dropdown
        /// <summary>
        /// Snehal: This function is for fatching the Utility Consumption By Work Order dropdown
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReportBO> GetAllWorkOrderNumber()
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_WorkOrderNumber_Get]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            //ID = Convert.ToInt32(reader["ID"]),
                            ID = Convert.ToInt32(reader["SO_Id"]),
                            WorkOrderNumber = reader["WorkOrderNumber"].ToString()
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