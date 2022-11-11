using InVanWebApp.DAL;
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
    public class StockTransferRepository: IStockTransferRepository 
    {
        //private readonly InVanDBContext _context;
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(StockTransferRepository));
         
        #region  Bind grid
        /// <summary>
        /// Rahul : This function is for fecthing list of order master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StockTransferBO> GetAll()
        {
            List<StockTransferBO> stockTransferMastersList = new List<StockTransferBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_StockTransfer_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var StockTransferMasters = new StockTransferBO()  
                        {

                            //PurchaseOrderId = Convert.ToInt32(reader["PurchaseOrderId"]),
                            //Tittle = reader["Tittle"].ToString(),
                            //PONumber = reader["PONumber"].ToString(),
                            //PurchaseOrderStatus = reader["PurchaseOrderStatus"].ToString(),
                            //SupplierAddress = reader["SupplierAddress"].ToString(),
                            //LastModifiedBy = Convert.ToInt32(reader["LastModifiedBy"]),
                            //LastModifiedDate = Convert.ToDateTime(reader["LastModifiedDate"])

                        };
                        stockTransferMastersList.Add(StockTransferMasters); 
                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return stockTransferMastersList; 

        }
        #endregion

        #region Function for binding dropdown Get From Location Name List.
        public IEnumerable<StockTransferBO> GetFromLocationNameList() 
        {
            List<StockTransferBO> resultList = new List<StockTransferBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_StockTransferFromLocationName_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader(); 

                    while (dataReader.Read())
                    {
                        var result = new StockTransferBO()
                        {
                            FromLocationId = Convert.ToInt32(dataReader["FromLocationId"]),
                            FromLocationName = dataReader["FromLocationName"].ToString()
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

        #region Function for binding dropdown Get To Location Name List.
        public IEnumerable<StockTransferBO> GetToLocationNameList()
        {
            List<StockTransferBO> resultList = new List<StockTransferBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_StockTransferToLocationName_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new StockTransferBO()
                        {
                            ToLocationId = Convert.ToInt32(dataReader["ToLocationId"]), 
                            ToLocationName = dataReader["ToLocationName"].ToString()
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

        #region Get list of Items for Stock Transfer dropdown
        public IEnumerable<StockMasterBO> GetItemDetailsForDD(int ItemType)
        {
            List<StockMasterBO> ItemList = new List<StockMasterBO>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_GetItemListForStockTransfer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemType", ItemType);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var item = new StockMasterBO() 
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
        public StockMasterBO GetItemDetails(int itemID)
        {
            try
            {
                StockMasterBO ItemDetails = new StockMasterBO();                
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_GetItemDetailsByIdForStockTransfer", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemId", itemID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        ItemDetails = new StockMasterBO()
                        {
                            ItemName = reader["ItemName"].ToString(),
                            Item_Code = reader["Item_Code"].ToString(),
                            ItemUnit = reader["ItemUnit"].ToString(),
                            StockQuantity = Convert.ToDouble(reader["RequiredQuantity"]),
                            //UnitPrice = Convert.ToDouble(reader["UnitPrice"]),
                            //ItemTaxValue = float.Parse(reader["ItemTaxValue"].ToString())
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
        /// <param name="stockTransferMaster"></param>                
        public ResponseMessageBO Insert(StockTransferBO stockTransferMaster) 
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {

                    SqlCommand cmd = new SqlCommand("usp_tbl_StockTransfer_Insert", con); 
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@FromLocationId", stockTransferMaster.FromLocationId);
                    cmd.Parameters.AddWithValue("@ToLocationId", stockTransferMaster.ToLocationId);
                    //cmd.Parameters.AddWithValue("@FromLocationName", stockTransferMaster.FromLocationName); 
                    //cmd.Parameters.AddWithValue("@ToLocationName", stockTransferMaster.ToLocationName); 
                    cmd.Parameters.AddWithValue("@ItemId", stockTransferMaster.ItemId); 
                    cmd.Parameters.AddWithValue("@Item_Name", stockTransferMaster.Item_Name); 
                    cmd.Parameters.AddWithValue("@Item_Code", stockTransferMaster.Item_Code); 
                    cmd.Parameters.AddWithValue("@TransferQuantity", stockTransferMaster.TransferQuantity); 
                    cmd.Parameters.AddWithValue("@RequiredQuantity", stockTransferMaster.RequiredQuantity); 
                    cmd.Parameters.AddWithValue("@Remarks", stockTransferMaster.Remarks);                                                            
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@CreatedBy", 1);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                    //cmd.Parameters.AddWithValue("@LocationId", stockTransferMaster.LocationId);                    
                    //cmd.Parameters.AddWithValue("@TotalAfterTax", stockTransferMaster.TotalAfterTax);
                    //cmd.Parameters.AddWithValue("@GrandTotal", stockTransferMaster.GrandTotal);
                    //con.Open(); 

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int ID = 0; 
                    while (dataReader.Read())
                    {
                        //response.PONumber = dataReader["PONumber"].ToString();
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                        ID = Convert.ToInt32(dataReader["ID"]); 
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(stockTransferMaster.TxtItemDetails);

                    //List<PurchaseOrderItemsDetail> itemDetails = new List<PurchaseOrderItemsDetail>();

                    //foreach (var item in data)
                    //{
                    //    PurchaseOrderItemsDetail objItemDetails = new PurchaseOrderItemsDetail();
                    //    objItemDetails.PurchaseOrderId = PurchaseOrderId;
                    //    objItemDetails.Item_ID = Convert.ToInt32(item.ElementAt(0).Value);
                    //    objItemDetails.Item_Code = item.ElementAt(1).Value.ToString();
                    //    objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                    //    objItemDetails.ItemQuantity = Convert.ToDecimal(item.ElementAt(3).Value);
                    //    objItemDetails.ItemUnit = item.ElementAt(4).Value.ToString();
                    //    objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(5).Value);
                    //    objItemDetails.ItemTaxValue = Convert.ToDecimal(item.ElementAt(6).Value);
                    //    objItemDetails.TotalItemCost = Convert.ToDecimal(item.ElementAt(7).Value);
                    //    itemDetails.Add(objItemDetails);
                    //}

                    //foreach (var item in itemDetails)
                    //{
                    //    con.Open();
                    //    SqlCommand cmdNew = new SqlCommand("usp_tbl_PurchaseOrderItemsDetails_Insert", con);
                    //    cmdNew.CommandType = CommandType.StoredProcedure;

                    //    cmdNew.Parameters.AddWithValue("@PurchaseOrderId", item.PurchaseOrderId);
                    //    cmdNew.Parameters.AddWithValue("@Item_ID", item.Item_ID);
                    //    cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                    //    cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                    //    cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                    //    cmdNew.Parameters.AddWithValue("@ItemQuantity", item.ItemQuantity);
                    //    cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                    //    cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                    //    cmdNew.Parameters.AddWithValue("@TotalItemCost", item.TotalItemCost);
                    //    cmdNew.Parameters.AddWithValue("@CreatedBy", 1);
                    //    cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    //    cmdNew.Parameters.AddWithValue("@LastModifiedBy", 1);
                    //    cmdNew.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));

                    //    SqlDataReader dataReaderNew = cmdNew.ExecuteReader();

                    //    while (dataReaderNew.Read())
                    //    {
                    //        response.Status = Convert.ToBoolean(dataReaderNew["Status"]);
                    //    }
                    //    con.Close();
                    //}
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