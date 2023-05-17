using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Dapper;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Common;
using InVanWebApp_BO;
using log4net;

namespace InVanWebApp.Repository
{
    public class StockAdjustmentRepository : IStockAdjustmentRepository
    {
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(StockAdjustmentRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of Stock Adjustment data.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StockAdjustmentBO> GetAll()
        {
            List<StockAdjustmentBO> resultList = new List<StockAdjustmentBO>();
            string stockAdjustment = "select SA.ID,SA.DocumentNo, SA.CreatedDate, SA.LocationName, SA.Remarks,UD.UserName as UserName from StockAdjustment SA inner join Users UD on UD.UserId=SA.CreatedBy where SA.IsDeleted=0 and UD.IsDeleted=0 order by 1 desc";

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    resultList = con.Query<StockAdjustmentBO>(stockAdjustment).ToList();
                }
            }
            catch (Exception ex)
            {
                //resultList = null;
                log.Error(ex.Message, ex);
            }
            return resultList;
        }

        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(StockAdjustmentBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_StockAdjustment_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DocumentNo", model.DocumentNo);
                    cmd.Parameters.AddWithValue("@DocumentDate", model.DocumentDate);
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    model.CreatedDate = Convert.ToDateTime(System.DateTime.Now);
                    con.Open();

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int StockAdjustmentId = 0;

                    while (dataReader.Read())
                    {
                        StockAdjustmentId = Convert.ToInt32(dataReader["ID"]);
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    if (StockAdjustmentId != 0)
                    {
                        var json = new JavaScriptSerializer();
                        var data = json.Deserialize<Dictionary<string, string>[]>(model.TxtItemDetails);

                        List<StockAdjustmentDetailsBO> itemDetails = new List<StockAdjustmentDetailsBO>();

                        foreach (var item in data)
                        {
                            StockAdjustmentDetailsBO objItemDetails = new StockAdjustmentDetailsBO();
                            objItemDetails.StockAdjustmentID = StockAdjustmentId;
                            objItemDetails.Item_Code = item.ElementAt(0).Value.ToString();
                            objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(1).Value);
                            objItemDetails.Item_Name = item.ElementAt(2).Value.ToString();
                            objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(3).Value);
                            objItemDetails.CurrencyName = item.ElementAt(4).Value.ToString();
                            objItemDetails.AvailableStock = Convert.ToDecimal(item.ElementAt(5).Value);
                            objItemDetails.ItemUnit = item.ElementAt(6).Value.ToString();
                            objItemDetails.PhysicalStock = Convert.ToDecimal(item.ElementAt(7).Value);
                            objItemDetails.DifferenceInStock = Convert.ToDecimal(item.ElementAt(8).Value);
                            objItemDetails.TransferPrice = Convert.ToDecimal(item.ElementAt(9).Value);
                            objItemDetails.Remarks = item.ElementAt(10).Value.ToString();
                            objItemDetails.CreatedBy = model.CreatedBy;
                            itemDetails.Add(objItemDetails);
                        }

                        foreach (var item in itemDetails)
                        {
                            con.Open();
                            SqlCommand cmdNew = new SqlCommand("usp_tbl_StockAdjustmentDetails_Insert", con);
                            cmdNew.CommandType = CommandType.StoredProcedure;

                            cmdNew.Parameters.AddWithValue("@StockAdjustment_ID", item.StockAdjustmentID);
                            cmdNew.Parameters.AddWithValue("@ItemId", item.ItemId);
                            cmdNew.Parameters.AddWithValue("@Item_Name", item.Item_Name);
                            cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                            cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                            cmdNew.Parameters.AddWithValue("@CurrencyName", item.CurrencyName);
                            cmdNew.Parameters.AddWithValue("@AvailableStock", item.AvailableStock);
                            cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                            cmdNew.Parameters.AddWithValue("@PhysicalStock", item.PhysicalStock);
                            cmdNew.Parameters.AddWithValue("@DifferenceInStock", item.DifferenceInStock);
                            cmdNew.Parameters.AddWithValue("@TransferPrice", item.TransferPrice);
                            cmdNew.Parameters.AddWithValue("@Remarks", item.Remarks);
                            cmdNew.Parameters.AddWithValue("@LocationId", model.LocationId);
                            cmdNew.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                            cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));

