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
                            TempData["Success"] = "<script>alert('Outward Note is created successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error! Outward Note cannot be created!');</script>";
                            BindLocationName();
                            GenerateDocumentNo();
                            BindUsers();

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
                            TempData["Success"] = "<script>alert('Outward Note updated successfully!');</script>";
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
            var result = _userDetailsRepository.GetAll();
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
                    TempData["Success"] = "<script>alert('Outward Note deleted successfully!');</script>";
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
            if (Session[ApplicationSession.USERID] != null)
            {
                OutwardNoteBO model = _repository.GetById(ID);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #endregion
    }
}