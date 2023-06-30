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
using InVanWebApp.Common;

namespace InVanWebApp.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly string conStr = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
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
                            GRN_Date = Convert.ToDateTime(reader["GRN_Date"]).ToString("dd/MM/yyyy"),
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
        //public List<LocationWiseStockBO> getTotalInventoryCostData(DateTime fromDate, DateTime toDate, int LocationId, int itemId)
        public List<LocationWiseStockBO> getTotalInventoryCostData(DateTime fromDate, DateTime toDate, int LocationId, int ItemId) 
        {
            List<LocationWiseStockBO> resultList = new List<LocationWiseStockBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_TotalInventoryCostWarehouseWiseStock", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    //cmd.Parameters.AddWithValue("@ItemID", itemId);
                    cmd.Parameters.AddWithValue("@ItemID", ItemId); 
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

        #region Inventory Analysis Report (FIFO) 

        /// <summary>
        /// Repository for Inventory Analysis Report (FIFO) 
        /// Developed by  - Rahul 22 Feb'23
        /// </summary>
        /// <param name="fromDate">From Date of report</param>
        /// <param name="toDate">To Date of Report</param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public List<StockMasterBO> getInventoryAnalysisFIFOReportData(DateTime fromDate, DateTime toDate, int itemId)
        {
            List<StockMasterBO> resultList = new List<StockMasterBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_InventoryAnalysis_Report", con);
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
                            CompanyName = reader["VendorName"].ToString(),
                            ItemID = reader["ItemId"] is DBNull ? 0 : Convert.ToInt32(reader["ItemId"]),
                            Item_Code = reader["ItemCode"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            GRNCode = reader["GRN_No"].ToString(),
                            PO_Number = reader["PO_No"].ToString(),
                            Outward_No = reader["Outward_No"].ToString(),
                            GRNDate = reader["StockInDate"] is DBNull ? "" : Convert.ToDateTime(reader["StockInDate"]).ToString("dd/MM/yyyy hh:mm:ss").Trim(),
                            StockInQty = reader["StockInQty"] is DBNull ? 0 : float.Parse(reader["StockInQty"].ToString()),
                            ItemUnitPrice = reader["StockInUnitPrice"] is DBNull ? 0 : Convert.ToDecimal(reader["StockInUnitPrice"]),
                            StockInTotalPrice = reader["StockInTotalPrice"] is DBNull ? 0 : Convert.ToDecimal(reader["StockInTotalPrice"]),
                            CurrencyName = reader["StockInCurrency"].ToString(),
                            DeliveryChallanDate = reader["StockOutDate"] is DBNull ? "" : Convert.ToDateTime(reader["StockOutDate"]).ToString("dd/MM/yyyy hh:mm:ss").Trim(),
                            StockOutQty = reader["StockOutQty"] is DBNull ? 0 : float.Parse(reader["StockOutQty"].ToString()),
                            StockOutUnitPrice = reader["StockOutUnitPrice"] is DBNull ? 0 : Convert.ToDecimal(reader["StockOutUnitPrice"]),
                            StockOutTotalPrice = reader["StockOutTotalPrice"] is DBNull ? 0 : Convert.ToDecimal(reader["StockOutTotalPrice"]),
                            StockOutCurrency = reader["StockOutCurrency"] is DBNull ? "" : reader["StockOutCurrency"].ToString(),
                            AvlDate = reader["AvlDate"] is DBNull ? "" : Convert.ToDateTime(reader["AvlDate"]).ToString("dd/MM/yyyy hh:mm:ss").Trim(),
                            AvlQty = reader["AvlQty"] is DBNull ? 0 : float.Parse(reader["AvlQty"].ToString()),
                            AvlUnitPrice = reader["AvlUnitPrice"] is DBNull ? 0 : float.Parse(reader["AvlUnitPrice"].ToString()),
                            AvlTotalPrice = reader["AvlTotalPrice"] is DBNull ? 0 : float.Parse(reader["AvlTotalPrice"].ToString()),
                            AvlCurrency = reader["AvlCurrency"] is DBNull ? "" : reader["AvlCurrency"].ToString()

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

        #region Purchase Invoice report

        /// Repository for Get Purchase Invoice Report Data 
        /// Developed by  - Siddharth Purohit on 09-03-2023

        public List<PurchaseOrderBO> getPurchaseInvoiceReportData(DateTime fromDate, DateTime toDate, int PoNumber, string Status)
        {
            List<PurchaseOrderBO> resultList = new List<PurchaseOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_PurchaseInvoice_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@PONumber", PoNumber);
                    cmd.Parameters.AddWithValue("@PaymentStatus", Status);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new PurchaseOrderBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            PONumber = reader["PONumber"].ToString(),
                            PurchaseOrderDate = Convert.ToDateTime(reader["PurchaseOrderDate"]).ToString("dd/MM/yyyy"),
                            DelDate = Convert.ToDateTime(reader["DeliveryDate"]).ToString("dd/MM/yyyy"),
                            CompanyName = reader["CompanyName"].ToString(),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            AdvancedPayment = float.Parse(reader["AdvancedPayment"].ToString()),
                            AmountPaid = float.Parse(reader["AmountPaid"].ToString()),
                            BalancePayment = float.Parse(reader["BalancePayment"].ToString()),
                            PayDate = Convert.ToDateTime(reader["PaymentDate"]).ToString("dd/MM/yyyy"),
                            PurchaseOrderStatus = reader["PaymentStatus"].ToString()
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

        #region Issue Note Report

        /// Repository for Get ISsue Note Report Data 
        /// Developed by  - Siddharth Purohit on 10-03-2023

        public List<IssueNoteBO> getIssueNoteReportData(DateTime fromDate, DateTime toDate, int IssueNoteNumber)
        {
            List<IssueNoteBO> resultList = new List<IssueNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_IssueNote_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@IssueNoteNumber", IssueNoteNumber);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new IssueNoteBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            IssueNoteNo = reader["IssueNoteNumber"].ToString(),
                            IssueNoteNoDate = Convert.ToDateTime(reader["IssueNoteDate"]).ToString("dd/MM/yyyy"),
                            ItemName = reader["ItemName"].ToString(),
                            QuantityRequested = float.Parse(reader["QuantityRequested"].ToString()),
                            QuantityIssued = float.Parse(reader["QuantityIssued"].ToString()),
                            AvailableStockBeforeIssue = float.Parse(reader["AvailableStockBeforeIssue"].ToString()),
                            StockAfterIssuing = float.Parse(reader["StockAfterIssuing"].ToString())
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

        #region GRN Report

        /// Repository for Get GRN Report Data 
        /// Developed by  - Siddharth Purohit on 10-03-2023

        public List<GRN_BO> getGRNReportData(DateTime fromDate, DateTime toDate, int GRNCode)
        {
            List<GRN_BO> resultList = new List<GRN_BO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_GRN_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@GRNCode", GRNCode);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new GRN_BO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            ItemName = reader["ItemName"].ToString(),
                            ItemCode = reader["ItemCode"].ToString(),
                            GRN_Date = Convert.ToDateTime(reader["GRNDate"]).ToString("dd/MM/yyyy"),
                            GRNCode = reader["GRNCode"].ToString(),
                            OrderQty = float.Parse(reader["OrderQuantity"].ToString()),
                            ReceivedQty = float.Parse(reader["ReceivedQuantity"].ToString()),
                            InwardQty = float.Parse(reader["InwardQuantity"].ToString()),
                            InwardNoteNumber = reader["InwardNoteNumber"].ToString(),
                            InwardQCNumber = reader["InwardQCNumber"].ToString(),
                            DeliveryAddress = reader["DeliveryAddress"].ToString()
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

        #region Rejection Report data
        public List<RejectionNoteItemDetailsBO> getRejectionReportData(DateTime fromDate, DateTime toDate, int rejectionNumber, int FlagDebitNote = 0)
        {
            List<RejectionNoteItemDetailsBO> resultList = new List<RejectionNoteItemDetailsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_Rejection_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@rejectionNumber", rejectionNumber);
                    cmd.Parameters.AddWithValue("@flag", FlagDebitNote);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new RejectionNoteItemDetailsBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            RejectionNoteDate = Convert.ToDateTime(reader["Date"]).ToString("dd/MM/yyyy"),
                            RejectionNoteNo = reader["RejectionNumber"].ToString(),
                            InwardNumber = reader["InwardNumber"].ToString(),
                            ReasonForRR = reader["ReasonForRejection"].ToString(),
                            Item_Name = reader["ItemName"].ToString(),
                            Item_Code = reader["ItemCode"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(reader["ItemUnitPrice"]),
                            TotalRecevingQuantiy = Convert.ToDouble(reader["RecivedQuantity"]),
                            RejectedQuantity = Convert.ToDouble(reader["RejectedQuantity"]),
                            ApprovedBy = reader["ApprovedBy"].ToString(),
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

        #region Company data 

        /// <summary>
        /// Company report
        /// Developed by  - Ytari 10 March'23
        /// </summary>
        /// <param name="fromDate">From Date of report</param>
        /// <param name="toDate">To Date of Report</param>
        /// <param name="CompanyType"></param>
        /// <returns></returns>
        public List<CompanyBO> getCompanyDataByType(string CompanyType)
        {
            List<CompanyBO> resultList = new List<CompanyBO>();

            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {

                    SqlCommand cmd1 = new SqlCommand("usp_rpt_Company_Report", con);
                    cmd1.Parameters.AddWithValue("@CompanyType", CompanyType);


                    cmd1.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader2 = cmd1.ExecuteReader();

                    while (dataReader2.Read())
                    {
                        var result = new CompanyBO()
                        {
                            ID = Convert.ToInt32(dataReader2["ID"]),
                            CompanyType = Convert.ToString(dataReader2["CompanyType"]),
                            CompanyName = dataReader2["CompanyName"].ToString(),
                            ContactPersonName = dataReader2["ContactPersonName"].ToString(),
                            ContactPersonNo = dataReader2["ContactPersonNo"].ToString(),
                            EmailId = dataReader2["EmailId"].ToString(),
                            Address = dataReader2["Address"].ToString(),
                            Remarks = dataReader2["Remarks"].ToString()
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

        #region Batchwise Production report data 
        public List<ReportBO> getBatchwiseProductionCostReportData(DateTime fromDate, DateTime toDate, string batchNumber)
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_BatchwiseProductionCost_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@BatchNumber", batchNumber);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            WorkOrderNumber = reader["WorkOrderNumber"].ToString(),
                            ProductionMaterailIssueNoteNumber = reader["POMaterialIssueNoteNumber"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            BatchNumber = reader["BatchNumber"].ToString(),
                            RawMaterialCost = Convert.ToDecimal(reader["Rawmaterialcost"])

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

        #region  Bind BatchNumber
        /// <summary>
        /// Siddharth: This function is for fatching the batch number
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReportBO> GetAll()
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_BatchNumber_Get", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            BatchNumber = reader["BatchNumber"].ToString()
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

        #region  Bind FG Location BatchNumber
        /// <summary>
        /// Siddharth: This function is for fatching the batch number of FG Location
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReportBO> GetGFLocationBatchNumber()
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_FGLocationBatchNumber_Get", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            BatchNumber = reader["BatchNumber"].ToString()
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

        #region FG Locationwise report data 
        public List<ReportBO> getFGLocationwiseReportData(DateTime fromDate, DateTime toDate, int locationId)
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_FGLocationwise_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@LocationId", locationId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            LocationName = reader["LocationName"].ToString(),
                            ItemCode = reader["ItemCode"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            ItemUnit = reader["ItemUnit"].ToString(),
                            ItemUnitPrice = reader["ItemUnitPrice"].ToString(),
                            Quantity = reader["Quantity"].ToString()

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

        #region Yeild report data 
        public List<ReportBO> getYeildReportData(DateTime fromDate, DateTime toDate, int batchNumber)
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_Yeild_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@BatchNumber", batchNumber);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            WorkOrderNumber = reader["WorkOrderNumber"].ToString(),
                            BatchNumber = reader["BatchNumber"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            ExpectedYeild = Convert.ToDecimal(reader["ExpectedYeild"]),
                            ActualYeild = Convert.ToDecimal(reader["ActualYeild"])

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

        #region Raw Material Cost Analysisreport data 
        public List<ReportBO> getRawMaterialCostAnalysisReportData(DateTime fromDate, DateTime toDate, int itemId)
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_RawMaterialCostAnalysis_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            WorkOrderNumber = reader["WorkOrderNumber"].ToString(),
                            ItemCode = reader["ItemCode"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            QuantityUsed = reader["QuantityUsed"].ToString(),
                            ItemUnitPrice = reader["ItemUnitPrice"].ToString(),
                            RawMaterialCost = Convert.ToDecimal(reader["Rawmaterialcost"])

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

        #region  Bind WrokOrderNumber
        /// <summary>
        /// Yatri: This function is for fatching the Wrok Order number
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReportBO> Getall()
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_WorkOrderNumber_Get", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
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

        #region Delivery Challan (Against SO) report data
        public List<ReportBO> getDeliveryChallanAgainstSOReportData(DateTime fromDate, DateTime toDate, string SONumberId)
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_DeliveryChallanAgainstSO_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@SONumberId", SONumberId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            SONumber = reader["SONumber"].ToString(),
                            SODate = reader["SODate"].ToString(),
                            DeliveryChallanNo = reader["DeliveryChallanNo"].ToString(),
                            DeliveryChallanDate = reader["DeliveryChallanDate"].ToString(),
                            ClientName = reader["ClientName"].ToString()

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

        #region  Bind SONumber
        /// <summary>
        /// Siddharth: This function is for fatching the SO number
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReportBO> GetSONumber()
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SONumber_Get", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            SONumber = reader["SONumber"].ToString()
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

        #region Sales report data
        public List<ReportBO> getSalesReportData(DateTime fromDate, DateTime toDate, string SONumberId)
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_Sales_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@SONumberId", SONumberId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            InquiryNo = reader["InquiryNo"].ToString(),
                            SONumber = reader["SONumber"].ToString(),
                            SODate = reader["SODate"].ToString(),
                            ClientName = reader["ClientName"].ToString(),
                            Status = reader["Status"].ToString()

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

        #region Sales Invoice report data
        public List<ReportBO> getSalesInvoiceReportData(DateTime fromDate, DateTime toDate, string SONumberId)
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_SalesInvoice_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@SONumberId", SONumberId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            SONumber = reader["SONumber"].ToString(),
                            InvoiceNo = reader["InvoiceNo"].ToString(),
                            InvoiceAmount = reader["InvoiceAmount"].ToString(),
                            AmountRecived = reader["AmountRecived"].ToString(),
                            BalanceRecivable = reader["BalanceRecivable"].ToString()

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

        #region Debit Note report data
        public List<ReportBO> getDBNoteReportData(DateTime fromDate, DateTime toDate, int DBNoteNumberId)
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_DBNoteReport", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@DBNoteNumberId", DBNoteNumberId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            DebitNoteNo = reader["DebitNoteNo"].ToString(),
                            DebitNoteDate = reader["DebitNoteDate"].ToString(),
                            PONo = reader["PONo"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            POQuantity = reader["POQuantity"].ToString(),
                            DebitedQuantity = reader["DebitedQuantity"].ToString()

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

        #region  Bind Debit Note Number
        /// <summary>
        /// Siddharth: This function is for fatching the SO number
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReportBO> GetDebitNoteNumber()
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_DBBoteNumber_Get", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            DebitNoteNo = reader["DBNoteNo"].ToString()
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

        #region Wastage Report data 
        /// <summary>
        /// Rahul 7 Apr 2023
        /// To Bind Vendor-wise Inward wastage report against each PO.    
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="inwardNumber"></param>
        /// <returns></returns>
        public List<RejectionNoteItemDetailsBO> getWastageReportData(DateTime fromDate, DateTime toDate, int inwardNumber)
        {
            List<RejectionNoteItemDetailsBO> resultList = new List<RejectionNoteItemDetailsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_Wastage_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@inwardNumber", inwardNumber);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new RejectionNoteItemDetailsBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            RejectionNoteDate = Convert.ToDateTime(reader["Date"]).ToString("dd/MM/yyyy"),  ///updated 17-04-23.
                            RejectionNoteNo = reader["RejectionNumber"].ToString(), ///added 17-04-23.  
                            InwardNumber = reader["InwardNumber"].ToString(),
                            //InwardQCNumber = reader["InwardQCNumber"].ToString(), ///Remove 17-05-23.
                            ProductionQCNumber = reader["ProductionQCNumber"].ToString(),   ///added 17-05-23.  
                            PONumber = reader["PONumber"].ToString(),
                            SupplierName = reader["SupplierName"].ToString(),
                            Item_Name = reader["ItemName"].ToString(),
                            Item_Code = reader["ItemCode"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(reader["ItemUnitPrice"]),
                            InwardQuantity = Convert.ToDouble(reader["InwardQuantity"]),
                            QuantityTookForSorting = Convert.ToDouble(reader["RecivedQuantity"]),
                            //BalanceQuantity = Convert.ToDouble(reader["BalanceQuantity"]), ///Remove 17-04-23.
                            RejectedQuantity = Convert.ToDouble(reader["RejectedQuantity"]),
                            WastageQuantityInPercentage = Convert.ToDouble(reader["WastageQuantity"]),
                            ReasonForRR = reader["ReasonForWastage"].ToString(),
                            ApprovedBy = reader["ApprovedBy"].ToString(),
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

        #region Pre-Production_QC Report data 
        /// <summary>
        /// Rahul 11 Apr 2023
        /// To Bind Pre-Production_QC report against each WO.    
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="PreProductionQCId"></param>
        /// <returns></returns>
        public List<PreProduction_QC_Details> getPreProduction_QCReportData(DateTime fromDate, DateTime toDate, int PreProductionQCId)
        {
            List<PreProduction_QC_Details> resultList = new List<PreProduction_QC_Details>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_PreProduction_QC_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@PreProductionQCId", PreProductionQCId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row. 
                    while (reader.Read())
                    {
                        var result = new PreProduction_QC_Details()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            InwardQCDate = Convert.ToDateTime(reader["Date"]).ToString("dd/MM/yyyy"),
                            WONumber = reader["WONumber"].ToString(),
                            MaterialIssueNumber = reader["MaterialIssueNumber"].ToString(),
                            PQCNumber = reader["PQCNumber"].ToString(),
                            Item_Name = reader["ItemName"].ToString(),
                            Item_Code = reader["ItemCode"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(reader["ItemUnitPrice"]),
                            IssuedQuantity = Convert.ToDouble(reader["InwardQuantity"]),
                            QuantityTookForSorting = Convert.ToDouble(reader["RecivedQuantity"]),
                            BalanceQuantity = Convert.ToDouble(reader["BalanceQuantity"]),
                            RejectedQuantity = Convert.ToDouble(reader["RejectedQuantity"]),
                            WastageQuantityInPercentage = Convert.ToDouble(reader["WastageQuantity"]),
                            ReasonForWastage = reader["ReasonForWastage"].ToString(),
                            ApprovedBy = reader["ApprovedBy"].ToString(),
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

        #region Post Production Rejection note data
        public List<ReportBO> getPostProductionRejectionReportData(DateTime fromDate, DateTime toDate, string BatchNumber, string WorkOrderNumber)
        {
            List<ReportBO> resultList = new List<ReportBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_PostProductionRejection_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@batchNumber", BatchNumber);
                    cmd.Parameters.AddWithValue("@workOrderNumber", WorkOrderNumber);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new ReportBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            RNDate = Convert.ToDateTime(reader["RNDate"]).ToString("dd/MM/yyyy"),
                            WorkOrderNumber = reader["WONumber"].ToString(),
                            BatchNumber = reader["BatchNumber"].ToString(),
                            PostProductionRejectionNoteNo = reader["PostProductionRejectionNoteNo"].ToString(),
                            Stage = reader["Stage"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            ItemCode = reader["ItemCode"].ToString(),
                            ItemUnitPrice = reader["ItemUnitPrice"].ToString(),
                            TotalQty = Convert.ToDecimal(reader["TotalQty"]),
                            RejectedQty = Convert.ToDecimal(reader["RejectedQty"]),
                            Remarks = reader["Remarks"].ToString(),
                            ApprovedBy = reader["ApprovedBy"].ToString()

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

        #region Consolidated Production Stages 1to3 Report 
        /// <summary>
        /// Rahul 05 June 2023
        /// Binding the Consolidated Production Stages 1to3 Report data 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="ItemId"></param>
        /// <returns></returns>
        public List<RQCCPBO> getConsolidatedStagesReportData(DateTime fromDate, DateTime toDate, int ItemId)  
        {
            List<RQCCPBO> resultList = new List<RQCCPBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_Consolidated_Production_Stages_1_2_3_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@ItemId", ItemId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new RQCCPBO() 
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            RCCPDate = Convert.ToDateTime(reader["Date"]).ToString("dd/MM/yyyy"),
                            ProductName = reader["ProductName"].ToString(),
                            LotNumber = reader["LotNo"].ToString(),
                            RawBatchesNo = reader["RawBatchNo"].ToString(),
                            WeightofRawBatches = reader["WeightofRawBatches"].ToString(),
                            TansferTimeintoHoldingSilo = reader["TansferTimeintoHoldingSilo"].ToString(),
                            Weight = reader["Weight"].ToString(),  
                            HoldingSiloReceivingTime = reader["HoldingSiloReceivingTime"].ToString(),
                            Temperature = reader["Temperature"].ToString(),
                            Pressure = reader["Pressure"].ToString(),
                            PackingHopperTemp = reader["PackingHopperTemp"].ToString(),
                            ChillerTemp = reader["ChillerTemp"].ToString(),
                            Consistency = reader["Consistency"].ToString(),
                            NoOfPackets = reader["NoOfPackets"].ToString(),
                            RejectedPackets = reader["RejectedPackets"].ToString(),
                            FinalPackets = reader["FinalPackets"].ToString(),
                            
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
