using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Common;
using InVanWebApp_BO;
using System.Text;
using System.IO;
using iTextSharp.tool.xml;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;

namespace InVanWebApp.Controllers
{
    public class StockAdjustmentController : Controller
    {
        private IStockAdjustmentRepository _repository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(StockAdjustmentController));

        #region Initializing Constructor

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 17 Jan'23
        /// </summary>
        public StockAdjustmentController()
        {
            _repository = new StockAdjustmentRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
        }

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 17 Jan'23
        /// </summary>
        /// <param name="stockAdjustmentRepository"></param>
        public StockAdjustmentController(IStockAdjustmentRepository stockAdjustmentRepository)
        {
            _repository = stockAdjustmentRepository;
        }
        #endregion

        #region Bind Grid
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            var model = _repository.GetAll();
            return View(model);
        }
        #endregion

        #region Insert functions
        /// <summary>
        /// Farheen: Rendered the user to the add Stock adjustment note.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddStockadjustment()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindLocationName();
                GenerateDocumentNo();                
                StockAdjustmentBO model = new StockAdjustmentBO();
                model.DocumentDate = DateTime.Today;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Farheen: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddStockadjustment(StockAdjustmentBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _repository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Stock adjusted successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error! Can not be inserted!');</script>";
                            BindLocationName();
                            GenerateDocumentNo();
                            model.DocumentDate = DateTime.Today;
                            return View(model);
                        }

                        return RedirectToAction("Index", "StockAdjustment");

                    }
                    else
                    {
                        BindLocationName();
                        GenerateDocumentNo();
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        model.DocumentDate = DateTime.Today;
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

                BindLocationName();
                GenerateDocumentNo();
                model.DocumentDate = DateTime.Today;
                return View(model);
            }
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 28 Nov'22
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteStockAdjustment(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                ResponseMessageBO result = new ResponseMessageBO();
                result = _repository.Delete(ID, userID);

                if (result.Status)
                    TempData["Success"] = "<script>alert('Adjusted stock is deleted successfully!');</script>";
                else
                    TempData["Success"] = "<script>alert('Error while deleting!');</script>";

                return RedirectToAction("Index", "StockAdjustment");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region This method is for View the Stock adjustment
        [HttpGet]
        public ActionResult ViewStockAdjustment(int ID)
        {
            Session["StockAdjustmentID"] = ID;  //Maharshi added 13-07-23.  
            if (Session[ApplicationSession.USERID] != null)
            {
                StockAdjustmentBO model = _repository.GetById(ID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Export PDF Bind Stock Adjustment Details
        /// <summary>
        /// Create by Snehal on 19/06/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPDF()
        {
            StringBuilder sb = new StringBuilder();

            StockAdjustmentBO StockAdjustmentList = _repository.GetById(Convert.ToInt32(Session["StockAdjustmentID"]));
            //List<GRN_BO> GRNUTEMDetalis = _repository.GetGRNItemDetails(Convert.ToInt32(Session["GRNItemDetail_Id"]));
            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string strSign = Request.Url.GetLeftPart(UriPartial.Authority) + "/Signatures/" + OutwardNoteList.Signature;
            //string strAuthorizedSign = Request.Url.GetLeftPart(UriPartial.Authority) + "/Signatures/" + GRNUTEMDetalis.Signature;

            string ReportName = "Stock Adjustment Document";

            //string PODate = Convert.ToDateTime(OutwardNoteList.PODate).ToString("dd/MM/yyyy") + " ";
            //string DeliveryDate = Convert.ToDateTime(OutwardNoteList.DeliveryDate).ToString("dd/MM/yyyy") + " ";

            sb.Append("<div style='vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "<img height='80' width='90' src='" + strPath + "'/></th>");
            sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<th colspan='4' style=' padding-left: -130px; font-size:20px;text-align:center;font-family:Times New Roman;'>" + ReportName + "</th>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:30%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Document Number</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + StockAdjustmentList.DocumentNo + "</td>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Document Date</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + Convert.ToDateTime(StockAdjustmentList.DocumentDate).ToString("dd/MM/yyyy") + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:30%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Location Name</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + StockAdjustmentList.LocationName + "</td>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Remarks</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + StockAdjustmentList.Remarks + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:30%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:30%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Adjust By</th>");
            sb.Append("<td style='width:50%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + StockAdjustmentList.UserName + "</td>");
            //sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Remarks</th>");
            //sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + StockAdjustmentList.Remark + "</td>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("</table>");
            sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");

            //sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            //sb.Append("<thead>");
            //sb.Append("<tr>");
            //sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "</th>");
            //sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + "" + "</label>");
            //sb.Append("<th colspan='4' style=' padding-left: -130px; font-size:15px;text-align:center;font-family:Times New Roman;'>" + "Shipping Details" + "</th>");
            //sb.Append("</tr>");

            //sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            //sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Location</th>");
            //sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + StockAdjustmentList.LocationName + "</td>");
            //sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'></th>");
            //sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "</td>");
            //sb.Append("</tr>");

            //sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            //sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Shipping DetailS</th>");
            //sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + StockAdjustmentList.DeliveryAddress + "</td>");
            //sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Supplier Details</th>");
            //sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + StockAdjustmentList.SupplierAddress + "</td>");
            //sb.Append("</tr>");

            //sb.Append("</thead>");
            //sb.Append("</table>");
            //sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");

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

            //sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;font-size:10px;border: 0.01px black;'>#</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Item</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Price (Per Unit)</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Available Stock</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Physical Stock</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Difference</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>UOM</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Transfer Price</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;font-size:10px;border: 0.01px black;'>Remarks</th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            //int i = 1;

            foreach (var item in StockAdjustmentList.stockAdjustmentDetails)
            {
                sb.Append("<tr style='text-align:center;'>");
                //sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + i + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Item_Name + "</td>");
                sb.Append("<td style='text-align:Right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnitPrice + " " + item.CurrencyName + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.AvailableStock + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.PhysicalStock + "</td>");
                sb.Append("<td style='text-align:Right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.DifferenceInStock + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.TransferPrice + " " + item.CurrencyName + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Remarks + "</td>");

                sb.Append("</tr>");
                //i++;
            }
            sb.Append("</tbody>");
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
                    string filename = "Stock_Adjustment_Document_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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

            //---------------------------------------

            content.SetColorStroke(BaseColor.RED);
            content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
            content.Stroke();
        }
        #endregion

        #region Pdf Helper Class
        public class PageHeaderFooter : PdfPageEventHelper
        {
            private readonly Font _pageNumberFont = new Font(Font.NORMAL, 10f, Font.NORMAL, BaseColor.BLACK);
            //private readonly Font _dateTime = new Font(Font.NORMAL, 10f, Font.NORMAL, BaseColor.BLACK);
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PurchaseOrderController purchaseOrderController = new PurchaseOrderController();
                purchaseOrderController.setBorder(writer, document);

                AddPageNumber(writer, document);
            }

            private void AddPageNumber(PdfWriter writer, Document document)
            {
                //----------------Font Value for Header & PageHeaderFooter--------------------
                Font plainFont = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL);

                //--------------------------------------------For Generated Date-----------------------------------------------------
                //var GeneratedDate = "Generated: " + DateTime.Now;
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


        #region Fetch location stocks details for stock adjustment
        public JsonResult GetLocationStocksDetails(string id, string itemId=null)
        {
            int Location_Id = 0;
            if (id != "" && id != null)
                Location_Id = Convert.ToInt32(id);
            int Item_Id = 0;
            if (itemId != null && itemId != "")
                Item_Id = Convert.ToInt32(itemId);

            var result = _repository.GetLocationStocksDetailsById(Location_Id,Item_Id);
            return Json(result);
        }
        #endregion

        #region Bind dropdowns 
        public void BindLocationName()
        {
            var result = _purchaseOrderRepository.GetLocationNameList();
            var resultList = new SelectList(result.ToList(), "LocationId", "LocationName");
            ViewData["LocationList"] = resultList;
        }

        public void GenerateDocumentNo()
        {
            //==========Document number for Stock adjustment============//
            GetDocumentNumber objDocNo = new GetDocumentNumber();

            //=========here document type=12 i.e. for generating the Stock adjustment (logic is in SP).====//
            var DocumentNumber = objDocNo.GetDocumentNo(12);
            ViewData["DocumentNo"] = DocumentNumber;
        }

        public JsonResult GetItemList(string id)
        {
            int Location_Id = 0;
            if (id != "" && id != null)
                Location_Id = Convert.ToInt32(id);

            var result = _repository.GetItemListByLocationId(Location_Id);
            return Json(result);
        }
        #endregion
    }
}