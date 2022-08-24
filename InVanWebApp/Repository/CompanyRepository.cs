using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace InVanWebApp.Repository
{
    public class CompanyRepository:ICompanyRepository
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private static ILog log = LogManager.GetLogger(typeof(CompanyRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of supplier/customer master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CompanyBO> GetAll()
        {
            List<CompanyBO> companyList = new List<CompanyBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SupplierCustomer_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var company = new CompanyBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            CompanyName = reader["CompanyName"].ToString(),
                            ContactPersonName = reader["ContactPersonName"].ToString(),
                            EmailId = reader["EmailId"].ToString(),
                            ContactPersonNo = Convert.ToInt32(reader["ContactPersonNo"]),
                            CityName = reader["CityName"].ToString()
                        };
                        companyList.Add(company);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return companyList;

            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public bool Insert(CompanyBO model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_ItemCategory_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemTypeId", model.CompanyName);
                    cmd.Parameters.AddWithValue("@ItemCategoryName", model.CompanyType);
                    cmd.Parameters.AddWithValue("@Description", model.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", 1);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                };
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return false;
                //Content("<script language='javascript' type='text/javascript'>alert('Thanks for Feedback!');</script>");
                // throw;
            }

        }
        #endregion

        #region Update functions

        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CompanyBO GetById(int ID)
        {
            var company = new CompanyBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SupplierCustomer_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        company = new CompanyBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            CompanyName = reader["CompanyName"].ToString()
                            //ItemCategory_ID = Convert.ToInt32(reader["ItemCategory_ID"]),
                            //Item_Code = reader["Item_Code"].ToString(),
                            //Item_Name = reader["Item_Name"].ToString(),
                            //HSN_Code = reader["HSN_Code"].ToString(),
                            //MinStock = Convert.ToInt32(reader["MinStock"]),
                            //Description = reader["Description"].ToString()
                        };
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

            return company;
        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="item"></param>
        //public ResponseMessageBO Udate(ItemBO item)
        //{
        //    ResponseMessageBO response = new ResponseMessageBO();
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(conString))
        //        {
        //            SqlCommand cmd = new SqlCommand("usp_tbl_Item_Update", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@Item_ID", item.ID);
        //            cmd.Parameters.AddWithValue("@ItemTypeID", item.ItemTypeID);
        //            cmd.Parameters.AddWithValue("@ItemCategory_ID", item.ItemCategory_ID);
        //            cmd.Parameters.AddWithValue("@Item_Code", item.Item_Code);
        //            cmd.Parameters.AddWithValue("@Item_Name", item.Item_Name);
        //            cmd.Parameters.AddWithValue("@HSN_Code", item.HSN_Code);
        //            cmd.Parameters.AddWithValue("@MinStock", item.MinStock);
        //            cmd.Parameters.AddWithValue("@Description", item.Description);
        //            cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
        //            cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
        //            con.Open();
        //            //cmd.ExecuteNonQuery();
        //            SqlDataReader dataReader = cmd.ExecuteReader();

        //            while (dataReader.Read())
        //            {
        //                response.Status = Convert.ToBoolean(dataReader["Status"]);
        //            }
        //            con.Close();
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Status = false;
        //        log.Error(ex.Message, ex);
        //        return response;
        //    }
        //}
        #endregion

        #region Delete function
        public void Delete(int ID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_SupplierCustomer_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
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
    }
}