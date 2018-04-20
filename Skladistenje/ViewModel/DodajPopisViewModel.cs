using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skladistenje.ViewModel
{
    public class DodajPopisViewModel : BindableBase
    {
        public DodajPopisViewModel()
        {
        }

        public Korisnik UserOnSession { get; internal set; }
    }
}
