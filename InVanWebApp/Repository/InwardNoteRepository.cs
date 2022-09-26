using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using log4net;

namespace InVanWebApp.Repository
{
    public class InwardNoteRepository : IInwardNoteRepository
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(InwardNoteRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of organisation.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InwardNoteBO> GetAll()
        {
            List<InwardNoteBO> resultList = new List<InwardNoteBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_InwardNote_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new InwardNoteBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            PONumber = reader["PONumber"].ToString(),
                            InwardNumber = reader["InwardNumber"].ToString(),
                            InwardDate = Convert.ToDateTime(reader["InwardDate"]),
                            Remarks = reader["Remarks"].ToString()
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

        #region Bind dropdown of PO Number
        public IEnumerable<PurchaseOrderBO> GetPONumberForDropdown()
        {
            List<PurchaseOrderBO> resultList = new List<PurchaseOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PODetails_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new PurchaseOrderBO()
                        {
                            PurchaseOrderId = Convert.ToInt32(dataReader["PurchaseOrderId"]),
                            PONumber = dataReader["PONumber"].ToString()
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

        #region Bind all PO details 
        public IEnumerable<PurchaseOrderBO> GetPODetailsById(int PO_Id)
        {
            List<PurchaseOrderBO> resultList = new List<PurchaseOrderBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_PODetails_GetByID", con);
                    cmd.Parameters.AddWithValue("@ID",PO_Id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new PurchaseOrderBO()
                        {
                            PurchaseOrderId = Convert.ToInt32(dataReader["PurchaseOrderId"]),
                            PODate = Convert.ToDateTime(dataReader["PODate"]),
                            BuyerAddress = dataReader["BuyerAddress"].ToString(),
                            SupplierAddress = dataReader["SupplierAddress"].ToString()
                            //ItemName = dataReader["ItemName"].ToString(),
                            //Item_Code= dataReader["Item_Code"].ToString(),
                            //ItemUnitPrice =(dataReader["ItemUnitPrice"]!=null)? (float.Parse((string)dataReader["ItemUnitPrice"])):0,
                            //ItemUnit = dataReader["ItemUnit"].ToString(),
                            //ItemQuantity = (dataReader["ItemQuantity"] != null) ? (float.Parse((string)dataReader["ItemQuantity"])) : 0
                        };
                        resultList.Add(result);
                    }
                    con.Close();
                    
                    SqlCommand cmd1 = new SqlCommand("usp_tbl_POItemsDetails_GetByID", con);
                    cmd1.Parameters.AddWithValue("@ID", PO_Id);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader1 = cmd1.ExecuteReader();

                    while (dataReader1.Read())
                    {
                        var result = new PurchaseOrderBO()
                        {
                            ItemName = dataReader1["ItemName"].ToString(),
                            Item_Code = dataReader1["Item_Code"].ToString(),
                            ItemUnitPrice = Convert.ToDecimal(dataReader1["ItemUnitPrice"]),
                            ItemUnit = dataReader1["ItemUnit"].ToString(),
                            ItemQuantity = Convert.ToDecimal(dataReader1["ItemQuantity"]),
                            ItemTaxValue =dataReader1["ItemTaxValue"].ToString()
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