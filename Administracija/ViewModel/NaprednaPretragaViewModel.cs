using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracija.ViewModel
{
    public class NaprednaPretragaViewModel : BindableBase
    {
        //komande u okviru pretrage su odlazak na izmenu pretrazenog, brisanje pretrazenog i pretraga
        public MyICommand<string> IzmeniKorisnikaNavCommand { get; private set; }
        public MyICommand<string> IzbrisiKorisnikaCommand { get; private set; }
        public MyICommand<string> IzmeniUloguNavCommand { get; private set; }
        public MyICommand<string> IzbrisiUloguCommand { get; private set; }
        public MyICommand<string> PretraziKorisnikeCommand { get; private set; }
        public MyICommand<string> PretraziUlogeCommand { get; private set; }

        public NaprednaPretragaViewModel()
        {
            IzmeniKorisnikaNavCommand = new MyICommand<string>(IzmeniKorisnikNav);
            IzbrisiKorisnikaCommand = new MyICommand<string>(IzbrisiKorisnika);
            IzmeniUloguNavCommand = new MyICommand<string>(IzmeniUloguNav);
            IzbrisiUloguCommand = new MyICommand<string>(IzbrisiUlogu);
            PretraziKorisnikeCommand = new MyICommand<string>(PretraziKorisnike);
            PretraziUlogeCommand = new MyICommand<string>(PretraziUloge);
        }

        private void PretraziUloge(string obj)
        {
            throw new NotImplementedException();
        }

        private void PretraziKorisnike(string obj)
        {
            throw new NotImplementedException();
        }

        private void IzbrisiUlogu(string obj)
        {
            throw new NotImplementedException();
        }

        private void IzmeniUloguNav(string obj)
        {
            throw new NotImplementedException();
        }

        private void IzbrisiKorisnika(string obj)
        {
            throw new NotImplementedException();
        }

        private void IzmeniKorisnikNav(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
