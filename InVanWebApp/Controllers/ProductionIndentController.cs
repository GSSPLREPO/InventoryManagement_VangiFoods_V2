using InVanWebApp.Common;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InVanWebApp.Controllers
{
    public class ProductionIndentController : Controller
    {
        private IProductionIndentRepository _productionIndentRepository;
        private IUserDetailsRepository _userDetailsRepository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private IIndentRepository _repository;
        private IRecipeMaterRepository _productionRecipeRepository;
        private IProductMasterRepository _productMasterRepository;
        private ISalesOrderRepository _salesOrderRepository;
        private IBatchPlanningRepository _batchPlanningRepository;

        private static ILog log = LogManager.GetLogger(typeof(ProductionIndentController));

        #region Initializing constructor
        /// <summary>
        /// Date: 24 Feb'23
        /// Rahul:  Constructor without parameter 
        /// </summary>
        public ProductionIndentController()
        {
            _productionIndentRepository = new ProductionIndentRepository();
            _userDetailsRepository = new UserDetailsRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
            _repository = new IndentRepository();
            _productionRecipeRepository = new RecipeMaterRepository();
            _productMasterRepository = new ProductMasterRepository();
            _salesOrderRepository = new SalesOrderRepository();
            _batchPlanningRepository = new BatchPlanningRepository();
        }

        /// <summary>
        /// Date: 24 Feb'23
        /// Rahul:  Constructor with parameters for initializing the interface object. 
        /// </summary>
        ///<param name="itemRepository"></param>
        public ProductionIndentController(IProductionIndentRepository productionIndentRepository)
        {
            _productionIndentRepository = productionIndentRepository; 
        }
        #endregion

        #region Bind grid 
        /// <summary>
        /// Date: 24 Feb 2023
        /// Rahul:  Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: ProductionIndent
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _productionIndentRepository.GetAll();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        #endregion

        #region Bind dorpdowns
        public void BindUsers()
        {
            var UserId = Convert.ToInt32(Session[ApplicationSession.USERID]);
            var result = _userDetailsRepository.GetAll(UserId);
            var resultList = new SelectList(result.ToList(), "EmployeeID", "EmployeeName");
            ViewData["EmployeeName"] = resultList;
        }        
        public void BindItemTypeCategory()
        {
            var product = _productionRecipeRepository.GetAll();
            var dd4 = new SelectList(product.ToList(), "RecipeID", "RecipeName");
            ViewData["ProductName"] = dd4;

            //Binding item grid with Recipe. 
            var recipeList = _productionRecipeRepository.GetItemDetailsForRecipe();
            var dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", "UOM_Id");
            ViewData["Ingredients"] = dd;

            //Bind SO Number 
            //var result = _productionIndentRepository.GetSONumberForD/*ropdown();*/

            var result = _salesOrderRepository.GetAll();
            //var resultList = new SelectList(result.ToList(), "SalesOrderId", "SONumber");
            var resultList = new SelectList(result.ToList(), "SalesOrderId", "SONo");
            ViewData["SONumberAndId"] = resultList;

            //Bind WO Number 
            //var resultWO = _productionIndentRepository.GetSONumberForDropdown();
            //var resultListWO = new SelectList(resultWO.ToList(), "SalesOrderId", "WorkOrderNo");

            var resultWO = _salesOrderRepository.GetAll();
            var resultListWO = new SelectList(resultWO.ToList(), "SalesOrderId", "WorkOrderNo");
            ViewData["WONumberAndId"] = resultListWO;

        }
        #endregion

        #region Bind all Recipe details 

        public JsonResult GetRecipe(string id, string RecipeID = null)
        {
            int ProductId = Convert.ToInt32(id);
            int recipeID = Convert.ToInt32(RecipeID);

            var result = _productionIndentRepository.GetRecipeDetailsById(ProductId, recipeID);
            return Json(result);
        }

        //public JsonResult BindRecipeDetails(string id, string RecipeID=null)  //Not in use 25-05-23.
        //{
        //    int ProductId = 0;  
        //    int RecipeId = 0;  

        //    if (id != "" && id != null)
        //        ProductId = Convert.ToInt32(id);

        //    if (RecipeID != "" && RecipeID != null)
        //        RecipeId = Convert.ToInt32(RecipeID);

        //    var result = _productionIndentRepository.GetRecipeDetailsById(ProductId,RecipeId);
        //    return Json(result);
        //}
        #endregion

        //#region Bind all BindBatch Number details   //Not in use 25-05-23.
        //public JsonResult BindBatchNumber(string id, string TotalBatches = null) 
        //{
        //    int SO_Id = 0;
        //    int Total_Batches = 0;

        //    if (id != "" && id != null)
        //        SO_Id = Convert.ToInt32(id);

        //    if (TotalBatches != "" && TotalBatches != null)
        //        Total_Batches = Convert.ToInt32(TotalBatches);

        //    var result = _productionIndentRepository.GetBatchNumberById(SO_Id, Total_Batches);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

        #region Bind all BindBatch Number details 
        public JsonResult BindBatchNumber(string id, string SO_Id)
        {
            int Item_ID = 0; 
            int SO_ID = 0; 

            if (id != "" && id != null)
                Item_ID = Convert.ToInt32(id); 
            if (SO_Id != "" && SO_Id != null)
                SO_ID = Convert.ToInt32(SO_Id); 

            var result = _productionIndentRepository.GetBatchNumberById(Item_ID, SO_ID); 
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Insert functionality of Production Indent
        [HttpGet]
        public ActionResult AddProductionIndent()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindUsers();
                BindItemTypeCategory();
                BindSONumber();

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=16 i.e. for generating the Production Indent Number(logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(16);
                ViewData["DocumentNo"] = DocumentNumber;

                //Binding item grid with sell type item.
                var itemList = _repository.GetItemDetailsForDD();
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                ViewData["itemListForDD"] = dd;

                ProductionIndentBO model = new ProductionIndentBO();
                model.IssueDate = DateTime.Today;
                model.ProductionDate = DateTime.Today;

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Create By:Rahul 
        /// Dscription: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddProductionIndent(ProductionIndentBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();

                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _productionIndentRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Production Indent inserted successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Production Indent or insertion of the Indent is done! Can not be inserted!');</script>";
                            BindUsers();
                            BindItemTypeCategory();
                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            //=========here document type=16 i.e. for generating the Production Indent (logic is in SP).====//
                            var DocumentNumber = objDocNo.GetDocumentNo(16);
                            ViewData["DocumentNo"] = DocumentNumber;

                            //Binding item grid with sell type item.
                            var itemList = _repository.GetItemDetailsForDD();
                            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                            ViewData["itemListForDD"] = dd;

                            return View(model);
                        }

                        return RedirectToAction("Index", "ProductionIndent");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindUsers();
                        BindItemTypeCategory();
                        var itemList = _repository.GetItemDetailsForDD();
                        var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                        ViewData["itemListForDD"] = dd;
                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        //=========here document type=16 i.e. for generating the Production Indent (logic is in SP).====//
                        var DocumentNumber = objDocNo.GetDocumentNo(16);
                        ViewData["DocumentNo"] = DocumentNumber;

                        return View(model);
                    }
                }
                else
                    return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";

                BindUsers();
                BindItemTypeCategory();
                var itemList = _repository.GetItemDetailsForDD();
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                ViewData["itemListForDD"] = dd;
                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=16 i.e. for generating the Production Indent (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(16);
                ViewData["DocumentNo"] = DocumentNumber;

                return View(model);
            }
        }
        #endregion

        #region Update Functions
        /// <summary>
        ///Create By: Rahul 
        ///Description: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditProductionIndent(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindUsers();
                BindItemTypeCategory();

                ProductionIndentBO model = _productionIndentRepository.GetById(ID);
                model.indent_Details = _productionIndentRepository.GetItemDetailsByProductionIndentId(ID); 

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");

        }

        /// <summary>
        /// Rahul:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProductionIndent(ProductionIndentBO model) 
        {
            ResponseMessageBO response = new ResponseMessageBO();

            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _productionIndentRepository.Update(model);

                        if (response.Status)
                            TempData["Success"] = "<script>alert('Production Indent updated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                            BindUsers();
                            BindItemTypeCategory();

                            ProductionIndentBO model1 = _productionIndentRepository.GetById(model.ID);
                            model1.indent_Details = _productionIndentRepository.GetItemDetailsByProductionIndentId(model.ID);

                            return View(model1);
                        }

                        return RedirectToAction("Index", "ProductionIndent");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindUsers();
                        BindItemTypeCategory();

                        ProductionIndentBO model1 = _productionIndentRepository.GetById(model.ID);
                        model1.indent_Details = _productionIndentRepository.GetItemDetailsByProductionIndentId(model.ID);

                        return View(model1);
                    }
                }
                else
                    return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while update!');</script>";
                return RedirectToAction("Index", "ProductionIndent");
            }
        }
        #endregion


        #region Delete function
        /// <summary>
        /// Create By: Rahul
        /// Description: This function is for deleting the Production Indent and 
        /// Production Indent Ingredients details by using Production Indent ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteProductionIndent(int ID) 
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _productionIndentRepository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('Production Indent deleted successfully!');</script>";
                return RedirectToAction("Index", "ProductionIndent");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion


        #region Bind Dropdowns
        public void BindSONumber()
        {
            var result = _salesOrderRepository.GetAll();
            var resultList = new SelectList(result.ToList(), "SalesOrderId", "SONo");

            ViewData["SO_dd"] = resultList;
        }

        public void BindProductDropDown()
        {
            var result = _productMasterRepository.GetAll();
            var resultList = new SelectList(result.ToList(), "ProductID", "ProductName");
            ViewData["Product_dd"] = resultList;
        }

        public JsonResult GetWorkOrderNumber(string id)
        {
            var Id = Convert.ToInt32(id);
            var result = _productionIndentRepository.GetWorkOrderNumber(Id);
            return Json(result);
        }

        #endregion

        #region View Production Indent 
        /// <summary>
        ///Create By: Snehal 
        ///Description: View Indent Particular record.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewProductionIndent(int ID)
        {
            Session["ProductionIndent"] = ID; //Vedant added 13-07-23. 
            TempData["ProductionIndent"] = ID; //Vedant added 13-07-23. 

            if (Session[ApplicationSession.USERID] != null)
            {
                ProductionIndentBO model = _productionIndentRepository.GetById(ID);
                model.indent_Details = _productionIndentRepository.GetItemDetailsByProductionIndentId(ID);

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");

        }
        #endregion

        #region Export As Pdf Production Indent Report 
        /// <summary>
        /// Created By Vedant Parikh
        /// Date : 13 July'23
        /// </summary>
        [Obsolete]
        public ActionResult ExportAsPdf()
        {
            StringBuilder sb = new StringBuilder();
            ProductionIndentBO productionIndentList = _productionIndentRepository.GetById(Convert.ToInt32(Session["ProductionIndent"]));
            List<ProductionIndent_DetailsBO> ItemDetails = _productionIndentRepository.GetItemDetailsByProductionIndentId(Convert.ToInt32(TempData["ProductionIndent"]));

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            string ReportName = "Production Indent";

            sb.Append("<div style='vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "<img height='80' width='90' src='" + strPath + "'/></th>");
            sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<th colspan='4' style=' padding-left: -130px; font-size:20px;text-align:center;font-family:Times New Roman;'>" + ReportName + "</th>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Indent Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + productionIndentList.ProductionIndentNo + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Issue Date</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + productionIndentList.IssueDate + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Indent By</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + productionIndentList.UserName + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Production Date</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + productionIndentList.ProductionDate + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Product Name</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + productionIndentList.RecipeName + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Total Batches</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + productionIndentList.TotalBatches + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>SO Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + productionIndentList.SONo + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Work Order No</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + productionIndentList.WorkOrderNo + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Batch Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + productionIndentList.BatchNumber + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Batch Planning No.</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + productionIndentList.BatchPlanningDocumentNo + "</td>");
            sb.Append("</tr>");
            //top: -10 px;
            sb.Append("<tr style='width:100%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding-top: -14px; font-family:Times New Roman;font-size:12px;;'>Remarks</th>");
            sb.Append("<td colspan='3' style='width:90%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + productionIndentList.Description + "</td>");
            //sb.Append("<th style='width:0%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'></th>");
            //sb.Append("<td style='width:0%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'></td>");
            sb.Append("</tr>");

            sb.Append("</thead>");
            sb.Append("</table>");
            sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");

            sb.Append("<table>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center; font-family:Times New Roman;width:87%;font-size:15px;'>Item Details</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("</table>");

            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;repeat-header: yes;page-break-inside: auto;'>");
            sb.Append("<thead>");


            sb.Append("<tr style='text-align:center;padding: 5px; font-family:Times New Roman;background-color:#C0DBEA'>");

            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:0.5%;font-size:10px;border: 0.01px black;'>#</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2.5%;font-size:10px;border: 0.01px black;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;font-size:10px;border: 0.01px black;'>Item Name</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2.5%;font-size:10px;border: 0.01px black;'>Percentage (%)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Batch Quantity (Kg)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Final Quantity (Kg)</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int i = 1;

            foreach (var item in ItemDetails)
            {
                sb.Append("<tr style='text-align:center;'>");
                sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + i + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemCode + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Percentage + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.FinalQuantity + "</td>");

                sb.Append("</tr>");
                i++;
            }
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("<br />");

            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr><th colspan='4'>&nbsp;</th></tr>");
            sb.Append("<tr><th colspan='4' style='text-align:right;padding: 2px; font-family:Times New Roman;font-size:12px;'>Authorized Signature</th></tr>");
            sb.Append("</thead>");
            sb.Append("</table>");

            sb.Append("</div>");

            using (var sr = new StringReader(sb.ToString()))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Document pdfDoc = new Document(PageSize.A4);

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    writer.PageEvent = new PageHeaderFooter();
                    pdfDoc.Open();
                    setBorder(writer, pdfDoc);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    string filename = "Production_Indent_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    TempData["ReportName"] = ReportName.ToString();
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }
        #endregion

        #region PDF Helper both Set Border, Report Generated Date and Page Number Sheet

        #region Set Border
        /// <summary>
        /// setting border to pdf document
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="pdfDoc"></param>
        public void setBorder(PdfWriter writer, Document pdfDoc)
        {
            //---------------------------------------
            var content = writer.DirectContent;
            var pageBorderRect = new Rectangle(pdfDoc.PageSize);

            pageBorderRect.Left += pdfDoc.LeftMargin - 15;
            pageBorderRect.Right -= pdfDoc.RightMargin - 15;
            pageBorderRect.Top -= pdfDoc.TopMargin - 7;
            pageBorderRect.Bottom += pdfDoc.BottomMargin - 5;

            //content.SetColorStroke(BaseColor.DARK_GRAY);
            //content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom + 5, pageBorderRect.Width, pageBorderRect.Height);
            ////content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom - 5, pageBorderRect.Top, pageBorderRect.Right);
            //content.Stroke();

            //---------------------------------------

            content.SetColorStroke(BaseColor.RED);
            content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
            content.Stroke();

        }
        #endregion

        #region PDF Helper Class
        public class PageHeaderFooter : PdfPageEventHelper
        {
            private readonly Font _pageNumberFont = new Font(Font.NORMAL, 10f, Font.NORMAL, BaseColor.BLACK);

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PurchaseOrderController purchaseOrderController = new PurchaseOrderController();
                purchaseOrderController.setBorder(writer, document);

                AddPageNumber(writer, document);
                //base.OnEndPage(writer, document);
            }

            private void AddPageNumber(PdfWriter writer, Document document)
            {
                //----------------Font Value for Header & PageHeaderFooter--------------------
                Font plainFont = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL);

                //--------------------------------------------For Generated Date-----------------------------------------------------
                var GeneratedDate = "Generated By: " + System.Web.HttpContext.Current.Session[ApplicationSession.USERNAME] + " On " + DateTime.Now;

                var generatedDateTable = new PdfPTable(1);
                generatedDateTable.DefaultCell.Border = 0;

                var generatedDateCell = new PdfPCell(new Phrase(GeneratedDate, plainFont)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                generatedDateCell.Border = 0;
                generatedDateTable.TotalWidth = 250;
                generatedDateTable.AddCell(generatedDateCell);
                generatedDateTable.WriteSelectedRows(0, 1, document.Left - 50, document.Bottom - 5, writer.DirectContent);
                //-------------------------------------------For Generated Date-----------------------------------------------------

                //----------------------------------------For Page Number--------------------------------------------------
                var Page = "Page: " + writer.PageNumber.ToString();
                var pageNumberTable = new PdfPTable(1);

                pageNumberTable.DefaultCell.Border = 0;
                var pageNumberCell = new PdfPCell(new Phrase(Page, plainFont)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                pageNumberCell.Border = 0;
                pageNumberTable.TotalWidth = 50;
                pageNumberTable.AddCell(pageNumberCell);
                pageNumberTable.WriteSelectedRows(0, 1, document.Right - 30, document.Bottom - 5, writer.DirectContent);
                //----------------------------------------For Page Number------------------------------------------------------
            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                AddPageHeader(writer, document);
                //base.OnStartPage(writer, document);
            }
            private void AddPageHeader(PdfWriter writer, Document document)
            {
                var text = ApplicationSession.ORGANISATIONTIITLE;

                var numberTable = new PdfPTable(1);
                numberTable.DefaultCell.Border = 0;
                var numberCell = new PdfPCell(new Phrase(text)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                numberCell.Border = 0;

                numberTable.TotalWidth = 200;
                numberTable.WriteSelectedRows(0, 1, document.Left - 40, document.Top + 25, writer.DirectContent);
            }
        }
        #endregion

        #endregion

        //public JsonResult GetBatchNumber(string id, string TotalBatches)  //Not in use 25-05-23.
        //{
        //    var Id = Convert.ToInt32(id);
        //    var totalBatches = Convert.ToInt32(TotalBatches);
        //    var result = _productionIndentRepository.GetBatchNumberById(Id, totalBatches);
        //    return Json(result);
        //}

        public JsonResult GetBatchNumber(string id, string SO_Id)
        {
            var Id = Convert.ToInt32(id);            
            var SO_ID = Convert.ToInt32(SO_Id);             
            var result = _productionIndentRepository.GetBatchNumberById(Id, SO_ID);
            return Json(result);
        }

    }
}