using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Dapper;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;
using InVanWebApp.Common;

namespace InVanWebApp.Repository
{
    public class IndentRepository : IIndentRepository
    {
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(IndentRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of indents.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IndentBO> GetAll()
        {
            List<IndentBO> resultList = new List<IndentBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Indent_GetAllForIndent", con);
                    cmd.CommandType = CommandType.StoredProcedure;
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
                            IndentCount = Convert.ToInt32(reader["IndentCount"])
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

            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Insert function
        public ResponseMessageBO Insert(IndentBO model)
        {
            ResponseMessageBO result = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Indent_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IndentNumber", model.IndentNo);
                    cmd.Parameters.AddWithValue("@IndentDate", model.IndentDate);
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    cmd.Parameters.AddWithValue("@IndentBy", model.RaisedBy);
                    cmd.Parameters.AddWithValue("@UserName", model.UserName);
                    cmd.Parameters.AddWithValue("@DesignationId", model.DesignationId);
                    cmd.Parameters.AddWithValue("@DesignationName", model.DesignationName);
                    cmd.Parameters.AddWithValue("@Remarks", model.Description);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    var IndentID = 0;
                    while (dataReader.Read())
                    {
                        IndentID = Convert.ToInt32(dataReader["ID"]);
                        result.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.itemDetails);

                    List<Indent_DetailsBO> itemDetails = new List<Indent_DetailsBO>();

                    foreach (var item in data)
                    {
                        Indent_DetailsBO objItemDetails = new Indent_DetailsBO();
                        objItemDetails.IndentID = IndentID;
                        objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(0).Value);
                        objItemDetails.ItemCode = item.ElementAt(2).Value.ToString(); //ElementAt(1) 26-05-23.
                        objItemDetails.ItemName = item.ElementAt(3).Value.ToString(); //ElementAt(2) 26-05-23.
                        objItemDetails.RequiredQuantity = Convert.ToDouble(item.ElementAt(4).Value); //ElementAt(3) 26-05-23.
                        objItemDetails.ItemUnit = item.ElementAt(5).Value.ToString(); //ElementAt(4) 26-05-23.

                        itemDetails.Add(objItemDetails);
                    }

                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_IndentDetails_Insert", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@IndentId", item.IndentID);
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.ItemId);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.ItemCode);
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        cmdNew.Parameters.AddWithValue("@ItemQuantity", item.RequiredQuantity);
                        cmdNew.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                        cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));

                        SqlDataReader dataReaderNew = cmdNew.ExecuteReader();

                        while (dataReaderNew.Read())
                        {
                            result.Status = Convert.ToBoolean(dataReaderNew["Status"]);
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                result.Status = false;
            }
            return result;
        }
        #endregion

        #region Update functions
        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID 
        /// for Purchase Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public List<Indent_DetailsBO> GetItemDetailsById(int id, int CurrencyId = 0)
        {
            var resultList = new List<Indent_DetailsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_IndentItemDetails_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@CurrencyId", CurrencyId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = new Indent_DetailsBO()
                        {
                            IndentID = reader["IndentID"] is DBNull?0: (Convert.ToInt32(reader["IndentID"])),
                            ItemName = reader["ItemName"].ToString(),
                            ItemId = reader["ItemId"] is DBNull?0:(Convert.ToInt32(reader["ItemId"])),
                            RequiredQuantity = reader["RequiredQuantity"] is DBNull?0: (Convert.ToDouble(reader["RequiredQuantity"])),
                            SentQuantity = reader["SentQuantity"] is DBNull ? 0 : (Convert.ToDouble(reader["SentQuantity"])),
                            
                            //Added the below fields for binding the items in PO
                            ItemCode=reader["ItemCode"].ToString(),
                            ItemUnit= reader["ItemUnit"].ToString(),
                            ItemUnitPrice= reader["ItemUnitPrice"] is DBNull ? 0 : (Convert.ToDouble(reader["ItemUnitPrice"])),
                            ItemTax= reader["ItemTax"] is DBNull ? 0 : (Convert.ToDouble(reader["ItemTax"])),
                            BalanceQuantity= reader["BalanceQuantity"] is DBNull ? 0 : (Convert.ToDouble(reader["BalanceQuantity"]))

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
            //return _context.UnitMasters.Find(UnitID);
        }

        /// <summary>
        /// Created By: Farheen
        /// Description: Fetch Indent by it's ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IndentBO GetById(int id) 
        {
            var result = new IndentBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Indent_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = new IndentBO()
                        {
                            IndentNo=reader["IndentNo"].ToString(),
                            //IndentDate= DateTime.ParseExact(reader["IndentDate"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture),
                            IndentDate= Convert.ToDateTime(reader["IndentDate"].ToString()),
                            LocationId=Convert.ToInt32(reader["LocationId"]),
                            LocationName=reader["LocationName"].ToString(),
                            RaisedBy=Convert.ToInt32(reader["RaisedBy"]),
                            UserName=reader["UserName"].ToString(),
                            DesignationId=Convert.ToInt32(reader["DesignationId"]),
                            DesignationName=reader["DesignationName"].ToString(),
                            Description=reader["Description"].ToString()

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
        }

        /// <summary>
        ///Created By: Farheen
        ///Description: This function is used to get the list of Items againts Indent ID 
        ///using dapper for Indent module.
        /// </summary>
        /// <param name="IndentId"></param>
        /// <returns></returns>
        public List<Indent_DetailsBO> GetItemDetailsByIndentId(int IndentId)
        {
            string queryString = "select * From Indent_Details where IndentID = @IndentId AND IsDeleted = 0";
            using (SqlConnection con = new SqlConnection(conString))
            {
                var result = con.Query<Indent_DetailsBO>(queryString, new { @IndentId = IndentId }).ToList();
                return result;
            }
        }

        /// <summary>
        /// Created by: Farheen
        /// Description: Update function for Indent
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseMessageBO Update(IndentBO model)
        {
            ResponseMessageBO result = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Indent_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IndentID", model.ID);
                    cmd.Parameters.AddWithValue("@IndentNumber", model.IndentNo);
                    cmd.Parameters.AddWithValue("@IndentDate", model.IndentDate);
                    cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
                    cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
                    cmd.Parameters.AddWithValue("@IndentBy", model.RaisedBy);
                    cmd.Parameters.AddWithValue("@UserName", model.UserName);
                    cmd.Parameters.AddWithValue("@DesignationId", model.DesignationId);
                    cmd.Parameters.AddWithValue("@DesignationName", model.DesignationName);
                    cmd.Parameters.AddWithValue("@Remarks", model.Description);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedBy);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        result.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.itemDetails);

                    List<Indent_DetailsBO> itemDetails = new List<Indent_DetailsBO>();

                    foreach (var item in data)
                    {
                        Indent_DetailsBO objItemDetails = new Indent_DetailsBO();
                        objItemDetails.IndentID = model.ID;
                        objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(0).Value);
                        objItemDetails.ItemCode = item.ElementAt(1).Value.ToString();
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.ItemUnit = item.ElementAt(4).Value.ToString();
                        objItemDetails.RequiredQuantity = Convert.ToDouble(item.ElementAt(3).Value);

                        itemDetails.Add(objItemDetails);
                    }
                    var count = itemDetails.Count;
                    var i = 1;
                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_IndentDetails_Update", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@IndentId", item.IndentID);
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.ItemId);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.ItemCode);
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        cmdNew.Parameters.AddWithValue("@ItemQuantity", item.RequiredQuantity);
                        cmdNew.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedBy);
                        cmdNew.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                        if (count == 1)
                            cmdNew.Parameters.AddWithValue("@OneItemIdentifier", 1);
                        else { 
                            cmdNew.Parameters.AddWithValue("@OneItemIdentifier", 0);
                            cmdNew.Parameters.AddWithValue("@flagCheck", i);
                        }
                        i++;
                        SqlDataReader dataReaderNew = cmdNew.ExecuteReader();

                        while (dataReaderNew.Read())
                        {
                            result.Status = Convert.ToBoolean(dataReaderNew["Status"]);
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                result.Status = false;
            }
            return result;
        }

        #endregion

        #region Delete function
        public void Delete(int ID, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Indent_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IndentId", ID);
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

        #region Get list of Items for Indent
        public List<ItemBO> GetItemDetailsForDD()
        {
            List<ItemBO> ItemList = new List<ItemBO>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_GetItemListForIndent", con);
                cmd.CommandType = CommandType.StoredProcedure;
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

        #region This function is for pdf export/view
        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID and for downloading pdf
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        public IndentBO GetItemDetailsByIndentById(int ID)
        {
            IndentBO result = new IndentBO();
            try
            {
                string stringQuery = "select * From Indent where ID = @IndentId AND IsDeleted = 0";
                string stringItemQuery = "select * From Indent_Details where IndentID = @IndentId AND IsDeleted = 0";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    result = con.Query<IndentBO>(stringQuery, new { @IndentId = ID }).FirstOrDefault();
                    var ItemList = con.Query<Indent_DetailsBO>(stringItemQuery, new { @IndentId = ID }).ToList();
                    result.indent_Details = ItemList;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return result;
        }

        #endregion

    }
}