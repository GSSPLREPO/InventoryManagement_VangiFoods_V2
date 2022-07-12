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
    public class TransactionsPOandOCRepository : ITransactionsPOandOCRepository
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
        /// Farheen: This function is for fecthing list of transaction.
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
                        TransactionFlag = Convert.ToInt32(reader["TransactionFlag"]),
                        DraftFlag=Convert.ToBoolean(reader["DraftFlag"]),
                        //CompanyName = reader["CompanyName"].ToString(),
                        Tittle = reader["Tittle"].ToString(),
                        DocumentNumber = reader["DocumentNumber"].ToString(),
                        InvoiceStat = reader["InvoiceStatus"].ToString(),
                        GoodsStat = reader["GoodsStatus"].ToString(),
                        OrderStatus=reader["OrderStatus"].ToString(),
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

        #region Insert function for Order Confirmation
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
                cmd.Parameters.AddWithValue("@Title", orderConfirmation.Tittle);
                cmd.Parameters.AddWithValue("@DocumentNumber", orderConfirmation.DocumentNumber);
                cmd.Parameters.AddWithValue("@DocumentDate", orderConfirmation.DocumentDate);
                cmd.Parameters.AddWithValue("@DeliveryDate", orderConfirmation.DeliveryDate);
                cmd.Parameters.AddWithValue("@Amendment", orderConfirmation.Amendment);
                cmd.Parameters.AddWithValue("@WorkOrderNo", orderConfirmation.WorkOrderNo);
                cmd.Parameters.AddWithValue("@PONumber", orderConfirmation.PONumber);
                cmd.Parameters.AddWithValue("@BuyerAdd", orderConfirmation.BuyerAddress);
                cmd.Parameters.AddWithValue("@SupplierAdd", orderConfirmation.SupplierAddress);
                cmd.Parameters.AddWithValue("@ItemID", orderConfirmation.Item_ID);
                cmd.Parameters.AddWithValue("@ItemQuantity", orderConfirmation.ItemQuantity);
                cmd.Parameters.AddWithValue("@Tax", orderConfirmation.Tax);
                cmd.Parameters.AddWithValue("@TotalItemCost", orderConfirmation.TotalItemCost);
                cmd.Parameters.AddWithValue("@GrandTotal", orderConfirmation.GrandTotal);
                cmd.Parameters.AddWithValue("@AdvancedPAyment", orderConfirmation.AdvancedPAyment);
                cmd.Parameters.AddWithValue("@Signature", orderConfirmation.Signature);
                cmd.Parameters.AddWithValue("@DrafFlag", orderConfirmation.DraftFlag);
                cmd.Parameters.AddWithValue("@CreatedBy", 1);
                cmd.Parameters.AddWithValue("@CreatedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            };
        }
        #endregion

        #region Update function for Order Confirmation
        public PurchaseOrder GetById(int PurchaseOrderId)
        {
            var orderConf = new PurchaseOrder();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_OrderConfirmation_GetByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PurchaseOrderId", PurchaseOrderId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orderConf = new PurchaseOrder()
                    {
                        PurchaseOrderId = Convert.ToInt32(reader["PurchaseOrderId"]),
                        //CompanyName = reader["CompanyName"].ToString(),
                        Tittle = reader["Tittle"].ToString(),
                        PONumber = reader["PONumber"].ToString(),
                        DocumentDate = Convert.ToDateTime(reader["DocumentDate"]),
                        DocumentNumber = reader["DocumentNumber"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        Amendment = Convert.ToInt32(reader["Amendment"]),
                        WorkOrderNo = reader["WorkOrderNo"].ToString(),
                        BuyerAddress = reader["BuyerAddress"].ToString(),
                        SupplierAddress = reader["SupplierAddress"].ToString(),
                        Item_ID = Convert.ToInt32(reader["Item_ID"]),
                        ItemDescription = reader["ItemDescription"].ToString(),
                        ItemUnit = reader["ItemUnit"].ToString(),
                        ItemPrice = Convert.ToDecimal(reader["ItemPrice"]),
                        ItemTax = reader["ItemTax"].ToString(),
                        ItemQuantity = Convert.ToDecimal(reader["ItemQuantity"]),
                        Signature = reader["Signature"].ToString(),
                        TotalItemCost = Convert.ToDecimal(reader["TotalItemCost"]),
                        Tax = reader["Tax"].ToString(),
                        GrandTotal = Convert.ToDecimal(reader["GrandTotal"]),
                        AdvancedPAyment = Convert.ToDecimal(reader["AdvancedPayment"])
                    };
                }
                con.Close();
                return orderConf;
            }
        }

        public void Udate(PurchaseOrder orderConfirmation)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_OrderConfirmation_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PurchaseOrderID", orderConfirmation.PurchaseOrderId);
                cmd.Parameters.AddWithValue("@Title", orderConfirmation.Tittle);
                cmd.Parameters.AddWithValue("@DocumentNumber", orderConfirmation.DocumentNumber);
                cmd.Parameters.AddWithValue("@DocumentDate", orderConfirmation.DocumentDate);
                cmd.Parameters.AddWithValue("@DeliveryDate", orderConfirmation.DeliveryDate);
                cmd.Parameters.AddWithValue("@Amendment", orderConfirmation.Amendment);
                cmd.Parameters.AddWithValue("@WorkOrderNo", orderConfirmation.WorkOrderNo);
                cmd.Parameters.AddWithValue("@PONumber", orderConfirmation.PONumber);
                cmd.Parameters.AddWithValue("@BuyerAdd", orderConfirmation.BuyerAddress);
                cmd.Parameters.AddWithValue("@SupplierAdd", orderConfirmation.SupplierAddress);
                cmd.Parameters.AddWithValue("@ItemID", orderConfirmation.Item_ID);
                cmd.Parameters.AddWithValue("@ItemQuantity", orderConfirmation.ItemQuantity);
                cmd.Parameters.AddWithValue("@Tax", orderConfirmation.Tax);
                cmd.Parameters.AddWithValue("@TotalItemCost", orderConfirmation.TotalItemCost);
                cmd.Parameters.AddWithValue("@GrandTotal", orderConfirmation.GrandTotal);
                cmd.Parameters.AddWithValue("@AdvancedPAyment", orderConfirmation.AdvancedPAyment);
                cmd.Parameters.AddWithValue("@Signature", orderConfirmation.Signature);
                cmd.Parameters.AddWithValue("@DrafFlag", orderConfirmation.DraftFlag);
                cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        #endregion

        #region Delete function
        public void Delete(int PurchaseOrderID)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_TransactionsPO_OC_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PurchaseOrderId", PurchaseOrderID);
                cmd.Parameters.AddWithValue("@LastModifiedBy", 1);
                cmd.Parameters.AddWithValue("@LastModifiedDate", Convert.ToDateTime(System.DateTime.Now));
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

        #region Get company details by it's ID
        public IEnumerable<Company> GetCompanyDetailsById(int Id)
        {
            List<Company> CompanyList = new List<Company>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_Company_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", Id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var company = new Company()
                    {
                        Company_ID = Convert.ToInt32(reader["Company_ID"]),
                        Name = reader["Name"].ToString(),
                        Address = reader["Address"].ToString(),
                        CityName = reader["City"].ToString(),
                        StateName = reader["State"].ToString(),
                        CountryName = reader["Country"].ToString()
                    };
                    CompanyList.Add(company);
                }
                con.Close();
                return CompanyList;
            }
        }
        #endregion

        #region Generate document number
        public string GetDocumentNo(int DocumentType)
        {
            var DocumentNo = "";
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
            return DocumentNo;
        }

        #endregion

        #region Get list of Items for PO and OC dropdown
        public IEnumerable<Item> GetItemDetailsForDD(int ItemType)
        {
            List<Item> ItemList = new List<Item>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_GetItemListForPOandOC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemType", ItemType);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    var item = new Item()
                    {
                        Item_ID = Convert.ToInt32(reader["Item_ID"]),
                        Item_Code = reader["Item_Code"].ToString()
                    };
                    ItemList.Add(item);
                }
                con.Close();
                return ItemList;
            }
        }

        #endregion

        #region Get details of Items by ID
        public Item GetItemDetails(int itemID)
        {
            Item ItemDetails = new Item();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_GetItemDetailsByIdForPOandOC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", itemID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {
                    ItemDetails = new Item()
                    {
                        Item_Name = reader["Item_Name"].ToString(),
                        UnitName = reader["UnitID"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Tax = reader["Tax"].ToString()
                    };
                }
                con.Close();
                return ItemDetails;
            }
        }

        #endregion

        #region This method is for View the transaction form
        public PurchaseOrder ViewTransactions(int PurchaseOrderId, int TransactionFlag)
        {
            var orderConf = new PurchaseOrder();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_tbl_OrderConfirmation_View", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PurchaseOrderId", PurchaseOrderId);
                cmd.Parameters.AddWithValue("@TransactionFlag", TransactionFlag);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orderConf = new PurchaseOrder()
                    {
                        PurchaseOrderId = Convert.ToInt32(reader["PurchaseOrderId"]),
                        SupplierAddress = reader["SupplierAddress"].ToString(),
                        BuyerAddress = reader["BuyerAddress"].ToString(),
                        DocumentNumber = reader["DocumentNumber"].ToString(),
                        DocumentDate = Convert.ToDateTime(reader["DocumentDate"]),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        Amendment = Convert.ToInt32(reader["Amendment"]),
                        GrandTotal = Convert.ToDecimal(reader["GrandTotal"]),
                        WorkOrderNo=reader["WorkOrderNo"].ToString(),
                        ItemDescription = reader["Description"].ToString(),
                        Item_Code=reader["ItemCode"].ToString(),
                        Item_HSN_Code=reader["HSN_Code"].ToString(),
                        ItemQuantity = Convert.ToDecimal(reader["ItemQuantity"]),
                        ItemUnit = reader["ItemUnit"].ToString(),
                        ItemPrice = Convert.ToDecimal(reader["ItemPrice"]),
                        Signature = reader["Signature"].ToString(),
                        TotalItemCost=Convert.ToDecimal(reader["TotalItemCost"]),
                        Tax=reader["Tax"].ToString(),
                        AdvancedPAyment = Convert.ToDecimal(reader["AdvancedPayment"])
                    };
                }
                con.Close();
                return orderConf;
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

        #region  Bind grid for Report
        /// <summary>
        /// Farheen: This function is for fecthing list of order transaction for report.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PurchaseOrder> Report_OC_GetAll(DateTime FromDate, DateTime ToDate)
        {
            List<PurchaseOrder> purchaseOrdersList = new List<PurchaseOrder>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("usp_rpt_OrderConfirmation_GetAll", con);
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(); //returns the set of row.
                while (reader.Read())
                {

                    var PO = new PurchaseOrder()
                    {
                        PurchaseOrderId = Convert.ToInt32(reader["SrNo"]),
                        OrderStatus = reader["OrderStatus"].ToString(),
                        //CompanyName = reader["CompanyName"].ToString(),
                        Tittle = reader["Tittle"].ToString(),
                        DocumentNumber = reader["DocumentNumber"].ToString(),
                        DocumentDate = Convert.ToDateTime(reader["DocumentDate"]),
                        DeliveryDate= Convert.ToDateTime(reader["DeliveryDate"]),
                        GrandTotal=Convert.ToDecimal(reader["GrandTotal"]),
                        AdvancedPAyment=Convert.ToDecimal(reader["AdvancedPAyment"]),
                        InvoiceStat = reader["InvoiceStatus"].ToString(),
                        GoodsStat = reader["GoodsStatus"].ToString()
                    };
                    purchaseOrdersList.Add(PO);
                }
                con.Close();
                return purchaseOrdersList;
            }
            //return _context.UnitMasters.ToList();
        }
        #endregion


    }
}