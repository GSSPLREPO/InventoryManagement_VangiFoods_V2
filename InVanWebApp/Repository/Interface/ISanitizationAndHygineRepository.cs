﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface ISanitizationAndHygineRepository
    {
        //Define function for fetching details of SanitizationAndHygine.
        IEnumerable<SanitizationAndHygineBO> GetAll();

        //Function define for: Insert record.
        ResponseMessageBO Insert(SanitizationAndHygineBO model);

        //Function define for: Delete record of SanitizationAndHygine using it's Id
        void Delete(int Id, int userId);

        //Define function for fetching details of SanitizationAndHygine by Id.
        SanitizationAndHygineBO GetById(int Id);

        //Function define for: Update SanitizationAndHygine record.
        ResponseMessageBO Update(SanitizationAndHygineBO model);

        List<SanitizationAndHygineBO> SanitizationAndHygineList(int flagdate, DateTime? fromDate = null, DateTime? toDate = null);

    }
}
