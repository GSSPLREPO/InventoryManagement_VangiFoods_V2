using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InVanWebApp_BO
{
    public class SanitizationAndHygineBO
    {
        public int Id { get; set; }
        /*Date*/
        [Required(ErrorMessage = "Select Date!")]

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }

        /*Name of Empolyee*/
        [Required(ErrorMessage = "Enter Name of Employee!")]
        public string NameOfEmpolyee{ get; set; }

        /*Department*/
        [Required(ErrorMessage = "Enter Name of Department!")]
        public string Department { get; set; }


        /*Body Temperature*/
        [Required(ErrorMessage = "Select Body Temperature!")]
        public string BodyTemperature { get; set; }

        ///*Normal*/
        //[Required(ErrorMessage = "Select Body Temperature Normal!")]
        //public string Normal { get; set; }

        ///*Modrate*/
        //[Required(ErrorMessage = "Select Body Temperature!")]
        //public string Modrate { get; set; }

        ///*High*/
        //[Required(ErrorMessage = "Select Body Temperature!")]
        //public string High { get; set; }

        /*Hand Wash*/
        [Required(ErrorMessage = "Select Whether Hand is Washed or Not!")]
        public string HandWash { get; set; }

        /*Clean Nails*/
        [Required(ErrorMessage = "Select Whether the Nails are Cleaned or Not!")]
        public string CleanNails { get; set; }

        /*Clean Uniform*/
        [Required(ErrorMessage = "Select Whether the Uniform is Cleaned or Not!")]
        public string CleanUniform { get; set; }

        /*Appear Any Cuts and Wounds*/
        [Required(ErrorMessage = "Select the presence of any Cuts and Wounds on Body!")]
        public string AppearAnyCutsandWounds { get; set; }

        /*Select Wear Any Jwellery*/
        [Required(ErrorMessage = "Select Whether the Employee is carrying the Jewellery!")]
        public string WearAnyJwellery { get; set; }

        /*Fully Coverd Hair*/
        [Required(ErrorMessage = "Select Whether the Hair is Fully Coverd or Not!")]
        public string FullyCoverdHair { get; set; }

        /*Clean Shoes*/
        [Required(ErrorMessage = "Select Whether the Shoes are Cleaned or Not!!")]
        public string CleanShoes { get; set; }

        /*No Tobaco, Chewingum*/
        [Required(ErrorMessage = "Select Whether Employee is Carrying and Eating any Tobacco or Chewing Gum!")]
        public string NoTobacoChewingum { get; set; }

        /*Any Kind Of Illness/Seakness*/
        [Required(ErrorMessage = "Select Whether the is Employee Having Any Kind Of Illness/Seakness!")]
        public string AnyKindOfIllnessSeakness { get; set; }

        /*Name of User*/
        [Required(ErrorMessage = "Enter Name of User!")]
        public string VerifyByName { get; set; }
        /*Remark*/
        public string Remark { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
