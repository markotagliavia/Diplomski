using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracija.ViewModel
{
    public class DodajKorisnikaViewModel : BindableBase
    {
        //komande su: dodaj i otkazi

        #region Commands
        public MyICommand<string> DodajKorisnikaCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        #endregion

        public DodajKorisnikaViewModel()
        {
            DodajKorisnikaCommand = new MyICommand<string>(DodajKorisnika);
            OtkaziCommand = new MyICommand<string>(Otkazi);
        }

        #region CommandsImplementation
        private void Otkazi(string obj)
        {
            throw new NotImplementedException();
        }

        private void DodajKorisnika(string obj)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
