using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using System.Data.SqlClient;
using System.Configuration;
using log4net;
using System.Data;

namespace InVanWebApp.Repository
{
    public class CommonFunctions:ICommonFunctions
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(CommonFunctions));

        #region Generate document number
        public string GetDocumentNo(int DocumentType)
        {
            var DocumentNo = "";
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_GenerateDocumentNo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DocumentNo = reader["DocumentNumber"].ToString();
                    }
                    con.Close();
                };
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            
            return DocumentNo;
        }

        #endregion

        //#region Generate Batch number 
        //public string GetBatchNo(string WorkOrderNumber)
        //{
        //    var BatchNumber = "";
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(conString))
        //        {
        //            SqlCommand cmd = new SqlCommand("usp_tbl_GenerateBatchNumber", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@WorkOrderNumber", WorkOrderNumber);
        //            con.Open();
        //            SqlDataReader reader = cmd.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                BatchNumber = reader["BatchNumber"].ToString();
        //            }
        //            con.Close();
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message, ex);
        //    }

        //    return BatchNumber;
        //}

        //#endregion

    }
}