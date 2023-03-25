using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class PreStartupHygieneBO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Name of User!")]
        public string VerifyBy { get; set; }
        [Required(ErrorMessage = "Select Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }
        [Required(ErrorMessage = "Whether the RM Receving Area is clean?")]
        public string RMRecevingArea { get; set; }
        [Required(ErrorMessage = "Select avaibility of Blue Crates!")]
        public string CratesBlue { get; set; }
        [Required(ErrorMessage = "Select avaibility of Yellow Crates!")]
        public string CratesYellow { get; set; }
        [Required(ErrorMessage = "Select avaibility of Red Crates!")]
        public string CratesRed { get; set; }
        [Required(ErrorMessage = "Select Whether the Weighting Area is OK?")]
        public string WeightingArea { get; set; }
        [Required(ErrorMessage = "Select the Dosage of Water!")]
        public string Water { get; set; }
        [Required(ErrorMessage = "Select Whether the Hygiene Area is OK?")]
        public string HygineArea { get; set; }
        [Required(ErrorMessage = "Select avaibility of Raw Materials!")]
        public string RawMaterial { get; set; }
        [Required(ErrorMessage = "Select avaibility of Finish Goods!")]
        public string FinishGoods { get; set; }
        [Required(ErrorMessage = "Select cleanliness of Walk Way!")]
        public string WalkWay { get; set; }
        [Required(ErrorMessage = "Select cleanliness of Vegetable Washing Area1")]
        public string VegetableWashingArea { get; set; }
        [Required(ErrorMessage = "Select cleanliness of Peeling Machine!")]
        public string PeelingMachine { get; set; }
        [Required(ErrorMessage = "Select cleanliness of Cold Storage!")]
        public string ColdStorage { get; set; }
        [Required(ErrorMessage = "Select avaibility of Roboqubos!")]
        public string Roboqubos { get; set; }
        [Required(ErrorMessage = "Select avaibility of Silo!")]
        public string Silo { get; set; }
        [Required(ErrorMessage = "Select avaibility of Packaging Line!")]
        public string PackagingLine { get; set; }
        [Required(ErrorMessage = "Select chlorination of Chiller!")]
        public string Chiller { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }


        /*REPORT*/
        //Added the below field for report

        [Required(ErrorMessage = "Please Select From Date!")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Please Select To Date!")]
        public DateTime toDate { get; set; }


    }
}
