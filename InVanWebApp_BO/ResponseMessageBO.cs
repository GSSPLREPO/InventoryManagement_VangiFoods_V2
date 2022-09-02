using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class ResponseMessageBO
    {
        //Response message for Item
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public bool Status { get; set; }
        public string ItemType { get; set; }
        public string ItemCategory{ get; set; }
        public string LocationName{ get; set; }
        public string RoleName{ get; set; }
        public string TaxName{ get; set; }
        public string UnitName{ get; set; }
        public string EmployeeName { get; set; }
        public string CompanyName{ get; set; }

    }
}
