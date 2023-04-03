using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using log4net;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;

namespace InVanWebApp.Repository
{
    public class PreProduction_QCRepository : IPreProduction_QCRepository
    {
        //private readonly InVanDBContext _context;
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(PreProduction_QCRepository));

        #region  Bind grid
        /// <summary>
        /// Snehal: This function is for fecthing list of Pre Production QC data.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PreProduction_QCBO> GetAll()
        {
            List<PreProduction_QCBO> resultList = new List<PreProduction_QCBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_PreProduction_QC_GetAll]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new PreProduction_QCBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            QCNumber = reader["QCNumber"].ToString(),
                            QCDate = Convert.ToDateTime(reader["QCDate"]),
                            //QCNumber = reader["InwardQCNo"].ToString(),
                            //Item_Name = reader["Item_Name"].ToString(),
                            //Item_Code = reader["Item_Code"].ToString(),
                            //ItemUnitPrice = Convert.ToDecimal(reader["ItemUnitPrice"]),
                            IssuedQuantity = Convert.ToInt32(reader["IssuedQuantity"]),
                            RejectedQuantity = Convert.ToInt32(reader["RejectedQuantity"])
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

        #region Insert function
        /// <summary>
        /// Snehal: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(PreProduction_QCBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_PreProduction_QC_Insert]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@QCNumber", model.QCNumber);
                    cmd.Parameters.AddWithValue("@MaterialIssue_Id", model.MaterialIssue_Id);
                    cmd.Parameters.AddWithValue("@MaterialIssue_No", model.MaterialIssue_No);
                    cmd.Parameters.AddWithValue("@ProdIndent_Id", model.ProdIndent_Id);
                    cmd.Parameters.AddWithValue("@ProdIndent_No", model.ProdIndent_No);
                    cmd.Parameters.AddWithValue("@QCDate", model.QCDate);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int PreQCID = 0;
                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                        PreQCID = Convert.ToInt32(dataReader["QCNumberID"]);
                    }
                    con.Close();

                    var json = new JavaScriptSerializer();
                    var data = json.Deserialize<Dictionary<string, string>[]>(model.txtItemDetails);

                    List<PreProduction_QC_Details> itemDetails = new List<PreProduction_QC_Details>();

                    foreach (var item in data)
                    {
                        PreProduction_QC_Details objItemDetails = new PreProduction_QC_Details();
                        objItemDetails.QC_Id = PreQCID;
                        objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(0).Value);
                        objItemDetails.Item_Name = item.ElementAt(1).Value.ToString();
                        objItemDetails.Item_Code = item.ElementAt(2).Value.ToString();
                        objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(3).Value);
                        objItemDetails.ItemUnit = (item.ElementAt(4).Value).ToString();
                        objItemDetails.IssuedQuantity = Convert.ToDouble(item.ElementAt(5).Value);
                        objItemDetails.QuantityTookForSorting = Convert.ToDouble(item.ElementAt(6).Value);
                        objItemDetails.BalanceQuantity = Convert.ToDouble(item.ElementAt(7).Value);
                        objItemDetails.WastageQuantityInPercentage = Convert.ToDouble(item.ElementAt(8).Value);
                        objItemDetails.Remarks = (item.ElementAt(9).Value).ToString();
                        objItemDetails.CurrencyName = (item.ElementAt(10).Value).ToString();
                        objItemDetails.RejectedQuantity = Convert.ToDouble(item.ElementAt(11).Value);
                        //objItemDetails.ItemTaxValue = item.ElementAt(12).Value.ToString();

                        itemDetails.Add(objItemDetails);
                    }

                    foreach (var item in itemDetails)
                    {
                        con.Open();
                        SqlCommand cmdNew = new SqlCommand("usp_tbl_PreProduction_QCItemDetails_Insert", con);
                        cmdNew.CommandType = CommandType.StoredProcedure;

                        cmdNew.Parameters.AddWithValue("@IssueNoteId", model.MaterialIssue_Id);
                        cmdNew.Parameters.AddWithValue("@PreQCId", PreQCID);
                        cmdNew.Parameters.AddWithValue("@Item_ID", item.ItemId);
                        cmdNew.Parameters.AddWithValue("@ItemName", item.Item_Name);
                        cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                        cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                        cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                        //cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                        //cmdNew.Parameters.AddWithValue("@SupplierName", model.SupplierName);
                        cmdNew.Parameters.AddWithValue("@IssuedQuantity", item.IssuedQuantity);
                        cmdNew.Parameters.AddWithValue("@QuantityTookForSorting", item.QuantityTookForSorting);
                        cmdNew.Parameters.AddWithValue("@BalanceQuantity", item.BalanceQuantity);
                        cmdNew.Parameters.AddWithValue("@WastageQuantityInPercentage", item.WastageQuantityInPercentage);
                        cmdNew.Parameters.AddWithValue("@RejectedQuantity", item.RejectedQuantity);
                        cmdNew.Parameters.AddWithValue("@Remarks", item.Remarks);
                        cmdNew.Parameters.AddWithValue("@CurrencyName", item.CurrencyName);
                        cmdNew.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
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
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
                // return false;
            }
            return response;
        }

        #endregion

        #region Bind dropdown of QC Number
        public IEnumerable<ProductionMaterialIssueNoteBO> GetQCNumberForDropdown()
        {
            List<ProductionMaterialIssueNoteBO> resultList = new List<ProductionMaterialIssueNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_ProductionMaterialIssueNote_GetAll]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new ProductionMaterialIssueNoteBO()
                        {
                            ID = Convert.ToInt32(dataReader["ID"]),
                            ProductionMaterialIssueNoteNo = dataReader["ProductionMaterialIssueNoteNo"].ToString()
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
      
        #region Bind all Pre Production note details 
        //public IEnumerable<InwardNoteBO> GetInwDetailsById(int PPQCId, int PPNote_Id)
        public IEnumerable<ProductionMaterialIssueNoteBO> GetProdIndent_NoDeatils(int PPQCId, int PPNote_Id=0)
        {
            List<ProductionMaterialIssueNoteBO> resultList = new List<ProductionMaterialIssueNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {

                    if (PPNote_Id == 0)
                    {
                        SqlCommand cmd = new SqlCommand("[usp_tbl_PMNoteForPPQC_GetByID]", con);
                        cmd.Parameters.AddWithValue("@ID", PPQCId);
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader dataReader = cmd.ExecuteReader();

                        while (dataReader.Read())
                        {
                            var result = new ProductionMaterialIssueNoteBO()
                            {
                                ID = Convert.ToInt32(dataReader["ID"]),
                               // QCDate = Convert.ToDateTime(dataReader["ProductionDate"]),
                                //PONumber = dataReader["PONumber"].ToString(),
                                ProductionIndentID = Convert.ToInt32(dataReader["ProductionIndentID"]),
                                ProductionIndentNo = dataReader["ProductionIndentNo"].ToString()

                            };
                            resultList.Add(result);
                        }
                        con.Close();

                        SqlCommand cmd2 = new SqlCommand("[usp_tbl_PreQCItemDetails_GetById]", con);
                        cmd2.Parameters.AddWithValue("@ID", PPQCId);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader dataReader2 = cmd2.ExecuteReader();

                        while (dataReader2.Read())
                        {
                            var result = new ProductionMaterialIssueNoteBO()
                            {
                                ItemId = Convert.ToInt32(dataReader2["ItemId"]),
                                Item_Name = dataReader2["Item_Name"].ToString(),
                                Item_Code = dataReader2["Item_Code"].ToString(),
                                ItemUnitPrice = Convert.ToDecimal(dataReader2["ItemUnitPrice"]),
                                IssuedQuantity = Convert.ToDouble(dataReader2["MaterialIssuedQty"]),
                                QuantityTookForSorting = float.Parse(dataReader2["QuantityTookForSorting"].ToString()),
                                BalanceQuantity = float.Parse(dataReader2["BalanceQuantity"].ToString()),
                                RejectedQuantity = float.Parse(dataReader2["RejectedQuantity"].ToString()),
                                WastageQuantityInPercentage = float.Parse(dataReader2["WastageQuantityInPercentage"].ToString()),
                                Remarks = dataReader2["Remarks"].ToString(),
                                CurrencyID = Convert.ToInt32(dataReader2["CurrencyID"]),
                                CurrencyName = dataReader2["CurrencyName"].ToString()
                            };
                            resultList.Add(result);
                        }
                        con.Close();
                    }
                    else
                    {
                        SqlCommand cmd3 = new SqlCommand("[usp_tbl_PreQCItemDetailsForView_GetByID]", con);
                        cmd3.Parameters.AddWithValue("@ID", PPQCId);
                        cmd3.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader dataReader3 = cmd3.ExecuteReader();

                        while (dataReader3.Read())
                        {
                            var result = new ProductionMaterialIssueNoteBO()
                            {
                                Item_Name = dataReader3["Item_Name"].ToString(),
                                Item_Code = dataReader3["Item_Code"].ToString(),
                                ItemUnitPrice = Convert.ToDecimal(dataReader3["ItemUnitPrice"]),
                                IssuedQuantity = Convert.ToDouble(dataReader3["IssuedQuantity"]),
                                QuantityTookForSorting = float.Parse(dataReader3["QuantityTookForSorting"].ToString()),
                                RejectedQuantity = float.Parse(dataReader3["RejectedQuantity"].ToString()),
                                Remarks = dataReader3["Remarks"].ToString(),
                                CurrencyName = dataReader3["CurrencyName"].ToString()

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

        #region Function for viewing the perticular QC and download the same
        /// <summary>
        /// Snehal: This function is for fetch data for viewing by ID and for downloading pdf
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        public PreProduction_QCBO GetById(int ID)
        {
            var result = new PreProduction_QCBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_PreQC_GetByID]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = new PreProduction_QCBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            QCNumber = reader["QCNumber"].ToString(),
                            MaterialIssue_Id = Convert.ToInt32(reader["MaterialIssue_Id"]),
                            MaterialIssue_No = reader["MaterialIssueNoteNo"].ToString(),
                            ProdIndent_Id = Convert.ToInt32(reader["ProdIndent_Id"]),
                            ProdIndent_No = reader["ProdIndent_No"].ToString(),
                            QCDate = Convert.ToDateTime(reader["QCDate"]),
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

        #endregion

        #region Delete function

        /// <summary>
        /// Delete record by ID
        /// </summary>
        /// <param name="ID"></param>
        public void Delete(int ID, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("[usp_tbl_PreProduction_QC_Delete]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
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