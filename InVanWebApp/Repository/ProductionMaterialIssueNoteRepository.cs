using Dapper;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace InVanWebApp.Repository
{
    public class ProductionMaterialIssueNoteRepository : IProductionMaterialIssueNoteRepository
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(IProductionMaterialIssueNoteRepository));

        #region  Bind grid 
        /// <summary>
        /// Rahul: This function is for fecthing list of Production Material Issue Note data.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductionMaterialIssueNoteBO> GetAll()
        {
            List<ProductionMaterialIssueNoteBO> resultList = new List<ProductionMaterialIssueNoteBO>();
            string stockAdjustment = "Select * from ProductionMaterialIssueNote where IsDeleted=0";

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    resultList = con.Query<ProductionMaterialIssueNoteBO>(stockAdjustment).ToList();
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
        /// Rahul: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(ProductionMaterialIssueNoteBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ProductionMaterialIssueNote_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IssueNoteNo", model.ProductionMaterialIssueNoteNo);
                    cmd.Parameters.AddWithValue("@IssueNoteDate", model.ProductionMaterialIssueNoteDate);
                    cmd.Parameters.AddWithValue("@IssueBy", model.IssueBy);
                    cmd.Parameters.AddWithValue("@IssueByName", model.IssueByName);
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    cmd.Parameters.AddWithValue("@Purpose", model.Purpose);
                    cmd.Parameters.AddWithValue("@WorkOrderNumber", model.WorkOrderNumber);
                    cmd.Parameters.AddWithValue("@QCNumber", model.QCNumber);
                    cmd.Parameters.AddWithValue("@SONumber", model.SONumber);
                    cmd.Parameters.AddWithValue("@OtherPurpose", model.OtherPurpose);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int IssueNote_Id = 0;

                    while (dataReader.Read())
                    {
                        IssueNote_Id = Convert.ToInt32(dataReader["ID"]);
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    if (IssueNote_Id != 0)
                    {
                        var json = new JavaScriptSerializer();
                        var data = json.Deserialize<Dictionary<string, string>[]>(model.txtItemDetails);

                        List<ProductionMaterialIssueNoteDetailsBO> itemDetails = new List<ProductionMaterialIssueNoteDetailsBO>();

                        foreach (var item in data)
                        {
                            ProductionMaterialIssueNoteDetailsBO objItemDetails = new ProductionMaterialIssueNoteDetailsBO();
                            objItemDetails.IssueNoteId = IssueNote_Id;
                            objItemDetails.Item_Code = item.ElementAt(0).Value.ToString();
                            objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(1).Value);
                            objItemDetails.Item_Name = item.ElementAt(2).Value.ToString();
                            objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(3).Value);
                            objItemDetails.CurrencyName = item.ElementAt(4).Value.ToString();
                            objItemDetails.AvailableStockBeforeIssue = Convert.ToDouble(item.ElementAt(5).Value);
                            objItemDetails.ItemUnit = item.ElementAt(6).Value.ToString();
                            objItemDetails.QuantityRequested = Convert.ToDouble(item.ElementAt(7).Value);
                            objItemDetails.StockAfterIssuing = Convert.ToDouble(item.ElementAt(8).Value);
                            objItemDetails.QuantityIssued = Convert.ToDouble(item.ElementAt(9).Value);
                            objItemDetails.Description = item.ElementAt(10).Value.ToString();
                            objItemDetails.CreatedBy = model.CreatedBy;
                            itemDetails.Add(objItemDetails);
                        }

                        foreach (var item in itemDetails)
                        {
                            con.Open();
                            SqlCommand cmdNew = new SqlCommand("usp_tbl_ProductionMaterialIssueNoteDetails_Insert", con);
                            cmdNew.CommandType = CommandType.StoredProcedure;

                            cmdNew.Parameters.AddWithValue("@IssueNoteId", item.IssueNoteId);
                            cmdNew.Parameters.AddWithValue("@ItemId", item.ItemId);
                            cmdNew.Parameters.AddWithValue("@Item_Name", item.Item_Name);
                            cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                            cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                            cmdNew.Parameters.AddWithValue("@CurrencyName", item.CurrencyName);
                            cmdNew.Parameters.AddWithValue("@QuantityRequested", item.QuantityRequested);
                            cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                            cmdNew.Parameters.AddWithValue("@AvailableStockBeforeIssue", item.AvailableStockBeforeIssue);
                            cmdNew.Parameters.AddWithValue("@StockAfterIssuing", item.StockAfterIssuing);
                            cmdNew.Parameters.AddWithValue("@QuantityIssued", item.QuantityIssued);
                            cmdNew.Parameters.AddWithValue("@Description", item.Description);
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

        #region This function is for pdf export/view
        /// <summary>
        /// Rahul: This function is for fetch data for editing by ID and for downloading pdf
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ProductionMaterialIssueNoteBO GetById(int ID)
        {
            ProductionMaterialIssueNoteBO result = new ProductionMaterialIssueNoteBO();
            try
            {
                string stringQuery = "Select * from ProductionMaterialIssueNote where IsDeleted=0 and ID=@ID";
                string stringItemQuery = "Select * from ProductionMaterialIssueNoteDetails where IsDeleted=0 and IssueNoteId=@ID";
                using (SqlConnection con = new SqlConnection(connString))
                {
                    result = con.Query<ProductionMaterialIssueNoteBO>(stringQuery, new { @ID = ID }).FirstOrDefault();
                    var ItemList = con.Query<ProductionMaterialIssueNoteDetailsBO>(stringItemQuery, new { @ID = ID }).ToList();
                    result.ProductionMaterialIssueNoteDetails = ItemList;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return result;
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
                    SqlCommand cmd = new SqlCommand("usp_tbl_ProductionMaterialIssueNote_Delete", con);
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

    }
}