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

    }
}