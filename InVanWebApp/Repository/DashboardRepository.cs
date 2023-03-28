using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp.Repository;
using InVanWebApp_BO;
//using InVanWebApp.DAL;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using log4net;
using InVanWebApp.Repository.Interface;

namespace InVanWebApp.Repository
{
    public class DashboardRepository: IDashboardRepository
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

        #endregion#region Function for real time warehouse wise stock

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
        
        #region Function for Fifo system 
        public List<DashboardBO> GetFIFOSystem(DateTime fromDate,DateTime toDate, int ItemId = 0, int LocationID = 0)
        {
            List<DashboardBO> resultList = new List<DashboardBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_dashb_FIFOWithExpiryHighlighting", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemId", ItemId);
                    cmd.Parameters.AddWithValue("@locationId", LocationID);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);

                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader(); 
                    while (reader.Read())
                    {
                        var result = new DashboardBO()
                        {
                            GRNDate = reader["GRNDate"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            LocationName = reader["Location"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(reader["Price"]),
                            CurrencyName = (reader["Currency"].ToString()),
                            ReceivedQty=float.Parse(reader["Quantity"].ToString())
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

        #endregion#region Function for Fifo system 

        #region Function for Yeild Dashboard
        public List<DashboardBO> GetYeildSystem(DateTime fromDate,DateTime toDate, int BatchNumberID = 0, int WorkOrderNumberID = 0)
        {
            List<DashboardBO> resultList = new List<DashboardBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_dashb_Yeild", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@BatchNumber", BatchNumberID);
                    cmd.Parameters.AddWithValue("@WorkOrderNumber", WorkOrderNumberID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new DashboardBO()
                        {
                            //ID = Convert.ToInt32(reader["ID"]),
                            WorkOrderNumber = reader["WorkOrderNumber"].ToString(),
                            BatchNumber = reader["BatchNumber"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            ExpectedYeild = Convert.ToDecimal(reader["ExpectedYeild"]),
                            ActualYeild = Convert.ToDecimal(reader["ActualYeild"]),
                            //ItemUnit = (reader["ItemUnit"].ToString()),
                            //CurrencyName = (reader["CurrencyName"].ToString())
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