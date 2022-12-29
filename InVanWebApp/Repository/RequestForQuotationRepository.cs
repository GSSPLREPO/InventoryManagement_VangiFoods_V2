using Dapper;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace InVanWebApp.Repository
{
    public class RequestForQuotationRepository : IRequestForQuotationRepository 
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(RequestForQuotationRepository));

        #region  Bind grid
        /// <summary>
        /// Rahul : This function is for fecthing list of order master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RequestForQuotationBO> GetAll()
        {
            List<RequestForQuotationBO> requestForQuotationMastersList = new List<RequestForQuotationBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RequestForQuotation_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var requestForQuotationMasters = new RequestForQuotationBO()
                        {
                            RequestForQuotationId=Convert.ToInt32(reader["RequestForQuotationId"]),
                            RFQNO = reader["RFQNO"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]),
                            DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                            BiddingStartDate = Convert.ToDateTime(reader["BiddingStartDate"]),
                            BiddingEndDate = Convert.ToDateTime(reader["BiddingEndDate"]), 
                            HSN_Code = reader["HSN_Code"].ToString(), 
                            Quantity = Convert.ToDecimal(reader["Quantity"]),
                            CreatedByDate = Convert.ToDateTime(reader["CreatedByDate"]),

                        };
                        requestForQuotationMastersList.Add(requestForQuotationMasters); 
                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return requestForQuotationMastersList; 

        }
        #endregion

        #region Get details of Items by ID
        public ItemBO GetItemDetails(int itemID)
        {
            try
            {
                ItemBO ItemDetails = new ItemBO();                
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_GetItemDetailsByIdForPOandOC", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemId", itemID);                    
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        ItemDetails = new ItemBO()
                        {
                            Item_Name = reader["Item_Name"].ToString(),
                            Item_Code = reader["Item_Code"].ToString(),
                            UnitCode = reader["UnitID"].ToString(),                            
                        };
                    }
                    con.Close();
                    return ItemDetails;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }

        #endregion


        #region Insert function
        /// <summary>
        /// Rahul: Insert record.
        /// </summary>
        /// <param name="requestForQuotationMaster"></param>                
        public ResponseMessageBO Insert(RequestForQuotationBO requestForQuotationMaster)
        {

            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {

                    SqlCommand cmd = new SqlCommand("usp_tbl_RequestForQuotation_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RFQNO", requestForQuotationMaster.RFQNO);
                    //cmd.Parameters.AddWithValue("@VendorsID", requestForQuotationMaster.VendorsID);                    
                    cmd.Parameters.AddWithValue("@VendorIDs", requestForQuotationMaster.VendorIDs);                    
                    cmd.Parameters.AddWithValue("@LocationId", requestForQuotationMaster.LocationId);                                                                         
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(System.DateTime.Now));  
                    cmd.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(System.DateTime.Now));  
                    cmd.Parameters.AddWithValue("@BiddingStartDate", requestForQuotationMaster.BiddingStartDate);  
                    cmd.Parameters.AddWithValue("@BiddingEndDate", requestForQuotationMaster.BiddingEndDate);
                    cmd.Parameters.AddWithValue("@Quantity", requestForQuotationMaster.Quantity);
                    cmd.Parameters.AddWithValue("@HSN_Code", requestForQuotationMaster.HSN_Code); 
                    cmd.Parameters.AddWithValue("@Attachment", requestForQuotationMaster.Attachment);
                    cmd.Parameters.AddWithValue("@Signature", requestForQuotationMaster.Signature);
                    cmd.Parameters.AddWithValue("@Remarks", requestForQuotationMaster.Remarks);                    
                    cmd.Parameters.AddWithValue("@CreatedByID", requestForQuotationMaster.CreatedByID);
                    cmd.Parameters.AddWithValue("@CreatedByDate", Convert.ToDateTime(System.DateTime.Now));                                        

                    con.Open();                     
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int RequestForQuotationId = 0;
                    //response.Status = false;
                    while (dataReader.Read())
                    {                        
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                        RequestForQuotationId = Convert.ToInt32(dataReader["RequestForQuotationId"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(requestForQuotationMaster.TxtItemDetails);

                    List<RequestForQuotationItemDetailsBO> itemDetails = new List<RequestForQuotationItemDetailsBO>();

                    foreach (var item in data)
                    {
                        RequestForQuotationItemDetailsBO objItemDetails = new RequestForQuotationItemDetailsBO();
                        objItemDetails.RequestForQuotationId = RequestForQuotationId;
                        objItemDetails.Item_ID = Convert.ToInt32(item.ElementAt(0).Value);
                        objItemDetails.Item_Code = item.ElementAt(1).Value.ToString();
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.Quantity = Convert.ToDecimal(item.ElementAt(3).Value);
                        objItemDetails.ItemUnit = item.ElementAt(4).Value.ToString();
                        //objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(5).Value);
                        //objItemDetails.ItemTaxValue = Convert.ToDecimal(item.ElementAt(6).Value);
                        //objItemDetails.TotalItemCost = Convert.ToDouble(item.ElementAt(7).Value);
                        objItemDetails.DeliveryDate = Convert.ToDateTime(item.ElementAt(5).Value);
                        objItemDetails.HSN_Code = item.ElementAt(6).Value.ToString();
                        objItemDetails.Remarks = item.ElementAt(7).Value.ToString();
                        
                        itemDetails.Add(objItemDetails);
                    }

                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_RequestForQuotationItemDetails_Insert", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;
                        cmdNew.Parameters.AddWithValue("@RequestForQuotationId", item.RequestForQuotationId);
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.Item_ID);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);                        
                        //cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                        cmdNew.Parameters.AddWithValue("@ItemQuantity", item.Quantity);
                        //cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        //cmdNew.Parameters.AddWithValue("@TotalItemCost", item.TotalItemCost);
                        cmdNew.Parameters.AddWithValue("@HSN_Code", item.HSN_Code);                        
                        cmdNew.Parameters.AddWithValue("@DeliveryDate", item.DeliveryDate); 
                        cmdNew.Parameters.AddWithValue("@Remarks", item.Remarks); 
                        //cmdNew.Parameters.AddWithValue("@CreatedByID", item.CreatedByID);
                        cmdNew.Parameters.AddWithValue("@CreatedByID", requestForQuotationMaster.CreatedByID); 
                        cmdNew.Parameters.AddWithValue("@CreatedByDate", Convert.ToDateTime(System.DateTime.Now));
                        //cmdNew.Parameters.AddWithValue("@LastModifiedBy", 1);
                        //cmdNew.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));    

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
                log.Error(ex.Message, ex);
            }
            return response;
        }
        #endregion

        #region GetRFQbyId function for Request For Quotation 
        /// <summary>
        /// GetRFQbyId record by ID, Rahul 19/12/2022.  
        /// </summary>
        /// <param name="ID"></param>
        public RequestForQuotationBO GetRFQbyId(int RequestForQuotationId)  
        {
            string rfqQuery = "SELECT * FROM RequestForQuotation WHERE RequestForQuotationId = @RequestForQuotationId AND IsDeleted = 0";
            string rfqItemQuery = "SELECT * FROM RequestForQuotationItemDetails WHERE RequestForQuotationId = @RequestForQuotationId AND IsDeleted = 0";
            using (SqlConnection con = new SqlConnection(connString)) 
            {
                var rfq = con.Query<RequestForQuotationBO>(rfqQuery, new { @RequestForQuotationId = RequestForQuotationId }).FirstOrDefault();
                var rfqitemList = con.Query<RequestForQuotationItemDetailsBO>(rfqItemQuery, new { @RequestForQuotationId = RequestForQuotationId }).ToList();
                rfq.itemDetails = rfqitemList;  
                return rfq; 
            }
        }
        #endregion

        #region Update function for Request For Quotation 
        /// <summary>
        /// Rahul: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(RequestForQuotationBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RequestForQuotation_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure; 
                    cmd.Parameters.AddWithValue("@RequestForQuotationId", model.RequestForQuotationId);                    
                    cmd.Parameters.AddWithValue("@RFQNO", model.RFQNO);
                    cmd.Parameters.AddWithValue("@VendorIDs", model.VendorIDs); 
                    //cmd.Parameters.AddWithValue("@VendorsID", model.VendorsID);
                    //cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);                    
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    //cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    //cmd.Parameters.AddWithValue("@DeliveryAddress", model.DeliveryAddress);
                    //cmd.Parameters.AddWithValue("@SupplierAddress", model.SupplierAddress);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    cmd.Parameters.AddWithValue("@DeliveryDate", model.DeliveryDate);
                    cmd.Parameters.AddWithValue("@BiddingStartDate", model.BiddingStartDate);
                    cmd.Parameters.AddWithValue("@BiddingEndDate", model.BiddingEndDate);
                    cmd.Parameters.AddWithValue("@Quantity", model.Quantity); 
                    cmd.Parameters.AddWithValue("@HSN_Code", model.HSN_Code); 
                    //cmd.Parameters.AddWithValue("@Attachment", model.Attachment);
                    //cmd.Parameters.AddWithValue("@Signature", model.Signature);                    
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@LastModifiedByDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@LastModifiedByID", model.LastModifiedByID);
                    
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.TxtItemDetails);

                    List<RequestForQuotationItemDetailsBO> itemDetails = new List<RequestForQuotationItemDetailsBO>();

                    foreach (var item in data)
                    {
                        RequestForQuotationItemDetailsBO objItemDetails = new RequestForQuotationItemDetailsBO();
                        objItemDetails.RequestForQuotationId = model.RequestForQuotationId;
                        objItemDetails.Item_ID = Convert.ToInt32(item.ElementAt(0).Value);
                        objItemDetails.Item_Code = item.ElementAt(1).Value.ToString();
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.Quantity = Convert.ToDecimal(item.ElementAt(3).Value);
                        objItemDetails.ItemUnit = item.ElementAt(4).Value.ToString();
                        //objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(5).Value);
                        //objItemDetails.ItemTaxValue = Convert.ToDecimal(item.ElementAt(6).Value);
                        //objItemDetails.ItemTaxValue = item.ElementAt(7).Value.ToString();
                        //objItemDetails.TotalItemCost = Convert.ToDouble(item.ElementAt(8).Value);
                        objItemDetails.CreatedByID = model.LastModifiedByID;

                        itemDetails.Add(objItemDetails);
                    }

                    var count = itemDetails.Count;

                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_RFQItemsDetails_Update", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@RequestForQuotationId", item.RequestForQuotationId);
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.Item_ID);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);                        
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        cmdNew.Parameters.AddWithValue("@HSN_Code", item.HSN_Code);
                        cmdNew.Parameters.AddWithValue("@DeliveryDate", model.DeliveryDate);
                        cmdNew.Parameters.AddWithValue("@Remarks", item.@Remarks);                        
                        //cmdNew.Parameters.AddWithValue("@CreatedByID", item.CreatedByID);                        
                        //cmdNew.Parameters.AddWithValue("@CreatedByDate", item.CreatedByDate);
                        cmdNew.Parameters.AddWithValue("@LastModifiedByID", model.LastModifiedByID); 
                        cmdNew.Parameters.AddWithValue("@LastModifiedByDate", Convert.ToDateTime(System.DateTime.Now));
                        
                        if (count == 1)
                            cmdNew.Parameters.AddWithValue("@OneItemIdentifier", 1);
                        else
                            cmdNew.Parameters.AddWithValue("@OneItemIdentifier", 0);

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


        #region GetPOById function for timeline view 
        /// <summary>
        /// GetPOById record by ID, Rahul 17/12/2022.  
        /// </summary>
        /// <param name="ID"></param>
        public RequestForQuotationItemDetailsBO GetDetailsForRFQView(int RequestForQuotationId) 
        {
            RequestForQuotationItemDetailsBO rfqItemBo = new RequestForQuotationItemDetailsBO();            
            string rfqItemDetailsQuery = "SELECT TOP(1) * FROM RequestForQuotationItemDetails WHERE RequestForQuotationId = @RequestForQuotationId AND IsDeleted = 0 order by CreatedByDate desc ";
            using (SqlConnection con = new SqlConnection(connString))
            {                
                var rfqItemDetailsResult = con.Query<RequestForQuotationItemDetailsBO>(rfqItemDetailsQuery, new { @RequestForQuotationId = RequestForQuotationId }).FirstOrDefault(); 
                if (rfqItemDetailsResult == null)
                {
                    rfqItemBo.HSN_Code = null;
                }
                else
                {
                    rfqItemBo.DeliveryDate = (DateTime)rfqItemDetailsResult.DeliveryDate;
                    rfqItemBo.Item_Code = rfqItemDetailsResult.Item_Code;
                    rfqItemBo.ItemName = rfqItemDetailsResult.ItemName;
                    rfqItemBo.ItemUnit = rfqItemDetailsResult.ItemUnit;
                    rfqItemBo.Quantity = rfqItemDetailsResult.Quantity;

                }
                return rfqItemBo;
            }
        }
        #endregion

        #region Delete function  
        /// <summary>
        /// Delete record by ID 
        /// </summary>
        /// <param name="RequestForQuotationId"></param>
        public void Delete(int RequestForQuotationId, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_DeleteRequestForQuotation_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestForQuotationId", RequestForQuotationId);
                    cmd.Parameters.AddWithValue("@LastModifiedByID", userId);
                    cmd.Parameters.AddWithValue("@LastModifiedByDate", Convert.ToDateTime(System.DateTime.Now));
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


    }
}