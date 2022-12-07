using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class IndentBO
    {
        public int ID { get; set; }
        public string IndentNo { get; set; }
        public Nullable<System.DateTime> IndentDate { get; set; }
        public Nullable<System.DateTime> IndentDueDate { get; set; }
        public Nullable<int> RaisedTo { get; set; }
        public string Description { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        //Added for Indent
        public string IndentStatus { get; set; }
    }
}
