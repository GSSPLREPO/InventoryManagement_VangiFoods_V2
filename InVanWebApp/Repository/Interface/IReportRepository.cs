using System;
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
        List<RejectionNoteItemDetailsBO> getRejectionReportData(DateTime fromDate, DateTime toDate);
        List<DeliveryChallanItemDetailsBO> getFinishedGoodsReportData(DateTime fromDate, DateTime toDate,int itemId);
        List<StockMasterBO> getInventoryFIFOReportData(DateTime fromDate, DateTime toDate, int itemId);
        List<LocationWiseStockBO> getTotalInventoryCostData(DateTime fromDate, DateTime toDate, int LocationId, int itemId);
        List<StockAdjustmentDetailsBO> getStockReconciliationData(DateTime fromDate, DateTime toDate, int LocationId, int itemId);
        List<StockMasterBO> getInventoryAnalysisFIFOReportData(DateTime fromDate, DateTime toDate, int itemId);
        List<PurchaseOrderBO> getPurchaseInvoiceReportData(DateTime fromDate, DateTime toDate, int PoNumber);
    }
}
