using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface ICalibrationLogRepository
    {
        //Define function for fetching details of Calibration Log.
        IEnumerable<CalibrationLogBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(CalibrationLogBO model);

        //Define function for fetching details of Calibration Log by Id.
        CalibrationLogBO GetById(int Id);

        //Function define for: Update CalibrationLog record.
        ResponseMessageBO Update(CalibrationLogBO model);

        //Function define for: Delete record of Calibration Log using it's Id
        void Delete(int Id, int userId);
    }
}
