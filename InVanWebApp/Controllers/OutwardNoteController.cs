using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using InVanWebApp_BO;
using log4net;
using InVanWebApp.Common;
using System.IO;
using InVanWebApp.DAL;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;

namespace InVanWebApp.Controllers
{
    public class OutwardNoteController : Controller
    {
        private IOutwardNoteRepository _repository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private IUserDetailsRepository _userDetailsRepository;
        private static ILog log = LogManager.GetLogger(typeof(DeliveryChallanController));

        #region Initializing Constructor

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 31 Jan'23
        /// </summary>
        public OutwardNoteController()
        {
            _repository = new OutwardNoteRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
            _userDetailsRepository = new UserDetailsRepository();
        }

        /// <summary>
        /// Created By: Farheen
        /// Created Date: 31 Jan'23
        /// </summary>
        /// <param name="outwardNoteRepository"></param>
        public OutwardNoteController(IOutwardNoteRepository outwardNoteRepository)
        {
            _repository = outwardNoteRepository;
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
        /// Farheen: Rendered the user to the add outward note.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddOutwardNote()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindLocationName();
                GenerateDocumentNo();
                BindUsers();

                //Binding item grid with sell type item.
                var itemList = _repository.GetItemDetailsForDD();
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                ViewData["itemListForDD"] = dd;

                OutwardNoteBO model = new OutwardNoteBO();
                model.OutwardDate = DateTime.Today;
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
        public ActionResult AddOutwardNote(OutwardNoteBO model, HttpPostedFileBase Signature)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (Signature != null)
                    {
                        UploadSignature(Signature);
                        model.Signature = Signature.FileName.ToString();
                    }
                    else
                        model.Signature = null;

                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _repository.Insert(model);

                        if (response.Status)
                            TempData["Success"] = "<script>alert('Gate Pass is generated successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error! Gate Pass cannot be generated!');</script>";
                            BindLocationName();
                            GenerateDocumentNo();
                            BindUsers();

                            //Binding item grid with sell type item.
                            var itemList = _repository.GetItemDetailsForDD();
                            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                            ViewData["itemListForDD"] = dd;
                            
                            model.OutwardDate = DateTime.Today;
                            return View(model);
                        }


