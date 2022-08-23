using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository;
using InVanWebApp.DAL;
using System.Globalization;
using System.IO;
using System.Web.Helpers;

namespace InVanWebApp.Controllers
{
    public class TransactionsPOandOCController : Controller
    {
        private ITransactionsPOandOCRepository _transactionsPOandOC;

        #region Initializing constructor
        /// <summary>
        /// Date: 02 june'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public TransactionsPOandOCController()
        {
            _transactionsPOandOC = new TransactionsPOandOCRepository(new InVanDBContext());
        }

        /// <summary>
        /// Date: 02 june'22
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="addItemRepository"></param>
        public TransactionsPOandOCController(ITransactionsPOandOCRepository transactionsPOandOC)
        {
            _transactionsPOandOC = transactionsPOandOC;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 02 june'22
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public ActionResult Index()
        {
            //var companyList = _transactionsPOandOC.GetCompanyNameForDropDown();
            //var dd = new SelectList(companyList.ToList(), "Company_ID", "Name");
            //ViewData["company"] = dd;
            var model = _transactionsPOandOC.GetAll();
            return View(model);
        }
        #endregion

        #region Insert function for order confirmation
        /// <summary>
        /// Date: 03 june'22
        /// Farheen: Rendered the user to the add order confirmation form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddOrderConfirmation()
        {
            //Supplier address for OC
            var companyList = _transactionsPOandOC.GetCompanyDetailsById(2);
            ViewData["companyName"] = (companyList.ToList())[0].CompanyName;
            ViewData["companyAddress1"] = (companyList.ToList())[0].Address;
            ViewData["companyAddress2"] = (companyList.ToList())[0].CityName + " " + (companyList.ToList())[0].StateName + ", " + (companyList.ToList())[0].CountryName;

            //Document number for OC
            var DocumentNumber = _transactionsPOandOC.GetDocumentNo(1); //here document type=1 i.e. for generating the Order confirmation (logic is in SP).
            ViewData["DocumentNo"] = DocumentNumber;

            //Binding item grid with sell type item.
            var itemList = _transactionsPOandOC.GetItemDetailsForDD(1);
            var dd = new SelectList(itemList.ToList(), "Item_ID", "Item_Code");
            ViewData["itemListForDD"] = dd;

            return View();
        }

        /// <summary>
        /// Date: 03 june'22
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrderConfirmation(PurchaseOrder model, HttpPostedFileBase Signature)
        {
            if (ModelState.IsValid)
            {
                if (Signature != null)
                {
                    string path = Server.MapPath("~/SignatureImages/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    Signature.SaveAs(path + Path.GetFileName(Signature.FileName));
                    ViewBag.Message = "File uploaded successfully.";
                }
                model.Signature = Signature.FileName.ToString();
                _transactionsPOandOC.Insert(model);
                return RedirectToAction("Index", "TransactionsPOandOC");
            }
            var itemList = _transactionsPOandOC.GetItemDetailsForDD(1);
            var dd = new SelectList(itemList.ToList(), "Item_ID", "Item_Code");
            ViewData["itemListForDD"] = dd;
            return View();
        }

        #endregion

        #region  Update function
        /// <summary>
        /// Date: 18 june'22
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="orderConfId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditOrderConfirmation(int PurchaseOrderId, int TransactionFlag)
        {
            //Transaction flag is 1 that means this is Order confirmation transaction else it'll be Purchase order transaction.
            if (TransactionFlag == 1)
            {
                //For order confirmation getBy id.

                //Binding item grid with sell type item.
                var itemList = _transactionsPOandOC.GetItemDetailsForDD(1);
                var dd = new SelectList(itemList.ToList(), "Item_ID", "Item_Code");
                ViewData["itemListForDD"] = dd;
                PurchaseOrder model = _transactionsPOandOC.GetById(PurchaseOrderId);

                //var fileName = model.Signature.ToString();

                //DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/SignatureImages"));
                //if (dir != null)
                //{ 
                //    DirectoryInfo dir1 = new DirectoryInfo(Server.MapPath("~/SignatureImages/" + fileName));
                //}

                //ViewData["ff"] = model.Signature;
                return View(model);
            }
            else
            {
                //For Purchase order getBy Id.

                PurchaseOrder model = _transactionsPOandOC.GetById(PurchaseOrderId);
                return View(model);
            }
        }

        /// <summary>
        /// Date: 18 june'22
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditOrderConfirmation(PurchaseOrder model)
        {
            if (ModelState.IsValid)
            {
                _transactionsPOandOC.Udate(model);
                return RedirectToAction("Index", "TransactionsPOandOC");
            }
            else
                return View(model);
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 22 June'22
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="Location_ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteTransactions(int PurchaseOrderId)
        {
            PurchaseOrder model = _transactionsPOandOC.GetById(PurchaseOrderId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int PurchaseOrderId)
        {
            _transactionsPOandOC.Delete(PurchaseOrderId);
            //_unitRepository.Save();
            return RedirectToAction("Index", "TransactionsPOandOC");
        }
        #endregion

        #region This method is for View the transaction form
        [HttpGet]
        public ActionResult ViewTransaction(int PurchaseOrderId, int TransactionFlag)
        {
            PurchaseOrder model = _transactionsPOandOC.ViewTransactions(PurchaseOrderId, TransactionFlag);
            return View(model);
        }
        #endregion

        #region Function for get item details
        public JsonResult GetitemDetails(string id)
        {
            var itemId = Convert.ToInt32(id);
            var itemDetails = _transactionsPOandOC.GetItemDetails(itemId);
            //var finalDetials = itemDetails.Item_Name +"#"+ itemDetails.UnitName +"#"+ itemDetails.Price+"#"+itemDetails.Tax;
            //return Json(finalDetials);
            return Json(itemDetails);
        }
        #endregion

        #region Report for Order confirmation
        [HttpGet]
        public ActionResult Report_OC_Index()
        {
            return View();
        }

        public JsonResult BindGrid(string fromDate, string toDate)
        {
            try
            {
                //DateTime dateTime = DateTime.ParseExact(fromDate,"dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime dtFromDateTime = DateTime.ParseExact(fromDate + " " + "00:00:00", "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture);
                DateTime dtToDateTime = DateTime.ParseExact(toDate + " " + "23:59:59", "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture);
                if (dtFromDateTime <= dtToDateTime)
                {

                   // string[] columnNames = typeof(Academics.AcademicAssignment).GetProperties().Select(property => property.Name).ToArray();

                    var objResult = _transactionsPOandOC.Report_OC_GetAll(dtFromDateTime, dtToDateTime);
                    //string[] columnNames = typeof(PurchaseOrder).GetProperties().Select(p=>p.Name).ToArray();

                    //// string[] columnNames = typeof(Academics.AcademicAssignment).GetProperties().Select(property => property.Name).ToArray();

                    //List<WebGridColumn> columns = new List<WebGridColumn>();
                    //columns.Add(new WebGridColumn() { ColumnName = columnNames[1], Header = "OrderStatus" });
                    //columns.Add(new WebGridColumn() { ColumnName = columnNames[2], Header = "Stream" });
                    //columns.Add(new WebGridColumn() { ColumnName = columnNames[3], Header = "Section" });
                    //columns.Add(new WebGridColumn() { ColumnName = columnNames[6], Header = "Title" });
                    //columns.Add(new WebGridColumn() { ColumnName = columnNames[7], Header = "Issue Date" });
                    //columns.Add(new WebGridColumn() { ColumnName = columnNames[8], Header = "Deadline Date" });
                    //columns.Add(new WebGridColumn() { ColumnName = columnNames[10], Header = "Description" });

                    //ViewBag.Columns = columns;
                    //var result = objResult.ToList();
                    //return Json(result, JsonRequestBehavior.AllowGet);

                    return Json(objResult);
                }
                else
                {
                    return Json("NoDataFound");
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
                //throw;
            }
        }
        #endregion

    }
}