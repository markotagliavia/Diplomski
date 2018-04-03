using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracija.ViewModel
{
    public class IzmeniUloguViewModel : BindableBase
    {
        //komande su: dodaj i otkazi

        #region Commands
        public MyICommand<string> IzmeniUloguCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        #endregion

        public IzmeniUloguViewModel()
        {
            IzmeniUloguCommand = new MyICommand<string>(IzmeniUlogu);
            OtkaziCommand = new MyICommand<string>(Otkazi);
        }

        #region CommandsImplementation
        private void Otkazi(string obj)
        {
            throw new NotImplementedException();
        }

        private void IzmeniUlogu(string obj)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
