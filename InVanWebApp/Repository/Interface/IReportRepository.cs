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
        List<CompanyBO> getCompanyDataByType(DateTime fromDate, DateTime toDate, string CompanyType);

        //Calling Method for Purchase Invoice Report
        List<PurchaseOrderBO> getPurchaseInvoiceReportData(DateTime fromDate, DateTime toDate, int itemId, string Status);

        //Calling Method For Issue Note Report
        List<IssueNoteBO> getIssueNoteReportData(DateTime fromDate, DateTime toDate, int itemId);

        //Calling Method For GRN Report
        List<GRN_BO> getGRNReportData(DateTime fromDate, DateTime toDate, int itemId);

        //Calling Method For Rejection Report
        List<RejectionNoteItemDetailsBO> getRejectionReportData(DateTime fromDate, DateTime toDate, int ItemId);

    }
}
