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
        [Required(ErrorMessage = "Enter name of User!")]
        public string VerifyBy { get; set; }
        [Required(ErrorMessage = "Select Date!")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }
        public string RMRecevingArea { get; set; }
        public string CratesBlue { get; set; }
        public string CratesYellow { get; set; }
        public string CratesRed { get; set; }
        public string WeightingArea { get; set; }
        public string Water { get; set; }
        public string HygineArea { get; set; }
        public string RawMaterial { get; set; }
        public string FinishGoods { get; set; }
        public string WalkWay { get; set; }
        public string VegetableWashingArea { get; set; }
        public string PeelingMachine { get; set; }
        public string ColdStorage { get; set; }
        public string Roboqubos { get; set; }
        public string Silo { get; set; }
        public string PackagingLine { get; set; }
        public string Chiller { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
