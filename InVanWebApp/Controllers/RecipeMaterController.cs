using InVanWebApp.Common;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using InVanWebApp_BO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InVanWebApp.Controllers
{
    public class RecipeMaterController : Controller
    {
        private IRecipeMaterRepository _productionRecipeRepository;
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private static ILog log = LogManager.GetLogger(typeof(RecipeMaterController));

        #region Initializing constructor
        /// <summary>
        /// Date: 27 Feb '23
        /// Rahul: Constructor without parameter 
        /// </summary>
        public RecipeMaterController()
        {
            _productionRecipeRepository = new RecipeMaterRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
        }

        /// <summary>
        /// Date: 27 Feb '23
        /// Rahul: Constructor with parameters for initializing the interface object. 
        /// </summary>
        /// <param name="recipeRepository"></param>
        public RecipeMaterController(RecipeMaterRepository recipeRepository)
        {
            _productionRecipeRepository = recipeRepository;
        }
        #endregion

        #region  Bind grid
        /// <summary>
        /// Date: 27 Feb '23
        ///Rahul: Get data and rendered it in it's view. 
        /// </summary>
        /// <returns></returns>
        // GET: ProductionRecipe
        public ActionResult Index()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                var model = _productionRecipeRepository.GetAll();
                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region For binding the dropdown of Production Recipe Item Packing Size Unit. 
        public void BindItemTypeCategory()
        {
            var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1);
            var dd4 = new SelectList(itemList.ToList(), "ID", "Item_Code");
            ViewData["ProductName"] = dd4;
            //var product = _productMasterRepository.GetAll();
            //var dd4 = new SelectList(product.ToList(), "ProductID", "ProductCode", "ProductName");

            //Binding item grid with Recipe. 
            var recipeList = _productionRecipeRepository.GetItemDetailsForRecipe();
            var dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", "UOM_Id");
            ViewData["Ingredients"] = dd;
        }
        #endregion

        #region Function for get Recipe item details 
        public JsonResult GetRecipeitemDetails(string id)
        {
            var itemId = Convert.ToInt32(id);
            var recipeDetails = _productionRecipeRepository.GetRecipeDetails(itemId);
            return Json(recipeDetails);
        }
        #endregion

        #region Insert function
        /// <summary>
        /// Date: 27 Feb '23
        /// Rahul: Rendered the user to the add Recipe item form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddRecipeItems()
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindItemTypeCategory();
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 27 Feb '23 
        /// Rahul: Pass the data to the repository for insertion from it's view. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRecipeItems(RecipeMasterBO model)
        {
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {
                    ResponseMessageBO response = new ResponseMessageBO();
                    if (ModelState.IsValid)
                    {
                        model.CreatedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _productionRecipeRepository.Insert(model);
                        if (response.Status)
                            TempData["Success"] = "<script>alert('Recipe inserted successfully!');</script>";
                        else
                            TempData["Success"] = "<script>alert('Duplicate recipe name! Can not be inserted!');</script>";

                        return RedirectToAction("Index", "RecipeMater");

                    }
                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        return RedirectToAction("Index", "RecipeMater");
                    }
                }
                else
                    return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                TempData["Success"] = "<script>alert('Something went wrong!');</script>";
                log.Error(ex.Message, ex);
            }
            BindItemTypeCategory();
            return View();
        }
        #endregion

        #region  Update function
        /// <summary>
        /// Date: 27 Feb '23 
        ///Rahul: Rendered the user to the edit page with details of a perticular record.  
        /// </summary>
        /// <param name="Recipe_ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditRecipeItem(int Recipe_ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                BindItemTypeCategory();
                RecipeMasterBO model = _productionRecipeRepository.GetById(Recipe_ID);
                model.recipe_Details = _productionRecipeRepository.GetItemDetailsByRecipeId(Recipe_ID);

                //Binding item grid with Recipe. 
                var recipeList = _productionRecipeRepository.GetItemDetailsForRecipe();
                var dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", "UOM_Id");

                string Ingredients = "Ingredients";

                if (model != null)
                {
                    var ItemCount = model.recipe_Details.Count;
                    var i = 0;
                    while (i < ItemCount)
                    {
                        Ingredients = "Ingredients";
                        Ingredients = Ingredients + i;
                        dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", model.recipe_Details[i].ItemId);
                        ViewData[Ingredients] = dd;
                        i++;
                    }
                }

                ViewData[Ingredients] = dd;

                return View(model);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Date: 27 Feb '23
        /// Rahul:  Pass the data to the repository for updating that record. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditRecipeItem(RecipeMasterBO model)
        {
            ResponseMessageBO response = new ResponseMessageBO();
            try
            {
                if (Session[ApplicationSession.USERID] != null)
                {

                    if (ModelState.IsValid)
                    {
                        model.LastModifiedBy = Convert.ToInt32(Session[ApplicationSession.USERID]);
                        response = _productionRecipeRepository.Update(model);

                        if (response.Status)
                            TempData["Success"] = "<script>alert('Recipe updated successfully!');</script>";

                        else
                        {
                            TempData["Success"] = "<script>alert('Dublicate recipe name! Cannot be updated!');</script>";

                            BindItemTypeCategory();
                            RecipeMasterBO model1 = _productionRecipeRepository.GetById(model.RecipeID);
                            model1.recipe_Details = _productionRecipeRepository.GetItemDetailsByRecipeId(model.RecipeID);

                            //Binding item grid with Recipe. 
                            var recipeList = _productionRecipeRepository.GetItemDetailsForRecipe();
                            var dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", "UOM_Id");

                            string Ingredients = "Ingredients";

                            if (model1 != null)
                            {
                                var ItemCount = model1.recipe_Details.Count;
                                var i = 0;
                                while (i < ItemCount)
                                {
                                    Ingredients = "Ingredients";
                                    Ingredients = Ingredients + i;
                                    dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", model1.recipe_Details[i].ItemId);
                                    ViewData[Ingredients] = dd;
                                    i++;
                                }
                            }

                            ViewData[Ingredients] = dd;

                            return View(model1);
                        }
                        return RedirectToAction("Index", "RecipeMater");
                    }

                    else
                    {
                        TempData["Success"] = "<script>alert('Please enter the proper data!');</script>";
                        BindItemTypeCategory();
                        RecipeMasterBO model1 = _productionRecipeRepository.GetById(model.RecipeID);
                        model1.recipe_Details = _productionRecipeRepository.GetItemDetailsByRecipeId(model.RecipeID);

                        //Binding item grid with Recipe. 
                        var recipeList = _productionRecipeRepository.GetItemDetailsForRecipe();
                        var dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", "UOM_Id");

                        string Ingredients = "Ingredients";

                        if (model1 != null)
                        {
                            var ItemCount = model1.recipe_Details.Count;
                            var i = 0;
                            while (i < ItemCount)
                            {
                                Ingredients = "Ingredients";
                                Ingredients = Ingredients + i;
                                dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", model1.recipe_Details[i].ItemId);
                                ViewData[Ingredients] = dd;
                                i++;
                            }
                        }

                        ViewData[Ingredients] = dd;

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
                return RedirectToAction("Index", "RecipeMater");
            }
        }
        #endregion

        #region Delete function
        /// <summary>
        /// Date: 27 Feb '23 
        /// Rahul: Delete the perticular record 
        /// </summary>
        /// <param name="ID">record Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteRecipeItem(int Recipe_ID)
        {
            if (Session[ApplicationSession.USERID] != null)
            {
                int userId = Convert.ToInt32(Session[ApplicationSession.USERID]);
                _productionRecipeRepository.Delete(Recipe_ID, userId);
                TempData["Success"] = "<script>alert('Recipe deleted successfully!');</script>";
                return RedirectToAction("Index", "RecipeMater");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #endregion

        #region View Inquiry Form 
        /// <summary>
        /// Created By: Farheen
        /// Created Date : 18-05-2023. 
        /// Description: This method responsible for View of Recipe.
        /// </summary>
        /// <param name="RecipeID"></param>
        /// <returns></returns>
        public ActionResult ViewRecipe(int Recipe_ID)
        {
            if (Session[ApplicationSession.USERID] == null)
                return RedirectToAction("Index", "Login");

            BindItemTypeCategory();
            Session["Recipe_ID"] = Recipe_ID;
            RecipeMasterBO model = _productionRecipeRepository.GetById(Recipe_ID);
            model.recipe_Details = _productionRecipeRepository.GetItemDetailsByRecipeId(Recipe_ID);

            //Binding item grid with Recipe. 
            var recipeList = _productionRecipeRepository.GetItemDetailsForRecipe();
            var dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", "UOM_Id");

            string Ingredients = "Ingredients";

            if (model != null)
            {
                var ItemCount = model.recipe_Details.Count;
                var i = 0;
                while (i < ItemCount)
                {
                    Ingredients = "Ingredients";
                    Ingredients = Ingredients + i;
                    dd = new SelectList(recipeList.ToList(), "ID", "Item_Code", model.recipe_Details[i].ItemId);
                    ViewData[Ingredients] = dd;
                    i++;
                }
            }

            ViewData[Ingredients] = dd;

            return View(model);

            ////Binding item grid with sell type item.
            //var itemList = _purchaseOrderRepository.GetItemDetailsForDD(1);
            //var dd = new SelectList(itemList.ToList(), "ID", "Item_Code");
            //ViewData["itemListForDD"] = dd;
            //InquiryFormBO model = _inquiryFormRepository.GetInquiryFormById(InquiryID);
            //return View(model);
        }
        #endregion

        #region Export PDF recipe Report
        /// <summary>
        /// Create by Shweta on 10/08/2023
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public ActionResult ExportAsPDF()
        {


            StringBuilder sb = new StringBuilder();
            RecipeMasterBO RecipeList = _productionRecipeRepository.GetById(Convert.ToInt32(Session["Recipe_ID"]));
            RecipeList.recipe_Details = _productionRecipeRepository.GetItemDetailsByRecipeId(Convert.ToInt32(Session["Recipe_ID"]));

            string strPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Theme/MainContent/images/logo.png";
            string ReportName = "Recipe";

            sb.Append("<div style='vertical-align:top'>");
            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-left:center;width: 100%;border:none'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left; padding-left: 5px; '>" + "<img height='80' width='90' src='" + strPath + "'/></th>");
            sb.Append("<label style='font-size:22px;color:black; font-family:Times New Roman;'>" + ReportName + "</label>");
            sb.Append("<th colspan='4' style=' padding-left: -180px; font-size:20px;text-align:center;font-family:Times New Roman;'>" + ReportName + "</th>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;font-family:Times New Roman;'>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'>Product Number</th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + RecipeList.ProductName + "</td>");
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;'> Recipe Name </th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px; font-size:12px; font-family:Times New Roman;'>" + RecipeList.RecipeName + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='width:10%;text-align:left;padding: 1px; font-family:Times New Roman;'>");
            //@*added 'CreatedDate' 16 - 08 - 23.*@
            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'>Date </th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + RecipeList.CreatedDate + "</td>");

            sb.Append("<th style='width:10%;text-align:left;padding: 2px; font-family:Times New Roman;font-size:12px;;'> Description </th>");
            sb.Append("<td style='width:20%;text-align:left;padding: 2px;;font-size:12px; font-family:Times New Roman;'>" + RecipeList.Description + "</td>");
            sb.Append("</tr>");

            sb.Append("</thead>");
            sb.Append("</table>");
            sb.Append("<hr style='height: 1px; border: none; color:#333;background-color:#333;'></hr>");

            sb.Append("<table>");
            sb.Append("<thead>");
            sb.Append("<tr>");

            sb.Append("<th style='text-align:center;padding-left: -60px; font-family:Times New Roman;width:100%;font-size:15px;'>Recipe Ingredient Details</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("</table>");

            sb.Append("<table style='vertical-align: top;font-family:Times New Roman;text-align:center;border-collapse: collapse;width: 100%;repeat-header: yes;page-break-inside: auto;'>");
            sb.Append("<thead>");


            sb.Append("<tr style='text-align:center;padding: 5px; font-family:Times New Roman;background-color:#C0DBEA'>");

            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:0.5%;font-size:10px;border: 0.01px black;'>#</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Item Code</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:4%;font-size:10px;border: 0.01px black;'>Item</th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:0.5%;font-size:10px;border: 0.01px black;'>Unit</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:1%;font-size:10px;border: 0.01px black;'>Percentage(%)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:0.5%;font-size:10px;border: 0.01px black;'>Batch Size(KG)</ th>");
            sb.Append("<th style='text-align:center;padding: 5px; font-family:Times New Roman;width:2%;font-size:10px;border: 0.01px black;'>Description</ th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int i = 1;

            foreach (var item in RecipeList.recipe_Details)
            {
                sb.Append("<tr style='text-align:center;'>");
                sb.Append("<td style='text-align:center;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + i + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemCode + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.ItemName + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.UnitName + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Ratio + "</td>");
                sb.Append("<td style='text-align:right;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.BatchSize + "</td>");
                sb.Append("<td style='text-align:left;border: 0.01px black;font-size:10px; font-family:Times New Roman;padding: 5px;'>" + item.Description + "</td>");

                sb.Append("</tr>");
                i++;
            }
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append("<br />");


            sb.Append("</div>");

            using (var sr = new StringReader(sb.ToString()))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    Document pdfDoc = new Document(PageSize.A4);
                    //Document pdfDoc = new Document(PageSize.A4.Rotate());
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
                    string filename = "Recipe_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ".pdf";
                    TempData["ReportName"] = ReportName.ToString();
                    return File(memoryStream.ToArray(), "application/pdf", filename);
                }
            }
        }

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
                InquiryFormController InquiryFormController = new InquiryFormController();
                InquiryFormController.setBorder(writer, document);

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
        #endregion

    }
}