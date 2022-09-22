using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp_BO
{
    public class ScreenNameBO
    {
        public int ScreenId { get; set; }
        public string DisplayName { get; set; }
        public string ScreenName { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

    }
}
