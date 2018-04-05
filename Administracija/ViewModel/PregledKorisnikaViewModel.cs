using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracija.ViewModel
{
    public class PregledKorisnikaViewModel : BindableBase
    {
        //komande su : navigacija na dodavanje i izmenu i komande brisanja i pretrage

        #region Commands
        public MyICommand<string> DodajKorisnikaNavCommand { get; private set; }
        public MyICommand<string> IzmeniKorisnikaNavCommand { get; private set; }
        public MyICommand<string> IzbrisiKorisnikaCommand { get; private set; }
        public MyICommand<string> PretraziKorisnikaCommand { get; private set; }
        #endregion

        public PregledKorisnikaViewModel()
        {
            DodajKorisnikaNavCommand = new MyICommand<string>(DodajKorisnikaNav);
            IzmeniKorisnikaNavCommand = new MyICommand<string>(IzmeniKorisnikaNav);
            IzbrisiKorisnikaCommand = new MyICommand<string>(IzbrisiKorisnika);
            PretraziKorisnikaCommand = new MyICommand<string>(PretraziKorisnika);
        }

        #region CommandsImplementation
        private void PretraziKorisnika(string obj)
        {
            throw new NotImplementedException();
        }

        private void IzbrisiKorisnika(string obj)
        {
            throw new NotImplementedException();
        }

        private void IzmeniKorisnikaNav(string obj)
        {
            throw new NotImplementedException();
        }

        private void DodajKorisnikaNav(string obj)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
