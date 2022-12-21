using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class IndentBO
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="Please enter the indent no!")]
        public string IndentNo { get; set; }
        [Required(ErrorMessage ="Select the indent date!")]
        public Nullable<System.DateTime> IndentDate { get; set; }
        public Nullable<System.DateTime> IndentDueDate { get; set; }
        [Required(ErrorMessage ="Select user!")]
        public Nullable<int> RaisedBy { get; set; }
        public string Description { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        [Required(ErrorMessage ="Select warehouse!")]
        public int LocationId { get; set; }
        public string LocationName{ get; set; }
        [Required(ErrorMessage ="Select designation!")]
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string UserName { get; set; }

        //Added for Indent
        public string IndentStatus { get; set; }

        //This is for inserting the itemdetails
        public Nullable<int> Item_ID { get; set; }
        public string ItemCode { get; set; }
        public string ItemUnit { get; set; }
        public string ItemName { get; set; }
        public Nullable<double> RequiredQuantity { get; set; }
        public string itemDetails { get; set; }
        public int IndentCount { get; set; }

        public List<Indent_DetailsBO> indent_Details { get; set; }

    }
}
