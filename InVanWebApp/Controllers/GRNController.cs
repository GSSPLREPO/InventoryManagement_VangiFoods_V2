using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using InVanWebApp_BO;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Common;
using System.Text;
using System.IO;
using iTextSharp.tool.xml;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;

namespace InVanWebApp.Controllers
{
    public class GRNController : Controller
    {
        private IGRNRepository _repository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(GRNController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public GRNController()
        {
            _repository = new GRNRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public GRNController(IGRNRepository gRNRepository)
        {
            _repository = gRNRepository;
        }
        #endregion

        #region  Bind grid
        /// <summary>
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _repository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Insert functions
        /// <summary>
        /// Farheen: Rendered the user to the add GRN master form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddGRN()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindInwardNumber();
                BindLocationName();
                GRN_BO model = new GRN_BO();
                model.GRNDate = DateTime.Today;
                //==========Document number for GRN note============//
                GetDocumentNumber objDocNo = new GetDocumentNumber();

                //=========here document type=5 i.e. for generating the GRN note (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(7);
                ViewData["DocumentNo"] = DocumentNumber;

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
        public ActionResult AddGRN(GRN_BO model)
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
                            TempData["Success"] = "<script>alert('GRN created successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate GRN note! Can not be inserted!');</script>";
                            BindInwardNumber();
                            BindLocationName();
                            model.GRNDate = DateTime.Today;

                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            var DocumentNumber = objDocNo.GetDocumentNo(7);
                            ViewData["DocumentNo"] = DocumentNumber;

                            return View(model);
                        }
                        return RedirectToAction("Index", "GRN");
                    }
                    else
                    {
                        BindInwardNumber();
                        BindLocationName();
                        model.GRNDate = DateTime.Today;

                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        var DocumentNumber = objDocNo.GetDocumentNo(7);
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

                BindInwardNumber();
                BindLocationName();
                model.GRNDate = DateTime.Today;

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                var DocumentNumber = objDocNo.GetDocumentNo(7);
                ViewData["DocumentNo"] = DocumentNumber;
                return View(model);
            }
        }
        #endregion

        #region Get Inward number details
        public JsonResult BindInwardDetails(string id)
        {
            int IQCId = 0;
            if (id != "" && id != null)
                IQCId = Convert.ToInt32(id);
            
            var result = _repository.GetInwardDetailsById(IQCId);
            return Json(result);
        }
        #endregion

        #region Bind dropdowns 
        public JsonResult BindLocationMaster(string id)
        {
            int Id = 0;
            if (id != null && id != "")
                Id = Convert.ToInt32(id);
            var result = _purchaseOrderRepository.GetLocationMasterList(Id);
            return Json(result);
        }
        public void BindInwardNumber()
        {
            var result = _repository.GetInwardNumberForDropdown();
            var resultList = new SelectList(result.ToList(), "ID", "InwardNumber");
            ViewData["InwardNumber"] = resultList;
        }

        public void BindLocationName()
        {
            var result = _purchaseOrderRepository.GetLocationNameList();
            var resultList = new SelectList(result.ToList(), "LocationId", "LocationName");
            ViewData["LocationName"] = resultList;
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
        public ActionResult DeleteGRN(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _repository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('GRN deleted successfully!');</script>";
                return RedirectToAction("Index", "GRN");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region This method is for View the GRN Details
        [HttpGet]
        public ActionResult ViewGRN(int ID)
        {
            Session["GRN_Id"] = ID;
            if (Session[ApplicationSession.USERID] != null)
            {
                GRN_BO model = _repository.GetById(ID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public JsonResult BindGRNItemDetails(string id)
        {
            Session["GRNItemDetail_Id"] = id;
            int GRN_Id = 0;
            if (id != "" && id != null)
                GRN_Id = Convert.ToInt32(id);

            var result = _repository.GetGRNItemDetails(GRN_Id);
            return Json(result);
        }
        #endregion

        #region Export PDF Bind GRN Item Details
        /// <summary>
        /// Create by Snehal on 19/06/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPDF()
        {
            StringBuilder sb = new StringBuilder();

            GRN_BO GRN = _repository.GetById(Convert.ToInt32(Session["GRN_Id"]));
            List<GRN_BO> GRNUTEMDetalis = _repository.GetGRNItemDetails(Convert.ToInt32(Session["GRNItemDetail_Id"]));
            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            string ReportName = "GRN Document";

            sb.Append("<div style='vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "<img height='80' width='90' src='" + strPath + "'/></th>");
            sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<th colspan='4' style=' padding-left: -130px; font-size:20px;text-align:center;font-family:Times New Roman;'>" + ReportName + "</th>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>GRN Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + GRN.GRNCode + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>GRN Date</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + Convert.ToDateTime(GRN.GRNDate).ToString("dd/MM/yyyy") + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Inward QC Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + GRN.InwardQCNumber + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>PO Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + GRN.PONumber + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Inward Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + GRN.InwardNoteNumber + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Remarks</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + GRN.Remark + "</td>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("</table>");
            sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "</th>");
            sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + "" + "</label>");
            sb.Append("<th colspan='4' style=' padding-left: -130px; font-size:15px;text-align:center;font-family:Times New Roman;'>" + "Shipping Details" + "</th>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Location</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + GRN.LocationName + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'></th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:20%;top:-10 px;padding-top: -12px; font-family:Times New Roman;font-size:12px;'>Shipping DetailS</th>");
            sb.Append("<td style='width:20%;top:0 px;padding-top: 0px; font-size:12px; font-family:Times New Roman;'>" + GRN.DeliveryAddress + "</td>");
            sb.Append("<th style='width:20%;top:-10 px;padding-top: -12px; font-family:Times New Roman;font-size:12px;'>Supplier Details</th>");
            sb.Append("<td style='width:20%;top:0 px;padding-top: 0px; font-size:12px; font-family:Times New Roman;'>" + GRN.SupplierAddress + "</td>");

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

            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;font-size:10px;border: 0.01px black;'>Item</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2.5%;font-size:10px;border: 0.01px black;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;font-size:10px;border: 0.01px black;'>Unit</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Unit Price</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Ordered Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Inward Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Received Quantity</th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            foreach (var item in GRNUTEMDetalis)
            {
                sb.Append("<tr style='text-align:center;'>");
                sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemCode + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnitPrice + item.CurrencyName + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.OrderQty + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.InwardQty + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ReceivedQty + item.ItemUnit + "</td>");

                sb.Append("</tr>");
            }
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("<table style='vertical-align: top;padding-top:20px;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");

            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
            sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr><th colspan='4'>&nbsp;</th></tr>");

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
                    string filename = "GRN_Document_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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
    }
}