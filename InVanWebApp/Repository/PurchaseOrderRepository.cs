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
                            Amendment = Convert.ToInt32(reader["Amendment"]),
                            Tittle = reader["Tittle"].ToString(),
                            PONumber = reader["PONumber"].ToString(),
                            PurchaseOrderStatus = reader["PurchaseOrderStatus"].ToString(),
                            SupplierAddress = reader["SupplierAddress"].ToString(),
                            LastModifiedBy = Convert.ToInt32(reader["LastModifiedBy"]),
                            LastModifiedDate = Convert.ToDateTime(reader["LastModifiedDate"]),
                            DraftFlag = Convert.ToBoolean(reader["DraftFlag"]),
                            InwardCount = Convert.ToInt32(reader["InwardCount"])
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
                    cmd.Parameters.AddWithValue("@PODate", purchaseOrderMaster.PODate);
                    cmd.Parameters.AddWithValue("@DeliveryDate", purchaseOrderMaster.DeliveryDate);
                    cmd.Parameters.AddWithValue("@VendorsID", purchaseOrderMaster.VendorsID);
                    cmd.Parameters.AddWithValue("@CompanyName", purchaseOrderMaster.CompanyName);
                    cmd.Parameters.AddWithValue("@DiscountValue", purchaseOrderMaster.DiscountValue);
                    cmd.Parameters.AddWithValue("@CGST", purchaseOrderMaster.CGST);
                    cmd.Parameters.AddWithValue("@SGST", purchaseOrderMaster.SGST);
                    cmd.Parameters.AddWithValue("@IGST", purchaseOrderMaster.IGST);
                    cmd.Parameters.AddWithValue("@TermsAndConditionID", purchaseOrderMaster.TermsAndConditionID);
                    cmd.Parameters.AddWithValue("@PurchaseOrderStatus", "Open");
                    cmd.Parameters.AddWithValue("@Cancelled", purchaseOrderMaster.Cancelled);
                    cmd.Parameters.AddWithValue("@ReasonForCancellation", purchaseOrderMaster.ReasonForCancellation);
                    cmd.Parameters.AddWithValue("@DraftFlag", purchaseOrderMaster.DraftFlag);
                    cmd.Parameters.AddWithValue("@Amendment", purchaseOrderMaster.Amendment);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", purchaseOrderMaster.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@SupplierAddress", purchaseOrderMaster.SupplierAddress);
                    cmd.Parameters.AddWithValue("@AdvancedPayment", purchaseOrderMaster.AdvancedPayment);
                    cmd.Parameters.AddWithValue("@Attachment", purchaseOrderMaster.Attachment);
                    cmd.Parameters.AddWithValue("@Signature", purchaseOrderMaster.Signature);
                    cmd.Parameters.AddWithValue("@IndentNumber", purchaseOrderMaster.IndentNumber);
                    cmd.Parameters.AddWithValue("@Remarks", purchaseOrderMaster.Remarks);
                    cmd.Parameters.AddWithValue("@ApprovedBy", purchaseOrderMaster.CreatedBy);
                    cmd.Parameters.AddWithValue("@ApprovedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@CheckedBy", purchaseOrderMaster.CreatedBy);
                    cmd.Parameters.AddWithValue("@CheckedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@CreatedBy", purchaseOrderMaster.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@LastModifiedBy", purchaseOrderMaster.CreatedBy);
                    cmd.Parameters.AddWithValue("@LocationId", purchaseOrderMaster.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", purchaseOrderMaster.LocationName);
                    cmd.Parameters.AddWithValue("@TotalAfterTax", purchaseOrderMaster.TotalAfterTax);
                    cmd.Parameters.AddWithValue("@GrandTotal", purchaseOrderMaster.GrandTotal);
                    cmd.Parameters.AddWithValue("@TermDescription", purchaseOrderMaster.Terms);

                    //FN: Added the below field for Indent, currency and terms description
                    cmd.Parameters.AddWithValue("@IndentID", purchaseOrderMaster.IndentID);
                    cmd.Parameters.AddWithValue("@CurrencyID", purchaseOrderMaster.CurrencyID);
                    cmd.Parameters.AddWithValue("@CurrencyName", purchaseOrderMaster.CurrencyName);

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

                    List<PurchaseOrderItemsDetails> itemDetails = new List<PurchaseOrderItemsDetails>();

                    foreach (var item in data)
                    {
                        PurchaseOrderItemsDetails objItemDetails = new PurchaseOrderItemsDetails();
                        objItemDetails.PurchaseOrderId = PurchaseOrderId;
                        objItemDetails.Item_Code = item.ElementAt(0).Value.ToString();
                        objItemDetails.Item_ID = Convert.ToInt32(item.ElementAt(1).Value);
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.RequiredQuantity = float.Parse(item.ElementAt(3).Value);
                        objItemDetails.ItemQuantity = Convert.ToDecimal(item.ElementAt(4).Value);
                        objItemDetails.BalanceQuantity = float.Parse(item.ElementAt(5).Value);
                        objItemDetails.ItemUnit = item.ElementAt(6).Value.ToString();
                        objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(7).Value);
                        objItemDetails.ItemTaxValue = item.ElementAt(9).Value.ToString();
                        objItemDetails.TotalItemCost = Convert.ToDouble(item.ElementAt(10).Value);
                        objItemDetails.CreatedBy = purchaseOrderMaster.CreatedBy;
                        //Added the below field for Currency
                        objItemDetails.CurrencyID = purchaseOrderMaster.CurrencyID;
                        objItemDetails.CurrencyName = purchaseOrderMaster.CurrencyName;

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
                        cmdNew.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                        cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                        cmdNew.Parameters.AddWithValue("@BalanceQty", item.BalanceQuantity);
                        cmdNew.Parameters.AddWithValue("@RequiredQty", item.RequiredQuantity);    

                        //Added the below field for Indent, currency and terms description
                        cmdNew.Parameters.AddWithValue("@CurrencyID", purchaseOrderMaster.CurrencyID);
                        cmdNew.Parameters.AddWithValue("@CurrencyName", purchaseOrderMaster.CurrencyName);

                        cmdNew.Parameters.AddWithValue("@IndentID", purchaseOrderMaster.IndentID);

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
        public PurchaseOrderBO GetPurchaseOrderById(int PurchaseOrderId)
        {
            string purchaseOrderQuery = "SELECT * FROM PurchaseOrder WHERE PurchaseOrderId = @purchaseOrderId AND IsDeleted = 0";
            string purchaseOrderItemQuery = "SELECT * FROM PurchaseOrderItemsDetails WHERE PurchaseOrderId = @purchaseOrderId AND IsDeleted = 0";
            using (SqlConnection con = new SqlConnection(connString))
            {
                var purchaseOrder = con.Query<PurchaseOrderBO>(purchaseOrderQuery, new { @purchaseOrderId = PurchaseOrderId }).FirstOrDefault();
                var purchaseOrderList = con.Query<PurchaseOrderItemsDetails>(purchaseOrderItemQuery, new { @purchaseOrderId = PurchaseOrderId }).ToList();
                purchaseOrder.itemDetails = purchaseOrderList;
                return purchaseOrder;
            }
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
                    cmd.Parameters.AddWithValue("@DraftFlag", model.DraftFlag);
                    cmd.Parameters.AddWithValue("@VendorsID", model.VendorsID);
                    cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
                    cmd.Parameters.AddWithValue("@TermsAndConditionID", model.TermsAndConditionID);
                    cmd.Parameters.AddWithValue("@Terms", model.Terms);
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", model.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@SupplierAddress", model.SupplierAddress);
                    cmd.Parameters.AddWithValue("@Amendment", model.Amendment);
                    cmd.Parameters.AddWithValue("@AdvancedPayment", model.AdvancedPayment);
                    cmd.Parameters.AddWithValue("@Attachment", model.Attachment);
                    cmd.Parameters.AddWithValue("@Signature", model.Signature);
                    cmd.Parameters.AddWithValue("@IndentNumber", model.IndentNumber);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedBy);
                    cmd.Parameters.AddWithValue("@TotalAfterTax", model.TotalAfterTax);
                    cmd.Parameters.AddWithValue("@GrandTotal", model.GrandTotal);
                    //FN: Added the below field for Indent, currency and terms description

                    cmd.Parameters.AddWithValue("@IndentID", model.IndentID);
                    cmd.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
                    cmd.Parameters.AddWithValue("@CurrencyName", model.CurrencyName);


                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.TxtItemDetails);

                    List<PurchaseOrderItemsDetails> itemDetails = new List<PurchaseOrderItemsDetails>();

                    foreach (var item in data)
                    {
                        PurchaseOrderItemsDetails objItemDetails = new PurchaseOrderItemsDetails();
                        objItemDetails.PurchaseOrderId = model.PurchaseOrderId;
                        objItemDetails.Item_Code = item.ElementAt(0).Value.ToString();
                        objItemDetails.Item_ID = Convert.ToInt32(item.ElementAt(1).Value);
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.RequiredQuantity =float.Parse(item.ElementAt(3).Value);
                        objItemDetails.ItemQuantity = Convert.ToDecimal(item.ElementAt(4).Value);
                        objItemDetails.BalanceQuantity =float.Parse(item.ElementAt(5).Value);
                        objItemDetails.ItemUnit = item.ElementAt(6).Value.ToString();
                        objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(7).Value);
                        objItemDetails.ItemTaxValue = item.ElementAt(9).Value.ToString();
                        objItemDetails.TotalItemCost = Convert.ToDouble(item.ElementAt(10).Value);
                        objItemDetails.CreatedBy = model.LastModifiedBy;

                        itemDetails.Add(objItemDetails);
                    }

                    var count = itemDetails.Count;
                    var i = 1;
                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_PurchaseOrderItemsDetails_Update", con);
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
                        cmdNew.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                        cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                        cmdNew.Parameters.AddWithValue("@LastModifiedBy", item.CreatedBy);
                        cmdNew.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                        cmdNew.Parameters.AddWithValue("@RequiredQty", item.RequiredQuantity);
                        cmdNew.Parameters.AddWithValue("@BalanceQty", item.BalanceQuantity);

                        //Added the below field for Indent, currency and terms description
                        cmdNew.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
                        cmdNew.Parameters.AddWithValue("@CurrencyName", model.CurrencyName);
                        cmdNew.Parameters.AddWithValue("@IndentID", model.IndentID);

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
        /// <summary>
        /// The below function is not in used in Purchase Order but in use in some other
        /// modules.
        /// </summary>
        /// <param name="ItemType"></param>
        /// <returns></returns>
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
        public ItemBO GetItemDetails(int itemID, int currencyID)
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
                    cmd.Parameters.AddWithValue("@CurrencyID", currencyID); //added 
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
                            ItemTaxValue = float.Parse(reader["ItemTaxValue"].ToString()),
                            IndianCurrencyValue = reader["IndianCurrencyValue"].ToString()
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

        #region Amendment
        /// <summary>
        /// Created by : Raj
        /// Description: This is for saving the amendment values, but currently not in use.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                    //Fields for amendment table
                    cmd.Parameters.AddWithValue("@PurchaseOrderId", model.PurchaseOrderId);
                    cmd.Parameters.AddWithValue("@POAmendNo", model.PONumber);
                    cmd.Parameters.AddWithValue("@POAmendDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@TotalAmount", model.GrandTotal);
                    cmd.Parameters.AddWithValue("@PaymentsTerms", model.Terms);
                    //cmd.Parameters.AddWithValue("@DeliveryTerms", null);
                    //cmd.Parameters.AddWithValue("@ApprovalStatus", 0);
                    //cmd.Parameters.AddWithValue("@ApprovalRemarks", null);
                    //cmd.Parameters.AddWithValue("@ApprovedById", 1);
                    //cmd.Parameters.AddWithValue("@ApprovedByDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@CheckedById", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CheckedByDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@IsDeleted", 0);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    //cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    //cmd.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedBy);
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

        #region GetPOById function for timeline view 

        /// <summary>
        /// GetPOById record by ID, Rahul 08/12/2022. 
        /// </summary>
        /// <param name="ID"></param>
        public PurchaseOrderBO GetDetailsForTimelineView(int PO_Id)
        {
            PurchaseOrderBO poBo = new PurchaseOrderBO();
            string grnMasterQuery = "SELECT TOP(1) * FROM GRN_Master WHERE PO_Id = @purchaseOrderId AND IsDeleted = 0 order by CreatedDate desc";
            string poPaymentDetailsQuery = "SELECT TOP(1) * FROM PurchaseOrderPaymentDetails WHERE PurchaseOrderId = @purchaseOrderId AND IsDeleted = 0 order by CreatedDate desc ";
            using (SqlConnection con = new SqlConnection(connString))
            {
                var grnResult = con.Query<GRN_BO>(grnMasterQuery, new { @purchaseOrderId = PO_Id }).FirstOrDefault();
                var poPaymentResult = con.Query<POPaymentBO>(poPaymentDetailsQuery, new { @purchaseOrderId = PO_Id }).FirstOrDefault();
                if (grnResult == null)
                {
                    //poBo.GRNDate = null;
                    poBo.GRNCode = null;
                }
                else
                {
                    poBo.GRNDate = (DateTime)grnResult.GRNDate;
                    poBo.GRNCode = grnResult.GRNCode;
                }

                if (poPaymentResult == null)
                {
                    //poBo.PaymentDate = null;
                    poBo.InvoiceNumber = null;
                }
                else
                {
                    poBo.PaymentDate = (DateTime)poPaymentResult.PaymentDate;
                    poBo.InvoiceNumber = poPaymentResult.InvoiceNumber;
                }

                return poBo;
            }
        }
        #endregion

        ///Rahul added 02/12/2022. 
        #region Function for binding dropdown GetCurrencyPriceList.
        public IEnumerable<PurchaseOrderBO> GetCurrencyPriceList()
        {
            List<PurchaseOrderBO> resultList = new List<PurchaseOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_CurrencyMaster_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new PurchaseOrderBO()
                        {
                            CurrencyID = Convert.ToInt32(dataReader["CurrencyID"]),
                            CurrencyName = dataReader["CurrencyName"].ToString(),
                            IndianCurrencyValue = float.Parse(dataReader["IndianCurrencyValue"].ToString())

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

        #region Bind Indent dropdown
        public IEnumerable<IndentBO> GetIndentListForDropdown(string type = null)
        {
            List<IndentBO> resultList = new List<IndentBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Indent_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", type);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new IndentBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            IndentNo = reader["IndentNo"].ToString(),
                            IndentDate = Convert.ToDateTime(reader["IndentDate"]),
                            //IndentDueDate = Convert.ToDateTime(reader["IndentDueDate"]),
                            IndentStatus = reader["IndentStatus"].ToString(),
                            Description = reader["Description"].ToString(),
                            //IndentCount = Convert.ToInt32(reader["IndentCount"])
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

    }
}