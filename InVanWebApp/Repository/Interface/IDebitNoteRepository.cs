using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InVanWebApp.Repository.Interface
{
    public interface IDebitNoteRepository
    {
        Dictionary<int, string> GetInwardNoteNumbers();
    }
}
