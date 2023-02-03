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
                            UnitPrice = Convert.ToInt32(reader["UnitPrice"]),   ///added                          
                            IndianCurrencyValue = reader["IndianCurrencyValue"].ToString(),   ///added 
                            ItemTaxValue = Convert.ToInt32(reader["ItemTaxValue"]),   ///added 
                            
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

        #region Insert function fro RFQ. 
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
                    cmd.Parameters.AddWithValue("@LocationName", requestForQuotationMaster.LocationName);                                                                         
                    cmd.Parameters.AddWithValue("@DeliveryAddress", requestForQuotationMaster.DeliveryAddress);                                                                         
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(System.DateTime.Now));  
                    cmd.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(System.DateTime.Now));  
                    cmd.Parameters.AddWithValue("@BiddingStartDate", requestForQuotationMaster.BiddingStartDate);  
                    cmd.Parameters.AddWithValue("@BiddingEndDate", requestForQuotationMaster.BiddingEndDate);
                    cmd.Parameters.AddWithValue("@IndentID", requestForQuotationMaster.IndentID);
                    cmd.Parameters.AddWithValue("@IndentNumber", requestForQuotationMaster.IndentNumber);
                    cmd.Parameters.AddWithValue("@Signature", requestForQuotationMaster.Signature);
                    cmd.Parameters.AddWithValue("@Remarks", requestForQuotationMaster.Remarks);
                    cmd.Parameters.AddWithValue("@CGST", requestForQuotationMaster.CGST);
                    cmd.Parameters.AddWithValue("@SGST", requestForQuotationMaster.SGST);
                    cmd.Parameters.AddWithValue("@IGST", requestForQuotationMaster.IGST);
                    cmd.Parameters.AddWithValue("@CurrencyID", requestForQuotationMaster.CurrencyID);
                    cmd.Parameters.AddWithValue("@CurrencyName", requestForQuotationMaster.CurrencyName);
                    cmd.Parameters.AddWithValue("@CreatedByID", requestForQuotationMaster.CreatedByID);
                    cmd.Parameters.AddWithValue("@CreatedByDate", Convert.ToDateTime(System.DateTime.Now));                    
                    con.Open();                     
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int RequestForQuotationId = 0;
                    
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
                        objItemDetails.DeliveryDate = Convert.ToDateTime(item.ElementAt(5).Value);
                        objItemDetails.HSN_Code = item.ElementAt(6).Value.ToString();
                        //objItemDetails.Remarks = item.ElementAt(7).Value.ToString();
                        
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
                        cmdNew.Parameters.AddWithValue("@ItemQuantity", item.Quantity);                        
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);                                                
                        cmdNew.Parameters.AddWithValue("@HSN_Code", item.HSN_Code);                        
                        cmdNew.Parameters.AddWithValue("@DeliveryDate", item.DeliveryDate); 
                        cmdNew.Parameters.AddWithValue("@Remarks", requestForQuotationMaster.Remarks); 
                        //cmdNew.Parameters.AddWithValue("@Remarks", item.Remarks); 
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
                        objItemDetails.DeliveryDate = Convert.ToDateTime(item.ElementAt(5).Value);
                        objItemDetails.HSN_Code = item.ElementAt(6).Value.ToString();
                        objItemDetails.Remarks = item.ElementAt(7).Value.ToString();
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
                        cmdNew.Parameters.AddWithValue("@ItemQuantity", item.Quantity);
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

        //---GetDetailsForRFQView function for update RFQ details. 
        /// <summary>
        /// GetPOById record by ID, Rahul 17/12/2022.  
        /// </summary>
        /// <param name="ID"></param>

        public RequestForQuotationBO GetDetailsForRFQView(int RequestForQuotationId)
        {
            string rfqQuery = "SELECT * FROM RequestForQuotation WHERE RequestForQuotationId = @RequestForQuotationId AND IsDeleted = 0";
            string rfqItemQuery = "SELECT * FROM RequestForQuotationItemDetails WHERE RequestForQuotationId = @RequestForQuotationId AND IsDeleted = 0;";
            using (SqlConnection con = new SqlConnection(connString))
            {
                var rfq = con.Query<RequestForQuotationBO>(rfqQuery, new { @RequestForQuotationId = RequestForQuotationId }).FirstOrDefault();
                var rfqList = con.Query<RequestForQuotationItemDetailsBO>(rfqItemQuery, new { @RequestForQuotationId = RequestForQuotationId }).ToList();
                rfq.itemDetails = rfqList;
                return rfq;
            }
        }

        #endregion

        #region Get details of Items by ID
        public IEnumerable<RequestForQuotationBO> GetCompanyNameForRFQView(int ID)
        {
            try
            {
                List<RequestForQuotationBO> RFQCompanyDetails = new List<RequestForQuotationBO>();
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_CompanyList_RFQview_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RFQ_ID", ID); 
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var CompanyDetails = new RequestForQuotationBO()  
                        {
                            VendorsID = Convert.ToInt32(reader["ID"]),
                            CompanyName = reader["CompanyName"].ToString()
                            
                        };
                        RFQCompanyDetails.Add(CompanyDetails); 
                    }
                    con.Close();
                    return RFQCompanyDetails;
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

        #region GetRFQbyId,VendorsID function for RFQ Vendor details  
        /// <summary>
        /// GetRFQbyId record by ID, Rahul 19/12/2022.    
        /// </summary>
        /// <param name="RequestForQuotationId"></param>
        public RequestForQuotationBO GetRFQbyId(int RequestForQuotationId, int VendorsID)
        {
            string rfqQuery = "select  RF.*, (Select LM.Address from LocationMaster LM where LM.ID=RF.LocationId and LM.IsDeleted=0) as LocationAddress " + 
                ", (Select LM.LocationName from LocationMaster LM where LM.ID=RF.LocationId and LM.IsDeleted=0) as LocationName " + 
                "from RequestForQuotation RF where RF.IsDeleted=0 and RF.RequestForQuotationId=@RequestForQuotationId";
            string rfqItemQuery = "select  RFITM.*, (Select ITM.ItemTaxValue from ItemTaxMaster ITM where ITM.ItemId = RFITM.Item_ID and ITM.IsDeleted = 0) as ItemTaxValue from RequestForQuotationItemDetails RFITM where RFITM.IsDeleted = 0 and RFITM.RequestForQuotationId = @RequestForQuotationId";
            string rfqSupplierQuery = "SELECT * FROM Company WHERE ID = @VendorsID AND IsDeleted = 0";
            using (SqlConnection con = new SqlConnection(connString))
            {
                var rfq = con.Query<RequestForQuotationBO>(rfqQuery, new { @RequestForQuotationId = RequestForQuotationId }).FirstOrDefault();
                var rfqitemList = con.Query<RequestForQuotationItemDetailsBO>(rfqItemQuery, new { @RequestForQuotationId = RequestForQuotationId }).ToList();
                var rfqSupplierList = con.Query<CompanyBO>(rfqSupplierQuery, new { @VendorsID = VendorsID }).ToList();  
                
                rfq.itemDetails = rfqitemList; 
                rfq.companyDetails = rfqSupplierList;  
                  
                return rfq; 
            }
        }
        #endregion

        #region Insert function fro RFQ Supplier Details. 
        /// <summary>
        /// Rahul: Insert record Supplier Details.
        /// </summary>
        /// <param name="RFQSupplierDetailsMater"></param>                
        public ResponseMessageBO InsertRFQSupplierDetails(RFQ_VendorDetailsBO rfqSupplierDetailsMater) 
        {

            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                
                using (SqlConnection con = new SqlConnection(connString))
                {                    
                    SqlCommand cmdSupDtl = new SqlCommand("usp_tbl_RFQSupplierDetails_Insert", con); //created  
                    cmdSupDtl.CommandType = CommandType.StoredProcedure;
                    cmdSupDtl.Parameters.AddWithValue("@RequestForQuotationId", rfqSupplierDetailsMater.RequestForQuotationId); 
                    cmdSupDtl.Parameters.AddWithValue("@RFQNO", rfqSupplierDetailsMater.RFQNO);
                    cmdSupDtl.Parameters.AddWithValue("@Date", Convert.ToDateTime(System.DateTime.Now));
                    cmdSupDtl.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(System.DateTime.Now));                     
                    cmdSupDtl.Parameters.AddWithValue("@CurrencyID", rfqSupplierDetailsMater.CurrencyID);
                    cmdSupDtl.Parameters.AddWithValue("@CurrencyName", rfqSupplierDetailsMater.CurrencyName);
                    cmdSupDtl.Parameters.AddWithValue("@CurrencyPrice", rfqSupplierDetailsMater.IndianCurrencyValue); 
                    cmdSupDtl.Parameters.AddWithValue("@LocationId", rfqSupplierDetailsMater.LocationId);
                    cmdSupDtl.Parameters.AddWithValue("@LocationName", rfqSupplierDetailsMater.LocationName); 
                    cmdSupDtl.Parameters.AddWithValue("@DeliveryAddress", rfqSupplierDetailsMater.LocationAddress);                   
                    cmdSupDtl.Parameters.AddWithValue("@VendorsID", rfqSupplierDetailsMater.VendorsID);
                    cmdSupDtl.Parameters.AddWithValue("@CompanyName", rfqSupplierDetailsMater.CompanyName); 
                    cmdSupDtl.Parameters.AddWithValue("@Address", rfqSupplierDetailsMater.SupplierAddress);  
                    cmdSupDtl.Parameters.AddWithValue("@TotalAfterTax", rfqSupplierDetailsMater.TotalAfterTax);
                    cmdSupDtl.Parameters.AddWithValue("@GrandTotal", rfqSupplierDetailsMater.GrandTotal);
                    cmdSupDtl.Parameters.AddWithValue("@AdvancedPayment", rfqSupplierDetailsMater.AdvancedPayment);
                    cmdSupDtl.Parameters.AddWithValue("@Remarks", rfqSupplierDetailsMater.Remarks);
                    cmdSupDtl.Parameters.AddWithValue("@CGST", rfqSupplierDetailsMater.CGST);
                    cmdSupDtl.Parameters.AddWithValue("@SGST", rfqSupplierDetailsMater.SGST);
                    cmdSupDtl.Parameters.AddWithValue("@IGST", rfqSupplierDetailsMater.IGST);
                    cmdSupDtl.Parameters.AddWithValue("@TermsAndConditionID", 1);  
                    cmdSupDtl.Parameters.AddWithValue("@Terms", "First Term");  
                    cmdSupDtl.Parameters.AddWithValue("@CreatedByID", rfqSupplierDetailsMater.CreatedByID);
                    cmdSupDtl.Parameters.AddWithValue("@CreatedByDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReaderSupDtl = cmdSupDtl.ExecuteReader(); 
                    int RequestForQuotationId = 0;
                    int RFQ_VendorDetailsId = 0;    
                    while (dataReaderSupDtl.Read()) 
                    {
                        response.Status = Convert.ToBoolean(dataReaderSupDtl["Status"]);    
                        RequestForQuotationId = Convert.ToInt32(dataReaderSupDtl["RequestForQuotationId"]);
                        RFQ_VendorDetailsId = Convert.ToInt32(dataReaderSupDtl["RFQ_VendorDetailsId"]);                            
                    }
                    con.Close();    

                    var json = new JavaScriptSerializer();  
                    var data = json.Deserialize<Dictionary<string, string>[]>(rfqSupplierDetailsMater.TxtItemDetails);

                    List<RFQ_Vendor_ItemDetailsBO> rfqVendorItemDetails = new List<RFQ_Vendor_ItemDetailsBO>(); 

                    foreach (var item in data)
                    {
                        RFQ_Vendor_ItemDetailsBO objItemDetails = new RFQ_Vendor_ItemDetailsBO();                         
                        objItemDetails.RFQ_VendorDetailsId = RFQ_VendorDetailsId;   
                        objItemDetails.RequestForQuotationId = RequestForQuotationId;
                        objItemDetails.Item_ID = Convert.ToInt32(item.ElementAt(0).Value);
                        objItemDetails.Item_Code = item.ElementAt(1).Value.ToString();
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.ItemQuantity = Convert.ToDecimal(item.ElementAt(3).Value);
                        objItemDetails.ItemUnit = item.ElementAt(4).Value.ToString();
                        objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(5).Value);
                        //objItemDetails.CurrencyName = item.ElementAt(7).Value.ToString();
                        objItemDetails.CurrencyID = Convert.ToInt32(item.ElementAt(6).Value);
                        objItemDetails.CurrencyPrice = Convert.ToDouble(item.ElementAt(7).Value);
                        objItemDetails.ItemTaxValue = item.ElementAt(8).Value.ToString();
                        objItemDetails.TotalItemCost = Convert.ToDecimal(item.ElementAt(9).Value); 
                        objItemDetails.DeliveryDate = Convert.ToDateTime(item.ElementAt(10).Value);
                        objItemDetails.HSN_Code = item.ElementAt(11).Value.ToString();
                        objItemDetails.Remarks = item.ElementAt(12).Value.ToString();

                        rfqVendorItemDetails.Add(objItemDetails);
                    }

                    foreach (var item in rfqVendorItemDetails) 
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_RFQ_Vendor_ItemDetails_Insert", con); //CREATED 
                        cmdNew.CommandType = CommandType.StoredProcedure;
                        cmdNew.Parameters.AddWithValue("@RFQ_VendorDetailsId", item.RFQ_VendorDetailsId); 
                        cmdNew.Parameters.AddWithValue("@RequestForQuotationId", item.RequestForQuotationId);
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.Item_ID);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmdNew.Parameters.AddWithValue("@ItemQuantity", item.ItemQuantity);
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                        cmdNew.Parameters.AddWithValue("@CurrencyID", rfqSupplierDetailsMater.CurrencyID);
                        cmdNew.Parameters.AddWithValue("@CurrencyName", rfqSupplierDetailsMater.@CurrencyName);
                        cmdNew.Parameters.AddWithValue("@CurrencyPrice", item.CurrencyPrice); 
                        cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                        cmdNew.Parameters.AddWithValue("@TotalItemCost", item.TotalItemCost);
                        cmdNew.Parameters.AddWithValue("@DeliveryDate", item.DeliveryDate);
                        cmdNew.Parameters.AddWithValue("@HSN_Code", item.HSN_Code);
                        cmdNew.Parameters.AddWithValue("@Remarks", item.Remarks);                        
                        cmdNew.Parameters.AddWithValue("@CreatedByID", rfqSupplierDetailsMater.CreatedByID);
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