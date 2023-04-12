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
using InVanWebApp_BO;
using log4net;
using InVanWebApp.Common;

namespace InVanWebApp.Repository
{
    public class OutwardNoteRepository:IOutwardNoteRepository
    {
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(OutwardNoteRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of outward note.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OutwardNoteBO> GetAll()
        {
            List<OutwardNoteBO> resultList = new List<OutwardNoteBO>();
            string queryString = "Select * from OutwardNote where IsDeleted=0";

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    resultList = con.Query<OutwardNoteBO>(queryString).ToList();
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
        public ResponseMessageBO Insert(OutwardNoteBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_OutwardNote_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OutwardNumber", model.OutwardNumber);
                    cmd.Parameters.AddWithValue("@OutwardDate", model.OutwardDate);
                    cmd.Parameters.AddWithValue("@LocationID", model.LocationID);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    cmd.Parameters.AddWithValue("@LocationAddress", model.LocationAddress);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", model.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@DispatchThrough", model.DispatchThrough);
                    cmd.Parameters.AddWithValue("@DocketNumber", model.DocketNumber);
                    cmd.Parameters.AddWithValue("@ContactPerson", model.ContactPerson);
                    cmd.Parameters.AddWithValue("@ContactInformation", model.ContactInformation);
                    cmd.Parameters.AddWithValue("@VerifiedBy", model.VerifiedBy);
                    cmd.Parameters.AddWithValue("@VerifiedByName", model.VerifiedByName);
                    cmd.Parameters.AddWithValue("@IsReturnable", model.IsReturnable);
                    cmd.Parameters.AddWithValue("@Signature", model.Signature);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@VehicleNo", model.VehicleNo);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int OutwardNote_Id = 0;

                    while (dataReader.Read())
                    {
                        OutwardNote_Id = Convert.ToInt32(dataReader["ID"]);
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    if (OutwardNote_Id != 0)
                    {
                        var json = new JavaScriptSerializer();
                        var data = json.Deserialize<Dictionary<string, string>[]>(model.txtItemDetails);

                        List<OutwardNoteDetailsBO> itemDetails = new List<OutwardNoteDetailsBO>();

                        foreach (var item in data)
                        {
                            OutwardNoteDetailsBO objItemDetails = new OutwardNoteDetailsBO();
                            objItemDetails.OutwardNoteID = OutwardNote_Id;
                            //objItemDetails.ItemCode = item.ElementAt(0).Value.ToString();
                            //objItemDetails.ItemID = Convert.ToInt32(item.ElementAt(1).Value);
                            //objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                            //objItemDetails.OutwardQuantity = Convert.ToDecimal(item.ElementAt(3).Value);
                            //objItemDetails.ItemUnit = item.ElementAt(4).Value.ToString();
                            //objItemDetails.Remarks = item.ElementAt(5).Value.ToString();
                            objItemDetails.ItemID = Convert.ToInt32(item.ElementAt(0).Value);
                            objItemDetails.ItemCode = item.ElementAt(1).Value.ToString();
                            objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                            objItemDetails.OutwardQuantity = Convert.ToDecimal(item.ElementAt(3).Value);
                            objItemDetails.ItemUnit = item.ElementAt(4).Value.ToString();
                            objItemDetails.Remarks = item.ElementAt(5).Value.ToString();
                            objItemDetails.CreatedBy = model.CreatedBy;
                            itemDetails.Add(objItemDetails);
                        }

                        foreach (var item in itemDetails)
                        {
                            con.Open();
                            SqlCommand cmdNew = new SqlCommand("usp_tbl_OutwardNoteDetails_Insert", con);
                            cmdNew.CommandType = CommandType.StoredProcedure;

                            cmdNew.Parameters.AddWithValue("@OutwardNoteID", item.OutwardNoteID);
                            cmdNew.Parameters.AddWithValue("@ItemId", item.ItemID);
                            cmdNew.Parameters.AddWithValue("@Item_Name", item.ItemName);
                            cmdNew.Parameters.AddWithValue("@Item_Code", item.ItemCode);
                            cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                            cmdNew.Parameters.AddWithValue("@OutwardQuantity", item.OutwardQuantity);
                            cmdNew.Parameters.AddWithValue("@Remarks", item.Remarks);
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

        #region Update function
        public ResponseMessageBO Update(OutwardNoteBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_OutwardNote_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@OutwardNumber", model.OutwardNumber);
                    cmd.Parameters.AddWithValue("@OutwardDate", model.OutwardDate);
                    cmd.Parameters.AddWithValue("@LocationID", model.LocationID);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    cmd.Parameters.AddWithValue("@LocationAddress", model.LocationAddress);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", model.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@DispatchThrough", model.DispatchThrough);
                    cmd.Parameters.AddWithValue("@DocketNumber", model.DocketNumber);
                    cmd.Parameters.AddWithValue("@ContactPerson", model.ContactPerson);
                    cmd.Parameters.AddWithValue("@ContactInformation", model.ContactInformation);
                    cmd.Parameters.AddWithValue("@VerifiedBy", model.VerifiedBy);
                    cmd.Parameters.AddWithValue("@VerifiedByName", model.VerifiedByName);
                    cmd.Parameters.AddWithValue("@IsReturnable", model.IsReturnable);
                    cmd.Parameters.AddWithValue("@Signature", model.Signature);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@VehicleNo", model.VehicleNo);
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

                    List<OutwardNoteDetailsBO> itemDetails = new List<OutwardNoteDetailsBO>();

                    foreach (var item in data)
                    {
                        OutwardNoteDetailsBO objItemDetails = new OutwardNoteDetailsBO();
                        objItemDetails.OutwardNoteID = model.ID;
                        objItemDetails.ItemID = Convert.ToInt32(item.ElementAt(0).Value);
                        objItemDetails.ItemCode = item.ElementAt(1).Value.ToString();
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.OutwardQuantity = Convert.ToDecimal(item.ElementAt(3).Value);
                        objItemDetails.ItemUnit = item.ElementAt(4).Value.ToString();
                        objItemDetails.Remarks = item.ElementAt(5).Value.ToString();
                        objItemDetails.CreatedBy = model.LastModifiedBy;
                        itemDetails.Add(objItemDetails);
                    }

                    var count = itemDetails.Count;
                    var i = 1;
                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_OutwardNoteDetails_Update", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@OutwardNoteID", item.OutwardNoteID);
                        cmdNew.Parameters.AddWithValue("@ItemId", item.ItemID);
                        cmdNew.Parameters.AddWithValue("@Item_Name", item.ItemName);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.ItemCode);
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        cmdNew.Parameters.AddWithValue("@OutwardQuantity", item.OutwardQuantity);
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

            //_context.Entry(unitMaster).State = EntityState.Modified;
        }
        #endregion

        #region Fetch Item list for dropdown
        public List<ItemBO> GetItemDetailsForDD()
        {
            List<ItemBO> ItemList = new List<ItemBO>();
            string queryString = "Select ID, Item_Code+' ('+Item_Name+')' as Item_Code from Item where IsDeleted=0";
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    ItemList = con.Query<ItemBO>(queryString).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return ItemList;
        }
        #endregion

        #region Delete function

        /// <summary>
        /// Delete record by ID
        /// Created Date: 01 Feb'23
        /// </summary>
        /// <param name="Id">Record id which user want to delete.</param>
        /// <param name="userId">User id who is deleting the record.</param>
        public ResponseMessageBO Delete(int Id, int userId)
        {
            ResponseMessageBO responseMessage = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_OutwardNote_Delete", con);
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

        public OutwardNoteBO GetById(int ID)
        {
            OutwardNoteBO result = new OutwardNoteBO();
            try
            {
                string stringQuery = "Select * from OutwardNote where IsDeleted=0 and ID=@ID";
                string stringItemQuery = "Select * from OutwardNoteDetails where IsDeleted=0 and OutwardNoteID=@ID";
                using (SqlConnection con = new SqlConnection(connString))
                {
                    result = con.Query<OutwardNoteBO>(stringQuery, new { @ID = ID }).FirstOrDefault();
                    var ItemList = con.Query<OutwardNoteDetailsBO>(stringItemQuery, new { @ID = ID }).ToList();
                    result.outwardNoteDetails = ItemList;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return result;
        }

        #endregion
    }
}