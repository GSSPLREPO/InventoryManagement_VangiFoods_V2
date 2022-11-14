using InVanWebApp_BO;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web;
using InVanWebApp.Common;
using InVanWebApp.DAL;
using System.Globalization;
using System.Data.Entity.Validation;
using Newtonsoft.Json;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using Dapper;

namespace InVanWebApp.Repository
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        //private readonly InVanDBContext _context;
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(PurchaseOrderRepository));

        #region  Bind grid
        /// <summary>
        /// Rahul : This function is for fecthing list of order master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PurchaseOrderBO> GetAll()
        {
            List<PurchaseOrderBO> purchaseOrderMastersList = new List<PurchaseOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PurchaseOrder_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var PurchaseOrderMasters = new PurchaseOrderBO()
                        {
                            PurchaseOrderId = Convert.ToInt32(reader["PurchaseOrderId"]),
                            Tittle = reader["Tittle"].ToString(),
                            PONumber = reader["PONumber"].ToString(),
                            PurchaseOrderStatus = reader["PurchaseOrderStatus"].ToString(),
                            SupplierAddress = reader["SupplierAddress"].ToString(),
                            LastModifiedBy = Convert.ToInt32(reader["LastModifiedBy"]),
                            LastModifiedDate = Convert.ToDateTime(reader["LastModifiedDate"]),
                            DraftFlag=Convert.ToBoolean(reader["DraftFlag"]),
                            InwardCount=Convert.ToInt32(reader["InwardCount"])
                        };
                        purchaseOrderMastersList.Add(PurchaseOrderMasters);
                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return purchaseOrderMastersList;

        }
        #endregion

        #region Insert function
        /// <summary>
        /// Rahul: Insert record.
        /// </summary>
        /// <param name="purchaseOrderMaster"></param>                
        public ResponseMessageBO Insert(PurchaseOrderBO purchaseOrderMaster)
        {
          
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    
                    SqlCommand cmd = new SqlCommand("usp_tbl_PurchaseOrder_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Tittle", purchaseOrderMaster.Tittle);
                    cmd.Parameters.AddWithValue("@PONumber", purchaseOrderMaster.PONumber);
                    //cmd.Parameters.AddWithValue("@PODate", Convert.ToDateTime(System.DateTime.Now));                    
                    //cmd.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@PODate", purchaseOrderMaster.PODate);
                    cmd.Parameters.AddWithValue("@DeliveryDate", purchaseOrderMaster.DeliveryDate);
                    cmd.Parameters.AddWithValue("@VendorsID", purchaseOrderMaster.VendorsID);
                    cmd.Parameters.AddWithValue("@CompanyName", purchaseOrderMaster.CompanyName);
                    cmd.Parameters.AddWithValue("@DiscountValue", purchaseOrderMaster.DiscountValue);
                    cmd.Parameters.AddWithValue("@CGST", purchaseOrderMaster.CGST);
                    cmd.Parameters.AddWithValue("@SGST", purchaseOrderMaster.SGST);
                    cmd.Parameters.AddWithValue("@IGST", purchaseOrderMaster.IGST);
                    cmd.Parameters.AddWithValue("@TermsAndConditionID", purchaseOrderMaster.TermsAndConditionID);
                    cmd.Parameters.AddWithValue("@Terms", purchaseOrderMaster.Terms);
                    //cmd.Parameters.AddWithValue("@PurchaseOrderStatus", purchaseOrderMaster.PurchaseOrderStatus);
                    cmd.Parameters.AddWithValue("@PurchaseOrderStatus", "In Progress");
                    cmd.Parameters.AddWithValue("@Cancelled", purchaseOrderMaster.Cancelled);
                    cmd.Parameters.AddWithValue("@ReasonForCancellation", purchaseOrderMaster.ReasonForCancellation);                    
                    cmd.Parameters.AddWithValue("@DraftFlag", 0);
                    cmd.Parameters.AddWithValue("@Amendment", 0);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", purchaseOrderMaster.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@SupplierAddress", purchaseOrderMaster.SupplierAddress);
                    cmd.Parameters.AddWithValue("@AdvancedPayment", purchaseOrderMaster.AdvancedPayment);
                    cmd.Parameters.AddWithValue("@Attachment", purchaseOrderMaster.Attachment);
                    cmd.Parameters.AddWithValue("@Signature", purchaseOrderMaster.Signature);
                    cmd.Parameters.AddWithValue("@IndentNumber", purchaseOrderMaster.IndentNumber);
                    cmd.Parameters.AddWithValue("@Remarks", purchaseOrderMaster.Remarks);
                    cmd.Parameters.AddWithValue("@ApprovedBy", 1);
                    cmd.Parameters.AddWithValue("@ApprovedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@CheckedBy", 1);
                    cmd.Parameters.AddWithValue("@CheckedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@CreatedBy", 1);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                    cmd.Parameters.AddWithValue("@LocationId", purchaseOrderMaster.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", purchaseOrderMaster.LocationName);
                    cmd.Parameters.AddWithValue("@TotalAfterTax", purchaseOrderMaster.TotalAfterTax);
                    cmd.Parameters.AddWithValue("@GrandTotal", purchaseOrderMaster.GrandTotal);                   

                    //con.Open();

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int PurchaseOrderId = 0;
                    while (dataReader.Read())
                    {
                        //response.PONumber = dataReader["PONumber"].ToString();
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                        PurchaseOrderId = Convert.ToInt32(dataReader["PurchaseOrderId"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(purchaseOrderMaster.TxtItemDetails);

                    List<PurchaseOrderItemsDetail> itemDetails = new List<PurchaseOrderItemsDetail>();
                    
                    foreach (var item in data)
                    {
                        PurchaseOrderItemsDetail objItemDetails = new PurchaseOrderItemsDetail();
                        objItemDetails.PurchaseOrderId = PurchaseOrderId;
                        objItemDetails.Item_ID = Convert.ToInt32(item.ElementAt(0).Value);
                        objItemDetails.Item_Code = item.ElementAt(1).Value.ToString();
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.ItemQuantity = Convert.ToDecimal(item.ElementAt(3).Value);
                        objItemDetails.ItemUnit = item.ElementAt(4).Value.ToString();
                        objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(5).Value);
                        objItemDetails.ItemTaxValue = Convert.ToDecimal(item.ElementAt(6).Value);
                        objItemDetails.TotalItemCost = Convert.ToDouble(item.ElementAt(7).Value);
                        itemDetails.Add(objItemDetails);
                    }                                      
                    
                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_PurchaseOrderItemsDetails_Insert", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@PurchaseOrderId", item.PurchaseOrderId);
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.Item_ID);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                        cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                        cmdNew.Parameters.AddWithValue("@ItemQuantity", item.ItemQuantity);
                        cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        cmdNew.Parameters.AddWithValue("@TotalItemCost", item.TotalItemCost);
                        cmdNew.Parameters.AddWithValue("@CreatedBy", 1);
                        cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                        cmdNew.Parameters.AddWithValue("@LastModifiedBy", 1);
                        cmdNew.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));    

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

        #region Update functions
        /// <summary>
        /// Rahul: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="PurchaseOrderId"></param>
        /// <returns></returns>

        public PurchaseOrderBO GetById(int PurchaseOrderId)
        {
            var result = new PurchaseOrderBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PurchaseOrder_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PurchaseOrderId", PurchaseOrderId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = new PurchaseOrderBO()
                        {
                            PurchaseOrderId = Convert.ToInt32(reader["PurchaseOrderId"]),                            
                            Tittle = string.IsNullOrEmpty(reader["Tittle"].ToString()) ? "" : reader["Tittle"].ToString(),
                            PONumber = string.IsNullOrEmpty(reader["PONumber"].ToString()) ? "" : reader["PONumber"].ToString(),
                            //PODate = reader["PODate"].ToString()==null ? null : Convert.ToDateTime(reader["PODate"].ToString()),
                            //PODate = Convert.ToDateTime(reader["PODate"].ToString()),
                            PODate = DateTime.ParseExact(reader["PODate"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture),
                            DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"].ToString()),
                            VendorsID = Convert.ToInt32(reader["VendorsID"]),
                            CompanyName = reader["CompanyName"] == null ? "" : reader["CompanyName"].ToString(),
                            TermsAndConditionID = Convert.ToInt32(reader["TermsAndConditionID"]),
                            Terms = reader["Terms"] == null ? "" : reader["Terms"].ToString(),
                            ////PurchaseOrderStatus = reader["PurchaseOrderStatus"].ToString(),
                            //LocationId = Convert.ToInt32(reader["LocationId"]),
                            //LocationName = reader["LocationName"]==null?"": reader["LocationName"].ToString(),
                            //DeliveryAddress = reader["DeliveryAddress"]==null?"":reader["DeliveryAddress"].ToString(),
                            //SupplierAddress = reader["SupplierAddress"]==null?"": reader["SupplierAddress"].ToString(),
                            //Amendment = Convert.ToInt32(reader["Amendment"]),
                            //Signature = reader["Signature"]==null?"": reader["Signature"].ToString(),
                            //IndentNumber = reader["IndentNumber"]==null?"": reader["IndentNumber"].ToString(),
                            //Remarks = reader["Remarks"]==null?"": reader["Remarks"].ToString(),
                            //Item_ID = Convert.ToInt32(reader["Item_ID"]),
                            //Item_Code = reader["Item_Code"]==null?"": reader["Item_Code"].ToString(),
                            //ItemDescription = reader["ItemDescription"]==null?"": reader["ItemDescription"].ToString(),
                            //ItemQuantity = Convert.ToInt32(reader["ItemQuantity"]),
                            //ItemUnit = reader["ItemUnit"]==null?"": reader["ItemUnit"].ToString(),
                            //ItemPrice = Convert.ToInt32(reader["ItemPrice"]),
                            //ItemTax = reader["ItemTax"]==null?"": reader["ItemTax"].ToString(),
                            //TotalItemCost = Convert.ToInt32(reader["TotalItemCost"]),
                            //TotalAfterTax = Convert.ToInt32(reader["TotalAfterTax"]),
                            //GrandTotal = Convert.ToInt32(reader["GrandTotal"]),
                            //AdvancedPAyment = Convert.ToInt32(reader["AdvancedPAyment"]),
                            ////CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            ////CreatedBy = Convert.ToInt32(reader["CreatedBy"]),
                            ////DocumentNumber = reader["DocumentNumber"].ToString(),
                            ////DiscountValue = Convert.ToInt32(reader["DiscountValue"]), //set value
                            ////VAT = Convert.ToInt32(reader["VAT"]),
                            ////AddVAT = Convert.ToInt32(reader["AddVAT"]),
                            ////CST = Convert.ToInt32(reader["CST"]),                         
                            ////Cancelled = Convert.ToInt32(reader["Cancelled"]),
                            ////ReasonForCancellation = reader["ReasonForCancellation"].ToString(),
                            ////TotalAmount = Convert.ToInt32(reader["TotalAmount"]),
                            ////Attachment = reader["Attachment"].ToString(),
                            ////WorkOrderNo = reader["WorkOrderNo"].ToString(),
                            //LastModifiedBy = Convert.ToInt32(reader["LastModifiedBy"]),
                            //LastModifiedDate = Convert.ToDateTime(reader["LastModifiedDate"])
                        };
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return result;
            //return _context.UnitMasters.Find(UnitID);
        }

        /// <summary>
        /// Rahul: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(PurchaseOrderBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PurchaseOrder_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PurchaseOrderId", model.PurchaseOrderId);
                    cmd.Parameters.AddWithValue("@Tittle", model.Tittle);
                    cmd.Parameters.AddWithValue("@PONumber", model.PONumber);
                    cmd.Parameters.AddWithValue("@PODate", model.PODate);
                    cmd.Parameters.AddWithValue("@DeliveryDate", model.DeliveryDate);
                    //cmd.Parameters.AddWithValue("@DocumentNumber", model.DocumentNumber);
                    cmd.Parameters.AddWithValue("@VendorsID", model.VendorsID);
                    cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
                    cmd.Parameters.AddWithValue("@TermsAndConditionID", model.TermsAndConditionID);
                    cmd.Parameters.AddWithValue("@Terms", model.Terms);
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    //cmd.Parameters.AddWithValue("@PurchaseOrderStatus", model.PurchaseOrderStatus);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", model.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@SupplierAddress", model.SupplierAddress);
                    //cmd.Parameters.AddWithValue("@WorkOrderNo", model.WorkOrderNo);
                    cmd.Parameters.AddWithValue("@Amendment", model.Amendment);
                    cmd.Parameters.AddWithValue("@AdvancedPayment", model.AdvancedPayment);
                    cmd.Parameters.AddWithValue("@Attachment", model.Attachment);
                    cmd.Parameters.AddWithValue("@Signature", model.Signature);
                    cmd.Parameters.AddWithValue("@IndentNumber", model.IndentNumber);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    //cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    //cmd.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedBy);
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
                return response;
            }

            //_context.Entry(unitMaster).State = EntityState.Modified;
        }

        #endregion

        #region Delete function

        /// <summary>
        /// Delete record by ID
        /// </summary>
        /// <param name="PurchaseOrderId"></param>
        public void Delete(int PurchaseOrderId, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_DeletePurchaseOrder_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PurchaseOrderId", PurchaseOrderId);
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

        #region Function for binding dropdown Get Company List.
        public IEnumerable<PurchaseOrderBO> GetCompanyList()
        {
            List<PurchaseOrderBO> resultList = new List<PurchaseOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_CompanyList_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new PurchaseOrderBO()
                        {
                            VendorsID = Convert.ToInt32(dataReader["VendorsID"]),
                            CompanyName = dataReader["CompanyName"].ToString()
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

        #region Function for binding textarea Get Company Address List.
        public IEnumerable<PurchaseOrderBO> GetCompanyAddressList(int id)
        {
            List<PurchaseOrderBO> resultList = new List<PurchaseOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_CompanyAddressList_GetAll", con);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new PurchaseOrderBO()
                        {
                            //VendorsID = Convert.ToInt32(dataReader["VendorsID"]),
                            SupplierAddress = dataReader["SupplierAddress"].ToString()
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


        #region Function for binding dropdown Get Terms And Condition List.
        public IEnumerable<PurchaseOrderBO> GetTermsAndConditionList()
        {
            List<PurchaseOrderBO> resultList = new List<PurchaseOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_TermsAndCondition_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new PurchaseOrderBO()
                        {
                            TermsAndConditionID = Convert.ToInt32(dataReader["TermsAndConditionID"]),
                            Terms = dataReader["Terms"].ToString()
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


        #region Function for binding dropdown GetLocationNameList.
        public IEnumerable<PurchaseOrderBO> GetLocationNameList()
        {
            List<PurchaseOrderBO> resultList = new List<PurchaseOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_LocationName_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new PurchaseOrderBO()
                        {
                            LocationId = Convert.ToInt32(dataReader["LocationId"]),
                            LocationName = dataReader["LocationName"].ToString()
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


        #region Function for binding data Get Location Master List.
        //public IEnumerable<PurchaseOrderBO> GetOrganisationsList() 
        public IEnumerable<PurchaseOrderBO> GetLocationMasterList(int id)
        {
            List<PurchaseOrderBO> resultList = new List<PurchaseOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    //SqlCommand cmd = new SqlCommand("usp_tbl_Organisations_GetAll", con);
                    SqlCommand cmd = new SqlCommand("usp_tbl_LocationMaster_GetAll_DeliveryAddress", con);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new PurchaseOrderBO()
                        {
                            //OrganisationId = Convert.ToInt32(dataReader["OrganisationId"]), 
                            //OrganisationId = Convert.ToInt32(dataReader["OrganisationId"]), 
                            //LocationId = Convert.ToInt32(dataReader["LocationId"]), 
                            //LocationName = dataReader["LocationName"].ToString(), 
                            DeliveryAddress = dataReader["DeliveryAddress"].ToString()
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

        #region Get list of Items for PO and OC dropdown
        public IEnumerable<ItemBO> GetItemDetailsForDD(int ItemType)
        {
            List<ItemBO> ItemList = new List<ItemBO>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_GetItemListForPOandOC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemType", ItemType);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var item = new ItemBO()
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Item_Code = reader["Item_Code"].ToString()
                    };
                    ItemList.Add(item);
                }
                con.Close();
                return ItemList;
            }
        }

        #endregion

        #region Get details of Items by ID
        public ItemBO GetItemDetails(int itemID)
        {
            try
            {
                ItemBO ItemDetails = new ItemBO();
                //ItemCostMasterBO ItemCostDetails = new ItemCostMasterBO();
                //ItemTaxMasterBO ItemTaxDetails = new ItemTaxMasterBO();
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
                            UnitPrice = Convert.ToDouble(reader["UnitPrice"]),
                            ItemTaxValue = float.Parse(reader["ItemTaxValue"].ToString())
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


        public PurchaseOrderBO GetPurchaseOrderById(int PurchaseOrderId)
        {
            string purchaseOrderQuery = "SELECT * FROM PurchaseOrder WITH(NOLOCK) WHERE PurchaseOrderId = @purchaseOrderId AND IsDeleted = 0";
            string purchaseOrderItemQuery = "SELECT * FROM PurchaseOrderItemsDetails WITH(NOLOCK) WHERE PurchaseOrderId = @purchaseOrderId AND IsDeleted = 0";
            using (SqlConnection con = new SqlConnection(connString))
            {
                var purchaseOrder = con.Query<PurchaseOrderBO>(purchaseOrderQuery, new { @purchaseOrderId = PurchaseOrderId }).FirstOrDefault();
                var purchaseOrderList = con.Query<PurchaseOrderItemsDetails>(purchaseOrderItemQuery, new { @purchaseOrderId = PurchaseOrderId }).ToList();
                purchaseOrder.itemDetails = purchaseOrderList;
                return purchaseOrder;
            }
        }

        #region Amendment
        public ResponseMessageBO SaveAmendment(PurchaseOrderBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            int AmendmenrId = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PurchaseOrderAmendment_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@PurchaseOrderId", model.PurchaseOrderId);
                    cmd.Parameters.AddWithValue("@POAmendNo", model.PONumber);
                    cmd.Parameters.AddWithValue("@POAmendDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@TotalAmount", model.GrandTotal);
                    cmd.Parameters.AddWithValue("@PaymentsTerms", null);
                    cmd.Parameters.AddWithValue("@DeliveryTerms", null);
                    cmd.Parameters.AddWithValue("@ApprovalStatus", 0);
                    cmd.Parameters.AddWithValue("@ApprovalRemarks", null);
                    cmd.Parameters.AddWithValue("@ApprovedById", 1);
                    cmd.Parameters.AddWithValue("@ApprovedByDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@CheckedById", 1);
                    cmd.Parameters.AddWithValue("@CheckedByDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@IsDeleted", 0);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedBy);
                    cmd.Parameters.AddWithValue("@Amendment", model.Amendment);
                    cmd.Parameters.AddWithValue("@TotalAfterTax", model.TotalAfterTax);
                    cmd.Parameters.AddWithValue("@GrandTotal", model.GrandTotal);

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                        AmendmenrId = Convert.ToInt32(dataReader["AmendmenrId"]);
                    }
                    con.Close();


                    if (AmendmenrId > 0)
                    {


                        var json = new JavaScriptSerializer();
                        var data = json.Deserialize<Dictionary<string, string>[]>(model.TxtItemDetails);

                        List<PurchaseOrderAmendmentDetailBO> itemDetails = new List<PurchaseOrderAmendmentDetailBO>();

                        foreach (var item in data)
                        {
                            PurchaseOrderAmendmentDetailBO objItemDetails = new PurchaseOrderAmendmentDetailBO();
                            objItemDetails.PurchaseOrderAmendmentId = AmendmenrId;
                            objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(0).Value);
                            objItemDetails.Item_Code = item.ElementAt(1).Value.ToString();
                            objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                            objItemDetails.Quantity = Convert.ToDecimal(item.ElementAt(3).Value);
                            objItemDetails.AmendQuantity = Convert.ToDecimal(item.ElementAt(4).Value);
                            objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(6).Value);
                            //objItemDetails.AmendRate =Convert.ToDecimal(item.ElementAt(8).Value);
                            objItemDetails.IsDeleted = false;
                            objItemDetails.CreatedBy = model.CreatedBy;
                            objItemDetails.CreatedDate = model.CreatedDate;
                            objItemDetails.TotalItemCost = Convert.ToDecimal(item.ElementAt(8).Value);
                            objItemDetails.ItemTaxValue = Convert.ToDecimal(item.ElementAt(7).Value);
                            itemDetails.Add(objItemDetails);
                        }

                        foreach (var item in itemDetails)
                        {
                            con.Open();
                            SqlCommand cmdNew = new SqlCommand("usp_tbl_PurchaseOrderAmendmentDetails_Insert", con);
                            cmdNew.CommandType = CommandType.StoredProcedure;

                            cmdNew.Parameters.AddWithValue("@PurchaseOrderAmendmentId", item.PurchaseOrderAmendmentId);
                            cmdNew.Parameters.AddWithValue("@ItemId", item.ItemId);
                            cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                            cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                            cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                            cmdNew.Parameters.AddWithValue("@Quantity", item.Quantity);
                            cmdNew.Parameters.AddWithValue("@AmendQuantity", item.AmendQuantity);
                            cmdNew.Parameters.AddWithValue("@AmendRate", item.AmendRate);
                            cmdNew.Parameters.AddWithValue("@Discount", null);
                            cmdNew.Parameters.AddWithValue("@CGST", null);
                            cmdNew.Parameters.AddWithValue("@SGST", null);
                            cmdNew.Parameters.AddWithValue("@IGST", null);
                            cmdNew.Parameters.AddWithValue("@AmendDiscount", null);
                            cmdNew.Parameters.AddWithValue("@AmendCgst", null);
                            cmdNew.Parameters.AddWithValue("@AmendSgst", null);
                            cmdNew.Parameters.AddWithValue("@AmendIgst", null);
                            cmdNew.Parameters.AddWithValue("@IsDeleted", false);
                            cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                            cmdNew.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                            cmdNew.Parameters.AddWithValue("@LastModifiedDate", null);
                            cmdNew.Parameters.AddWithValue("@LastModifiedBy", null);
                            cmdNew.Parameters.AddWithValue("@PurchaseOrderId", model.PurchaseOrderId);
                            cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                            cmdNew.Parameters.AddWithValue("@TotalItemCost", item.TotalItemCost);


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
                log.Error(ex.Message, ex);
            }
            return response;
        }
        #endregion

    }
}