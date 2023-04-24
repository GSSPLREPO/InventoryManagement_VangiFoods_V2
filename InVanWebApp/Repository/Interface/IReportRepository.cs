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
        //Rejection 'getRejectionReportData' commented 24-04-23.
        //List<RejectionNoteItemDetailsBO> getRejectionReportData(DateTime fromDate, DateTime toDate); 
        List<DeliveryChallanItemDetailsBO> getFinishedGoodsReportData(DateTime fromDate, DateTime toDate,int itemId);
        List<StockMasterBO> getInventoryFIFOReportData(DateTime fromDate, DateTime toDate, int itemId);
        List<LocationWiseStockBO> getTotalInventoryCostData(DateTime fromDate, DateTime toDate, int LocationId, int itemId);
        List<StockAdjustmentDetailsBO> getStockReconciliationData(DateTime fromDate, DateTime toDate, int LocationId, int itemId);
        List<StockMasterBO> getInventoryAnalysisFIFOReportData(DateTime fromDate, DateTime toDate, int itemId);
        List<CompanyBO> getCompanyDataByType(string CompanyType);

        //Calling Method for Purchase Invoice Report
        List<PurchaseOrderBO> getPurchaseInvoiceReportData(DateTime fromDate, DateTime toDate, int itemId, string Status);

        //Calling Method For Issue Note Report
        List<IssueNoteBO> getIssueNoteReportData(DateTime fromDate, DateTime toDate, int itemId);

        //Calling Method For GRN Report
        List<GRN_BO> getGRNReportData(DateTime fromDate, DateTime toDate, int itemId);

        //Calling Method For Rejection Report updated 24-04-2023. 
        List<RejectionNoteItemDetailsBO> getRejectionReportData(DateTime fromDate, DateTime toDate, int ItemId, int FlagDebitNote = 0);


        ///Production Reprot
        //Batchwise Production cost Reprot
        List<ReportBO> getBatchwiseProductionCostReportData(DateTime fromDate, DateTime toDate, string ItemId);

        //To Bind BatchNumber for Batchwise Production Cost Report
        IEnumerable<ReportBO> GetAll();

        //To Bind BatchNumber for Batchwise Production Cost Report
        IEnumerable<ReportBO> GetGFLocationBatchNumber();

        //Yeild Report 
        List<ReportBO> getYeildReportData(DateTime fromDate, DateTime toDate, int ItemId);

        //FG Locationwise Report
        List<ReportBO> getFGLocationwiseReportData(DateTime fromDate, DateTime toDate, int LocationId);

        //Batchwise Production cost Reprot
        List<ReportBO> getRawMaterialCostAnalysisReportData(DateTime fromDate, DateTime toDate, int ItemId);

        //To Bind WorkOrder for Batchwise Production Cost Report
        IEnumerable<ReportBO> Getall();


        // Delivery Challan (Against SO) Report
        List<ReportBO> getDeliveryChallanAgainstSOReportData(DateTime fromDate, DateTime toDate, string SONumberId);

        // Sales Report
        List<ReportBO> getSalesReportData(DateTime fromDate, DateTime toDate, string SONumberId);


        //To Bind SONumber for Delivery Challan (Against SO) Report
        IEnumerable<ReportBO> GetSONumber();

        // Sales Invoice Report
        List<ReportBO> getSalesInvoiceReportData(DateTime fromDate, DateTime toDate, string SONumberId);

        // Debit Note Report
        List<ReportBO> getDBNoteReportData(DateTime fromDate, DateTime toDate, int DBNoteNumberId);


        //To Bind GetDebitNoteNumber for Debit Note Report
        IEnumerable<ReportBO> GetDebitNoteNumber();
        //To Bind Vendor-wise wastage report against each PO.    
        List<RejectionNoteItemDetailsBO> getWastageReportData(DateTime fromDate, DateTime toDate, int inwardNumber);
        //To Bind Pre-Production_QC report against each WO.     
        List<PreProduction_QC_Details> getPreProduction_QCReportData(DateTime fromDate, DateTime toDate, int PreProductionQCId);

    }
}
