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
    public class RejectionNoteRepository : IRejectionNoteRepository
    {
        //private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(RejectionNoteRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of inward QC data. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RejectionNoteBO> GetAll()
        {
            List<RejectionNoteBO> resultList = new List<RejectionNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RejectionNote_GetAll", con); //Rahul updated 12-01-2023. 
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new RejectionNoteBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            InwardNoteNumber = reader["InwardNumber"].ToString(),      //Rahul updated 13-01-2023.                         
                            InwardQuantity = Convert.ToDouble(reader["InwardQuantity"]),
                            RejectedQuantity = Convert.ToDouble(reader["RejectedQuantity"]),
                            NoteDate = Convert.ToDateTime(reader["RejectionNoteDate"]),
                            RejectionNoteNo = reader["RejectionNoteNo"].ToString(),

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

        #region Bind all inward note details 
        public IEnumerable<RejectionNoteItemDetailsBO> GetInwardDetailsById(int Id)
        {
            List<RejectionNoteItemDetailsBO> resultRNDtlsList = new List<RejectionNoteItemDetailsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RejectionNoteDetailsFor_RN_GetByID", con);
                    cmd.Parameters.AddWithValue("@ID", Id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new RejectionNoteItemDetailsBO()
                        {
                            PO_Id = Convert.ToInt32(dataReader["PO_Id"]),
                            PONumber = dataReader["PONumber"].ToString(),
                            SupplierID = Convert.ToInt32(dataReader["SupplierID"]),
                            SupplierName = dataReader["SupplierName"].ToString()
                        };
                        resultRNDtlsList.Add(result);
                    }
                    con.Close();

                    SqlCommand cmd1 = new SqlCommand("usp_tbl_InwardQCItemDetailsFor_RN_ItemDetails_GetByID", con);
                    cmd1.Parameters.AddWithValue("@ID", Id);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader1 = cmd1.ExecuteReader();

                    while (dataReader1.Read())
                    {
                        var resultRNDtls = new RejectionNoteItemDetailsBO()
                        {
                            Item_Name = dataReader1["Item_Name"].ToString(),
                            Item_Code = dataReader1["Item_Code"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(dataReader1["ItemUnitPrice"]),
                            CurrencyName = dataReader1["CurrencyName"].ToString(),
                            ItemUnit = dataReader1["ItemUnit"].ToString(),
                            TotalQuantity = float.Parse(dataReader1["TotalQuantity"].ToString()),
                            InwardQuantity = (dataReader1["InwardQuantity"] != null ? Convert.ToDouble(dataReader1["InwardQuantity"]) : 0),
                            RejectedQuantity = float.Parse(dataReader1["RejectedQuantity"].ToString()),
                            QuantityTookForSorting = float.Parse(dataReader1["QuantityTookForSorting"].ToString()),
                            WastageQuantityInPercentage = float.Parse(dataReader1["WastageQuantityInPercentage"].ToString()),
                            Remarks = dataReader1["Remarks"].ToString(),
                            ItemId = Convert.ToInt32(dataReader1["ItemId"]),
                            ItemTaxValue = dataReader1["ItemTaxValue"].ToString(),

                        };
                        resultRNDtlsList.Add(resultRNDtls);
                    }
                    con.Close();
                    // }
                };
            }
            catch (Exception ex)
            {
                resultRNDtlsList = null;
                log.Error(ex.Message, ex);
            }
            return resultRNDtlsList;
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Rahul: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(RejectionNoteBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RejectionNote_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RejectionNoteNo", model.RejectionNoteNo);
                    cmd.Parameters.AddWithValue("@NoteDate", model.NoteDate);
                    //cmd.Parameters.AddWithValue("@InwardNote_Id", model.InwardNote_Id);
                    cmd.Parameters.AddWithValue("@InwardNoteNumber", model.InwardNoteNumber);
                    cmd.Parameters.AddWithValue("@InwardQCId", model.InwardQCId);
                    cmd.Parameters.AddWithValue("@InwardQCNumber", model.InwardQCNumber);
                    cmd.Parameters.AddWithValue("@SupplierID", model.SupplierID);
                    cmd.Parameters.AddWithValue("@SupplierName", model.SupplierName);
                    cmd.Parameters.AddWithValue("@PO_Id", model.PO_Id);
                    cmd.Parameters.AddWithValue("@PONumber", model.PONumber);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    ////Added the below field for Rejection note 
                    cmd.Parameters.AddWithValue("@PreProductionQCId", model.PreProductionQCId);
                    cmd.Parameters.AddWithValue("@ProductionQCNumber", model.PreProductionQCNumber);
                    cmd.Parameters.AddWithValue("@ProductionMaterialIssueNoteId", model.ProductionMaterialIssueNoteId);
                    cmd.Parameters.AddWithValue("@ProductionMaterialIssueNoteNo", model.ProductionMaterialIssueNoteNo);
                    cmd.Parameters.AddWithValue("@ProductionIndentId", model.ProductionIndentId);
                    cmd.Parameters.AddWithValue("@ProductionIndentNo", model.ProductionIndentNo);
                    cmd.Parameters.AddWithValue("@QCType", model.QCType); 

                    con.Open();

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    int RejectionID = 0;
                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                        RejectionID = Convert.ToInt32(dataReader["RejectionID"]);
                    }
                    con.Close();
                    if (RejectionID != 0)
                    {
                        var json = new JavaScriptSerializer();
                        var data = json.Deserialize<Dictionary<string, string>[]>(model.InwardQuantities);

                        List<RejectionNoteItemDetailsBO> itemDetails = new List<RejectionNoteItemDetailsBO>();

                        foreach (var item in data)
                        {
                            RejectionNoteItemDetailsBO objItemDetails = new RejectionNoteItemDetailsBO();
                            objItemDetails.RejectionID = RejectionID;
                            objItemDetails.Item_Name = item.ElementAt(0).Value.ToString();
                            objItemDetails.Item_Code = item.ElementAt(1).Value.ToString();
                            objItemDetails.ItemUnitPrice = Convert.ToDecimal(item.ElementAt(2).Value);
                            objItemDetails.ItemUnit = (item.ElementAt(3).Value).ToString();
                            objItemDetails.TotalQuantity = Convert.ToDouble(item.ElementAt(4).Value);
                            objItemDetails.InwardQuantity = Convert.ToDouble(item.ElementAt(5).Value);
                            objItemDetails.RejectedQuantity = Convert.ToDouble(item.ElementAt(6).Value);
                            objItemDetails.QuantityTookForSorting = Convert.ToDouble(item.ElementAt(7).Value);
                            objItemDetails.WastageQuantityInPercentage = Convert.ToDouble(item.ElementAt(8).Value);
                            objItemDetails.Remarks = (item.ElementAt(9).Value).ToString();
                            objItemDetails.ItemId = Convert.ToInt32(item.ElementAt(10).Value);
                            objItemDetails.ItemTaxValue = item.ElementAt(11).Value.ToString();
                            objItemDetails.CurrencyName = (item.ElementAt(12).Value).ToString();

                            itemDetails.Add(objItemDetails);
                        }

                        foreach (var item in itemDetails)
                        {
                            con.Open();
                            SqlCommand cmdNew = new SqlCommand("usp_tbl_RejectionNoteItemDetails_Insert", con);
                            cmdNew.CommandType = CommandType.StoredProcedure;

                            cmdNew.Parameters.AddWithValue("@RejectionID", RejectionID);
                            //cmdNew.Parameters.AddWithValue("@InwardNote_Id", model.InwardNote_Id); 
                            cmdNew.Parameters.AddWithValue("@Item_ID", item.ItemId);
                            cmdNew.Parameters.AddWithValue("@ItemName", item.Item_Name);
                            cmdNew.Parameters.AddWithValue("@Item_Code", item.Item_Code);
                            cmdNew.Parameters.AddWithValue("@ItemUnitPrice", item.ItemUnitPrice);
                            cmdNew.Parameters.AddWithValue("@ItemUnit", item.ItemUnit);
                            cmdNew.Parameters.AddWithValue("@ItemTaxValue", item.ItemTaxValue);
                            cmdNew.Parameters.AddWithValue("@TotalQuantity", item.TotalQuantity);
                            cmdNew.Parameters.AddWithValue("@InwardQuantity", item.InwardQuantity);
                            cmdNew.Parameters.AddWithValue("@RejectedQuantity", item.RejectedQuantity);
                            cmdNew.Parameters.AddWithValue("@QuantityTookForSorting", item.QuantityTookForSorting);
                            cmdNew.Parameters.AddWithValue("@WastageQuantityInPercentage", item.WastageQuantityInPercentage);
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

        #region View function 
        /// <summary>
        /// Rahul: View record.  
        /// </summary>
        /// <param name="RejectionID"></param>        
        public RejectionNoteBO GetRejectionNoteById(int RejectionID) 
        {
            string rejectionNoteQuery = "SELECT *, ProductionQCNumber as PreProductionQCNumber FROM RejectionNote WHERE ID = @RejectionID AND IsDeleted = 0";
            string rejectionNoteItemQuery = "SELECT * FROM RejectionNoteItemDetails WHERE RejectionID = @RejectionID AND IsDeleted = 0;";
            using (SqlConnection con = new SqlConnection(connString)) 
            {
                var rejectionNote = con.Query<RejectionNoteBO>(rejectionNoteQuery, new { @RejectionID = RejectionID }).FirstOrDefault();
                var rejectionNoteList = con.Query<RejectionNoteItemDetailsBO>(rejectionNoteItemQuery, new { @RejectionID = RejectionID }).ToList();
                rejectionNote.itemDetails = rejectionNoteList; 
                return rejectionNote; 
            }
        }
        #endregion 

        #region Delete function  
        /// <summary>
        /// Delete record by ID 
        /// </summary>
        /// <param name="RejectionID"></param>
        public void Delete(int RejectionID, int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_DeleteRejectionNoteAndItemDteails_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RejectionID", RejectionID);
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

        #region Bind dropdown of Pre-Production QC Number
        /// <summary>
        /// Rahul: Bind dropdown of PreProduction QC Number
        /// 18 Apr 2023.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PreProduction_QCBO> GetPreProductionQCNumberForDropdown() 
        {
            List<PreProduction_QCBO> resultList = new List<PreProduction_QCBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_BindPreProductionQCForRejectionNote_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new PreProduction_QCBO() 
                        {
                            ID = Convert.ToInt32(dataReader["ID"]),
                            QCNumber = dataReader["PreProductionQCNumber"].ToString()
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
        public IEnumerable<ProductionMaterialIssueNoteBO> GetProdIndent_NoDeatils(int PPQCId, int PPNote_Id = 0)
        {
            List<ProductionMaterialIssueNoteBO> resultList = new List<ProductionMaterialIssueNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {

                    if (PPNote_Id == 0)
                    {
                        SqlCommand cmd = new SqlCommand("[usp_tbl_PINDForRN_GetByID]", con);
                        cmd.Parameters.AddWithValue("@ID", PPQCId);
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader dataReader = cmd.ExecuteReader();

                        while (dataReader.Read())
                        {
                            var result = new ProductionMaterialIssueNoteBO()
                            {                              
                                ID = Convert.ToInt32(dataReader["MaterialIssue_Id"]),
                                ProductionIndentID = Convert.ToInt32(dataReader["ProdIndent_Id"]),
                                ProductionIndentNo = dataReader["ProdIndent_No"].ToString(),

                            };
                            resultList.Add(result);
                        }
                        con.Close();

                        SqlCommand cmd2 = new SqlCommand("[usp_tbl_PreProduction_QCItemDetailsFor_RN_ItemDetails_GetByID]", con);
                        cmd2.Parameters.AddWithValue("@ID", PPQCId);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader dataReader2 = cmd2.ExecuteReader();

                        while (dataReader2.Read())
                        {
                            var result = new ProductionMaterialIssueNoteBO()
                            {
                                Item_Name = dataReader2["Item_Name"].ToString(),
                                Item_Code = dataReader2["Item_Code"].ToString(),
                                ItemUnitPrice = Convert.ToDecimal(dataReader2["ItemUnitPrice"]),
                                CurrencyName = dataReader2["CurrencyName"].ToString(),
                                ItemUnit = dataReader2["ItemUnit"].ToString(),
                                TotalQuantity = float.Parse(dataReader2["TotalQuantity"].ToString()),
                                IssuedQuantity = (dataReader2["IssuedQuantity"] != null ? Convert.ToDouble(dataReader2["IssuedQuantity"]) : 0),
                                RejectedQuantity = float.Parse(dataReader2["RejectedQuantity"].ToString()),
                                QuantityTookForSorting = float.Parse(dataReader2["QuantityTookForSorting"].ToString()),
                                WastageQuantityInPercentage = float.Parse(dataReader2["WastageQuantityInPercentage"].ToString()),
                                Remarks = dataReader2["Remarks"].ToString(),
                                ItemId = Convert.ToInt32(dataReader2["ItemId"]),
                                ItemTaxValue = dataReader2["ItemTaxValue"].ToString(),

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

        #region Bind all Rejection note details for Debit note item details
        /// <summary>
        /// //Rahul added 20-04-23.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IEnumerable<RejectionNoteItemDetailsBO> GetRejectionNoteDetailsById(int Id) 
        {
            List<RejectionNoteItemDetailsBO> resultRNDtlsList = new List<RejectionNoteItemDetailsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_RejectionNoteDetailsFor_DN_GetByID", con);
                    cmd.Parameters.AddWithValue("@ID", Id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new RejectionNoteItemDetailsBO()
                        {
                            PO_Id = Convert.ToInt32(dataReader["PO_Id"]),
                            PONumber = dataReader["PONumber"].ToString(),
                            RejectionID = Convert.ToInt32(dataReader["RejectionID"]),
                            RejectionNoteNo = dataReader["RejectionNoteNo"].ToString(),
                            SupplierID = Convert.ToInt32(dataReader["SupplierID"]),
                            SupplierName = dataReader["SupplierName"].ToString(),
                            CurrencyID = Convert.ToInt32(dataReader["CurrencyID"]),
                            CurrencyName = dataReader["CurrencyName"].ToString(),
                            CurrencyPrice = Convert.ToDouble(dataReader["CurrencyPrice"]),
                            LocationId = Convert.ToInt32(dataReader["LocationId"]),
                            LocationName = dataReader["LocationName"].ToString(),
                            DeliveryAddress = dataReader["DeliveryAddress"].ToString(),
                            SupplierAddress = dataReader["SupplierAddress"].ToString(),
                            TermsAndConditionID = Convert.ToInt32(dataReader["TermsAndConditionID"]),
                            Terms = dataReader["Terms"].ToString(),
                            OtherTax = Convert.ToDecimal(dataReader["OtherTax"])    
                        };
                        resultRNDtlsList.Add(result);
                    }
                    con.Close();

                    SqlCommand cmd1 = new SqlCommand("usp_tbl_RNItemDetailsFor_DN_ItemDetails_GetByID", con);
                    cmd1.Parameters.AddWithValue("@ID", Id);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader1 = cmd1.ExecuteReader();

                    while (dataReader1.Read())
                    {
                        var resultRNDtls = new RejectionNoteItemDetailsBO()
                        {
                            //Item_Name = dataReader1["ItemName"].ToString(),
                            //Item_Code = dataReader1["ItemCode"].ToString(),
                            //ItemUnitPrice = Convert.ToDecimal(dataReader1["ItemUnitPrice"]),
                            //CurrencyName = dataReader1["CurrencyName"].ToString(),
                            //ItemUnit = dataReader1["ItemUnit"].ToString(),
                            //TotalQuantity = float.Parse(dataReader1["TotalQuantity"].ToString()),
                            //InwardQuantity = (dataReader1["InwardQuantity"] != null ? Convert.ToDouble(dataReader1["InwardQuantity"]) : 0),
                            //RejectedQuantity = float.Parse(dataReader1["RejectedQuantity"].ToString()),
                            //QuantityTookForSorting = float.Parse(dataReader1["QuantityTookForSorting"].ToString()),
                            //WastageQuantityInPercentage = float.Parse(dataReader1["WastageQuantityInPercentage"].ToString()),
                            //Remarks = dataReader1["Remarks"].ToString(),
                            //ItemId = Convert.ToInt32(dataReader1["ItemId"]),
                            //ItemTaxValue = dataReader1["ItemTaxValue"].ToString(),

                            ItemId = Convert.ToInt32(dataReader1["ItemId"]),
                            Item_Code = dataReader1["ItemCode"].ToString(),
                            ItemName = dataReader1["ItemName"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(dataReader1["ItemUnitPrice"]),
                            ItemUnit = dataReader1["ItemUnit"].ToString(),
                            ItemTaxValue = dataReader1["ItemTaxValue"].ToString(),
                            TotalQuantity = float.Parse(dataReader1["TotalQuantity"].ToString()),
                            RejectedQuantity = float.Parse(dataReader1["RejectedQuantity"].ToString()),
                            CurrencyName = dataReader1["CurrencyName"].ToString(),
                            CurrencyID = Convert.ToInt32(dataReader1["CurrencyID"]),
                            Remarks = dataReader1["Remarks"].ToString(),
                            TotalItemCost = Convert.ToDecimal(dataReader1["TotalItemCost"])

                        };
                        resultRNDtlsList.Add(resultRNDtls);
                    }
                    con.Close();                    
                };
            }
            catch (Exception ex)
            {
                resultRNDtlsList = null;
                log.Error(ex.Message, ex);
            }
            return resultRNDtlsList;
        }
        #endregion

    }
}