                            SqlDataReader dataReaderNew = cmdNew.ExecuteReader();

                            while (dataReaderNew.Read())
                            {
                                response.Status = Convert.ToBoolean(dataReaderNew["Status"]);
                            }
                            con.Close();
                        }
                    }
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
        /// Delete record by ID
        /// </summary>
        /// <param name="ID"></param>
        public ResponseMessageBO Delete(int Id, int userId)
        {
            ResponseMessageBO responseMessage = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_StockAdjustment_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", Id);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", userId);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    responseMessage.Status = true;
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                responseMessage.Status = false;
                log.Error(ex.Message, ex);
            }
            return responseMessage;
        }
        #endregion

        #region This function is for pdf export/view
        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID and for downloading pdf
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        public StockAdjustmentBO GetById(int ID)
        {
            StockAdjustmentBO result = new StockAdjustmentBO();
            try
            {
                string stockAdjustmentQuery = "select SA.ID,SA.DocumentNo, SA.DocumentDate, SA.LocationName, SA.Remarks,UD.UserName as UserName, Remarks from StockAdjustment SA inner join Users UD on UD.UserId=SA.CreatedBy where SA.IsDeleted=0 and SA.ID = @ID and UD.IsDeleted=0"; //"SELECT * FROM StockAdjustment WHERE ID = @ID AND IsDeleted = 0";
                string stockAdjustmentItemQuery = "SELECT * FROM StockAdjustmentDetails WHERE StockAdjustmentID = @ID AND IsDeleted = 0";
                using (SqlConnection con = new SqlConnection(connString))
                {
                    result = con.Query<StockAdjustmentBO>(stockAdjustmentQuery, new { @ID = ID }).FirstOrDefault();
                    var stockAdjustmentItemList = con.Query<StockAdjustmentDetailsBO>(stockAdjustmentItemQuery, new { @ID = ID }).ToList();
                    result.stockAdjustmentDetails = stockAdjustmentItemList;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return result;
        }

        #endregion

        #region Fetch Location stock details for adjustment
        public IEnumerable<StockAdjustmentDetailsBO> GetLocationStocksDetailsById(int LocationId,int ItemId)
        {
            List<StockAdjustmentDetailsBO> resultList = new List<StockAdjustmentDetailsBO>();

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {

                    SqlCommand cmd1 = new SqlCommand("usp_tbl_LocationStockDetailsForStockAdjustment_GetByID", con);
                    cmd1.Parameters.AddWithValue("@Location_Id", LocationId);
                    cmd1.Parameters.AddWithValue("@Item_Id", ItemId);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader2 = cmd1.ExecuteReader();

                    while (dataReader2.Read())
                    {
                        var result = new StockAdjustmentDetailsBO()
                        {
                            ID = Convert.ToInt32(dataReader2["ID"]),
                            ItemId = Convert.ToInt32(dataReader2["ItemId"]),
                            Item_Code = dataReader2["Item_Code"].ToString(),
                            Item_Name = dataReader2["Item_Name"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(dataReader2["ItemUnitPrice"]),
                            ItemUnit = dataReader2["ItemUnit"].ToString(),
                            AvailableStock = Convert.ToDecimal(dataReader2["AvailableStock"]),
                            CurrencyName = dataReader2["CurrencyName"].ToString()
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

        #region Fetch Item list by location id.
        public IEnumerable<ItemBO> GetItemListByLocationId(int LocationId) {
            List<ItemBO> resultList = new List<ItemBO>();
            string itemQuery = "select distinct(LS.ItemId) as ID, (Select I.Item_Name from Item I where I.IsDeleted=0 and I.ID=LS.ItemId) as Item_Name from LocationWiseStock LS where LS.IsDeleted = 0 and LS.LocationID = @ID";

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    resultList = con.Query<ItemBO>(itemQuery,new { @ID=LocationId}).ToList();
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