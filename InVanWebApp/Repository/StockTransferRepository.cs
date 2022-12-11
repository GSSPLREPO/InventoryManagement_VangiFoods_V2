using Dapper;
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
    public class StockTransferRepository : IStockTransferRepository
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

                            ID = Convert.ToInt32(reader["ID"]),
                            FromLocationId = Convert.ToInt32(reader["FromLocationId"]),
                            FromLocationName = reader["FromLocationName"].ToString(),
                            ToLocationId = Convert.ToInt32(reader["ToLocationId"]),
                            ToLocationName = reader["ToLocationName"].ToString(), 
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            Item_Code = reader["Item_Code"].ToString(),
                            Item_Name = reader["Item_Name"].ToString(),
                            TransferQuantity = Convert.ToInt32(reader["TransferQuantity"]),
                            //RequiredQuantity = Convert.ToInt32(reader["RequiredQuantity"]),
                            FinalQuantity = Convert.ToInt32(reader["FinalQuantity"]),
                            Remarks = reader["Remarks"].ToString(),
                            LastModifiedDate = Convert.ToDateTime(reader["LastModifiedDate"])                          


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

        #region Function for binding data Get From Location Wise Stock Quantity List.        
        public IEnumerable<LocationWiseStockBO> GetFromLocationMasterList(int id)
        {
            List<LocationWiseStockBO> resultList = new List<LocationWiseStockBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_LocationWiseStock_GetAll_Quantity", con);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new LocationWiseStockBO()
                        {
                            //OrganisationId = Convert.ToInt32(dataReader["OrganisationId"]), 
                            //OrganisationId = Convert.ToInt32(dataReader["OrganisationId"]), 
                            LocationID = Convert.ToInt32(dataReader["LocationID"]),
                            //LocationName = dataReader["LocationName"].ToString(), 
                            Quantity = Convert.ToDouble(dataReader["RequiredQuantity"])
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
        public IEnumerable<ItemBO> GetItemDetailsForDD(int ItemType)
        {
            List<ItemBO> ItemList = new List<ItemBO>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_GetItemListForStockTransfer", con);
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
        public ItemBO GetItemDetails(int itemID, int LocationId)
        {
            ItemBO ItemDetails = new ItemBO();            
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_GetItemDetailsByIdForStockTransfer", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemId", itemID);
                    cmd.Parameters.AddWithValue("@LocationId", LocationId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        //LocationWiseStockBO locationWiseStockDetails = new LocationWiseStockBO();
                        //locationWiseStockDetails.Quantity = Convert.ToDouble(reader["RequiredQuantity"]);
                        ItemDetails = new ItemBO()
                        {
                            Item_Name = reader["Item_Name"].ToString(),
                            Item_Code = reader["Item_Code"].ToString(),
                            UnitCode = reader["ItemUnit"].ToString(),
                            RequiredQuantity = Convert.ToDouble(reader["RequiredQuantity"])
                            //locationwsieStockQuntity.Add(ItemDetails);
                            //UnitPrice = Convert.ToDouble(reader["UnitPrice"]),
                            //ItemTaxValue = float.Parse(reader["ItemTaxValue"].ToString())
                        };
                  
                    }
                    con.Close();
               
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
               
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
                return ItemDetails;
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
                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(stockTransferMaster.TxtItemDetails);

                    List<LocationWiseStockBO> locationwsieStockDetails = new List<LocationWiseStockBO>();
                    List<StockTransferBO> transfersDetails = new List<StockTransferBO>();

                    foreach (var item in data)
                    {
                        StockTransferBO stockTransfer = new StockTransferBO();
                        stockTransfer.FromLocationId = stockTransferMaster.FromLocationId;
                        stockTransfer.ToLocationId = stockTransferMaster.ToLocationId;
                        stockTransfer.ItemId = Convert.ToInt32(item.ElementAt(0).Value);
                        stockTransfer.Item_Code = item.ElementAt(1).Value.ToString();
                        stockTransfer.Item_Name = item.ElementAt(2).Value.ToString();
                        stockTransfer.RequiredQuantity = Convert.ToDouble(item.ElementAt(3).Value);
                        stockTransfer.TransferQuantity = Convert.ToDouble(item.ElementAt(5).Value);
                        stockTransfer.FinalQuantity = Convert.ToDouble(item.ElementAt(6).Value);
                        stockTransfer.Remarks = item.ElementAt(7).Value.ToString();

                        transfersDetails.Add(stockTransfer);

                    }
                    foreach (var item in transfersDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_StockTransfer_Insert", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@FromLocationId", item.FromLocationId);
                        cmdNew.Parameters.AddWithValue("@ToLocationId", item.ToLocationId);
                        cmdNew.Parameters.AddWithValue("@ItemId", item.ItemId);
                        cmdNew.Parameters.AddWithValue("@Item_Name", item.Item_Name);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                        cmdNew.Parameters.AddWithValue("@TransferQuantity", item.TransferQuantity);
                        cmdNew.Parameters.AddWithValue("@RequiredQuantity", item.RequiredQuantity);
                        cmdNew.Parameters.AddWithValue("@FinalQuantity", item.FinalQuantity);
                        cmdNew.Parameters.AddWithValue("@Remarks", item.Remarks);
                        cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                        cmdNew.Parameters.AddWithValue("@CreatedBy", stockTransferMaster.CreatedBy);
                        //cmdNew.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                        //cmdNew.Parameters.AddWithValue("@LastModifiedBy", stockTransferMaster.CreatedBy);
                        
                        SqlDataReader dataReaderNew = cmdNew.ExecuteReader();
                        while (dataReaderNew.Read())
                        {
                            response.Status = Convert.ToBoolean(dataReaderNew["Status"]);
                        }
                        con.Close();
                    }

                    //Location Wise Stock Update 
                    foreach (var item in data)
                    {
                        LocationWiseStockBO objLocationWiseStockDetails = new LocationWiseStockBO();
                        //objItemDetails.PO_Id = PO_Id;
                        objLocationWiseStockDetails.ItemId = Convert.ToInt32(item.ElementAt(0).Value);                        
                        objLocationWiseStockDetails.Quantity = Convert.ToDouble(item.ElementAt(6).Value);
                        objLocationWiseStockDetails.Trans_Quantity = Convert.ToDouble(item.ElementAt(5).Value);
                        objLocationWiseStockDetails.LocationID = Convert.ToInt32(stockTransferMaster.FromLocationId);
                        locationwsieStockDetails.Add(objLocationWiseStockDetails);
                    }
                    //Location Wise Stock 
                    foreach (var item in locationwsieStockDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_LocationWiseStock_Insert", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        //cmdNew.Parameters.AddWithValue("@ID", item.ID);
                        cmdNew.Parameters.AddWithValue("@From_LocationID", stockTransferMaster.FromLocationId);
                        cmdNew.Parameters.AddWithValue("@To_LocationID", stockTransferMaster.ToLocationId);
                        cmdNew.Parameters.AddWithValue("@Quantity", item.Quantity);
                        cmdNew.Parameters.AddWithValue("@Transf_Quantity", item.Trans_Quantity);
                        cmdNew.Parameters.AddWithValue("@ItemId", item.ItemId);                        
                        cmdNew.Parameters.AddWithValue("@CreatedBy", stockTransferMaster.CreatedBy);
                        cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                        cmdNew.Parameters.AddWithValue("@LastModifiedBy", stockTransferMaster.CreatedBy);
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

        //#region
        //public StockTransferBO GetStockTransferById(int ID) 
        //{
        //    string stockTransferQuery = "SELECT * FROM PurchaseOrder WITH(NOLOCK) WHERE PurchaseOrderId = @purchaseOrderId AND IsDeleted = 0";
        //    string stockMasterItemQuery = "SELECT * FROM PurchaseOrderItemsDetails WITH(NOLOCK) WHERE PurchaseOrderId = @purchaseOrderId AND IsDeleted = 0";
        //    using (SqlConnection con = new SqlConnection(connString))
        //    {
        //        var stockTransfers = con.Query<StockTransferBO>(stockTransferQuery, new { @ItemId = ID }).FirstOrDefault();
        //        var stockMasterList = con.Query<InVanWebApp_BO.StockMaster>(stockMasterItemQuery, new { @ItemId = ID }).ToList(); 
        //        stockTransfers.itemDetails = stockMasterList; 
        //        return stockTransfers;   
        //    }
        //}
        //#endregion

    }
}