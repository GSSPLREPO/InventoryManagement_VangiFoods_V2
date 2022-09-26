﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp_BO;

namespace InVanWebApp.Repository.Interface
{
    public interface IInwardNoteRepository
    {
        //Define function for fetching details of Item type.
        IEnumerable<InwardNoteBO> GetAll();

        //Get purchase order list for dropdown.
        IEnumerable<PurchaseOrderBO> GetPONumberForDropdown();

        //Get purchase order details by ID.
        IEnumerable<PurchaseOrderBO> GetPODetailsById(int PO_Id);
    }
}