                        return RedirectToAction("Index", "OutwardNote");

                    }
                    else
                    {
                        BindLocationName();
                        GenerateDocumentNo();
                        BindUsers();

                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        model.OutwardDate = DateTime.Today;
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
                BindUsers();
                model.OutwardDate = DateTime.Today;
                return View(model);
            }
        }
        #endregion

        #region  Update function
        /// <summary>
        ///Farheen: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="PurchaseOrderId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditOutwardNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindLocationName();
                BindUsers();

                OutwardNoteBO model = _repository.GetById(ID);

                //Binding item grid with sell type item.
                var itemList = _repository.GetItemDetailsForDD();
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                string itemListForDD = "itemListForDD";

                if (model != null)
                {
                    var ItemCount = model.outwardNoteDetails.Count;
                    var i = 0;
                    while (i < ItemCount)
                    {
                        itemListForDD = "itemListForDD";
                        itemListForDD = itemListForDD + i;
                        dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model.outwardNoteDetails[i].ItemID);
                        ViewData[itemListForDD] = dd;
                        i++;
                    }

                }

                ViewData[itemListForDD] = dd;

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");

        }

        /// <summary>
        /// Farheen:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditOutwardNote(OutwardNoteBO model, HttpPostedFileBase Signature)
        {
            ResponseMessageBO response = new ResponseMessageBO();

            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (Signature != null && Signature.ContentLength > 1000)
                    {
                        UploadSignature(Signature);
                        model.Signature = Signature.FileName.ToString();
                    }
                    else if (Signature.ContentLength < 1000 && Signature != null)
                        model.Signature = Signature.FileName.ToString();
                    else
                        model.Signature = null;

                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _repository.Update(model);
                        if (response.Status)
                        {
                            TempData["Success"] = "<script>alert('Gate Pass updated successfully!');</script>";
                            return RedirectToAction("Index", "OutwardNote");
                        }
                        else
                        {
                            TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";

                            BindLocationName();
                            BindUsers();

                            OutwardNoteBO model1 = _repository.GetById(model.ID);

                            //Binding item grid with sell type item.
                            var itemList = _repository.GetItemDetailsForDD();
                            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                            string itemListForDD = "itemListForDD";

                            if (model != null)
                            {
                                var ItemCount = model.outwardNoteDetails.Count;
                                var i = 0;
                                while (i < ItemCount)
                                {
                                    itemListForDD = "itemListForDD";
                                    itemListForDD = itemListForDD + i;
                                    dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model.outwardNoteDetails[i].ItemID);
                                    ViewData[itemListForDD] = dd;
                                    i++;
                                }

                            }

                            ViewData[itemListForDD] = dd;

                            return View(model1);
                        }

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindLocationName();
                        BindUsers();

                        OutwardNoteBO model1 = _repository.GetById(model.ID);

                        //Binding item grid with sell type item.
                        var itemList = _repository.GetItemDetailsForDD();
                        var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                        string itemListForDD = "itemListForDD";

                        if (model != null)
                        {
                            var ItemCount = model.outwardNoteDetails.Count;
                            var i = 0;
                            while (i < ItemCount)
                            {
                                itemListForDD = "itemListForDD";
                                itemListForDD = itemListForDD + i;
                                dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model.outwardNoteDetails[i].ItemID);
                                ViewData[itemListForDD] = dd;
                                i++;
                            }

                        }

                        ViewData[itemListForDD] = dd;

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
                return RedirectToAction("Index", "OutwardNote");
            }
        }

        #endregion

        #region Bind dropdowns 
        public void BindUsers()
        {
            var UserId = Convert.ToInt32(Session[ApplicationSession.USERID]);
            var result = _userDetailsRepository.GetAll(UserId);
            var resultList = new SelectList(result.ToList(), "EmployeeID", "EmployeeName");
            ViewData["EmployeeName"] = resultList;
        }

        public void BindLocationName()
        {
            var result = _purchaseOrderRepository.GetLocationNameList();
            var resultList = new SelectList(result.ToList(), "LocationId", "LocationName");
            ViewData["LocationList"] = resultList;
        }

        public void GenerateDocumentNo()
        {
            GetDocumentNumber objDocNo = new GetDocumentNumber();

            //=========here document type=15 i.e. for generating the Outward Note (logic is in SP).====//
            var DocumentNumber = objDocNo.GetDocumentNo(15);
            ViewData["DocumentNo"] = DocumentNumber;
        }

        #endregion

        #region Function for uploading the signature
        /// <summary>
        /// Date: 31 Jan 2023
        /// Farheen: Upload Signature File items.
        /// </summary>
        /// <returns></returns>

        public void UploadSignature(HttpPostedFileBase Signature)
        {
            if (Signature != null)
            {
                string SignFilename = Signature.FileName;
                SignFilename = Path.Combine(Server.MapPath("~/Signatures/"), SignFilename);
                Signature.SaveAs(SignFilename);

            }
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Date: 01 Feb'23
        /// Farheen: Delete the perticular record
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteOutwardNote(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                ResponseMessageBO result = new ResponseMessageBO();
                result = _repository.Delete(ID, userID);

                if (result.Status)
                    TempData["Success"] = "<script>alert('Gate Pass is deleted successfully!');</script>";
                else
                    TempData["Success"] = "<script>alert('Error while deleting!');</script>";

                return RedirectToAction("Index", "OutwardNote");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region This method is for View the Outward Note
        [HttpGet]
        public ActionResult ViewOutwardNote(int ID)
        {
            Session["OutwardNoteID"] = ID;
            if (Session[ApplicationSession.USERID] != null)
            {
                OutwardNoteBO model = _repository.GetById(ID);
                TempData["OutwardNotePDF"] = model;
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion

        #region Export PDF Out ward 
        /// <summary>
        /// Create by Maharshi on 03/05/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPDF()
        {


            StringBuilder sb = new StringBuilder();
            OutwardNoteBO OutwardNoteList = _repository.GetById(Convert.ToInt32(Session["OutwardNoteID"]));

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            //string strSign = Request.Url.GetLeftPart(UriPartial.Authority) + "/Signatures/" + OutwardNoteList.Signature;
            string strAuthorizedSign = Request.Url.GetLeftPart(UriPartial.Authority) + "/Signatures/" + OutwardNoteList.Signature;

            string ReportName = "Gate Pass";

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
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Gate Pass Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.OutwardNumber + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Document Date</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.OutwardDate + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Is Retunable</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.IsReturnable + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Purpose</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.Remarks + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");        
            sb.Append("<th style='width:10%;text-align:left;padding-top: -24px; font-family:Times New Roman;font-size:12px;'>Supplier Details</th>");
            sb.Append("<td style='width:20%;text-align:left;padding-top: 2px; font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.LocationName + " " + OutwardNoteList.LocationAddress + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding-top: -24px; font-family:Times New Roman;font-size:12px;;'>Delivery Details</th>");
            sb.Append("<td style='width:20%;text-align:left;padding-top: -24px; font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.DeliveryAddress + "</td>");

            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Verified By</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.VerifiedByName + "</td>");
            //sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Supplier</th>");
            //sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.CompanyName + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Contact Person</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.ContactPerson + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Contact Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.ContactInformation + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Dispatch Through</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.DispatchThrough + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Docket/LR Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.DocketNumber + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Vehicle Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.VehicleNo + "</td>");
            //sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Supplier</th>");
            //sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.CompanyName + "</td>");
            sb.Append("</tr>");

            sb.Append("</thead>");
            sb.Append("</table>");
            sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");

            //sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;repeat-header: yes;page-break-inside: auto;'>");
            //sb.Append("<thead>");

            //sb.Append("<tr>");
            //sb.Append("<th style='text-align:center; font-family:Times New Roman;width:3%;font-size:15px;'>Item Details</th>");
            //sb.Append("</tr>");
            //sb.Append("</thead>");
            //sb.Append("<tbody></tbody></table>");

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
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:5%;font-size:10px;border: 0.01px black;'>Item</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:3%;font-size:10px;border: 0.01px black;'>Shipping Quantity</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>UOM</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Comments</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int i = 1;
            //decimal totalBeforeTax = 0;
            //decimal totalTax = 0;

            foreach (var item in OutwardNoteList.outwardNoteDetails)
            {
                sb.Append("<tr style='text-align:center;'>");
                sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + i + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemCode + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.OutwardQuantity + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemUnit + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Remarks + "</td>");

                sb.Append("</tr>");
                i++;
                // totalBeforeTax = totalBeforeTax + Convert.ToDecimal(item.TotalItemCost);
                // totalTax = totalTax + (Convert.ToDecimal(item.ItemTaxValue) / 100) * Convert.ToDecimal(item.TotalItemCost);
            }
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("<table style='vertical-align: top;padding-top:20px;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            //sb.Append("<tr><th colspan='4'>&nbsp;</th></tr>");
            //sb.Append("<tr style='text-align:left;padding: 1px; font-family:Times New Roman;'>");
            //sb.Append("<th colspan='2' style='text-align:left;padding: 2px; width:60%; font-family:Times New Roman;font-size:14px;'></th>");

            //sb.Append("<th colspan='2' style='text-align:Center;padding: 2px; width:40%; font-family:Times New Roman;font-size:14px;'>Payment Details</th>");
            //sb.Append("</tr>");
            //sb.Append("<tr>");
            //sb.Append("</tr>");

            //sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            //sb.Append("<td rowspan='6' colspan='2' style='text-align:justify;font-size:10px; font-family:Times New Roman;padding-right:20px;'>" + "</td>");
            //sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Total (before tax):</th>");
            //sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + totalBeforeTax + " INR" + "</td>");
            //sb.Append("</tr>");
            //sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            //sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Total tax:</th>");
            //sb.Append("<td style='text-align:right;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + totalTax.ToString("0") + " INR" + "</td>");
            //sb.Append("</tr>");


            //sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            //sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Other Tax:</th>");
            //sb.Append("<td style='text-align:right;padding: 2px;font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.OtherTax + " INR" + "</td>");
            //sb.Append("</tr>");

            //sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            //sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Total (after tax):</th>");
            //sb.Append("<td style='text-align:right;padding: 2px;font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.TotalAfterTax + " INR" + "</td>");
            //sb.Append("</tr>");

            //sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            //sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Grand Total:</th>");
            //sb.Append("<td style='text-align:right;padding: 2px;font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.GrandTotal + " INR" + "</td>");
            //sb.Append("</tr>");

            //sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            //sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Advanced To Pay:</th>");
            //sb.Append("<td style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + OutwardNoteList.AdvancedPayment + " INR" + "</td>");
            //sb.Append("</tr>");

            sb.Append("<tr style='text-align:left;padding: 2px; font-family:Times New Roman;'>");
            sb.Append("<th style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'></th>");
            sb.Append("<td style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr><th colspan='4'>&nbsp;</th></tr>");
            sb.Append("<tr><td colspan='4' style='text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "<img height='40%' width='60%' src='" + strAuthorizedSign + "'/></td></tr>");
            sb.Append("<tr><th colspan='4' style='text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Authorized Signature</th></tr>");

            sb.Append("<tr><th colspan='4'>&nbsp;</th></tr>");
            //sb.Append("<tr><td colspan='4' style='text-align:right;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + "<img height='40%' width='60%' src='" + strSign + "'/></td></tr>");
            sb.Append("<tr><th colspan='4' style='text-align:right;padding: 2px; font-family:Times New Roman;font-size:12px;'>Receiver's Signature</th></tr>");
            sb.Append("</thead>");
            sb.Append("</table>");
            //sb.Append("<br />");

            //sb.Append("<table>");
            //sb.Append("<thead>");
            //sb.Append("<tr style='text-align:left;padding: 1px; font-family:Times New Roman;'>");
            //sb.Append("<th style='text-align:left;padding: 5px; font-family:Times New Roman;font-size:13px;;'>Terms And Condition</ th>");
            //sb.Append("</tr>");
            //sb.Append("<tr style='text-align:left;padding: 1px; font-family:Times New Roman;'>");
            //sb.Append("<td style='text-align:justify;padding: 5px;width:86%;font-size:11px; font-family:Times New Roman;'>" + OutwardNoteList.Terms + "</td>");
            //sb.Append("</tr>");
            //sb.Append("</thead>");
            //sb.Append("</table>");


            sb.Append("</div>");

            using (var sr = new StringReader(sb.ToString()))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    //Document pdfDoc = new Document(PageSize.A4.Rotate());
                    Document pdfDoc = new Document(PageSize.A4);
                    //pdfDoc.SetPageSize(new Rectangle(850f, 1100f));
                    //pdfDoc.SetPageSize(new Rectangle(1100f, 850f));

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    writer.PageEvent = new PageHeaderFooter();
                    pdfDoc.Open();
                    setBorder(writer, pdfDoc);


                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    ///Rahul updated 'filename' = 'Purchase_Order_' to 'Gate_Pass_' 24-07-23. 
                    string filename = "Gate_Pass_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
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
                //generatedDateTable.WriteSelectedRows(0, 1, document.Left - 135, document.Bottom - 5, writer.DirectContent);
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

        //#region This method is for View the Outward Note
        //[HttpGet]
        //public ActionResult ViewOutwardNote(int ID)
        //{
        //    if (Session[ApplicationSession.USERID] != null)
        //    {
        //        OutwardNoteBO model = _repository.GetById(ID);
        //        return View(model);
        //    }
        //    else
        //        return RedirectToAction("Index", "Login");
        //}

        //#endregion
    }
}