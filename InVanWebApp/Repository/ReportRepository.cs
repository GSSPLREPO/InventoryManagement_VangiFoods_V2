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
                            TotalRecevingQuantiy=Convert.ToDouble(reader["TotalRecevingQuantiy"]),
                            TotalRejectedQuantity=Convert.ToDouble(reader["TotalRejectedQuantity"]),
                            RejectionNoteNo = reader["RejectionNoteNo"].ToString(),
                            InwardNumber = reader["InwardNumber"].ToString(),
                            ApprovedBy= reader["ApprovedBy"].ToString()
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

        /// Repository for Finished Goods Dispatch report
        /// Developed by  - Farheen 12 Jan'23

        public List<OutwardNoteItemDetailsBO> getFinishedGoodsReportData(DateTime fromDate, DateTime toDate, int LocationId, int itemId)
        {
            List<OutwardNoteItemDetailsBO> resultList = new List<OutwardNoteItemDetailsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("usp_rpt_GetRawMaterialReceived_Report", con);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@ItemID", LocationId);
                    cmd.Parameters.AddWithValue("@WearhouseId", itemId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new OutwardNoteItemDetailsBO()
                        {
                            SrNo = Convert.ToInt32(reader["SrNo"]),
                            OutwardNoteNumber = reader["GRN_Number"].ToString(),
                            OutwardDate = Convert.ToDateTime(reader["GRN_Date"]).ToString("dd/MM/yyyy hh:mm:ss"),
                            Item_Code = reader["Item_Code"].ToString(),
                            ItemName = reader["Item_Name"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(reader["Price_Per_unit"]),
                            DispatchQuantity = float.Parse(reader["DispatchQuantity"].ToString())
                            
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
