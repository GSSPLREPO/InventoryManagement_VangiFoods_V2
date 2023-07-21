using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class WeighmentReceivedDataBO 
    {
        public int ID { get; set; }        
        public decimal Column1 { get; set; }  //Get Captured Weight
        public string Column2 { get; set; }  //Get Captured Unit 
        public Nullable<System.DateTime> Column3 { get; set; }   //Get Captured Weight DateTime 

    }

}
