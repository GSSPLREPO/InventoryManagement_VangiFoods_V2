﻿using System;
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
        public List<StockMasterBO> GetReorderPointDashboardData()
        {
            List<StockMasterBO> resultList = new List<StockMasterBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_dashb_ReorderPointOnAvailableStock", con);
                    cmd.CommandType = CommandType.StoredProcedure;
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
    }
}