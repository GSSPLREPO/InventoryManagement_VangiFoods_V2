using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp.Repository;
using InVanWebApp.DAL;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace InVanWebApp.Repository
{
    public class TransactionsPOandOCRepository:ITransactionsPOandOCRepository
    {
        private readonly InVanDBContext _context;
        private readonly string conString = ConfigurationManager.ConnectionStrings["InVanContext"].ConnectionString;

        #region Initializing constructor.
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public TransactionsPOandOCRepository()
        {
            //Define the DbContext object.
            _context = new InVanDBContext();
        }

        //Constructor with parameter for initializing the DbContext object.
        public TransactionsPOandOCRepository(InVanDBContext context)
        {
            _context = context;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Farheen: This function is for fecthing list of item category master's.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PurchaseOrder> GetAll()
        {
            List<PurchaseOrder> purchaseOrdersList = new List<PurchaseOrder>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_TransactionsPOandOC_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {

                    var PO = new PurchaseOrder()
                    {
                        PurchaseOrderId = Convert.ToInt32(reader["PurchaseOrderId"]),
                        CompanyName = reader["CompanyName"].ToString(),
                        Tittle = reader["Tittle"].ToString(),
                        DocumentNumber = reader["DocumentNumber"].ToString(),
                        InvoiceStat = reader["InvoiceStatus"].ToString(),
                        GoodsStat = reader["GoodsStatus"].ToString(),
                        LastModifiedDate = Convert.ToDateTime(reader["LastModifiedDate"])
                    };
                    purchaseOrdersList.Add(PO);
                }
                con.Close();
                return purchaseOrdersList;
            }
            //return _context.UnitMasters.ToList();
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 03 june'22
        /// Farheen: Insert order confirmation record.
        /// </summary>
        /// <param name="orderConfirmation"></param>
        public void Insert(PurchaseOrder orderConfirmation)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_OrderConfirmation_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LocationName", orderConfirmation.Tittle);
                cmd.Parameters.AddWithValue("@Address", orderConfirmation.DocumentNumber);
                cmd.Parameters.AddWithValue("@Country_ID", orderConfirmation.DocumentDate);
                cmd.Parameters.AddWithValue("@State_ID", orderConfirmation.DeliveryDate);
                cmd.Parameters.AddWithValue("@City_ID", orderConfirmation.Item);
                cmd.Parameters.AddWithValue("@Pincode", orderConfirmation.ItemQuantity);
                cmd.Parameters.AddWithValue("@CreatedBy", 1);
                cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            };
        }
        #endregion

        #region  Bind drop-down of Company
        public IEnumerable<Company> GetCompanyNameForDropDown()
        {
            List<Company> CompanyList = new List<Company>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Company_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var company = new Company()
                    {
                        Company_ID = Convert.ToInt32(reader["Company_ID"]),
                        Name = reader["Name"].ToString()
                    };
                    CompanyList.Add(company);
                }
                con.Close();
                return CompanyList;
            }
        }
        #endregion


        #region Dispose function
        private bool disposed = false;

        /// <summary>
        /// Date: 26 may'22
        /// For releasing unmanageable objects and scarce resources,
        /// like deallocating the controller instance.   
        ///And it get called when the view is rendered.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    _context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}