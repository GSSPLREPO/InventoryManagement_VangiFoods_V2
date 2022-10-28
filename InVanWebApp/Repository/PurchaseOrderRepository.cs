using InVanWebApp_BO;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using InVanWebApp.Common;
using InVanWebApp.DAL;
using System.Globalization;
using System.Data.Entity.Validation;

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

                            //DocumentNumber = reader["DocumentNumber"].ToString(),
                            //VendorsID = Convert.ToInt32(reader["VendorsID"]),
                            //DiscountValue = Convert.ToInt32(reader["DiscountValue"]), //set value
                            //VAT = Convert.ToInt32(reader["VAT"]),
                            //AddVAT = Convert.ToInt32(reader["AddVAT"]),
                            //CST = Convert.ToInt32(reader["CST"]),
                            //Terms = reader["Terms"].ToString(),
                            //TermsAndConditionID = Convert.ToInt32(reader["TermsAndConditionID"]),

                            //Cancelled = Convert.ToInt32(reader["Cancelled"]),
                            //ReasonForCancellation = reader["ReasonForCancellation"].ToString(),
                            //TotalAmount = Convert.ToInt32(reader["TotalAmount"]),
                            //Amendment = Convert.ToInt32(reader["Amendment"]),
                            //DeliveryAddress = reader["DeliveryAddress"].ToString(),

                            //Attachment = reader["Attachment"].ToString(),
                            //WorkOrderNo = reader["WorkOrderNo"].ToString(),
                            //Signature = reader["Signature"].ToString(),
                            //IndentNumber = reader["IndentNumber"].ToString(),
                            //Remarks = reader["Remarks"].ToString(),
                            //CreatedBy = Convert.ToInt32(reader["CreatedBy"]),

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
                    cmd.Parameters.AddWithValue("@Tittle", purchaseOrderMaster.Tittle);
                    cmd.Parameters.AddWithValue("@PONumber", purchaseOrderMaster.PONumber);
                    cmd.Parameters.AddWithValue("@PODate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@VendorsID", purchaseOrderMaster.VendorsID);
                    cmd.Parameters.AddWithValue("@CompanyName", purchaseOrderMaster.CompanyName);                    
                    cmd.Parameters.AddWithValue("@DiscountValue", purchaseOrderMaster.DiscountValue);
                    cmd.Parameters.AddWithValue("@CGST", purchaseOrderMaster.CGST);
                    cmd.Parameters.AddWithValue("@SGST", purchaseOrderMaster.SGST);
                    cmd.Parameters.AddWithValue("@IGST", purchaseOrderMaster.IGST);
                    cmd.Parameters.AddWithValue("@TermsAndConditionID", purchaseOrderMaster.TermsAndConditionID);
                    cmd.Parameters.AddWithValue("@Terms", purchaseOrderMaster.Terms);
                    cmd.Parameters.AddWithValue("@PurchaseOrderStatus", purchaseOrderMaster.PurchaseOrderStatus);
                    cmd.Parameters.AddWithValue("@Cancelled", purchaseOrderMaster.Cancelled);
                    cmd.Parameters.AddWithValue("@ReasonForCancellation", purchaseOrderMaster.ReasonForCancellation);
                    //cmd.Parameters.AddWithValue("@DraftFlag", purchaseOrderMaster.DraftFlag);
                    cmd.Parameters.AddWithValue("@DraftFlag", 0);
                    cmd.Parameters.AddWithValue("@Amendment", 0);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", purchaseOrderMaster.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@SupplierAddress", purchaseOrderMaster.SupplierAddress);
                    //cmd.Parameters.AddWithValue("@TotalPOAmount", purchaseOrderMaster.TotalPOAmount);
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

                    cmd.Parameters.AddWithValue("@Item_ID", purchaseOrderMaster.Item_ID);
                    cmd.Parameters.AddWithValue("@Item_Code", purchaseOrderMaster.Item_Code);
                    cmd.Parameters.AddWithValue("@ItemName", purchaseOrderMaster.ItemName);
                    cmd.Parameters.AddWithValue("@ItemQuantity", purchaseOrderMaster.ItemQuantity);
                    cmd.Parameters.AddWithValue("@ItemUnit", purchaseOrderMaster.ItemUnit);
                    cmd.Parameters.AddWithValue("@ItemUnitPrice", purchaseOrderMaster.ItemUnitPrice);
                    cmd.Parameters.AddWithValue("@ItemTaxValue", purchaseOrderMaster.ItemTaxValue);
                    //cmd.Parameters.AddWithValue("@TotalAmount", purchaseOrderMaster.TotalAmount); 
                    cmd.Parameters.AddWithValue("@TotalItemCost", purchaseOrderMaster.TotalItemCost);
                    cmd.Parameters.AddWithValue("@TotalAfterTax", purchaseOrderMaster.TotalAfterTax);
                    cmd.Parameters.AddWithValue("@GrandTotal", purchaseOrderMaster.GrandTotal);          

                    con.Open();

                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        //response.PONumber = dataReader["PONumber"].ToString();
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
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
                            PODate = Convert.ToDateTime(reader["PODate"].ToString()),
                            DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
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
                    cmd.Parameters.AddWithValue("@PODate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(System.DateTime.Now));
                    //cmd.Parameters.AddWithValue("@DocumentNumber", model.DocumentNumber);
                    cmd.Parameters.AddWithValue("@VendorsID", model.VendorsID);
                    cmd.Parameters.AddWithValue("@TermsAndConditionID", model.TermsAndConditionID);
                    //cmd.Parameters.AddWithValue("@PurchaseOrderStatus", model.PurchaseOrderStatus);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", model.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@SupplierAddress", model.SupplierAddress);
                    //cmd.Parameters.AddWithValue("@WorkOrderNo", model.WorkOrderNo);
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

    }
}