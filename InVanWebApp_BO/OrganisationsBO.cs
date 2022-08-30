﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class OrganisationsBO
    {
        public int OrganisationId { get; set; }
        public int OrganisationGroupId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Logo { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string Range { get; set; }
        public string Division { get; set; }
        public string Commisionerate { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string City { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> StateCode { get; set; }
        public string Country { get; set; }
        public string PANNo { get; set; }
        public string CINNo { get; set; }
        public string GSTINNo { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
