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
using System.IO;
using iTextSharp.tool.xml;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using System.Text;

namespace InVanWebApp.Controllers
{
    public class InwardNoteController : Controller
    {
        private IInwardNoteRepository _repository;
        private static ILog log = LogManager.GetLogger(typeof(InwardNoteController));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public InwardNoteController()
        {
            _repository = new InwardNoteRepository();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public InwardNoteController(IInwardNoteRepository inwardNoteRepository)
        {
            _repository = inwardNoteRepository;
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

        #region Insert function
        /// <summary>
        /// Farheen: Rendered the user to the add inward master form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddInwardNote()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindPONumber();

                InwardNoteBO model = new InwardNoteBO();
                model.InwardDate = DateTime.Today;
                //==========Document number for Inward note============//
                GetDocumentNumber objDocNo = new GetDocumentNumber();

                //=========here document type=3 i.e. for generating the Inward note (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(3);
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
        //public ActionResult AddInwardNote(InwardNoteBO model, HttpPostedFileBase Signature)
        public ActionResult AddInwardNote(InwardNoteBO model)
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
                            TempData["Success"] = "<script>alert('Inward note created successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate Inward note! Can not be inserted!');</script>";
                            BindPONumber();
                            model.InwardDate = DateTime.Today;

                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            var DocumentNumber = objDocNo.GetDocumentNo(3);
                            ViewData["DocumentNo"] = DocumentNumber;

                            return View(model);
                        }

                        return RedirectToAction("Index", "InwardNote");

                    }
                    else
                    {
                        //TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindPONumber();
                        model.InwardDate = DateTime.Today;

                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        var DocumentNumber = objDocNo.GetDocumentNo(3);
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

                BindPONumber();
                model.InwardDate = DateTime.Today;

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                var DocumentNumber = objDocNo.GetDocumentNo(3);
                ViewData["DocumentNo"] = DocumentNumber;
                return View(model);
            }
            //return View();
        }
        #endregion

