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
using InVanWebApp.Common;

namespace InVanWebApp.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        //private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;
        private readonly string conString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
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
                    SqlCommand cmd = new SqlCommand("usp_tbl_CompanyDetails_GetAll", con);
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
                            ContactPersonNo = reader["ContactPersonNo"].ToString(),
                            IsActive =Convert.ToBoolean(reader["IsActive"])
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
        public ResponseMessageBO Insert(CompanyBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_CompanyDetails_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyType", model.CompanyType);
                    cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
                    cmd.Parameters.AddWithValue("@EmailId", model.EmailId);
                    cmd.Parameters.AddWithValue("@ContactPersonName", model.ContactPersonName);
                    cmd.Parameters.AddWithValue("@ContactPersonNo", model.ContactPersonNo);
                    cmd.Parameters.AddWithValue("@Address", model.Address);
                    cmd.Parameters.AddWithValue("@GSTNumber", model.GSTNumber);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                    cmd.Parameters.AddWithValue("@IsBlackListed", model.IsBlackListed);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.CompanyName = dataReader["CompanyName"].ToString();
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
                };
            }
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
            }
            return response;
        }

        /// <summary>
        /// Date: 02 Sept 2022
        /// Farheen: This function is for uploading companies list (Bulk upload)
        /// </summary>
        /// <param name="model"></param>
        public List<ResponseMessageBO> SaveCompanyData(List<CompanyBO> model)
        {
            try
            {
                //var success = false;
                int cnt = 0;
                List<ResponseMessageBO> responsesList = new List<ResponseMessageBO>();
                if (model != null && model.Count > 0)
                {

                    for (int i = 0; i < model.Count; i++)
                    {
                        CompanyBO material = new CompanyBO();

                        material.CompanyName = model[i].CompanyName;
                        material.CompanyType = model[i].CompanyType;
                        material.ContactPersonName = model[i].ContactPersonName;
                        material.ContactPersonNo = model[i].ContactPersonNo;
                        material.EmailId = model[i].EmailId;
                        material.Address= model[i].Address;
                        material.GSTNumber = model[i].GSTNumber;
                        material.Remarks = model[i].Remarks;
                        material.IsActive = model[i].IsActive;
                        material.IsBlackListed = model[i].IsBlackListed;

                        material.IsDeleted = false;
                        material.CreatedBy = model[i].CreatedBy;
                        material.CreatedDate = Convert.ToDateTime(model[i].CreatedDate);

                        using (SqlConnection con = new SqlConnection(conString))
                        {
                            SqlCommand cmd = new SqlCommand("usp_tbl_CompanyDetails_Insert", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CompanyType", material.CompanyType);
                            cmd.Parameters.AddWithValue("@CompanyName", material.CompanyName);
                            cmd.Parameters.AddWithValue("@EmailId", material.EmailId);
                            cmd.Parameters.AddWithValue("@ContactPersonName", material.ContactPersonName);
                            cmd.Parameters.AddWithValue("@ContactPersonNo", material.ContactPersonNo);
                            cmd.Parameters.AddWithValue("@Address", material.Address);
                            cmd.Parameters.AddWithValue("@GSTNumber", material.GSTNumber);
                            cmd.Parameters.AddWithValue("@Remarks", material.Remarks);
                            cmd.Parameters.AddWithValue("@IsActive", material.IsActive);
                            cmd.Parameters.AddWithValue("@IsBlackListed", material.IsBlackListed);
                            cmd.Parameters.AddWithValue("@CreatedBy", 1);
                            cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                            con.Open();
                            SqlDataReader dataReader = cmd.ExecuteReader();


                            while (dataReader.Read())
                            {
                                ResponseMessageBO response = new ResponseMessageBO();

                                response.CompanyName = dataReader["CompanyName"].ToString();
                                response.Status = Convert.ToBoolean(dataReader["Status"]);

                                responsesList.Add(response);
                            }
                            con.Close();
                        };

                        //success = true;

                        cnt += 1;
                    }
                }
                //return success;
                return responsesList;
            }
            catch (Exception ex)
            {
                //dbContextTransaction.Dispose();
                throw ex;
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
                    SqlCommand cmd = new SqlCommand("usp_tbl_CompanyDetails_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        company = new CompanyBO()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            CompanyName = reader["CompanyName"].ToString(),
                            CompanyType = reader["CompanyType"].ToString(),
                            EmailId = reader["EmailId"].ToString(),
                            ContactPersonName = reader["ContactPersonName"].ToString(),
                            ContactPersonNo = reader["ContactPersonNo"].ToString(),
                            Address=reader["Address"].ToString(),
                            GSTNumber=reader["GSTNumber"].ToString(),
                            Remarks=reader["Remarks"].ToString(),
                            IsActive=Convert.ToBoolean(reader["IsActive"]),
                            IsBlackListed=Convert.ToBoolean(reader["IsBlackListed"])
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
        /// <param name="model"></param>
        public ResponseMessageBO Update(CompanyBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_CompanyDetails_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@CompanyType", model.CompanyType);
                    cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
                    cmd.Parameters.AddWithValue("@EmailId", model.EmailId);
                    cmd.Parameters.AddWithValue("@ContactPersonName", model.ContactPersonName);
                    cmd.Parameters.AddWithValue("@ContactPersonNo", model.ContactPersonNo);
                    cmd.Parameters.AddWithValue("@Address", model.Address);
                    cmd.Parameters.AddWithValue("@GSTNumber", model.GSTNumber);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                    cmd.Parameters.AddWithValue("@IsBlackListed", model.IsBlackListed);
                    cmd.Parameters.AddWithValue("@LastModifiedBy", model.LastModifiedBy);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
            }
            return response;
        }
        #endregion

        #region Delete function
        public void Delete(int ID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_CompanyDetails_Delete", con);
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