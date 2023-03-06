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
        [Required(ErrorMessage = "Enter RM Receving Area!")]
        public string RMRecevingArea { get; set; }
        [Required(ErrorMessage = "Enter Crates Blue!")]
        public string CratesBlue { get; set; }
        [Required(ErrorMessage = "Enter Crates Yellow!")]
        public string CratesYellow { get; set; }
        [Required(ErrorMessage = "Enter Crates Red!")]
        public string CratesRed { get; set; }
        [Required(ErrorMessage = "Enter Weighting Area!")]
        public string WeightingArea { get; set; }
        [Required(ErrorMessage = "Enter Water!")]
        public string Water { get; set; }
        [Required(ErrorMessage = "Enter Hygine Area!")]
        public string HygineArea { get; set; }
        [Required(ErrorMessage = "Enter Raw Material!")]
        public string RawMaterial { get; set; }
        [Required(ErrorMessage = "Enter Finish Goods!")]
        public string FinishGoods { get; set; }
        [Required(ErrorMessage = "Enter Walk Way!")]
        public string WalkWay { get; set; }
        [Required(ErrorMessage = "Enter Vegetable Washing Area!")]
        public string VegetableWashingArea { get; set; }
        [Required(ErrorMessage = "Enter Peeling Machine!")]
        public string PeelingMachine { get; set; }
        [Required(ErrorMessage = "Enter Cold Storage!")]
        public string ColdStorage { get; set; }
        [Required(ErrorMessage = "Enter Roboqubos!")]
        public string Roboqubos { get; set; }
        [Required(ErrorMessage = "Enter Silo!")]
        public string Silo { get; set; }
        [Required(ErrorMessage = "Enter Packaging Line!")]
        public string PackagingLine { get; set; }
        [Required(ErrorMessage = "Enter Chiller!")]
        public string Chiller { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
