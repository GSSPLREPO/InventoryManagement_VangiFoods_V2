using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface ICommonFunctions
    {
        //For fetching the document no
        string GetDocumentNo(int DocumentType); //Logic is in SP: Document type=1 for Order Confirmation 
                                               //Document type=2 for Purchase Order
                                               //Document type=3 for Inward note
        string GetWorkOrderNo(int DocumentType,string WorkOrderType); 
    }
}
