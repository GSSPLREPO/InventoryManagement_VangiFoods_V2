using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using InVanWebApp.Common;
using System.IO;
using Dapper;

namespace InVanWebApp.Repository
{
    public class FinishedGoodSeriesRepository : IFinishedGoodSeriesRepository
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(FinishedGoodSeriesRepository));

        #region  Bind grid for datatable
        /// <summary>
        /// Date: 22 March'23
        /// Snehal: This function is for fecthing list of FinishedGoodSeries.
        /// </summary>
        /// <returns></returns>
        public List<FinishedGoodSeriesBO> GetAllFinishedGoodSeriesList()
        {
            List<FinishedGoodSeriesBO> finishedGoodSeriesList = new List<FinishedGoodSeriesBO>();
            try
            {
                //if (fromDate == null && toDate == null)
                //{
                //    fromDate = DateTime.Today;
                //    toDate = DateTime.Today;
                //}
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_FinishGoodSeries_GetAll", con);
                    //cmd.Parameters.AddWithValue("@flagdate", flagdate);
                    //cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    //cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var finishedGoodSeries = new FinishedGoodSeriesBO()
                        {
                            FGSID = Convert.ToInt32(reader["FGSID"]),
                            ProductName = reader["ProductName"].ToString(),
                            PackageSize = reader["PackageSize"].ToString(),
                            MfgDate = Convert.ToDateTime(reader["MfgDate"]),
                            NoOfCartonBox = Convert.ToInt32(reader["NoOfCartonBox"]),
                            QuantityInKG = Convert.ToDouble(reader["QuantityInKG"]),
                            BatchNo = reader["BatchNo"].ToString(),
                            SalesOrderId = Convert.ToInt32(reader["SalesOrderId"]),
                            SONo = reader["SONo"].ToString(),
                            Packaging = reader["Packaging"].ToString(),
                            Sealing = reader["Sealing"].ToString(),
                            Labelling = reader["Labeling"].ToString(),
                            QCCheck = reader["QCCheck"].ToString(),
                            ActualPackets = reader["ActualPackets"].ToString(),
                            ExpectedPackets = reader["ExpectedPackets"].ToString(),
                            ExpectedYield = Convert.ToDecimal(reader["ExpectedYield"]),
                            ActualYield = Convert.ToDecimal(reader["ActualYield"]),
                            WorkOrderNo = reader["WorkOrderNo"].ToString(),
                            Remarks = reader["Remarks"].ToString(),
                        };
                        finishedGoodSeriesList.Add(finishedGoodSeries);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return finishedGoodSeriesList;
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 22-03-23
        /// Snehal: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(FinishedGoodSeriesBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_FinishGoodSeries_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    cmd.Parameters.AddWithValue("@PackageSize", model.PackageSize);
                    cmd.Parameters.AddWithValue("@MfgDate", model.MfgDate);
                    cmd.Parameters.AddWithValue("@NoOfCartonBox", model.NoOfCartonBox);
                    cmd.Parameters.AddWithValue("@QuantityInKG", model.QuantityInKG);
                    cmd.Parameters.AddWithValue("@BatchNo", model.BatchNo);
                    cmd.Parameters.AddWithValue("@SalesOrderId", model.SalesOrderId);
                    cmd.Parameters.AddWithValue("@SONo", model.SONo);
                    cmd.Parameters.AddWithValue("@Packaging", model.Packaging);
                    cmd.Parameters.AddWithValue("@Sealing", model.Sealing);
                    cmd.Parameters.AddWithValue("@Labeling", model.Labelling);
                    cmd.Parameters.AddWithValue("@QCCheck", model.QCCheck);
                    cmd.Parameters.AddWithValue("@ActualPackets", model.ActualPackets);
                    cmd.Parameters.AddWithValue("@ExpectedPackets", model.ExpectedPackets);
                    cmd.Parameters.AddWithValue("@ExpectedYield", model.ExpectedYield);
                    cmd.Parameters.AddWithValue("@ActualYield", model.ActualYield);
                    cmd.Parameters.AddWithValue("@WorkOrderNo", model.WorkOrderNo);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
                };
            }
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
            }
            return response;
        }
        #endregion

        #region Update functions

        /// <summary>
        /// Date: 22-03-23
        /// Snehal: This function is for fetch data for editing by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public FinishedGoodSeriesBO GetById(int Id)
        {
            var FinishedGoodSeriesBO = new FinishedGoodSeriesBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_FinishGoodSeries_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        FinishedGoodSeriesBO = new FinishedGoodSeriesBO()
                        {
                            FGSID = Convert.ToInt32(reader["FGSID"]),
                            ProductName = reader["ProductName"].ToString(),
                            PackageSize = reader["PackageSize"].ToString(),
                            MfgDate = Convert.ToDateTime(reader["MfgDate"]),
                            NoOfCartonBox = Convert.ToInt32(reader["NoOfCartonBox"]),
                            QuantityInKG = Convert.ToDouble(reader["QuantityInKG"]),
                            BatchNo = reader["BatchNo"].ToString(),
                            SalesOrderId = Convert.ToInt32(reader["SalesOrderId"]),
                            SONo = reader["SONo"].ToString(),
                            Packaging = reader["Packaging"].ToString(),
                            Sealing = reader["Sealing"].ToString(),
                            Labelling = reader["Labeling"].ToString(),
                            QCCheck = reader["QCCheck"].ToString(),
                            ActualPackets = reader["ActualPackets"].ToString(),
                            ExpectedPackets = reader["ExpectedPackets"].ToString(),
                            ExpectedYield = Convert.ToDecimal(reader["ExpectedYield"]),
                            ActualYield = Convert.ToDecimal(reader["ActualYield"]),
                            WorkOrderNo = reader["WorkOrderNo"].ToString(),
                            Remarks = reader["Remarks"].ToString(),
                        };
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

            return FinishedGoodSeriesBO;
        }

        /// <summary>
        /// Date: 22-03-23
        /// Snehal: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(FinishedGoodSeriesBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_FinishGoodSeries_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FGSID", model.FGSID);
                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    cmd.Parameters.AddWithValue("@PackageSize", model.PackageSize);
                    cmd.Parameters.AddWithValue("@MfgDate", model.MfgDate);
                    cmd.Parameters.AddWithValue("@NoOfCartonBox", model.NoOfCartonBox);
                    cmd.Parameters.AddWithValue("@QuantityInKG", model.QuantityInKG);
                    cmd.Parameters.AddWithValue("@BatchNo", model.BatchNo);
                    cmd.Parameters.AddWithValue("@SalesOrderId", model.SalesOrderId);
                    cmd.Parameters.AddWithValue("@SONo", model.SONo);
                    cmd.Parameters.AddWithValue("@Packaging", model.Packaging);
                    cmd.Parameters.AddWithValue("@Sealing", model.Sealing);
                    cmd.Parameters.AddWithValue("@Labeling", model.Labelling);
                    cmd.Parameters.AddWithValue("@QCCheck", model.QCCheck);
                    cmd.Parameters.AddWithValue("@ActualPackets", model.ActualPackets);
                    cmd.Parameters.AddWithValue("@ExpectedPackets", model.ExpectedPackets);
                    cmd.Parameters.AddWithValue("@ExpectedYield", model.ExpectedYield);
                    cmd.Parameters.AddWithValue("@ActualYield", model.ActualYield);
                    cmd.Parameters.AddWithValue("@WorkOrderNo", model.WorkOrderNo);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedBy);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));

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

        #region Delete function
        /// <summary>
        /// /// Date: 22 Narch'23
        /// Snehal: This function is for delete record of FinishedGoodSeries using it's Id
        /// /// </summary>
        /// /// <param name="Id"></param>
        /// /// <param name="userId"></param>
        /// <returns></returns>
        public void Delete(int Id, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_FinishGoodSeries_Delete]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", userId);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                };
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        #endregion

        #region Functions for dropdown binding
        public IEnumerable<SalesOrderBO> GetSONUmberForDropDown()
        {
            List<SalesOrderBO> salesOrderList = new List<SalesOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SONumber_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var salesOrder = new SalesOrderBO()
                        {
                            SalesOrderId = Convert.ToInt32(reader["SalesOrderId"]),
                            SONo = reader["SONo"].ToString(),
                            WorkOrderNo = reader["WorkOrderNo"].ToString()
                        };
                        salesOrderList.Add(salesOrder);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return salesOrderList;
        }


        #endregion

        #region Function for binding data to get Work Order NO
        public IEnumerable<SalesOrderBO> GetBindWorkOrderNo(int id)
        {
            List<SalesOrderBO> resultList = new List<SalesOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SalesOrder_GetAll_WorkOrderNo", con);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new SalesOrderBO()
                        {
                            WorkOrderNo = dataReader["WorkOrderNo"].ToString()
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                };
            }
            catch (Exception ex)
            {
                resultList = null;
                log.Error(ex.Message, ex);
            }
            return resultList;
        }
        #endregion

    }
}