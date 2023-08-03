using Dapper;
using InVanWebApp.Common;
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
    public class MaterialReturnNoteRepository: IMaterialReturnNoteRepository
    {
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        public static ILog log = LogManager.GetLogger(typeof(IMaterialReturnNoteRepository));

        #region Bind Grid
        ///<summary>
        /// Rahul: This function is for fecthing list of Material Return Note data. 
        ///</summary>
        public IEnumerable<MaterialReturnNoteBO> GetAll()
        {
            List<MaterialReturnNoteBO> resultList = new List<MaterialReturnNoteBO>();
            string materialReturnNote = "Select * from MaterialReturnNote where IsDeleted=0"; 

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    resultList = con.Query<MaterialReturnNoteBO>(materialReturnNote).ToList();
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
        /// Rahul: Insert record Material Return Note and Material Return Note Details. 
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(MaterialReturnNoteBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    //SqlCommand cmd = new SqlCommand("usp_tbl_IntermediateRejectionNote_Insert", con);
                    SqlCommand cmd = new SqlCommand("usp_tbl_MaterialReturnNote_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaterialReturnNoteNo", model.MaterialReturnNoteNo); 
                    cmd.Parameters.AddWithValue("@MaterialReturnNoteDate", model.MaterialReturnNoteDate);
                    cmd.Parameters.AddWithValue("@ReturnBy", model.ReturnBy); 
                    cmd.Parameters.AddWithValue("@ReturnByName", model.ReturnByName); 
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    cmd.Parameters.AddWithValue("@WorkOrderNumber", model.WorkOrderNumber);
                    cmd.Parameters.AddWithValue("@QCNumber", model.QCNumber);
                    cmd.Parameters.AddWithValue("@SONumber", model.SONumber);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int MaterialReturnNoteId = 0;

                    while (dataReader.Read())
                    {
                        MaterialReturnNoteId = Convert.ToInt32(dataReader["ID"]);
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    if (MaterialReturnNoteId != 0)
                    {
                        var json = new JavaScriptSerializer();
                        var data = json.Deserialize<Dictionary<string, string>[]>(model.txtItemDetails);

                        List<MaterialReturnNoteDetailsBO> itemDetails = new List<MaterialReturnNoteDetailsBO>();

                        foreach (var item in data)
                        {
                            MaterialReturnNoteDetailsBO objItemDetails = new MaterialReturnNoteDetailsBO();
                            objItemDetails.MaterialReturnNoteId = MaterialReturnNoteId;
                            objItemDetails.Item_Code = item.ElementAt(0).Value.ToString();
                            objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(1).Value);
                            objItemDetails.Item_Name = item.ElementAt(2).Value.ToString();
                            objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(3).Value);
                            objItemDetails.CurrencyName = item.ElementAt(4).Value.ToString();
                            objItemDetails.CurrentStockQuantity = Convert.ToDouble(item.ElementAt(5).Value);
                            objItemDetails.ItemUnit = item.ElementAt(6).Value.ToString();
                            objItemDetails.ReturnQuantity = Convert.ToDouble(item.ElementAt(7).Value);
                            objItemDetails.FinalQuantity = Convert.ToDouble(item.ElementAt(8).Value);
                            objItemDetails.Description = item.ElementAt(9).Value.ToString();
                            objItemDetails.CreatedBy = model.CreatedBy;
                            itemDetails.Add(objItemDetails);
                        }

                        foreach (var item in itemDetails)
                        {
                            con.Open();
                            //SqlCommand cmdNew = new SqlCommand("usp_tbl_IntermediateRejectionNoteDetails_Insert", con);
                            SqlCommand cmdNew = new SqlCommand("usp_tbl_MaterialReturnNoteDetails_Insert", con);
                            cmdNew.CommandType = CommandType.StoredProcedure;

                            cmdNew.Parameters.AddWithValue("@MaterialReturnNoteId", item.MaterialReturnNoteId);
                            cmdNew.Parameters.AddWithValue("@ItemId", item.ItemId);
                            cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                            cmdNew.Parameters.AddWithValue("@Item_Name", item.Item_Name);
                            cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                            cmdNew.Parameters.AddWithValue("@CurrencyName", item.CurrencyName);
                            cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                            cmdNew.Parameters.AddWithValue("@CurrentStockQuantity", item.CurrentStockQuantity);
                            cmdNew.Parameters.AddWithValue("@ReturnQuantity", item.ReturnQuantity);
                            cmdNew.Parameters.AddWithValue("@FinalQuantity", item.FinalQuantity);
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

        #region Delete function

        /// <summary>
        /// Delete Material Return Note record by ID 
        /// </summary>
        /// <param name="ID"></param>
        public ResponseMessageBO Delete(int Id, int userId)
        {
            ResponseMessageBO responseMessage = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_IntermediateRejectionNote_Delete", con);
                    SqlCommand cmd = new SqlCommand("usp_tbl_IntermediateRejectionNote_Delete", con);
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