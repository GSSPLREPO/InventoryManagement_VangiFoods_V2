﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using InVanWebApp_BO;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp.Common;

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
        public ActionResult AddInwardNote(InwardNoteBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                       // model.UserId = Convert.ToInt32(Session[ApplicationSession.USERID]);

                        //response = _organisationRepository.Insert(model);
                        //if (response.Status)
                        //    TempData["Success"] = "<script>alert('Organisation inserted successfully!');</script>";
                        //else
                        //{
                        //    TempData["Success"] = "<script>alert('Duplicate organisation! Can not be inserted!');</script>";
                        //    BindOrganisationGroup();
                        //    return View(model);
                        //}

                        return RedirectToAction("Index", "InwardNote");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        return View();
                    }
                }
                else
                    return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
            }
            return View();
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
        public JsonResult BindPODetails(string id)
        {
            var POId = Convert.ToInt32(id);
            var result = _repository.GetPODetailsById(POId);
            return Json(result);
        }
        #endregion
    }
}