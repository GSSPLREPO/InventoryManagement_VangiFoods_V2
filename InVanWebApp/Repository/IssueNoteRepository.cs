using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Dapper;
using InVanWebApp.Common;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;

namespace InVanWebApp.Repository
{
    public class IssueNoteRepository:IIssueNoteRepository
    {
        //private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(IssueNoteRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of Issue Note data.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IssueNoteBO> GetAll()
        {
            List<IssueNoteBO> resultList = new List<IssueNoteBO>();
            string stockAdjustment = "Select * from IssueNote where IsDeleted=0";

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    resultList = con.Query<IssueNoteBO>(stockAdjustment).ToList();
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
        public ResponseMessageBO Insert(IssueNoteBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_IssueNote_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IssueNoteNo", model.IssueNoteNo);
                    cmd.Parameters.AddWithValue("@IssueNoteDate", model.IssueNoteDate);
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

                        List<IssueNoteDetailsBO> itemDetails = new List<IssueNoteDetailsBO>();

                        foreach (var item in data)
                        {
                            IssueNoteDetailsBO objItemDetails = new IssueNoteDetailsBO();
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
                            SqlCommand cmdNew = new SqlCommand("usp_tbl_IssueNoteDetails_Insert", con);
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
                    SqlCommand cmd = new SqlCommand("usp_tbl_IssueNote_Delete", con);
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

        public IssueNoteBO GetById(int ID)
        {
            IssueNoteBO result = new IssueNoteBO();
            try
            {
                string stringQuery = "Select * from IssueNote where IsDeleted=0 and ID=@ID";
                string stringItemQuery = "Select * from IssueNoteDetails where IsDeleted=0 and IssueNoteId=@ID";
                using (SqlConnection con = new SqlConnection(connString))
                {
                    result = con.Query<IssueNoteBO>(stringQuery, new { @ID = ID }).FirstOrDefault();
                    var ItemList = con.Query<IssueNoteDetailsBO>(stringItemQuery, new { @ID = ID }).ToList();
                    result.IssueNoteDetails = ItemList;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return result;
        }

        #endregion

        #region Bind Dropdown 
        public IEnumerable<UsersBO> GetUserNameList()
        {
            List<UsersBO> resultList = new List<UsersBO>();
            string QueryString = "Select * from Users where IsDeleted=0 and IsActive=1";

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    resultList = con.Query<UsersBO>(QueryString).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return resultList;
        }

        #endregion

        #region Bind Dropdown For Issue Note
        /// <summary>
        /// Siddharth  : This function is for fecthing list of Issue Note Number from Issue Note.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IssueNoteBO> GetIssueNoteNumber()
        {
            List<IssueNoteBO> IssueNoteNumber = new List<IssueNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InvoiceNoteNumber_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var IssueNoteMasters = new IssueNoteBO()
                        {
                            ID = Convert.ToInt32(reader["IssueNoteId"]),
                            IssueNoteNo = reader["IssueNoteNO"].ToString()
                        };
                        IssueNoteNumber.Add(IssueNoteMasters);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return IssueNoteNumber;
        }


        #endregion
    }
}