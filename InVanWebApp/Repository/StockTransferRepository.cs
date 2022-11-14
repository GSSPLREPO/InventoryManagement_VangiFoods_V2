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

                    //SqlCommand cmd = new SqlCommand("usp_tbl_StockTransfer_Insert", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //con.Open();
                    //cmd.Parameters.AddWithValue("@FromLocationId", stockTransferMaster.FromLocationId);
                    //cmd.Parameters.AddWithValue("@ToLocationId", stockTransferMaster.ToLocationId);
                    ////cmd.Parameters.AddWithValue("@FromLocationName", stockTransferMaster.FromLocationName); 
                    ////cmd.Parameters.AddWithValue("@ToLocationName", stockTransferMaster.ToLocationName); 

                    //cmd.Parameters.AddWithValue("@ItemId", stockTransferMaster.ItemId); 
                    //cmd.Parameters.AddWithValue("@Item_Name", stockTransferMaster.Item_Name); 
                    //cmd.Parameters.AddWithValue("@Item_Code", stockTransferMaster.Item_Code); 
                    //cmd.Parameters.AddWithValue("@TransferQuantity", stockTransferMaster.TransferQuantity); 
                    //cmd.Parameters.AddWithValue("@RequiredQuantity", stockTransferMaster.RequiredQuantity); 
                    //cmd.Parameters.AddWithValue("@Remarks", stockTransferMaster.Remarks);                                                            
                    //cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    //cmd.Parameters.AddWithValue("@CreatedBy", 1);
                    //cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    //cmd.Parameters.AddWithValue("@LastModifiedBy", 1);

                    //con.Open(); 
                    //SqlDataReader dataReader = cmd.ExecuteReader();
                    //int PO_Id = 0; 
                    //while (dataReader.Read())
                    //{
                    //    //response.PONumber = dataReader["PONumber"].ToString();
                    //    response.Status = Convert.ToBoolean(dataReader["Status"]);
                    //    PO_Id = Convert.ToInt32(dataReader["ID"]);  
                    //}
                    //con.Close();

                    //[{"Item_ID":"7","Item_Code":"Code_102","ItemName":"Tomato","RequiredQuantity":"1000","ItemUnit":"Kg","TransferQuantity":"100","FinalQuantity":"900","Remarks":"ok"},
                    //{"ItemID_1 ":"8","itemCode_1":"Code_101","itemDescription_1":"Tomato gravy","itemQuantity_1":"2","itemUnit_1":"KG","transferQuantity_1":"200","finalQuantity_1 ":"-198","remarks_1":"ok"}]

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(stockTransferMaster.TxtItemDetails);

                    List<StockMasterBO> itemDetails = new List<StockMasterBO>();                    
                    List<StockTransferBO> transfersDetails = new List<StockTransferBO>();

                    foreach (var item in data)
                    {
                        StockTransferBO stockTransfer = new StockTransferBO();
                        stockTransfer.FromLocationId = stockTransferMaster.FromLocationId;
                        stockTransfer.ToLocationId = stockTransferMaster.ToLocationId;
                        stockTransfer.ItemId = Convert.ToInt32(item.ElementAt(0).Value);
                        stockTransfer.Item_Code = item.ElementAt(1).Value.ToString();
                        stockTransfer.Item_Name = item.ElementAt(2).Value.ToString();
                        //stockTransfer.ItemUnit = item.ElementAt(4).Value.ToString();
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
                        //cmd.Parameters.AddWithValue("@FromLocationName", stockTransferMaster.FromLocationName); 
                        //cmd.Parameters.AddWithValue("@ToLocationName", stockTransferMaster.ToLocationName);  
                        cmdNew.Parameters.AddWithValue("@ItemId", item.ItemId);
                        cmdNew.Parameters.AddWithValue("@Item_Name", item.Item_Name);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                        cmdNew.Parameters.AddWithValue("@TransferQuantity", item.TransferQuantity);
                        cmdNew.Parameters.AddWithValue("@RequiredQuantity", item.RequiredQuantity);
                        cmdNew.Parameters.AddWithValue("@FinalQuantity", item.FinalQuantity);                        
                        cmdNew.Parameters.AddWithValue("@Remarks", item.Remarks);
                        cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                        cmdNew.Parameters.AddWithValue("@CreatedBy", 1);
                        cmdNew.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                        cmdNew.Parameters.AddWithValue("@LastModifiedBy", 1);
                        //con.Open();
                        SqlDataReader dataReaderNew = cmdNew.ExecuteReader();

                        while (dataReaderNew.Read())
                        {
                            response.Status = Convert.ToBoolean(dataReaderNew["Status"]);
                        }
                        con.Close();
                    }


                    //Stock Master
                    foreach (var item in data)
                    {
                        StockMasterBO objItemDetails = new StockMasterBO();
                        //objItemDetails.PO_Id = PO_Id;
                        objItemDetails.ItemID = Convert.ToInt32(item.ElementAt(0).Value);
                        objItemDetails.Item_Code = item.ElementAt(1).Value.ToString();
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();                      
                        objItemDetails.ItemUnit = item.ElementAt(4).Value.ToString();
                        objItemDetails.StockQuantity = Convert.ToDouble(item.ElementAt(6).Value);                        
                        objItemDetails.Remarks = item.ElementAt(7).Value.ToString();                        
                        itemDetails.Add(objItemDetails);
                    }
                    //Update 
                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_usp_tbl_StockMaster_Insert_Insert", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@PO_Id", item.PO_Id);
                        cmdNew.Parameters.AddWithValue("@ItemID", item.ItemID);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);                        
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        cmdNew.Parameters.AddWithValue("@StockQuantity", item.StockQuantity);
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


                    //Location Wise Stock

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