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
        [Required(ErrorMessage = "Enter Name of Empolyee!")]
        public string NameOfEmpolyee{ get; set; }

        /*Department*/
        [Required(ErrorMessage = "Enter Department!")]
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
        [Required(ErrorMessage = "Enter Hand Wash!")]
        public string HandWash { get; set; }

        /*Clean Nails*/
        [Required(ErrorMessage = "Enter Clean Nails!")]
        public string CleanNails { get; set; }

        /*Clean Uniform*/
        [Required(ErrorMessage = "Enter Clean Uniform!")]
        public string CleanUniform { get; set; }

        /*Appear Any Cuts and Wounds*/
        [Required(ErrorMessage = "Enter Appear Any Cuts and Wounds!")]
        public string AppearAnyCutsandWounds { get; set; }

        /*Enter Wear Any Jwellery*/
        [Required(ErrorMessage = "Enter Wear Any Jwellery!")]
        public string WearAnyJwellery { get; set; }

        /*Fully Coverd Hair*/
        [Required(ErrorMessage = "Enter Fully Coverd Hair!")]
        public string FullyCoverdHair { get; set; }

        /*Clean Shoes*/
        [Required(ErrorMessage = "Enter No Tobaco, Chewingum!")]
        public string CleanShoes { get; set; }

        /*No Tobaco, Chewingum*/
        [Required(ErrorMessage = "Enter No Tobaco, Chewingum!")]
        public string NoTobacoChewingum { get; set; }

        /*Any Kind Of Illness/Seakness*/
        [Required(ErrorMessage = "Enter Any Kind Of Illness/Seakness!")]
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
