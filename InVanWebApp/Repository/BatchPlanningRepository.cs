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
    public class BatchPlanningRepository : IBatchPlanningRepository
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(CommonFunctions));

        #region Bind Grid
        ///<Summary>
        ///Rahul:  This function is for fecthing list of Production Batch Planning. 
        ///</Summary>
        ///<returns></returns>
        public IEnumerable<BatchPlanningMasterBO> GetAll()
        {
            List<BatchPlanningMasterBO> batchPlanningMasterList = new List<BatchPlanningMasterBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_BatchPlanningMaster_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new BatchPlanningMasterBO()
                        {
                            ID = Convert.ToInt32(reader["ID "]),
                            WorkOrderNumber = reader["WorkOrderNumber"].ToString(),
                            SONumber = reader["SONumber"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            Remarks = reader["Remarks"].ToString(),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                        };
                    batchPlanningMasterList.Add(result);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);                
            }
            return batchPlanningMasterList;
        }
        #endregion
    }
}