        #region  Update function
        /// <summary>28 Sep'22
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditInwardNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {

                BindPONumber();
                InwardNoteBO model = _repository.GetById(ID);
                string SignFilename = model.Signature;
                SignFilename = Path.Combine(Server.MapPath("~/Signatures/"), SignFilename);
                ViewData["Signature"] = SignFilename;

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 25 May 2022
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditInwardNote(InwardNoteBO model, HttpPostedFileBase Signature)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (Signature != null)
                        {
                            string SignFilename = model.Signature;
                            SignFilename = Path.Combine(Server.MapPath("~/Signatures/"), SignFilename);
                            Signature.SaveAs(SignFilename);
                            //string path = Server.MapPath("~/Signatures/");

                            //if (!Directory.Exists(path))
                            //{
                            //    Directory.CreateDirectory(path);
                            //}

                            //Signature.SaveAs(path + Path.GetFileName(Signature.FileName));
                            model.Signature = Signature.FileName.ToString();
                        }

                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _repository.Update(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Inward note updated successfully!');</script>";

                        else
                        {
                            TempData["Success"] = "<script>alert('Duplicate organisation! Can not be updated!');</script>";
                            BindPONumber();
                            return View(model);
                        }
                        return RedirectToAction("Index", "InwardNote");
                    }
                    else
                        return View(model);
                }
                else
                    return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                TempData["Success"] = "<script>alert('Error while update!');</script>";
                return RedirectToAction("Index", "InwardNote");
            }
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 28 Sep'22
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteInwardNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _repository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('Inward note deleted successfully!');</script>";
                return RedirectToAction("Index", "InwardNote");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Bind dropdown of PO Number
        public void BindPONumber()
        {
            var result = _repository.GetPONumberForDropdown();
            var resultList = new SelectList(result.ToList(), "PurchaseOrderId", "PONumber");
            ViewData["PONumberAndId"] = resultList;
        }
        #endregion

        #region Bind all PO details 
        public JsonResult BindPODetails(string id,string InwId=null)
        {
            int POId = 0;
            int InwdId = 0;
            if (id != "" && id != null)
                POId= Convert.ToInt32(id);
            if (InwId != "" && InwId != null)
                InwdId = Convert.ToInt32(InwId);
            Session["POId"] = POId;
            Session["InwdId"] = InwdId; 
            var result = _repository.GetPODetailsById(POId,InwdId);
            return Json(result);
        }
        #endregion

        //#region This method is for View the Inward note
        //[HttpGet]
        //public ActionResult ViewInwardNote(int ID)
        //{
        //    if (Session[ApplicationSession.USERID] != null)
        //    {
        //        InwardNoteBO model = _repository.GetById(ID);
        //        string filename = System.IO.Path.GetFileName(model.Signature);
        //        //string filepathtosave = "Signatures/" + filename;
        //        string filepathtosave = Server.MapPath("~/Signatures/" + filename);
        //        /*Storing image path to show preview*/
        //        ViewBag.ImageURL = filepathtosave;
        //        return View(model);
        //    }
        //    else
        //        return RedirectToAction("Index", "Login");
        //}
        //#endregion

        #region This method is for View the Inward note
        [HttpGet]
        public ActionResult ViewInwardNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                InwardNoteBO model = _repository.GetById(ID);
                Session["InwardNotID"] = ID;
                string filename = System.IO.Path.GetFileName(model.Signature);
                //string filepathtosave = "Signatures/" + filename;
                string filepathtosave = Server.MapPath("~/Signatures/" + filename);
                TempData["ImgUrl"] = filepathtosave;
                /*Storing image path to show preview*/
                ViewBag.ImageURL = filepathtosave;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Export PDF Bind inward Note
        /// <summary>
        /// Create by Snehal on 19/06/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPDF()
        {
            StringBuilder sb = new StringBuilder();

            InwardNoteBO InwardNote = _repository.GetById(Convert.ToInt32(Session["InwardNotID"]));
            var PODetails = _repository.GetPODetailsById(Convert.ToInt32(Session["POId"]), Convert.ToInt32(Session["InwdId"]));
            //List<GRN_BO> GRNUTEMDetalis = _repository.GetGRNItemDetails(Convert.ToInt32(Session["GRNItemDetail_Id"]));
            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string strSign = Request.Url.GetLeftPart(UriPartial.Authority) + "/Signatures/" + OutwardNoteList.Signature;
            //string strAuthorizedSign = Request.Url.GetLeftPart(UriPartial.Authority) + "/Signatures/" + GRNUTEMDetalis.Signature;

            string ReportName = "Inward Note";

            var ShippingDetails = "";
            var SupplierDetails = "";
            foreach (var items in PODetails)
            {
                ShippingDetails = items.DeliveryAddress;
                SupplierDetails = items.SupplierAddress;
                break;
            }

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

            sb.Append("<tr style='width:10%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Good Received By</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + ShippingDetails + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Good Send By</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + SupplierDetails + "</td>");
            sb.Append("</tr>");

            sb.Append("</thead>");
            sb.Append("</table>");

            sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");
            sb.Append("<table>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center; font-family:Times New Roman;width:87%;font-size:15px;'>Inward Details</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("</table>");

            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Inward Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + InwardNote.InwardNumber + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Inward Date</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + Convert.ToDateTime(InwardNote.InwardDate).ToString("dd-MM-yyyy") + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>PO Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + InwardNote.PONumber + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>PO Date</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + Convert.ToDateTime(InwardNote.PODate).ToString("dd-MM-yyyy") + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Bill/Challan No.</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + InwardNote.ChallanNo + "</td>");
            //sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>PO Date</th>");
            //sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + InwardNote.PODate + "</td>");
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
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>#</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Item</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>PO Total Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Tax</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Unit Price</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;font-size:10px;border: 0.01px black;'>Inward Quantity</th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int i = 1;
            int j = 1;
            foreach (var item in PODetails)
            {
                if (j == 1)
                {
                    j++;
                    continue;
                }
                sb.Append("<tr style='text-align:center;'>");
                sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + i + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Item_Code + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemQuantity + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemTaxValue + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnitPrice + item.CurrencyName + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.InwardQuantity + item.ItemUnit + "</td>");

                sb.Append("</tr>");
                i++;
                j++;
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
            //sb.Append("<tr><td colspan='4' style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "<img height='40%' width='60%' src='" + strSign + "'/></td></tr>");
            sb.Append("<tr><th colspan='4' style='text-align:right;padding: 2px; font-family:Times New Roman;font-size:12px;'>Authorized Signaure</th></tr>");
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
                    string filename = "Inward_Document_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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

    }
}