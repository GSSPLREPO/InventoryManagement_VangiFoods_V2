using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface ICertificateOfAnalysisRepository
    {
        //Define function for fetching details of CertificateOfAnalysis
        IEnumerable<CertificateOfAnalysisBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(CertificateOfAnalysisBO model);

        //Function define for: Delete record of CertificateOfAnalysis using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of CertificateOfAnalysis by Id.
        CertificateOfAnalysisBO GetById(int Id);

        //Function define for: Update CertificateOfAnalysis record.
        ResponseMessageBO Update(CertificateOfAnalysisBO model);
        //Report Method
        List<CertificateOfAnalysisBO> GetAllCertificateOfAnalysisList(int flag, DateTime? fromDate = null, DateTime? toDate = null);

    }
}
