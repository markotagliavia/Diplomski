using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracija.ViewModel
{
    public class IzmeniKorisnikaViewModel : BindableBase
    {
        //komande su: izmeni i otkazi

        #region Commands
        public MyICommand<string> IzmeniKorisnikaCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        
        #endregion

        #region Members
        private Korisnik userOnSession;
        private ZaposleniKorisnik userForEdit;
        #endregion

        public IzmeniKorisnikaViewModel()
        {
            IzmeniKorisnikaCommand = new MyICommand<string>(IzmeniKorisnika);
            OtkaziCommand = new MyICommand<string>(Otkazi);
        }

        #region Constructors
        public Korisnik UserOnSession { get => userOnSession; set => userOnSession = value; }
        public ZaposleniKorisnik UserForEdit { get => userForEdit; set => userForEdit = value; }
        #endregion

        #region CommandsImplementation
        private void Otkazi(string obj)
        {
            throw new NotImplementedException();
        }

        private void IzmeniKorisnika(string obj)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
