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
using System.Web.Script.Serialization;

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
                            SO_No = reader["SO_No"].ToString(),
                            DraftFlag=Convert.ToBoolean(reader["DraftFlag"])
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
        public ResponseMessageBO Insert(PostProductionRejectionNoteBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {

                    SqlCommand cmd = new SqlCommand("usp_tbl_PostProdRN_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@PostProdRejectionNoteNo", model.PostProdRejectionNoteNo);
                    cmd.Parameters.AddWithValue("@PostProdRejectionNoteDate", model.PostProdRejectionNoteDate);
                    cmd.Parameters.AddWithValue("@PostProdRejectionType", model.PostProdRejectionType);
                    cmd.Parameters.AddWithValue("@DraftFlag", model.DraftFlag);
                    cmd.Parameters.AddWithValue("@FGS_ID", model.FGS_ID);
                    cmd.Parameters.AddWithValue("@SO_Id", model.SO_Id);
                    cmd.Parameters.AddWithValue("@SO_No", model.SO_No);
                    cmd.Parameters.AddWithValue("@WorkOrderNo", model.WorkOrderNo);
                    cmd.Parameters.AddWithValue("@Stage", model.Stage);
                    cmd.Parameters.AddWithValue("@WholeBatchRejection", model.WholeBatchRejection);
                    cmd.Parameters.AddWithValue("@BatchNumber", model.BatchNumber);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int PostProdRN_Id = 0;
                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                        PostProdRN_Id = Convert.ToInt32(dataReader["PostProdRNID"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    if (PostProdRN_Id != 0)
                    {
                        var data = json.Deserialize<Dictionary<string, string>[]>(model.txtItemDetails);

                        List<PostProductionRejectionNote_DetailsBO> itemDetails = new List<PostProductionRejectionNote_DetailsBO>();
                        foreach (var item in data)
                        {
                            PostProductionRejectionNote_DetailsBO objItemDetails = new PostProductionRejectionNote_DetailsBO();

                            objItemDetails.PostProdRejectionID = PostProdRN_Id;
                            objItemDetails.ItemCode = item.ElementAt(0).Value.ToString();
                            objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(1).Value);
                            objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                            objItemDetails.OrderedQuantity = Convert.ToDecimal(item.ElementAt(3).Value);
                            objItemDetails.TotalQuantity = Convert.ToDecimal(item.ElementAt(4).Value);
                            objItemDetails.RejectedQuantity = Convert.ToDecimal(item.ElementAt(5).Value);
                            objItemDetails.Remarks = item.ElementAt(6).Value.ToString();

                            itemDetails.Add(objItemDetails);
                        }

                        foreach (var item in itemDetails)
                        {
                            con.Open();
                            SqlCommand cmdNew = new SqlCommand("usp_tbl_PostProdRNDetails_Insert", con);
                            cmdNew.CommandType = CommandType.StoredProcedure;

                            cmdNew.Parameters.AddWithValue("@PostProdRejectionID", item.PostProdRejectionID);
                            cmdNew.Parameters.AddWithValue("@Item_ID", item.ItemId);
                            cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                            cmdNew.Parameters.AddWithValue("@Item_Code", item.ItemCode);
                            cmdNew.Parameters.AddWithValue("@OrderedQuantity", item.OrderedQuantity);
                            cmdNew.Parameters.AddWithValue("@TotalQuantity", item.TotalQuantity);
                            cmdNew.Parameters.AddWithValue("@RejectedQuantity", item.RejectedQuantity);
                            cmdNew.Parameters.AddWithValue("@Remarks", item.Remarks);
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
                log.Error(ex.Message, ex);
            }
            return response;
        }

        #endregion

        #region Update functions
        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PostProductionRejectionNoteBO GetById(int Id)
        {
            string purchaseOrderQuery = "SELECT * FROM PostProductionRejectionNote WHERE ID = @Id AND IsDeleted = 0";
            string purchaseOrderItemQuery = "SELECT * FROM PostProductionRejectionNote_Details WHERE PostProdRejectionID = @Id AND IsDeleted = 0";
            PostProductionRejectionNoteBO result = new PostProductionRejectionNoteBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    result = con.Query<PostProductionRejectionNoteBO>(purchaseOrderQuery, new { @Id = Id }).FirstOrDefault();
                    var purchaseOrderList = con.Query<PostProductionRejectionNote_DetailsBO>(purchaseOrderItemQuery, new { @Id = Id }).ToList();
                    result.note_DetailsBOs = purchaseOrderList;
                }

            }
            catch (Exception ex)
            {
                result = null;
                log.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(PostProductionRejectionNoteBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PostProdRN_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@PostProdRejectionNoteNo", model.PostProdRejectionNoteNo);
                    cmd.Parameters.AddWithValue("@PostProdRejectionNoteDate", model.PostProdRejectionNoteDate);
                    cmd.Parameters.AddWithValue("@PostProdRejectionType", model.PostProdRejectionType);
                    cmd.Parameters.AddWithValue("@DraftFlag", model.DraftFlag);
                    cmd.Parameters.AddWithValue("@FGS_ID", model.FGS_ID);
                    cmd.Parameters.AddWithValue("@SO_Id", model.SO_Id);
                    cmd.Parameters.AddWithValue("@SO_No", model.SO_No);
                    cmd.Parameters.AddWithValue("@WorkOrderNo", model.WorkOrderNo);
                    cmd.Parameters.AddWithValue("@Stage", model.Stage);
                    cmd.Parameters.AddWithValue("@WholeBatchRejection", model.WholeBatchRejection);
                    cmd.Parameters.AddWithValue("@BatchNumber", model.BatchNumber);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedBy);


                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.txtItemDetails);

                    List<PostProductionRejectionNote_DetailsBO> itemDetails = new List<PostProductionRejectionNote_DetailsBO>();

                    foreach (var item in data)
                    {
                        PostProductionRejectionNote_DetailsBO objItemDetails = new PostProductionRejectionNote_DetailsBO();
                        objItemDetails.PostProdRejectionID = model.ID;
                        objItemDetails.ItemCode = item.ElementAt(0).Value.ToString();
                        objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(1).Value);
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.OrderedQuantity = Convert.ToDecimal(item.ElementAt(3).Value);
                        objItemDetails.TotalQuantity = Convert.ToDecimal(item.ElementAt(4).Value);
                        objItemDetails.RejectedQuantity = Convert.ToDecimal(item.ElementAt(5).Value);
                        objItemDetails.Remarks = item.ElementAt(6).Value.ToString();
                        objItemDetails.LastModifiedBy = model.LastModifiedBy;

                        itemDetails.Add(objItemDetails);
                    }

                    var count = itemDetails.Count;
                    var i = 1;
                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_PostProdRNDetails_Update", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@PostProdRejectionID", item.PostProdRejectionID);
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.ItemId);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.ItemCode);
                        cmdNew.Parameters.AddWithValue("@OrderedQuantity", item.OrderedQuantity);
                        cmdNew.Parameters.AddWithValue("@TotalQuantity", item.TotalQuantity);
                        cmdNew.Parameters.AddWithValue("@RejectedQuantity", item.RejectedQuantity);
                        cmdNew.Parameters.AddWithValue("@Remarks", item.Remarks);
                        cmdNew.Parameters.AddWithValue("@LastModifiedBy", item.LastModifiedBy);
                        cmdNew.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                        
                        if (count == 1)
                            cmdNew.Parameters.AddWithValue("@OneItemIdentifier", 1);
                        else
                        {
                            cmdNew.Parameters.AddWithValue("@OneItemIdentifier", 0);
                            cmdNew.Parameters.AddWithValue("@flagCheck", i);
                        }
                        i++;
                        SqlDataReader dataReaderNew = cmdNew.ExecuteReader();

                        while (dataReaderNew.Read())
                        {
                            response.Status = Convert.ToBoolean(dataReaderNew["Status"]);
                        }
                        con.Close();
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
        public void Delete(int ID, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PostProdRN_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
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
                    cmd.Parameters.AddWithValue("@FSID", FGSID);
                    cmd.Parameters.AddWithValue("@Stage", FGSStage);
                    cmd.Parameters.AddWithValue("@Type", RNType);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        result = new FinishedGoodSeriesBO()
                        {
                            BatchNo = reader["BatchNo"].ToString(),
                            SalesOrderId = reader["SalesOrderId"] is DBNull ? 0 : Convert.ToInt32(reader["SalesOrderId"]),
                            SONo = reader["SONumber"].ToString(),
                            ItemId = reader["ItemId"] is DBNull ? 0 : Convert.ToInt32(reader["ItemId"]),
                            ProductName = reader["ProductName"].ToString(),
                            Item_Code = reader["Item_Code"].ToString(),
                            OrderQty = reader["OrderQty"] is DBNull ? 0 : Convert.ToDecimal(reader["OrderQty"]),
                            QuantityInKG = reader["QuantityInKG"] is DBNull ? 0 : Convert.ToDouble(reader["QuantityInKG"])
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
                            BatchNo = reader["BatchNo"].ToString(),
                            WorkOrderAndBN = reader["WorkOrderNo"].ToString() + " (" + reader["BatchNo"].ToString() + ")",
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