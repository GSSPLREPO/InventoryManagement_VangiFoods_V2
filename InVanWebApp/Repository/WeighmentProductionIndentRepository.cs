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

namespace InVanWebApp.Repository
{
    public class WeighmentProductionIndentRepository: IWeighmentProductionIndentRepository
    {
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(IProductionIndentRepository));

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

        #region MyRegion
        /// <summary>
        /// Date: 17 July'23 
        /// Rahul  :Method used to insert the values Weighment data from weighing machine 
        /// reading from device(Input HOLDING REGISTERS) into [Weighment_ProductionIndentIngredientsDetails] table for Weight   
        /// </summary>

        /// <param name="RecordId"></param>
        /// <returns></returns>
        public bool InsertValuesFromDevices(int[] TempValues, int RecordId, DateTime capturedDateTime)
        {
            //ResponseMessageBO result = new ResponseMessageBO();
            //try
            //{
            //    using (SqlConnection con = new SqlConnection(conString))
            //    {
            //        //db_DeconEntities.usp_tbl_InsertValuesFromDevices_T
            //        //(TempValues[0], TempValues[1], TempValues[2], TempValues[3],
            //        //capturedDateTime, userId, DateTime.Now, userId, DateTime.Now, RecordId);
            //        //db_DeconEntities.SaveChanges();

            //        con.Open();

            //        SqlDataReader dataReader = cmd.ExecuteReader();
            //        var IndentID = 0;
            //        while (dataReader.Read())
            //        {
            //            IndentID = Convert.ToInt32(dataReader["IndentID"]);
            //            result.Status = Convert.ToBoolean(dataReader["Status"]);
            //        }
            //        con.Close();

            //        var json = new JavaScriptSerializer();
            //        var data = json.Deserialize<Dictionary<string, string>[]>(model.TxtItemDetails);

            //        List<ProductionIndent_DetailsBO> itemDetails = new List<ProductionIndent_DetailsBO>();

            //        foreach (var item in data)
            //        {
            //            ProductionIndent_DetailsBO objItemDetails = new ProductionIndent_DetailsBO();
            //            objItemDetails.ProductionIndentID = IndentID;
            //            objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(0).Value);
            //            objItemDetails.ItemCode = item.ElementAt(1).Value.ToString();
            //            objItemDetails.ItemName = item.ElementAt(2).Value.ToString();
            //            objItemDetails.BatchQuantity = Convert.ToDouble(item.ElementAt(3).Value);
            //            objItemDetails.FinalQuantity = Convert.ToDouble(item.ElementAt(4).Value);
            //            objItemDetails.ItemUnit = item.ElementAt(5).Value.ToString();
            //            objItemDetails.Percentage = Convert.ToDouble(item.ElementAt(6).Value);

            //            itemDetails.Add(objItemDetails);
            //        }

            //        foreach (var item in itemDetails)
            //        {
            //            con.Open();
            //            SqlCommand cmdNew = new SqlCommand("usp_tbl_ProductionIndentIngredientsDetails_Insert", con);
            //            cmdNew.CommandType = CommandType.StoredProcedure;

            //            cmdNew.Parameters.AddWithValue("@ProductionIndentID", item.ProductionIndentID);
            //            cmdNew.Parameters.AddWithValue("@Item_ID", item.ItemId);
            //            cmdNew.Parameters.AddWithValue("@Item_Code", item.ItemCode);
            //            cmdNew.Parameters.AddWithValue("@ItemName", item.ItemName);
            //            cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
            //            cmdNew.Parameters.AddWithValue("@BatchQuantity", item.BatchQuantity);
            //            cmdNew.Parameters.AddWithValue("@FinalQuantity", item.FinalQuantity);
            //            cmdNew.Parameters.AddWithValue("@Percentage", item.Percentage);
            //            cmdNew.Parameters.AddWithValue("@ProductionCheck", model.UserName);
            //            cmdNew.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
            //            cmdNew.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));

            //            SqlDataReader dataReaderNew = cmdNew.ExecuteReader();

            //            while (dataReaderNew.Read())
            //            {
            //                result.Status = Convert.ToBoolean(dataReaderNew["Status"]);
            //            }
            //            con.Close();
            //        }

            //    }
            //    //return true;
            //    return result;
            //}
            //catch (Exception)
            //{
            //    log.Error(ex.Message, ex);
            //    result.Status = false;
            //    //throw;
            //}
            ////return result;
            return true;
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

    }
}