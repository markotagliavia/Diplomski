using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skladistenje.ViewModel
{
    public class DodajProizvodViewModel : BindableBase
    {
        #region Members
        private int context;
        private Proizvod proizvodForEdit = new Proizvod();
        private Korisnik userOnSession = new Korisnik();
        #endregion

        public DodajProizvodViewModel(int v, Proizvod p)
        {
            context = v;
            ProizvodForEdit = p;
        }



        #region Constructors
        public Proizvod ProizvodForEdit { get => proizvodForEdit; set { proizvodForEdit = value; OnPropertyChanged("ProizvodForEdit"); } }
        public Korisnik UserOnSession { get => userOnSession; set => userOnSession = value; }
        #endregion
    }
}
