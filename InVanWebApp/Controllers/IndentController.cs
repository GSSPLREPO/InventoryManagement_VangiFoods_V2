using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InVanWebApp_BO;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Repository;
using log4net;
using InVanWebApp.Common;

namespace InVanWebApp.Controllers
{
    public class IndentController : Controller
    {
        private IIndentRepository _repository;
        private IUserDetailsRepository _userDetailsRepository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(IndentController));

        #region Initializing constructor
        /// <summary>
        /// Date: 07 Dec'22
        /// Farheen: Constructor without parameter
        /// </summary>
        public IndentController()
        {
            _repository = new IndentRepository();
            _userDetailsRepository = new UserDetailsRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
        }

        /// <summary>
        /// Date: 07 Dec'22
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="itemRepository"></param>
        public IndentController(IndentRepository indentRepository)
        {
            _repository = indentRepository;
        }

        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 25 May 2022
        ///Farheen: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: Item 
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

        #region Insert functionality of Indent
        [HttpGet]
        public ActionResult AddIndent()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindUsers();
                BindLocation();
                BindDesignations();

                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=9 i.e. for generating the Indent (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(9);
                ViewData["DocumentNo"] = DocumentNumber;

                //Binding item grid with sell type item.
                var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                ViewData["itemListForDD"] = dd;

                IndentBO model = new IndentBO();
                model.IndentDate = DateTime.Today;

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Create By:Farheen
        /// Dscription: Pass the data to the repository for insertion from it's view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddIndent(IndentBO model)
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
                            TempData["Success"] = "<script>alert('Indent inserted successfully!');</script>";
                        else
                        {
                            TempData["Success"] = "<script>alert('Error while insertion!');</script>";
                            BindUsers();
                            BindLocation();
                            BindDesignations();
                            GetDocumentNumber objDocNo = new GetDocumentNumber();
                            //=========here document type=9 i.e. for generating the Indent (logic is in SP).====//
                            var DocumentNumber = objDocNo.GetDocumentNo(9);
                            ViewData["DocumentNo"] = DocumentNumber;

                            //Binding item grid with sell type item.
                            var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                            ViewData["itemListForDD"] = dd;

                            return View(model);
                        }

                        return RedirectToAction("Index", "Indent");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindUsers();
                        BindLocation();
                        BindDesignations();
                        var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                        var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                        ViewData["itemListForDD"] = dd;
                        GetDocumentNumber objDocNo = new GetDocumentNumber();
                        //=========here document type=9 i.e. for generating the Indent (logic is in SP).====//
                        var DocumentNumber = objDocNo.GetDocumentNo(9);
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
                BindLocation();
                BindDesignations();
                var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                ViewData["itemListForDD"] = dd;
                GetDocumentNumber objDocNo = new GetDocumentNumber();
                //=========here document type=9 i.e. for generating the Indent (logic is in SP).====//
                var DocumentNumber = objDocNo.GetDocumentNo(9);
                ViewData["DocumentNo"] = DocumentNumber;

                return View(model);
            }
        }


        #endregion

        #region Update Functions
        /// <summary>
        ///Create By: Farheen
        ///Description: Rendered the user to the edit page with details of a perticular record.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditIndent(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindUsers();
                BindLocation();
                BindDesignations();

                IndentBO model = _repository.GetById(ID);
                model.indent_Details = _repository.GetItemDetailsByIndentId(ID);
                //Binding item grid with sell type item.
                var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                string itemListForDD = "itemListForDD";

                if (model != null)
                {
                    var ItemCount = model.indent_Details.Count;
                    var i = 0;
                    while (i < ItemCount)
                    {
                        itemListForDD = "itemListForDD";
                        itemListForDD = itemListForDD + i;
                        dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model.indent_Details[i].ItemId);
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
        /// Rahul:  Pass the data to the repository for updating that record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditIndent(IndentBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();

            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _repository.Update(model);
                        
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Indent updated successfully!');</script>";

                        
                        else
                        {
                            TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                            BindUsers();
                            BindLocation();
                            BindDesignations();
                            IndentBO model1 = _repository.GetById(model.ID);
                            model1.indent_Details = _repository.GetItemDetailsByIndentId(model.ID);

                            //Binding item grid with sell type item.
                            var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                            var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                            string itemListForDD = "itemListForDD";

                            if (model1 != null)
                            {
                                var ItemCount = model1.indent_Details.Count;
                                var i = 0;
                                while (i < ItemCount)
                                {
                                    itemListForDD = "itemListForDD";
                                    itemListForDD = itemListForDD + i;
                                    dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model1.indent_Details[i].ItemId);
                                    ViewData[itemListForDD] = dd;
                                    i++;
                                }

                            }

                            ViewData[itemListForDD] = dd;

                            return View(model1);
                        }

                        return RedirectToAction("Index", "Indent");
                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindUsers();
                        BindLocation();
                        BindDesignations();
                        IndentBO model1 = _repository.GetById(model.ID);
                        model1.indent_Details = _repository.GetItemDetailsByIndentId(model.ID);

                        //Binding item grid with sell type item.
                        var itemList = _purchaseOrderRepository.GetItemDetailsForDD(2);
                        var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
                        string itemListForDD = "itemListForDD";

                        if (model1 != null)
                        {
                            var ItemCount = model1.indent_Details.Count;
                            var i = 0;
                            while (i < ItemCount)
                            {
                                itemListForDD = "itemListForDD";
                                itemListForDD = itemListForDD + i;
                                dd = new SelectList(itemList.ToList(), "ID", "Item_Code", model1.indent_Details[i].ItemId);
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
                return RedirectToAction("Index", "Indent");
            }
        }

        #endregion

        #region Delete function
        /// <summary>
        /// Create By: Farheen
        /// Description: This function is for deleting the Indent and 
        /// Indent details by using Indent ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteIndent(int ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var userID = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _repository.Delete(ID, userID);
                TempData["Success"] = "<script>alert('Indent deleted successfully!');</script>";
                return RedirectToAction("Index", "Indent");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region Bind dorpdowns
        public void BindUsers()
        {
            var result = _userDetailsRepository.GetAll();
            var resultList = new SelectList(result.ToList(), "EmployeeID", "EmployeeName");
            ViewData["EmployeeName"] = resultList;
        }
        public void BindLocation()
        {
            var result = _purchaseOrderRepository.GetLocationNameList();
            var resultList = new SelectList(result.ToList(), "LocationId", "LocationName");
            ViewData["LocationName"] = resultList;
        }
        public void BindDesignations()
        {
            var designations = _userDetailsRepository.GetDesignationForDropDown();
            var designationsList = new SelectList(designations.ToList(), "DesignationID", "DesignationName");
            ViewData["Designations"] = designationsList;

        }

        #endregion
    }
}