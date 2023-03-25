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

namespace InVanWebApp.Repository
{
    public class BatchPlanningRepository:IBatchPlanningRepository
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
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
                            BatchPlanningDocumentNo= reader["BatchPlanningDocumentNo"].ToString(),
                            WorkOrderNumber = reader["WorkOrderNumber"].ToString(),
                            SONumber = reader["SONumber"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            TotalBatchSize = Convert.ToDecimal(reader["TotalBatchSize"]),
                            TotalNoBatches = Convert.ToDecimal(reader["TotalNoBatches"])
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
                    //cmd.Parameters.AddWithValue("@PO_Id", model.PO_Id);
                    ////cmd.Parameters.AddWithValue("@PONumber", model.PONumber);
                    //cmd.Parameters.AddWithValue("@InwardNumber", model.InwardNumber);
                    //cmd.Parameters.AddWithValue("@InwardDate", model.InwardDate);
                    ////cmd.Parameters.AddWithValue("@InwardQuantities", model.InwardQuantities);
                    ////cmd.Parameters.AddWithValue("@BalanceQuantities", model.BalanceQuantities);
                    //cmd.Parameters.AddWithValue("@Signature", model.Signature);
                    //cmd.Parameters.AddWithValue("@ChallanNo", model.ChallanNo);
                    //cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    //cmd.ExecuteNonQuery();
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int InwardID = 0;
                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                        InwardID = Convert.ToInt32(dataReader["InwardNoteId"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.txtItemDetails);

                    List<BatchPlanning_DetailsBO> itemDetails = new List<BatchPlanning_DetailsBO>();

                    foreach (var item in data)
                    {
                        BatchPlanning_DetailsBO objItemDetails = new BatchPlanning_DetailsBO();
                        //objItemDetails.PO_ID = model.ID;
                        //objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(1).Value);
                        //objItemDetails.Item_Name = item.ElementAt(4).Value.ToString();
                        //objItemDetails.Item_Code = item.ElementAt(5).Value.ToString();
                        //objItemDetails.POQuantity = Convert.ToDecimal(item.ElementAt(6).Value);
                        //objItemDetails.ItemTaxValue = item.ElementAt(8).Value.ToString();
                        //objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(2).Value);
                        //objItemDetails.ItemUnit = (item.ElementAt(7).Value).ToString();
                        //objItemDetails.InwardQuantity = Convert.ToDouble(item.ElementAt(0).Value);
                        //objItemDetails.BalanceQuantity = Convert.ToDouble(item.ElementAt(3).Value);
                        //objItemDetails.CurrencyName = (item.ElementAt(9).Value).ToString();
                        objItemDetails.CreatedBy = model.CreatedBy;
                        objItemDetails.CreatedDate = Convert.ToDateTime(System.DateTime.Now);

                        itemDetails.Add(objItemDetails);
                    }

                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_BatchPlanningDetails_Insert", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        //cmdNew.Parameters.AddWithValue("@PurchaseOrderId", item.BatchPlanningId);
                        //cmdNew.Parameters.AddWithValue("@InwardNoteId", InwardID);
                        //cmdNew.Parameters.AddWithValue("@Item_ID", item.ItemId);
                        //cmdNew.Parameters.AddWithValue("@ItemName", item.Item_Name);
                        //cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                        //cmdNew.Parameters.AddWithValue("@POQuantity", item.POQuantity);
                        //cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                        //cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                        //cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        //cmdNew.Parameters.AddWithValue("@InwardQuantity", item.InwardQuantity);
                        //cmdNew.Parameters.AddWithValue("@BalanceQuantity", item.BalanceQuantity);
                        //cmdNew.Parameters.AddWithValue("@CurrencyName", item.CurrencyName);
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
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
            }
            return response;
        }

        #endregion

    }
}