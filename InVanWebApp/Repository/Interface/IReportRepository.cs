﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IReportRepository
    {

        //Calling Method For PO Report
        List<PurchaseOrderBO> getPOReportData(DateTime fromDate, DateTime toDate, string Status, int VendorId);

        //Calling Method For Raw Material Received Report
        List<GRN_BO> getRawMaterialReceivedData(DateTime fromDate, DateTime toDate, int item, int wearhouse);
    }
}