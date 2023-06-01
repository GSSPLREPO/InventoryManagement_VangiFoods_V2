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
using System.Web.Script.Serialization;
using Dapper;
using InVanWebApp.Common;

namespace InVanWebApp.Repository
{
    public class BatchPlanningRepository : IBatchPlanningRepository
    {
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(BatchPlanningRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of organisation.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BatchPlanningMasterBO> GetAll()
        {
            List<BatchPlanningMasterBO> resultList = new List<BatchPlanningMasterBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_BatchPlanning_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new BatchPlanningMasterBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            BatchPlanningDocumentNo = reader["BatchPlanningDocumentNo"].ToString(),
                            WorkOrderNumber = reader["WorkOrderNumber"].ToString(),
                            SONumber = reader["SONumber"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            TotalBatchSize = float.Parse(reader["TotalBatchSize"].ToString()),
                            TotalNoBatches = float.Parse(reader["TotalNoBatches"].ToString())
                            //TotalBatchSize = Convert.ToDecimal(reader["TotalBatchSize"]),
                            //TotalNoBatches = Convert.ToDecimal(reader["TotalNoBatches"])
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

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(BatchPlanningMasterBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_BatchPlanning_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BatchPlanningDocumentNo", model.BatchPlanningDocumentNo);
                    cmd.Parameters.AddWithValue("@SO_Id", model.SO_Id);
                    cmd.Parameters.AddWithValue("@SONumber", model.SONumber);
                    cmd.Parameters.AddWithValue("@WorkOrderNumber", model.WorkOrderNumber);
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    cmd.Parameters.AddWithValue("@ItemId", model.ProductId);
                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    cmd.Parameters.AddWithValue("@PackingSize", model.PackingSize);
                    cmd.Parameters.AddWithValue("@PackingSizeUnit", model.PackingSizeUnit);
                    cmd.Parameters.AddWithValue("@TotalRawMaterialYeild", model.TotalRawMaterialYeild);
                    cmd.Parameters.AddWithValue("@PackingType", model.PackingType);
                    cmd.Parameters.AddWithValue("@OrderQuantity", model.OrderQuantity);
                    cmd.Parameters.AddWithValue("@OrderQuantityUnit", model.OrderQuantityUnit);
                    cmd.Parameters.AddWithValue("@RequiredQuantityInKG", model.RequiredQuantityInKG);
                    cmd.Parameters.AddWithValue("@TotalBatchSize", model.TotalBatchSize);
                    cmd.Parameters.AddWithValue("@TotalNoBatches", model.TotalNoBatches);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int BatchPlanningId = 0;
                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                        BatchPlanningId = Convert.ToInt32(dataReader["BatchPlanID"]);
                    }
                    con.Close();

                    if (BatchPlanningId != 0)
                    {
                        var json = new JavaScriptSerializer();
                        var data = json.Deserialize<Dictionary<string, string>[]>(model.txtItemDetails);

                        List<BatchPlanning_DetailsBO> itemDetails = new List<BatchPlanning_DetailsBO>();
                        foreach (var item in data)
                        {
                            BatchPlanning_DetailsBO objItemDetails = new BatchPlanning_DetailsBO();
                            objItemDetails.BatchPlanningId = BatchPlanningId;
                            objItemDetails.ItemCode = item.ElementAt(0).Value.ToString();
                            objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(1).Value);
                            objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                            objItemDetails.QuantityPercentage = float.Parse(item.ElementAt(3).Value);
                            objItemDetails.BatchSize = float.Parse(item.ElementAt(4).Value);
                            objItemDetails.TotalQuantityInBatch = float.Parse(item.ElementAt(5).Value);
                            objItemDetails.YieldPercentage = float.Parse(item.ElementAt(6).Value);
                            objItemDetails.ActualRequirement = float.Parse(item.ElementAt(7).Value);
                            objItemDetails.StockInHand = float.Parse(item.ElementAt(8).Value);
                            objItemDetails.ToBeProcured = float.Parse(item.ElementAt(9).Value);

                            itemDetails.Add(objItemDetails);
                        }

                        foreach (var item in itemDetails)
                        {
                            con.Open();
                            SqlCommand cmdNew = new SqlCommand("usp_tbl_BatchPlanningDetails_Insert", con);
                            cmdNew.CommandType = CommandType.StoredProcedure;
                            cmdNew.Parameters.AddWithValue("@BatchPlanningId", item.BatchPlanningId);
                            cmdNew.Parameters.AddWithValue("@TotalNoBatches", model.TotalNoBatches);
                            cmdNew.Parameters.AddWithValue("@SO_Id", model.SO_Id);
                            cmdNew.Parameters.AddWithValue("@WorkOrderNumber", model.WorkOrderNumber);
                            cmdNew.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                            cmdNew.Parameters.AddWithValue("@ItemId", item.ItemId);
                            cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                            cmdNew.Parameters.AddWithValue("@QuantityPercentage", item.QuantityPercentage);
                            cmdNew.Parameters.AddWithValue("@BatchSize", item.BatchSize);
                            cmdNew.Parameters.AddWithValue("@TotalQuantityInBatch", item.TotalQuantityInBatch);
                            cmdNew.Parameters.AddWithValue("@YieldPercentage", item.YieldPercentage);
                            cmdNew.Parameters.AddWithValue("@ActualRequirement", item.ActualRequirement);
                            cmdNew.Parameters.AddWithValue("@StockInHand", item.StockInHand);
                            cmdNew.Parameters.AddWithValue("@ToBeProcured", item.ToBeProcured);
                            cmdNew.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
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

        #region Bind Work order no
        public SalesOrderBO GetWorkOrderNumber(int id)
        {
            string purchaseOrderQuery = "SELECT WorkOrderNo,LocationId,LocationName FROM SalesOrder WHERE SalesOrderId = @Id AND IsDeleted = 0";
            string itemDetails = "Select Item_ID,ItemName from SalesOrderItemsDetails where SalesOrderId=@Id and IsDeleted=0";

            SalesOrderBO result = new SalesOrderBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    result = con.Query<SalesOrderBO>(purchaseOrderQuery, new { @Id = id }).FirstOrDefault();
                    var resultList = con.Query<SalesOrderItemsDetail>(itemDetails, new { @Id = id }).ToList();

                    result.salesOrderItemsDetails = resultList;
                }
                var flag = BatchPlanningIsDone(id);
                result.IsBatchDone = flag;
            }
            catch (Exception ex)
            {
                result = null;
                log.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Created by: Farheen
        /// Description: This function is for finding whether the Batch planning of the passed SO_Id is done.
        /// </summary>
        /// <param name="SOid"></param>
        /// <returns></returns>
        public int BatchPlanningIsDone(int SOid)
        {
            string Query = "Select Count(*) as SOCount from BatchPlanningMaster where IsDeleted=0 and SO_Id=@Id";
            string Query1 = "Select Count(*) as ItemCount from SalesOrderItemsDetails where IsDeleted=0 and SalesOrderId=@Id";

            SalesOrderItemsDetail result = new SalesOrderItemsDetail();
            BatchPlanningMasterBO result1 = new BatchPlanningMasterBO();
            int flagResult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    result = con.Query<SalesOrderItemsDetail>(Query1, new { @Id = SOid }).FirstOrDefault();
                    result1 = con.Query<BatchPlanningMasterBO>(Query, new { @Id = SOid }).FirstOrDefault();

                }

                if (result.ItemCount == result1.SOCount)
                    flagResult = 1;
                else
                    flagResult = 0;

            }
            catch (Exception ex)
            {
                flagResult = 0;
                log.Error(ex.Message, ex);
            }
            return flagResult;
        }
        #endregion

        #region Fetch Recipe by product id
        public IEnumerable<Recipe_DetailsBO> GetRecipe(int id, int locationId, int SOId)
        {
            List<Recipe_DetailsBO> resultList = new List<Recipe_DetailsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RecipeDetailsForBatchPlanning_GetbyProductId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductId", id);
                    cmd.Parameters.AddWithValue("@LocationId", locationId);
                    cmd.Parameters.AddWithValue("@SOId", SOId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new Recipe_DetailsBO()
                        {
                            RecipeIngredientsDetailID = Convert.ToInt32(reader["RecipeIngredientsDetailID"]),
                            RecipeID = Convert.ToInt32(reader["RecipeID"]),
                            RecipeName = reader["RecipeName"].ToString(),
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            ItemCode = reader["ItemCode"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            RoundedRatio = Convert.ToDecimal(reader["RoundedRatio"]),
                            BatchSize = float.Parse(reader["BatchSize"].ToString()),
                            TotalBatches = Convert.ToDecimal(reader["TotalBatches"]),
                            Yield = Convert.ToDecimal(reader["Yield"]),
                            ActualRequirement = Convert.ToDecimal(reader["ActualRequirement"]),
                            StockInHand = Convert.ToDecimal(reader["StockInHand"]),
                            ToBeProcured = Convert.ToDecimal(reader["ToBeProcured"]),
                            SalesOrderQty = Convert.ToDecimal(reader["OrderedQty"]),
                            UnitName =Convert.ToString(reader["ItemUnit"])  //Rahul added 01-06-23.
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

        #region Delete function

        /// <summary>
        /// Delete record by ID
        /// </summary>
        /// <param name="ID"></param>
        public void Delete(int ID, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_BatchPlanningOrder_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BatchID", ID);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", userId);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
        #endregion

        #region View function
        /// <summary>
        /// Rahul: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public BatchPlanningMasterBO GetById(int Id)
        {
            string purchaseOrderQuery = "SELECT * FROM BatchPlanningMaster WHERE ID = @Id AND IsDeleted = 0";
            string purchaseOrderItemQuery = "SELECT * FROM BatchPlanning_Details WHERE BatchPlanningId= @Id AND IsDeleted = 0";
            BatchPlanningMasterBO result = new BatchPlanningMasterBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    result = con.Query<BatchPlanningMasterBO>(purchaseOrderQuery, new { @Id = Id }).FirstOrDefault();
                    var purchaseOrderList = con.Query<BatchPlanning_DetailsBO>(purchaseOrderItemQuery, new { @Id = Id }).ToList();
                    result.batchPlanningItemDetails = purchaseOrderList;
                }

            }
            catch (Exception ex)
            {
                result = null;
                log.Error(ex.Message, ex);
            }
            return result;
        }
        #endregion
    }
}