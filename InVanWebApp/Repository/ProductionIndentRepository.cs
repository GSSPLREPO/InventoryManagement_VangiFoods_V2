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
    public class ProductionIndentRepository : IProductionIndentRepository 
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(IProductionIndentRepository));

        #region Bind grid
        ///<summary>
        ///Rahul:  This function is for fecthing list of Production Indents. 
        ///</summary>
        ///<returns></returns>
        public IEnumerable<ProductionIndentBO> GetAll()
        {
            List<ProductionIndentBO> productionIndentList = new List<ProductionIndentBO>();
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
                        var result = new ProductionIndentBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            ProductionIndentNo = reader["ProductionIndentNo"].ToString(),
                            IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                            ProductionDate = Convert.ToDateTime(reader["ProductionDate"]),                            
                            IndentStatus = reader["IndentStatus"].ToString(),
                            Description = reader["Description"].ToString(),
                            //IndentCount = Convert.ToInt32(reader["IndentCount"])
                        };
                        productionIndentList.Add(result); 
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return productionIndentList;
        }
        #endregion

        #region Bind all Recipe details 
        public IEnumerable<RecipeMasterBO> GetRecipeDetailsById(int ProductId, int Recipe_Id) 
        {
            List<RecipeMasterBO> resultList = new List<RecipeMasterBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RecipeIngredientsDetail_GetBy_Id", con);
                    cmd.Parameters.AddWithValue("@ID", ProductId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new RecipeMasterBO()
                        {                            
                            RecipeID = Convert.ToInt32(dataReader["RecipeID"]),
                            RecipeName = dataReader["RecipeName"].ToString(),
                            Item_ID = Convert.ToInt32(dataReader["ItemId"]),
                            ItemCode = dataReader["ItemCode"].ToString(),
                            ItemName = dataReader["ItemName"].ToString(),
                            ItemQuantity = ((dataReader["BatchSize"] != null) ? Convert.ToDecimal(dataReader["BatchSize"]) : 0),
                            FinalQuantity = ((dataReader["BatchSize"] != null) ? Convert.ToDecimal(dataReader["BatchSize"]) : 0),
                            UnitName = dataReader["UnitName"].ToString(),
                            Ratio = ((dataReader["Ratio"] != null) ? float.Parse(dataReader["Ratio"].ToString()) : 0)
                        };
                        resultList.Add(result);
                    }
                    con.Close();

                    //==========This condition is for edit functionality "Recipe_Id == 0".===========///
                    if (Recipe_Id == 0)
                    {                        
                        SqlCommand cmd2 = new SqlCommand("usp_tbl_RecipeIngredientsDetails_GetByID", con);
                        cmd2.Parameters.AddWithValue("@RecipeID", Recipe_Id);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader dataReader2 = cmd2.ExecuteReader();

                        while (dataReader2.Read())
                        {
                            var result = new RecipeMasterBO()
                            {
                                Item_ID = Convert.ToInt32(dataReader2["ItemID"]),
                                ItemCode = dataReader2["Item_Code"].ToString(),
                                ItemName = dataReader2["ItemName"].ToString(),
                                ItemQuantity = ((dataReader2["ItemQuantity"] != null) ? Convert.ToDecimal(dataReader2["ItemQuantity"]) : 0),
                                FinalQuantity = ((dataReader2["FinalQuantity"] != null) ? Convert.ToDecimal(dataReader2["FinalQuantity"]) : 0),
                                UnitName = dataReader2["ItemUnit"].ToString(),                                
                                Ratio = ((dataReader2["Percentage"] != null) ? float.Parse(dataReader2["Percentage"].ToString()): 0)
                            };
                            resultList.Add(result);
                        }
                        con.Close();
                    }

                    else //==========This else will execute for generating view.===============//
                    {
                        SqlCommand cmd1 = new SqlCommand("usp_tbl_RecipeIngredientsDetailsForView_GetByID", con);
                        cmd1.Parameters.AddWithValue("@ID", Recipe_Id);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader dataReader1 = cmd1.ExecuteReader();

                        while (dataReader1.Read())
                        {
                            var result = new RecipeMasterBO()
                            {
                                ItemCode = dataReader1["Item_Code"].ToString(),
                                ItemName = dataReader1["ItemName"].ToString(),
                                ItemQuantity = ((dataReader1["ItemQuantity"] != null) ? Convert.ToDecimal(dataReader1["ItemQuantity"]) : 0),
                                FinalQuantity = ((dataReader1["FinalQuantity"] != null) ? Convert.ToDecimal(dataReader1["FinalQuantity"]) : 0),
                                UnitName = dataReader1["ItemUnit"].ToString(),
                                Ratio = ((dataReader1["Percentage"] != null) ? float.Parse(dataReader1["Percentage"].ToString()) : 0)
                            };
                            resultList.Add(result);
                        }
                        con.Close();
                    }
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

        #region Bind dropdown of SO Number 
        public IEnumerable<SalesOrderBO> GetSONumberForDropdown() 
        {
            List<SalesOrderBO> resultList = new List<SalesOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SalesOrder_For_Production_Indent_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new SalesOrderBO()
                        {
                            SalesOrderId = Convert.ToInt32(dataReader["SalesOrderId"]),
                            SONumber = dataReader["SONumber"].ToString(),
                            WorkOrderNo = dataReader["WorkOrderNo"].ToString()
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


        #region Insert function
        public ResponseMessageBO Insert(ProductionIndentBO model)
        {
            ResponseMessageBO result = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ProductionIndent_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProductionIndentNo", model.ProductionIndentNo);
                    cmd.Parameters.AddWithValue("@IndentDate", model.IssueDate);
                    cmd.Parameters.AddWithValue("@ProductionDate", model.ProductionDate);
                    cmd.Parameters.AddWithValue("@IndentBy", model.RaisedBy);
                    cmd.Parameters.AddWithValue("@Remarks", model.Description);
                    cmd.Parameters.AddWithValue("@ProductID", model.ProductID);
                    cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                    cmd.Parameters.AddWithValue("@SalesOrderId", model.SalesOrderId);
                    cmd.Parameters.AddWithValue("@SONo", model.SONo);                    
                    cmd.Parameters.AddWithValue("@WorkOrderNo", model.WorkOrderNo);
                    cmd.Parameters.AddWithValue("@UserName", model.UserName);
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
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.TxtItemDetails);

                    List<ProductionIndent_DetailsBO> itemDetails = new List<ProductionIndent_DetailsBO>();

                    foreach (var item in data)
                    {
                        ProductionIndent_DetailsBO objItemDetails = new ProductionIndent_DetailsBO();
                        objItemDetails.ProductionIndentID = IndentID;
                        objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(0).Value);
                        objItemDetails.ItemCode = item.ElementAt(1).Value.ToString();
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.BatchQty = Convert.ToDouble(item.ElementAt(3).Value);
                        objItemDetails.FinalQty = Convert.ToDouble(item.ElementAt(4).Value);
                        objItemDetails.ItemUnit = item.ElementAt(5).Value.ToString();
                        objItemDetails.Percentage = Convert.ToDouble(item.ElementAt(6).Value);

                        itemDetails.Add(objItemDetails);
                    }

                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_IndentDetails_Insert", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@ProductionIndentID", item.ProductionIndentID);
                        cmdNew.Parameters.AddWithValue("@QCcheck_1", item.QCcheck_1);
                        cmdNew.Parameters.AddWithValue("@QCcheck_2", item.QCcheck_2);
                        cmdNew.Parameters.AddWithValue("@QCcheck_3", item.QCcheck_3);
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.ItemId);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.ItemCode);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        cmdNew.Parameters.AddWithValue("@BatchQuantity", item.BatchQty);
                        cmdNew.Parameters.AddWithValue("@FinalQuantity", item.FinalQty);
                        cmdNew.Parameters.AddWithValue("@ProductionCheck", model.UserName);
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

    }
}