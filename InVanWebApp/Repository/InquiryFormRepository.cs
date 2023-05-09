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
    public class InquiryFormRepository : IInquiryFormRepository
    {
        //private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(RequestForQuotationRepository));

        #region  Bind grid
        /// <summary>
        /// Rahul : This function is for fecthing list of order master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InquiryFormBO> GetAll()
        {
            List<InquiryFormBO> inquiryFormMastersList = new List<InquiryFormBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InquiryForm_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var inquiryFormMasters = new InquiryFormBO()
                        {
                            InquiryID = Convert.ToInt32(reader["InquiryID"]), 
                            InquiryNumber = reader["InquiryNumber"].ToString(),
                            InquiryStatus = reader["InquiryStatus"].ToString(), 
                            DateOfInquiry = Convert.ToDateTime(reader["DateOfInquiry"]),
                            ContactPersonName = reader["ContactPersonName"].ToString(),
                            ClientEmail = reader["ClientEmail"].ToString(),                            
                            CompanyName = reader["CompanyName"].ToString(),                            
                            //SONumber = reader["SONumber"].ToString(),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),                            
                            ContactNo = (reader["ContactNo"].ToString())                            

                        };
                        inquiryFormMastersList.Add(inquiryFormMasters);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return inquiryFormMastersList;
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Rahul: Insert record.
        /// </summary>
        /// <param name="inquiryFormBOMaster"></param>                
        public ResponseMessageBO Insert(InquiryFormBO inquiryFormBOMaster)
        {

             ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {

                    SqlCommand cmd = new SqlCommand("usp_tbl_InquiryForm_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InquiryNumber", inquiryFormBOMaster.InquiryNumber);                    
                    cmd.Parameters.AddWithValue("@InquiryStatusID", inquiryFormBOMaster.InquiryStatusID);
                    cmd.Parameters.AddWithValue("@DateOfInquiry", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@ContactPersonName", inquiryFormBOMaster.ContactPersonName);
                    cmd.Parameters.AddWithValue("@ClientEmail", inquiryFormBOMaster.ClientEmail);
                    cmd.Parameters.AddWithValue("@ContactNo", inquiryFormBOMaster.ContactNo); 
                    cmd.Parameters.AddWithValue("@VendorsID", inquiryFormBOMaster.VendorsID); 
                    cmd.Parameters.AddWithValue("@CompanyName", inquiryFormBOMaster.CompanyName);
                    cmd.Parameters.AddWithValue("@LocationId", inquiryFormBOMaster.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", inquiryFormBOMaster.LocationName);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", inquiryFormBOMaster.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@SupplierAddress", inquiryFormBOMaster.SupplierAddress);
                    cmd.Parameters.AddWithValue("@CGST", inquiryFormBOMaster.CGST);
                    cmd.Parameters.AddWithValue("@SGST", inquiryFormBOMaster.SGST);
                    cmd.Parameters.AddWithValue("@IGST", inquiryFormBOMaster.IGST);
                    cmd.Parameters.AddWithValue("@TotalAfterTax", inquiryFormBOMaster.TotalAfterTax);
                    cmd.Parameters.AddWithValue("@GrandTotal", inquiryFormBOMaster.GrandTotal);
                    cmd.Parameters.AddWithValue("@AdvancedPayment", inquiryFormBOMaster.AdvancedPayment); 
                    cmd.Parameters.AddWithValue("@SONumber", inquiryFormBOMaster.SONumber);
                    cmd.Parameters.AddWithValue("@CurrencyID", inquiryFormBOMaster.CurrencyID);
                    cmd.Parameters.AddWithValue("@CurrencyName", inquiryFormBOMaster.CurrencyName);
                    cmd.Parameters.AddWithValue("@Remarks", inquiryFormBOMaster.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", inquiryFormBOMaster.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@LastModifiedBy", inquiryFormBOMaster.CreatedBy);                    

                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int InquiryID = 0;
                    
                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);  
                        InquiryID = Convert.ToInt32(dataReader["InquiryID"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(inquiryFormBOMaster.TxtItemDetails);

                    List<InquiryFormItemDetailsBO> itemDetails = new List<InquiryFormItemDetailsBO>();

                    foreach (var item in data)
                    {
                        InquiryFormItemDetailsBO objItemDetails = new InquiryFormItemDetailsBO(); 
                        objItemDetails.InquiryID = InquiryID;
                        objItemDetails.Item_ID = Convert.ToInt32(item.ElementAt(0).Value);
                        objItemDetails.Item_Code = item.ElementAt(1).Value.ToString();
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.ItemQuantity = Convert.ToDecimal(item.ElementAt(3).Value);
                        objItemDetails.ItemUnit = item.ElementAt(4).Value.ToString();
                        objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(5).Value);
                        objItemDetails.QuotedPrice = Convert.ToDouble(item.ElementAt(6).Value);
                        objItemDetails.ExpectedPrice = Convert.ToDouble(item.ElementAt(7).Value);
                        objItemDetails.CloserPrice = Convert.ToDouble(item.ElementAt(8).Value); 
                        objItemDetails.CurrencyName = item.ElementAt(9).Value.ToString();
                        objItemDetails.ItemTaxValue = Convert.ToDecimal(item.ElementAt(10).Value);
                        objItemDetails.TotalItemCost = Convert.ToDecimal(item.ElementAt(11).Value);
                        objItemDetails.DeliveryDate = Convert.ToDateTime(item.ElementAt(12).Value);
                        objItemDetails.HSN_Code = item.ElementAt(13).Value.ToString();
                        objItemDetails.Remarks = item.ElementAt(14).Value.ToString();

                        itemDetails.Add(objItemDetails);
                    }

                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_InquiryFormItemDetails_Insert", con);                         
                        cmdNew.CommandType = CommandType.StoredProcedure; 
                        cmdNew.Parameters.AddWithValue("@InquiryID", item.InquiryID); 
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.Item_ID);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                        cmdNew.Parameters.AddWithValue("@QuotedPrice", item.QuotedPrice);
                        cmdNew.Parameters.AddWithValue("@ExpectedPrice", item.ExpectedPrice);
                        cmdNew.Parameters.AddWithValue("@CloserPrice", item.CloserPrice); 
                        cmdNew.Parameters.AddWithValue("@ItemQuantity", item.ItemQuantity); 
                        cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        cmdNew.Parameters.AddWithValue("@TotalItemCost", item.TotalItemCost);
                        cmdNew.Parameters.AddWithValue("@HSN_Code", item.HSN_Code);
                        cmdNew.Parameters.AddWithValue("@DeliveryDate", item.DeliveryDate);
                        cmdNew.Parameters.AddWithValue("@Remarks", item.Remarks);
                        cmdNew.Parameters.AddWithValue("@CurrencyID", inquiryFormBOMaster.CurrencyID);  
                        cmdNew.Parameters.AddWithValue("@CurrencyName", item.CurrencyName);                         
                        cmdNew.Parameters.AddWithValue("@CreatedBy", inquiryFormBOMaster.CreatedBy);
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
                log.Error(ex.Message, ex);
            }
            return response;
        }
        #endregion

        #region Edit function 
        /// <summary>
        /// Rahul: edit record.  
        /// </summary>
        /// <param name="InquiryID"></param>        
        public InquiryFormBO GetInquiryFormById(int InquiryID)  
        {
            string inquiryFormQuery = "SELECT * FROM InquiryMaster WHERE InquiryID = @InquiryID AND IsDeleted = 0"; 
            string inquiryFormItemQuery = "SELECT * FROM InquiryFormItemDetails WHERE InquiryID = @InquiryID AND IsDeleted = 0"; 
            using (SqlConnection con = new SqlConnection(connString))
            {
                var inquiryForm = con.Query<InquiryFormBO>(inquiryFormQuery, new { @InquiryID = InquiryID }).FirstOrDefault();
                var inquiryFormList = con.Query<InquiryFormItemDetailsBO>(inquiryFormItemQuery, new { @InquiryID = InquiryID }).ToList();
                inquiryForm.itemDetails = inquiryFormList;
                return inquiryForm;
            }
        }
        #endregion 

        #region Update function 
        /// <summary>
        /// Rahul: Update record 
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(InquiryFormBO model) 
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString)) 
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InquiryMaster_Update", con); 
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InquiryID", model.InquiryID);
                    cmd.Parameters.AddWithValue("@InquiryNumber", model.InquiryNumber);
                    cmd.Parameters.AddWithValue("@InquiryStatusID", model.InquiryStatusID);
                    cmd.Parameters.AddWithValue("@DateOfInquiry", model.DateOfInquiry);
                    cmd.Parameters.AddWithValue("@ContactPersonName", model.ContactPersonName);
                    cmd.Parameters.AddWithValue("@ClientEmail", model.ClientEmail);
                    cmd.Parameters.AddWithValue("@ContactNo", model.ContactNo); 
                    cmd.Parameters.AddWithValue("@VendorsID", model.VendorsID);
                    cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", model.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@SupplierAddress", model.SupplierAddress);
                    cmd.Parameters.AddWithValue("@CGST", model.CGST);
                    cmd.Parameters.AddWithValue("@SGST", model.SGST);
                    cmd.Parameters.AddWithValue("@IGST", model.IGST);
                    cmd.Parameters.AddWithValue("@GrandTotal", model.GrandTotal);
                    cmd.Parameters.AddWithValue("@TotalAfterTax", model.TotalAfterTax);
                    cmd.Parameters.AddWithValue("@AdvancedPayment", model.AdvancedPayment);
                    cmd.Parameters.AddWithValue("@SONumber", model.SONumber);
                    cmd.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
                    cmd.Parameters.AddWithValue("@CurrencyName", model.CurrencyName);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);                    
                    cmd.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedBy);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));

                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.TxtItemDetails);

                    List<InquiryFormItemDetailsBO> itemDetails = new List<InquiryFormItemDetailsBO>();

                    foreach (var item in data)
                    {
                        InquiryFormItemDetailsBO objItemDetails = new InquiryFormItemDetailsBO();
                        objItemDetails.InquiryID = model.InquiryID;
                        objItemDetails.Item_ID = Convert.ToInt32(item.ElementAt(0).Value);
                        objItemDetails.Item_Code = item.ElementAt(1).Value.ToString();
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.ItemQuantity = Convert.ToDecimal(item.ElementAt(3).Value);
                        objItemDetails.ItemUnit = item.ElementAt(4).Value.ToString();
                        objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(5).Value);
                        objItemDetails.QuotedPrice = Convert.ToDouble(item.ElementAt(6).Value);
                        objItemDetails.ExpectedPrice = Convert.ToDouble(item.ElementAt(7).Value);
                        objItemDetails.CloserPrice = Convert.ToDouble(item.ElementAt(8).Value);
                        objItemDetails.CurrencyName = item.ElementAt(9).Value.ToString();
                        objItemDetails.ItemTaxValue = Convert.ToDecimal(item.ElementAt(10).Value);
                        objItemDetails.TotalItemCost = Convert.ToDecimal(item.ElementAt(11).Value);
                        objItemDetails.DeliveryDate = Convert.ToDateTime(item.ElementAt(12).Value);
                        objItemDetails.HSN_Code = item.ElementAt(13).Value.ToString();
                        objItemDetails.Remarks = item.ElementAt(14).Value.ToString();                        
                        objItemDetails.CreatedBy = model.LastModifiedBy; 

                        itemDetails.Add(objItemDetails);
                    }

                    var count = itemDetails.Count;
                    var i = 1;
                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_InquiryFormItemDetails_Update", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;
                        cmdNew.Parameters.AddWithValue("@InquiryID", item.InquiryID);
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.Item_ID);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                        cmdNew.Parameters.AddWithValue("@QuotedPrice", item.QuotedPrice);
                        cmdNew.Parameters.AddWithValue("@ExpectedPrice", item.ExpectedPrice);
                        cmdNew.Parameters.AddWithValue("@CloserPrice", item.CloserPrice);
                        cmdNew.Parameters.AddWithValue("@ItemQuantity", item.ItemQuantity);
                        cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        cmdNew.Parameters.AddWithValue("@TotalItemCost", item.TotalItemCost);
                        cmdNew.Parameters.AddWithValue("@HSN_Code", item.HSN_Code);
                        cmdNew.Parameters.AddWithValue("@DeliveryDate", item.DeliveryDate);
                        cmdNew.Parameters.AddWithValue("@Remarks", item.Remarks);
                        cmdNew.Parameters.AddWithValue("@CurrencyID", model.CurrencyID); 
                        cmdNew.Parameters.AddWithValue("@CurrencyName", item.CurrencyName);                        
                        //cmdNew.Parameters.AddWithValue("@CreatedBy", model.CreatedBy); 
                        //cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                        cmdNew.Parameters.AddWithValue("@LastModifiedBy", item.CreatedBy);
                        cmdNew.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));                                             

                        if (count == 1)
                            cmdNew.Parameters.AddWithValue("@OneItemIdentifier", 1);
                        else
                        {
                            cmdNew.Parameters.AddWithValue("@OneItemIdentifier", 0);
                            cmdNew.Parameters.AddWithValue("@IsDeleted", i); 
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
        /// <param name="InquiryID"></param>
        public void Delete(int InquiryID, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InquiryMasterDetails_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InquiryID", InquiryID);
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


    }
}