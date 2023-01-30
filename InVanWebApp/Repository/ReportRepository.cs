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
    public class ReportRepository : IReportRepository
    {
        private readonly string conStr = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(ReportRepository));

        #region PO report

        /// Repository for Get PO Report Data 
        /// Developed by  - Siddharth Purohit on 30-12-2022

        public List<PurchaseOrderBO> getPOReportData(DateTime fromDate, DateTime toDate, string Status, int VendorId)
        {
            List<PurchaseOrderBO> resultList = new List<PurchaseOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_PurchaseOrder_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@Vendors", VendorId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new PurchaseOrderBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            PurchaseOrderDate = Convert.ToDateTime(reader["PO_Date"]).ToString("dd/MM/yyyy hh:mm:ss"),
                            PONumber = reader["PO_Number"].ToString(),
                            IndentNumber = reader["Indent_Number"].ToString(),
                            PurchaseOrderStatus = reader["PO_Status"].ToString(),
                            CompanyName = reader["Vendor_Name"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);

                resultList = null;

            }
            return resultList;
        }

        #endregion

        #region Raw material received data

        /// Repository for Raw Material Received Report Data 
        /// Developed by  - Siddharth Purohit on 02-01-2023

        public List<GRN_BO> getRawMaterialReceivedData(DateTime fromDate, DateTime toDate, int item, int wearhouse)
        {
            List<GRN_BO> resultList = new List<GRN_BO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_GetRawMaterialReceived_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@ItemID", item);
                    cmd.Parameters.AddWithValue("@WearhouseId", wearhouse);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new GRN_BO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            GRNCode = reader["GRN_Number"].ToString(),
                            GRN_Date = Convert.ToDateTime(reader["GRN_Date"]).ToString("dd/MM/yyyy hh:mm:ss"),
                            ItemCode = reader["Item_Code"].ToString(),
                            ItemName = reader["Item_Name"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(reader["Price_Per_unit"]),
                            ReceivedQty = float.Parse(reader["Received_Qty"].ToString()),
                            LocationName = reader["Location"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);

                resultList = null;

            }
            return resultList;




        }

        #endregion

        #region Rejection note data
        public List<RejectionNoteItemDetailsBO> getRejectionReportData(DateTime fromDate, DateTime toDate)
        {
            List<RejectionNoteItemDetailsBO> resultList = new List<RejectionNoteItemDetailsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_RejectionNote_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new RejectionNoteItemDetailsBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            RejectionNoteDate = Convert.ToDateTime(reader["Date"]).ToString("dd/MM/yyyy hh:mm:ss"),
                            Item_Name=reader["ItemName"].ToString(),
                            Item_Code=reader["ItemCode"].ToString(),
                            ItemUnitPrice=Convert.ToDecimal(reader["ItemUnitPrice"]),
                            TotalRecevingQuantiy=Convert.ToDouble(reader["QuantityTookForSorting"]),
                            RejectedQuantity = Convert.ToDouble(reader["RejectedQuantity"]),
                            RejectionNoteNo = reader["RejectionNoteNo"].ToString(),
                            InwardQCNumber = reader["InwardQCNumber"].ToString(),
                            ApprovedBy= reader["ApprovedBy"].ToString(),
                            CurrencyName=reader["CurrencyName"].ToString(),
                            ItemUnit=reader["ItemUnit"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);

                resultList = null;

            }
            return resultList;
        }

        #endregion

        #region Finished Goods Dispatch report

        /// <summary>
        /// Repository for Finished Goods Dispatch report
        /// Developed by  - Farheen 12 Jan'23
        /// </summary>
        /// <param name="fromDate">From Date of report</param>
        /// <param name="toDate">To Date of Report</param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public List<DeliveryChallanItemDetailsBO> getFinishedGoodsReportData(DateTime fromDate, DateTime toDate, int itemId)
        {
            List<DeliveryChallanItemDetailsBO> resultList = new List<DeliveryChallanItemDetailsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_FinishGoodDispatch_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@ItemID", itemId);
                    //cmd.Parameters.AddWithValue("@WearhouseId", LocationId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); 
                    while (reader.Read())
                    {
                        var result = new DeliveryChallanItemDetailsBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            DeliveryChallanNumber = reader["DeliveryChallanNumber"].ToString(),
                            OutwardDate = Convert.ToDateTime(reader["OutwardDate"]).ToString("dd/MM/yyyy hh:mm:ss"),
                            DeliveryAddress = reader["FGLocation"].ToString(),
                            ApprovedBy = reader["ApprovedBy"].ToString(),
                            Item_Code = reader["ItemCode"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(reader["PricePerUnit"]),
                            ItemUnit = reader["ItemUnit"].ToString(),
                            DispatchQuantity = float.Parse(reader["QuantityDispatched"].ToString()),
                            CurrencyName=reader["CurrencyName"].ToString()
                            
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);

                resultList = null;

            }
            return resultList;
        }
        #endregion

        #region Inventory FIFO report

        /// <summary>
        /// Repository for Inventory FIFO report
        /// Developed by  - Farheen 21 Jan'23
        /// </summary>
        /// <param name="fromDate">From Date of report</param>
        /// <param name="toDate">To Date of Report</param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public List<StockMasterBO> getInventoryFIFOReportData(DateTime fromDate, DateTime toDate, int itemId)
        {
            List<StockMasterBO> resultList = new List<StockMasterBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_Inventory_FIFO_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@ItemID", itemId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); 
                    while (reader.Read())
                    {
                        var result = new StockMasterBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            Item_Code = reader["Item_Code"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            ItemUnitPriceWithCurrency =reader["ItemPrice"].ToString(),
                            ItemUnit = reader["ItemUnit"].ToString(),
                            StockQuantity = Convert.ToDouble(reader["StockQuantity"]),
                            InwardDate = reader["CreatedDate"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);

                resultList = null;

            }
            return resultList;
        }

        #endregion

        #region Total Inventory Cost Warehouse wise report data 

        /// <summary>
        /// Repository for Total Inventory Cost Warehouse wise report
        /// Developed by  - Farheen 12 Jan'23
        /// </summary>
        /// <param name="fromDate">From Date of report</param>
        /// <param name="toDate">To Date of Report</param>
        /// <param name="LocationId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public List<LocationWiseStockBO> getTotalInventoryCostData(DateTime fromDate, DateTime toDate, int LocationId, int itemId)
        {
            List<LocationWiseStockBO> resultList = new List<LocationWiseStockBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_TotalInventoryCostWarehouseWiseStock", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@ItemID", itemId);
                    cmd.Parameters.AddWithValue("@LocationId", LocationId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = new LocationWiseStockBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            Item_Code = reader["Item_Code"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            ItemUnitPriceWithCurrency = reader["ItemUnitPrice"].ToString(),
                            Quantity = Convert.ToDouble(reader["Quantity"]),
                            ItemUnit = reader["ItemUnit"].ToString(),
                            TotalInventoryValue = reader["TotalInventoryValue"].ToString()

                        };
                        resultList.Add(result);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);

                resultList = null;

            }
            return resultList;
        }
        #endregion

        #region Stock Reconciliation data 

        /// <summary>
        /// Repository for Stock Reconciliation report
        /// Developed by  - Farheen 12 Jan'23
        /// </summary>
        /// <param name="fromDate">From Date of report</param>
        /// <param name="toDate">To Date of Report</param>
        /// <param name="LocationId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public List<StockAdjustmentDetailsBO> getStockReconciliationData(DateTime fromDate, DateTime toDate, int LocationId, int itemId)
        {
            List<StockAdjustmentDetailsBO> resultList = new List<StockAdjustmentDetailsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_StockReconciliation_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@ItemID", itemId);
                    cmd.Parameters.AddWithValue("@WearhouseId", LocationId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = new StockAdjustmentDetailsBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            StockAdjustedDate=reader["StockAdjustedDate"].ToString(),
                            Item_Code = reader["Item_Code"].ToString(),
                            Item_Name = reader["Item_Name"].ToString(),
                            ItemUnitPriceWithCurrency = reader["ItemUnitPrice"].ToString(),
                            AvailableStock = Convert.ToDecimal(reader["PreviousStock"]),
                            DifferenceInStock = Convert.ToDecimal(reader["StockAdjusted"]),
                            PhysicalStock = Convert.ToDecimal(reader["FinalStock"]),
                            ItemUnit = reader["ItemUnit"].ToString(),
                            Remarks= reader["Remarks"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);

                resultList = null;

            }
            return resultList;
        }
        #endregion
    }
}
