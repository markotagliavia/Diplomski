using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracija.ViewModel
{
    public class PregledUlogaViewModel : BindableBase
    {
        //komande su : navigacija na dodavanje i izmenu, komanda brisanja i pretrage

        #region Commands
        public MyICommand<string> DodajUloguNavCommand { get; private set; }
        public MyICommand<string> IzmeniUloguNavCommand { get; private set; }
        public MyICommand<string> IzbrisiUloguCommand { get; private set; }
        public MyICommand<string> PretraziUloguCommand { get; private set; }
        #endregion

        public PregledUlogaViewModel()
        {
            DodajUloguNavCommand = new MyICommand<string>(DodajUloguNav);
            IzmeniUloguNavCommand = new MyICommand<string>(IzmeniUloguNav);
            IzbrisiUloguCommand = new MyICommand<string>(IzbrisiUlogu);
            PretraziUloguCommand = new MyICommand<string>(PretraziUlogu);
        }

        #region CommandImplementation
        private void PretraziUlogu(string obj)
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

        private void DodajUloguNav(string obj)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
