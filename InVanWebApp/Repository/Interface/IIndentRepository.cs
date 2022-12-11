﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IIndentRepository
    {
        //Define function for fetching details of Item master, edited the passing parameter for PO.
        IEnumerable<IndentBO> GetAll(string type=null);

        //Get by Id Indent details
        List<Indent_DetailsBO> GetById(int id);

    }
}
