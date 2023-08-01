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
    public class IntermediateRejectionNoteRepository: IIntermediateRejectionNoteRepository 
    {
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        public static ILog log = LogManager.GetLogger(typeof(IntermediateRejectionNoteRepository));

        #region Bind Grid
        ///<summary>
        /// Rahul: This function is for fecthing list of Intermediate Rejection Note data. 
        ///</summary>
        public IEnumerable<IntermediateRejectionNoteBO> GetAll()
        {
            List<IntermediateRejectionNoteBO> resultList = new List<IntermediateRejectionNoteBO>();
            string intermediateRejectionNote = "Select * from IntermediateRejectionNote where IsDeleted=0"; 

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    resultList = con.Query<IntermediateRejectionNoteBO>(intermediateRejectionNote).ToList();
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
        /// Rahul: Insert record Intermediate Rejection Note and Intermediate Rejection Note Details. 
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(IntermediateRejectionNoteBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_IntermediateRejectionNote_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Intermediate_Rej_NoteNo", model.Intermediate_Rej_NoteNo);
                    cmd.Parameters.AddWithValue("@Intermediate_Rej_NoteDate", model.Intermediate_Rej_NoteDate);
                    cmd.Parameters.AddWithValue("@IssueBy", model.IssueBy);
                    cmd.Parameters.AddWithValue("@IssueByName", model.IssueByName);
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
                    int Intermediate_Rej_NoteId = 0;

                    while (dataReader.Read())
                    {
                        Intermediate_Rej_NoteId = Convert.ToInt32(dataReader["ID"]);
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    if (Intermediate_Rej_NoteId != 0)
                    {
                        var json = new JavaScriptSerializer();
                        var data = json.Deserialize<Dictionary<string, string>[]>(model.txtItemDetails);

                        List<IntermediateRejectionNoteDetailsBO> itemDetails = new List<IntermediateRejectionNoteDetailsBO>();

                        foreach (var item in data)
                        {
                            IntermediateRejectionNoteDetailsBO objItemDetails = new IntermediateRejectionNoteDetailsBO();
                            objItemDetails.Intermediate_Rej_NoteId = Intermediate_Rej_NoteId;
                            objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(1).Value);
                            objItemDetails.Item_Code = item.ElementAt(0).Value.ToString();
                            objItemDetails.Item_Name = item.ElementAt(2).Value.ToString();
                            objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(3).Value);
                            objItemDetails.ItemUnit = item.ElementAt(5).Value.ToString();
                            objItemDetails.CurrencyName = item.ElementAt(4).Value.ToString();
                            objItemDetails.AvailableStockQuantity = Convert.ToDouble(item.ElementAt(6).Value);                            
                            objItemDetails.RejectedQuantity = Convert.ToDouble(item.ElementAt(7).Value);
                            objItemDetails.BalanceQuantity = Convert.ToDouble(item.ElementAt(8).Value);
                            objItemDetails.Description = item.ElementAt(9).Value.ToString();
                            objItemDetails.CreatedBy = model.CreatedBy;
                            itemDetails.Add(objItemDetails);
                        }

                        foreach (var item in itemDetails)
                        {
                            con.Open();
                            SqlCommand cmdNew = new SqlCommand("usp_tbl_IntermediateRejectionNoteDetails_Insert", con);
                            cmdNew.CommandType = CommandType.StoredProcedure;

                            cmdNew.Parameters.AddWithValue("@Intermediate_Rej_NoteId", item.Intermediate_Rej_NoteId);
                            cmdNew.Parameters.AddWithValue("@ItemId", item.ItemId);
                            cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                            cmdNew.Parameters.AddWithValue("@Item_Name", item.Item_Name);
                            cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                            cmdNew.Parameters.AddWithValue("@CurrencyName", item.CurrencyName);
                            cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                            cmdNew.Parameters.AddWithValue("@AvailableStockQuantity", item.AvailableStockQuantity);                            
                            cmdNew.Parameters.AddWithValue("@RejectedQuantity", item.RejectedQuantity);
                            cmdNew.Parameters.AddWithValue("@BalanceQuantity", item.BalanceQuantity);
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


    }
}