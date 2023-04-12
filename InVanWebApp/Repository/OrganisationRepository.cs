using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using log4net;
using System.Configuration;
using InVanWebApp.Common;

namespace InVanWebApp.Repository
{
    public class OrganisationRepository : IOrganisationRepository
    {
        //private readonly InVanDBContext _context;
        private readonly string connString = Encryption.Decrypt_Static(ConfigurationManager.ConnectionStrings["InVanContext"].ToString());
        private static ILog log = LogManager.GetLogger(typeof(OrganisationRepository));

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of organisation.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrganisationsBO> GetAll()
        {
            List<OrganisationsBO> organisationList = new List<OrganisationsBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Organisation_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                    while (reader.Read())
                    {
                        var result = new OrganisationsBO()
                        {
                            OrganisationId = Convert.ToInt32(reader["OrganisationId"]),
                            Name = reader["Name"].ToString(),
                            OrganisationGroupName = reader["OrganisationGroupName"].ToString(),
                            Abbreviation = reader["Abbreviation"].ToString(),
                            ContactPerson = reader["ContactPerson"].ToString(),
                            ContactNo = reader["ContactNo"].ToString(),
                            //CityName = reader["CityName"].ToString(),
                            Description = reader["Description"].ToString()
                        };
                        organisationList.Add(result);
                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return organisationList;
        }
        #endregion

        #region Update functions
        /// <summary>
        /// Farheen: This function is for fetch data for editing by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        public OrganisationsBO GetById(int ID)
        {
            var result = new OrganisationsBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Organisation_GetByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = new OrganisationsBO()
                        {
                            OrganisationId = Convert.ToInt32(reader["OrganisationId"]),
                            OrganisationGroupId = Convert.ToInt32(reader["OrganisationGroupId"]),
                            Name = reader["Name"].ToString(),
                            Abbreviation=reader["Abbreviation"].ToString(),
                            Address=reader["Address"].ToString(),
                            ContactPerson=reader["ContactPerson"].ToString(),
                            ContactNo=reader["ContactNo"].ToString(),
                            Email=reader["Email"].ToString(),
                            GSTINNo=reader["GSTINNo"].ToString(),
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
            //return _context.UnitMasters.Find(UnitID);
        }

        /// <summary>
        /// Farheen: Update record
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Update(OrganisationsBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Organisation_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrganisationId", model.OrganisationId);
                    cmd.Parameters.AddWithValue("@OrganisationGroupId", model.OrganisationGroupId);
                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@Abbreviation", model.Abbreviation);
                    cmd.Parameters.AddWithValue("@Address", model.Address);
                    cmd.Parameters.AddWithValue("@ContactPerson", model.ContactPerson);
                    cmd.Parameters.AddWithValue("@ContactNo", model.ContactNo);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@GSTINNo", model.GSTINNo);
                    cmd.Parameters.AddWithValue("@Description", model.Description); 
                    cmd.Parameters.AddWithValue("@LastModifiedBy", model.UserId);
                    cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                log.Error(ex.Message, ex);
                return response;
            }

            //_context.Entry(unitMaster).State = EntityState.Modified;
        }

        #endregion

        #region Insert function
        /// <summary>
        /// Farheen: Insert record.
        /// </summary>
        /// <param name="model"></param>
        public ResponseMessageBO Insert(OrganisationsBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                //var userId = Session[ApplicationSession.USERID];
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_Organisation_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrganisationGroupId", model.OrganisationGroupId);
                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@Abbreviation", model.Abbreviation);
                    cmd.Parameters.AddWithValue("@Address", model.Address);
                    cmd.Parameters.AddWithValue("@ContactPerson", model.ContactPerson);
                    cmd.Parameters.AddWithValue("@ContactNo", model.ContactNo);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@GSTINNo", model.GSTINNo);
                    cmd.Parameters.AddWithValue("@Description", model.Description);
                    cmd.Parameters.AddWithValue("@CreatedBy", model.UserId) ;
                    cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                    con.Open();
                    //cmd.ExecuteNonQuery();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        response.Status = Convert.ToBoolean(dataReader["Status"]);
                    }
                    con.Close();
                }
                //return true;

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
                    SqlCommand cmd = new SqlCommand("usp_tbl_Organisation_Delete", con);
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

        #region Function for binding dropdown.
        public IEnumerable<OrganisationGroupBO> GetOrganisationGroupList()
        {
            List<OrganisationGroupBO> resultList = new List<OrganisationGroupBO>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("usp_tbl_OrganisationGroups_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        var result = new OrganisationGroupBO()
                        {
                            OrganisationGroupId = Convert.ToInt32(dataReader["OrganisationGroupId"]),
                            Name = dataReader["Name"].ToString()
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