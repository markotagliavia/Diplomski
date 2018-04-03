using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracija.ViewModel
{
    public class DodajUloguViewModel : BindableBase
    {
        //komande su: dodaj i otkazi

        #region Commands
        public MyICommand<string> DodajUloguCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        #endregion

        public DodajUloguViewModel()
        {
            DodajUloguCommand = new MyICommand<string>(DodajUlogu);
            OtkaziCommand = new MyICommand<string>(Otkazi);
        }

        #region CommandsImplementation
        private void Otkazi(string obj)
        {
            throw new NotImplementedException();
        }

        private void DodajUlogu(string obj)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
