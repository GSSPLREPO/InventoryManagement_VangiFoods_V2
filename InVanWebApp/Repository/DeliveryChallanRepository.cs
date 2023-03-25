using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Dapper;
using InVanWebApp.Common;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;

namespace InVanWebApp.Repository
{
    public class DeliveryChallanRepository: IDeliveryChallanRepository
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(DeliveryChallanRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of delivery challan.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DeliveryChallanBO> GetAll()
        {
            List<DeliveryChallanBO> resultList = new List<DeliveryChallanBO>();
            string queryString = "Select * from DeliveryChallan where IsDeleted=0";

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    resultList = con.Query<DeliveryChallanBO>(queryString).ToList();
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
        public ResponseMessageBO Insert(DeliveryChallanBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_DeliveryChallan_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DeliveryChallanNumber", model.DeliveryChallanNumber);
                    cmd.Parameters.AddWithValue("@DeliveryChallanDate", model.DeliveryChallanDate);
                    cmd.Parameters.AddWithValue("@SO_Id", model.SO_Id);
                    cmd.Parameters.AddWithValue("@SONumber", model.SONumber);
                    cmd.Parameters.AddWithValue("@CurrencyID", model.CurrencyID);
                    cmd.Parameters.AddWithValue("@CurrencyName", model.CurrencyName);
                    cmd.Parameters.AddWithValue("@CurrencyPrice", model.CurrencyPrice);
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    cmd.Parameters.AddWithValue("@VendorsID", model.VendorsID);
                    cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
                    cmd.Parameters.AddWithValue("@SupplierAddress", model.SupplierAddress);
                    cmd.Parameters.AddWithValue("@ShippingAddress", model.ShippingAddress);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@TermsAndCondition_ID", model.TermsAndCondition_ID);
                    cmd.Parameters.AddWithValue("@Terms", model.Terms);
                    cmd.Parameters.AddWithValue("@TotalAfterTax", model.TotalAfterTax);
                    cmd.Parameters.AddWithValue("@OtherTax", model.OtherTax);
                    cmd.Parameters.AddWithValue("@GrandTotal", model.GrandTotal);
                    cmd.Parameters.AddWithValue("@DiscountPercentage", model.DiscountPercentage);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    cmd.Parameters.AddWithValue("@Signature", model.Signature);
                    con.Open();

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int DeliveryChallan_Id = 0;

                    while (dataReader.Read())
                    {
                        DeliveryChallan_Id = Convert.ToInt32(dataReader["ID"]);
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    if (DeliveryChallan_Id != 0)
                    {
                        var json = new JavaScriptSerializer();
                        var data = json.Deserialize<Dictionary<string, string>[]>(model.txtItemDetails);

                        List<DeliveryChallanItemDetailsBO> itemDetails = new List<DeliveryChallanItemDetailsBO>();

                        foreach (var item in data)
                        {
                            DeliveryChallanItemDetailsBO objItemDetails = new DeliveryChallanItemDetailsBO();
                            objItemDetails.DeliveryChallanID = DeliveryChallan_Id;
                            objItemDetails.Item_Code = item.ElementAt(0).Value.ToString();
                            objItemDetails.Item_ID = Convert.ToInt32(item.ElementAt(1).Value);
                            objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                            objItemDetails.OrderedQuantity = Convert.ToDecimal(item.ElementAt(3).Value);
                            objItemDetails.BalanceQuantity = Convert.ToDecimal(item.ElementAt(4).Value);
                            objItemDetails.OutwardQuantity = Convert.ToDecimal(item.ElementAt(5).Value);
                            objItemDetails.ItemUnit = item.ElementAt(6).Value.ToString();
                            objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(7).Value);
                            objItemDetails.CurrencyName = item.ElementAt(8).Value.ToString();
                            objItemDetails.ItemTaxValue = item.ElementAt(9).Value.ToString();
                            objItemDetails.TotalItemCost = Convert.ToDecimal(item.ElementAt(10).Value);
                            objItemDetails.CreatedBy = model.CreatedBy;
                            itemDetails.Add(objItemDetails);
                        }

                        foreach (var item in itemDetails)
                        {
                            con.Open();
                            SqlCommand cmdNew = new SqlCommand("usp_tbl_DeliveryChallanDetails_Insert", con);
                            cmdNew.CommandType = CommandType.StoredProcedure;

                            cmdNew.Parameters.AddWithValue("@DeliveryChallanID", item.DeliveryChallanID);
                            cmdNew.Parameters.AddWithValue("@ItemId", item.Item_ID);
                            cmdNew.Parameters.AddWithValue("@Item_Name", item.ItemName);
                            cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                            cmdNew.Parameters.AddWithValue("@OrderedQuantity", item.OrderedQuantity);
                            cmdNew.Parameters.AddWithValue("@BalanceQuantity", item.BalanceQuantity);
                            cmdNew.Parameters.AddWithValue("@OutwardQuantity", item.OutwardQuantity);
                            cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                            cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                            cmdNew.Parameters.AddWithValue("@CurrencyName", item.CurrencyName);
                            cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                            cmdNew.Parameters.AddWithValue("@TotalItemCost", item.TotalItemCost);
                            cmdNew.Parameters.AddWithValue("@LocationId", model.LocationId);
                            cmdNew.Parameters.AddWithValue("@SO_Id", model.SO_Id);
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

        #region Delete function

        /// <summary>
        /// Delete record by ID
        /// </summary>
        /// <param name="ID"></param>
        public ResponseMessageBO Delete(int Id, int userId)
        {
            ResponseMessageBO responseMessage = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_DeliveryChallan_Delete", con);
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

        public DeliveryChallanBO GetById(int ID)
        {
            DeliveryChallanBO result = new DeliveryChallanBO();
            try
            {
                string stringQuery = "Select * from DeliveryChallan where IsDeleted=0 and ID=@ID";
                string stringItemQuery = "Select * from DeliveryChallanItemDetails where IsDeleted=0 and DeliveryChallanID=@ID";
                using (SqlConnection con = new SqlConnection(connString))
                {
                    result = con.Query<DeliveryChallanBO>(stringQuery, new { @ID = ID }).FirstOrDefault();
                    var ItemList = con.Query<DeliveryChallanItemDetailsBO>(stringItemQuery, new { @ID = ID }).ToList();
                    result.deliveryChallanItemDetails = ItemList;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return result;
        }

        #endregion

        #region Bind dropdowns
        public IEnumerable<SalesOrderBO> GetSONumberList() 
        {
            List<SalesOrderBO> resultList = new List<SalesOrderBO>();
            string queryString = "usp_tbl_SalesOrder_GetAll";

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    resultList = con.Query<SalesOrderBO>(queryString).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return resultList;
        }
        #endregion

        #region Bind all SO details 
        public IEnumerable<SalesOrderBO> GetSODetailsById(int SOId)
        {
            List<SalesOrderBO> resultList = new List<SalesOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SODetails_GetByID", con);
                    cmd.Parameters.AddWithValue("@ID", SOId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new SalesOrderBO()
                        {
                            SalesOrderId = Convert.ToInt32(dataReader["SalesOrderId"]),
                            SONo = dataReader["SONo"].ToString(),
                            CurrencyID = Convert.ToInt32(dataReader["CurrencyID"]),
                            CurrencyName = dataReader["CurrencyName"].ToString(),
                            CurrencyPrice = Convert.ToDecimal(dataReader["CurrencyPrice"]),
                            LocationId = Convert.ToInt32(dataReader["LocationId"]),
                            LocationName = dataReader["LocationName"].ToString(),
                            ClientID = Convert.ToInt32(dataReader["ClientID"]),
                            CompanyName = dataReader["CompanyName"].ToString(),
                            DeliveryAddress = dataReader["DeliveryAddress"].ToString(),
                            SupplierAddress = dataReader["SupplierAddress"].ToString(),
                            TermsAndConditionID = Convert.ToInt32(dataReader["TermsAndConditionID"]),
                            Terms = dataReader["Terms"].ToString(),
                            OtherTax = Convert.ToDecimal(dataReader["OtherTax"])
                        };
                        resultList.Add(result);
                    }
                    con.Close();

                    //==========This is for fetching Item details".===========///

                    SqlCommand cmd2 = new SqlCommand("usp_tbl_SOItemDetailsForOutwardNote_GetByID", con);
                    cmd2.Parameters.AddWithValue("@SO_Id", SOId);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader2 = cmd2.ExecuteReader();

                    while (dataReader2.Read())
                    {
                        var result = new SalesOrderBO()
                        {
                            Item_ID = Convert.ToInt32(dataReader2["Item_ID"]),
                            Item_Code = dataReader2["Item_Code"].ToString(),
                            ItemName = dataReader2["ItemName"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(dataReader2["ItemUnitPrice"]),
                            ItemUnit = dataReader2["ItemUnit"].ToString(),
                            ItemTaxValue = Convert.ToDecimal(dataReader2["ItemTaxValue"]),
                            ItemQuantity = Convert.ToDecimal(dataReader2["ItemQuantity"]),
                            BalanceQuantity = Convert.ToDecimal(dataReader2["BalanceQuantity"]),
                            CurrencyName = dataReader2["CurrencyName"].ToString(),
                            CurrencyID = Convert.ToInt32(dataReader2["CurrencyID"]),
                            TotalItemCost = Convert.ToDecimal(dataReader2["TotalItemCost"])
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
    }
}