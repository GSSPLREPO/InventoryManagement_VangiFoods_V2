using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InVanWebApp_BO;
using InVanWebApp.Repository;
using InVanWebApp.Repository.Interface;
using log4net;

namespace InVanWebApp.Common
{
    public class GetDocumentNumber
    {
        private ICommonFunctions _repository;
        private static ILog log = LogManager.GetLogger(typeof(GetDocumentNumber));

        #region Initializing constructor
        /// <summary>
        /// Farheen: Constructor without parameter
        /// </summary>
        public GetDocumentNumber()
        {
            _repository = new CommonFunctions();
        }
        /// <summary>
        /// Farheen: Constructor with parameters for initializing the interface object.
        /// </summary>
        /// <param name="unitRepository"></param>
        public GetDocumentNumber(ICommonFunctions commonFunctions)
        {
            _repository = commonFunctions;
        }
        #endregion

        #region Get document number
        public string GetDocumentNo(int DocumentType)
        {
            var DocumentNumber="";
            try
            {
                DocumentNumber = _repository.GetDocumentNo(DocumentType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return DocumentNumber;
        }
        #endregion

        //#region Get Batch number
        //public string GetBatchNo()
        //{
        //    var BatchNumber = "";
        //    try
        //    {
        //        BatchNumber = _repository.GetBatchNo();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message, ex);
        //    }
        //    return BatchNumber; 
        //}
        //#endregion

    }
}