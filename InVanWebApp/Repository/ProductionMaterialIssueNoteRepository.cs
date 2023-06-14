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
using InVanWebApp.Common;

namespace InVanWebApp.Repository
{
    public class ProductionMaterialIssueNoteRepository : IProductionMaterialIssueNoteRepository
    {
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
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

        #region Bind dropdown of Production Indent Number
        public IEnumerable<ProductionIndentBO> GetProductionIndentNumberForDropdown()
        {
            List<ProductionIndentBO> resultList = new List<ProductionIndentBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ProductionIndent_GetAll_For_Material_Issue", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new ProductionIndentBO()
                        {
                            ID = Convert.ToInt32(dataReader["ProductionIndentId"]),
                            ProductionIndentNo = dataReader["ProductionIndentNo"].ToString()
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

        #region Fetch Production Indent Ingredients Details ById for Production Material IssueNote
        public IEnumerable<ProductionIndent_DetailsBO> GetProductionIndentIngredientsDetailsById(int ProductionIndentId,int LocationID)
        {
            List<ProductionIndent_DetailsBO> resultList = new List<ProductionIndent_DetailsBO>();

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {

                    SqlCommand cmd1 = new SqlCommand("usp_tbl_ProductionIndentIngredientsDetails_GetByID", con);
                    cmd1.Parameters.AddWithValue("@ProductionIndent_Id", ProductionIndentId);
                    cmd1.Parameters.AddWithValue("@Location_Id", LocationID);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader2 = cmd1.ExecuteReader();

                    while (dataReader2.Read())
                    {
                        var result = new ProductionIndent_DetailsBO()
                        {
                            //ID = Convert.ToInt32(dataReader2["ID"]),
                            ItemId = Convert.ToInt32(dataReader2["ItemId"]), 
                            ItemName = dataReader2["Item_Name"].ToString(),
                            ItemCode = dataReader2["Item_Code"].ToString(),
                            RequestedQty=Convert.ToDecimal(dataReader2["RequestedQty"]),
                            IssuedQty=Convert.ToDecimal(dataReader2["IssuedQty"]),
                            IssuingQty=Convert.ToDecimal(dataReader2["IssuingQty"]),
                            BalanceQty=Convert.ToDecimal(dataReader2["BalanceQty"]),
                            ItemUnit = dataReader2["ItemUnit"].ToString(), 
                            AvailableStock = Convert.ToDecimal(dataReader2["AvailableStock"]),
                            ItemUnitPrice = Convert.ToDecimal(dataReader2["ItemUnitPrice"]),
                            CurrencyName = dataReader2["CurrencyName"].ToString(),
                            FinalStock = Convert.ToDecimal(dataReader2["FinalStock"]),
                            WorkOrderNo=dataReader2["WorkOrderNo"].ToString(),
                            BatchPlanningDocId = Convert.ToInt32(dataReader2["BatchPlanningDocId"]),  //Rahul added 'BatchPlanningDocId' 13-06-23.  
                            BatchPlanningDocumentNo = dataReader2["BatchPlanningDocumentNo"].ToString() //Rahul added 'BatchPlanningDocumentNo' 13-06-23.   
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
                    cmd.Parameters.AddWithValue("@ProductionIndentID", model.ProductionIndentID);
                    cmd.Parameters.AddWithValue("@ProductionIndentNo", model.ProductionIndentNo);
                    cmd.Parameters.AddWithValue("@WorkOrderNumber", model.WorkOrderNumber);
                    cmd.Parameters.AddWithValue("@BatchPlanningDocId", model.BatchPlanningDocId);    //Rahul added 'BatchPlanningDocId' 13-06-23. 
                    cmd.Parameters.AddWithValue("@BatchPlanningDocumentNo", model.BatchPlanningDocumentNo);  //Rahul added 'BatchPlanningDocumentNo' 13-06-23.
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
                            objItemDetails.QuantityRequested = Convert.ToDouble(item.ElementAt(3).Value);
                            objItemDetails.QuantityIssued= Convert.ToDouble(item.ElementAt(4).Value);
                            objItemDetails.IssuingQty= Convert.ToDouble(item.ElementAt(5).Value);
                            objItemDetails.BalanceQty= Convert.ToDouble(item.ElementAt(6).Value);
                            objItemDetails.AvailableStockBeforeIssue = Convert.ToDouble(item.ElementAt(7).Value);
                            objItemDetails.ItemUnit = item.ElementAt(8).Value.ToString();
                            objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(9).Value);
                            objItemDetails.CurrencyName = item.ElementAt(10).Value.ToString();
                            objItemDetails.StockAfterIssuing = Convert.ToDouble(item.ElementAt(11).Value);
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
                            cmdNew.Parameters.AddWithValue("@QuantityRequested", item.QuantityRequested);
                            cmdNew.Parameters.AddWithValue("@QuantityIssued", item.QuantityIssued);
                            cmdNew.Parameters.AddWithValue("@IssuingQty", item.IssuingQty);
                            cmdNew.Parameters.AddWithValue("@BalanceQty", item.BalanceQty);
                            cmdNew.Parameters.AddWithValue("@AvailableStockBeforeIssue", item.AvailableStockBeforeIssue);
                            cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                            cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                            cmdNew.Parameters.AddWithValue("@CurrencyName", item.CurrencyName);
                            cmdNew.Parameters.AddWithValue("@StockAfterIssuing", item.StockAfterIssuing);
                            cmdNew.Parameters.AddWithValue("@LocationId", model.LocationId);
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