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
    public class WeighmentProductionIndentRepository: IWeighmentProductionIndentRepository
    {
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(IProductionIndentRepository));

        #region Bind grid
        ///<summary>
        ///Rahul:  This function is for fecthing list of Weighment Production Indents. 
        ///</summary>
        ///<returns></returns>
        public IEnumerable<Weighment_ProductionIndentBO> GetAll()
        {
            List<Weighment_ProductionIndentBO> productionIndentList = new List<Weighment_ProductionIndentBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Weighment_ProductionIndent_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new Weighment_ProductionIndentBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            WeighmentNo = reader["WeighmentNo"].ToString(),
                            ProductionIndentNo = reader["ProductionIndentNo"].ToString(),
                            BatchNumber = reader["BatchNumber"].ToString(),
                            WeighmentDate = Convert.ToDateTime(reader["WeighmentDate"]),                            
                            IndentStatus = reader["IndentStatus"].ToString(),
                            Description = reader["Description"].ToString(),                             
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

        #region Bind Dropdown For Get Production Indent Number 
        /// <summary>
        /// Date: 14 July'23 
        /// Rahul  : This function is for fecthing list of Production Indent Number from Production Indent.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductionIndentBO> GetProductionIndentNumber()
        {
            List<ProductionIndentBO> IssueNoteNumber = new List<ProductionIndentBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ProductionIndentNumber_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var IssueNoteMasters = new ProductionIndentBO() 
                        {
                            ID = Convert.ToInt32(reader["ProductionIndentId"]),
                            ProductionIndentNo = reader["ProductionIndentNoteNO"].ToString()
                        };
                        IssueNoteNumber.Add(IssueNoteMasters);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return IssueNoteNumber;
        }
        #endregion

        #region Bind Get Production Indent Details 
        public ProductionIndentBO GetProductionIndentDetails(int id)
        {
            string productionIndentQuery = "Select  * from ProductionIndent where ID = @Id AND IsDeleted = 0";
            string itemDetails = "Select  * from ProductionIndentIngredientsDetails where ProductionIndentID = @Id AND IsDeleted = 0";

            ProductionIndentBO result = new ProductionIndentBO(); 
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    result = con.Query<ProductionIndentBO>(productionIndentQuery, new { @Id = id }).FirstOrDefault();
                    var resultList = con.Query<ProductionIndent_DetailsBO>(itemDetails, new { @Id = id }).ToList();

                    result.indent_Details = resultList;
                }

            }
            catch (Exception ex)
            {
                result = null;
                log.Error(ex.Message, ex);
            }
            return result;
        }
        #endregion

        #region Bind Get Captured Weight Indent Details 
        public WeighmentReceivedDataBO GetCapturedWeightIndentDetails() 
        {
            string wighmentIndentQuery = "declare @ID int = null; set @ID = (Select  top 1(ID) from WeighmentReceivedData order by 1 desc); Select* from WeighmentReceivedData where ID = @ID; ";

            WeighmentReceivedDataBO result = new WeighmentReceivedDataBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    result = con.Query<WeighmentReceivedDataBO>(wighmentIndentQuery, new { @Ids = result.ID,result.Column1 }).FirstOrDefault();                    
                }
            }
            catch (Exception ex)
            {
                result = null;
                log.Error(ex.Message, ex);
            }
            return result;
        }
        #endregion

        #region Convert Array To DataTable 
        /// <summary>
        /// Date: 20 Jul'23.
        /// Rahul: Convert Array To DataTable 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public DataTable ConvertArrayToDataTable(string[] array)
        {
            //DataTable dt = new DataTable();
            //dt.Columns.Add("Column1", typeof(string)); // Change "string" to the actual datatype of Column1
            //dt.Columns.Add("Column2", typeof(string)); // Change "string" to the actual datatype of Column2
            //                                           // Add more columns as needed

            //foreach (var item in array)
            //{
            //    DataRow row = dt.NewRow();
            //    row["Column1"] = item; // Change "Column1" to the actual column name in your temporary table
            //    row["Column2"] = item;                        // Set other columns as needed
            //    dt.Rows.Add(row);
            //}

            //return dt;

            DataTable dt = new DataTable();

            // Create DataColumns with names "Column1", "Column2", etc.
            for (int i = 0; i < array.Length; i++)
            {
                dt.Columns.Add($"Column{i + 1}", array[i].GetType());
            }

            // Add data to the DataTable
            DataRow row = dt.NewRow();
            for (int i = 0; i < array.Length; i++)
            {
                row[i] = array[i];
            }
            dt.Rows.Add(row);

            return dt;
        }
        #endregion

        #region Clear Captured Weight Temp Data Delete function
        /// <summary>
        /// /// Date: 25 Jul'23
        /// Rahul: //This function is for delete all the temp records of Clear Captured Weight temp Data    
        /// /// </summary>
        /// <returns></returns>
        public void ClearCapturedWeightDataDelete()  
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_WeighmentReceivedData_Delete]", con);
                    cmd.CommandType = CommandType.StoredProcedure;                                                            
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                };
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
        #endregion

        #region Insert function
        public ResponseMessageBO Insert(Weighment_ProductionIndentBO model) 
        {
            ResponseMessageBO result = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Weighment_ProductionIndent_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@WeighmentNo", model.WeighmentNo);
                    cmd.Parameters.AddWithValue("@ProductionIndentID", model.ProductionIndentID);
                    cmd.Parameters.AddWithValue("@ProductionIndentNo", model.ProductionIndentNo);
                    cmd.Parameters.AddWithValue("@WeighmentDate", model.WeighmentDate);                    
                    cmd.Parameters.AddWithValue("@IndentBy", model.RaisedBy);
                    cmd.Parameters.AddWithValue("@UserName", model.UserName);
                    cmd.Parameters.AddWithValue("@SalesOrderId", model.SO_Id);
                    cmd.Parameters.AddWithValue("@SONo", model.SONo);
                    cmd.Parameters.AddWithValue("@WorkOrderNo", model.WorkOrderNo);
                    cmd.Parameters.AddWithValue("@BatchPlanningDocId", model.BatchPlanningDocId);    //Rahul added 'BatchPlanningDocId' 13-06-23. 
                    cmd.Parameters.AddWithValue("@BatchPlanningDocumentNo", model.BatchPlanningDocumentNo);  //Rahul added 'BatchPlanningDocumentNo' 13-06-23.
                    cmd.Parameters.AddWithValue("@ItemID", model.RecipeID);
                    cmd.Parameters.AddWithValue("@RecipeName", model.RecipeName);
                    cmd.Parameters.AddWithValue("@TotalBatches", model.TotalBatches);
                    cmd.Parameters.AddWithValue("@BatchNumber", model.BatchNumber); //Rahul added 25-03-2023.
                    cmd.Parameters.AddWithValue("@Remarks", model.Description);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    var WeighmentID = 0;
                    var IndentID = 0;
                    while (dataReader.Read())
                    {
                        WeighmentID = Convert.ToInt32(dataReader["WeighmentID"]);
                        IndentID = Convert.ToInt32(dataReader["IndentID"]);
                        result.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.TxtItemDetails);

                    List<Weighment_ProductionIndent_DetailsBO> itemDetails = new List<Weighment_ProductionIndent_DetailsBO>(); 

                    foreach (var item in data)
                    {
                        Weighment_ProductionIndent_DetailsBO objItemDetails = new Weighment_ProductionIndent_DetailsBO(); 
                        objItemDetails.WeighmentID = WeighmentID;
                        objItemDetails.ProductionIndentID = IndentID;
                        objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(0).Value);
                        objItemDetails.ItemCode = item.ElementAt(1).Value.ToString();
                        objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
                        objItemDetails.BatchQuantity = Convert.ToDouble(item.ElementAt(3).Value);
                        objItemDetails.FinalQuantity = Convert.ToDouble(item.ElementAt(4).Value);
                        objItemDetails.ItemUnit = item.ElementAt(5).Value.ToString();
                        objItemDetails.Percentage = Convert.ToDouble(item.ElementAt(6).Value);
                        objItemDetails.Weight = Convert.ToDouble(item.ElementAt(7).Value);
                        objItemDetails.Difference = Convert.ToDouble(item.ElementAt(8).Value);

                        itemDetails.Add(objItemDetails);
                    }

                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_Weighment_ProductionIndentIngredientsDetails_Insert", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@WeighmentID", item.WeighmentID);
                        cmdNew.Parameters.AddWithValue("@ProductionIndentID", item.ProductionIndentID);
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.ItemId);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.ItemCode);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        cmdNew.Parameters.AddWithValue("@BatchQuantity", item.BatchQuantity);
                        cmdNew.Parameters.AddWithValue("@FinalQuantity", item.FinalQuantity);
                        cmdNew.Parameters.AddWithValue("@Percentage", item.Percentage);
                        cmdNew.Parameters.AddWithValue("@Weight", item.Weight);
                        cmdNew.Parameters.AddWithValue("@Difference", item.Difference);
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

        #region View functions
        /// <summary>
        /// Created By: Rahul
        /// Description: Fetch Weighment Production by it's ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Weighment_ProductionIndentBO GetById(int id)
        {
            var result = new Weighment_ProductionIndentBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Weighment_ProductionIndent_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = new Weighment_ProductionIndentBO()
                        {
                            WeighmentNo = reader["WeighmentNo"].ToString(),
                            ProductionIndentNo = reader["ProductionIndentNo"].ToString(),
                            WeighmentDate = Convert.ToDateTime(reader["WeighmentDate"]),                            
                            RaisedBy = reader["RaisedBy"] is DBNull ? 0 : Convert.ToInt32(reader["RaisedBy"]),
                            UserName = reader["UserName"].ToString(),
                            RecipeID = reader["RecipeID"] is DBNull ? 0 : Convert.ToInt32(reader["RecipeID"]),
                            RecipeName = reader["RecipeName"].ToString(),
                            SalesOrderId = reader["SalesOrderId"] is DBNull ? 0 : Convert.ToInt32(reader["SalesOrderId"]),
                            SO_Id = reader["SalesOrderId"] is DBNull ? 0 : Convert.ToInt32(reader["SalesOrderId"]),
                            SONo = reader["SONo"].ToString(),
                            WorkOrderNo = reader["WorkOrderNo"].ToString(),
                            BatchPlanningDocId = reader["BatchPlanningDocId"] is DBNull ? 0 : Convert.ToInt32(reader["BatchPlanningDocId"]),
                            BatchPlanningDocumentNo = reader["BatchPlanningDocumentNo"].ToString(), 
                            TotalBatches = reader["TotalBatches"] is DBNull ? 0 : Convert.ToInt32(reader["TotalBatches"]),
                            BatchNumber = reader["BatchNumber"].ToString(),
                            Description = reader["Description"].ToString()

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
        ///Created By: Rahul 
        ///Description: This function is used to get the list of Items againts Weighment ID  
        ///using dapper for Weighment Production module. 
        /// </summary>
        /// <param name="WeighmentID"></param>
        /// <returns></returns>
        public List<Weighment_ProductionIndent_DetailsBO> GetItemDetailsByProductionWeighmentID(int WeighmentID) 
        {
            string queryString = "Select * From Weighment_ProductionIndentIngredientsDetails where WeighmentID = @WeighmentID and IsDeleted = 0";
            using (SqlConnection con = new SqlConnection(conString))
            {
                var result = con.Query<Weighment_ProductionIndent_DetailsBO>(queryString, new { @WeighmentID = WeighmentID }).ToList();
                return result;
            }
        }
        #endregion

        #region Delete function Weighment Production and Ingredients details by using Weighment ID.
        public void Delete(int ID, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Weighment_ProductionIndent_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@WeighmentID", ID);
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