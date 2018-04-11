using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Skladistenje.ViewModel
{
    public class SkladistaViewModel : BindableBase
    {
        //komande su : navigacija na dodavanje i izmenu i komande brisanja i pretrage
        #region Commands
        public MyICommand<string> DodajSkladisteNavCommand { get; private set; }
        public MyICommand<string> IzmeniSkladisteNavCommand { get; private set; }
        public MyICommand<string> IzbrisiSkladisteCommand { get; private set; }
        public MyICommand<string> PretraziSkladisteCommand { get; private set; }
        #endregion

        #region Properties
        private Korisnik userOnSession;
        private int _selectedIndex = -1;
        private bool selectedInGrid;
        private string textSearch;
        private ObservableCollection<Skladiste> skladista;
        private Skladiste selectedValue;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ICollectionView defaultView;
        #endregion

        public SkladistaViewModel()
        {
            DodajSkladisteNavCommand = new MyICommand<string>(DodajSkladisteNav);
            IzmeniSkladisteNavCommand = new MyICommand<string>(IzmeniSkladisteNav);
            IzbrisiSkladisteCommand = new MyICommand<string>(IzbrisiSkladiste);
            PretraziSkladisteCommand = new MyICommand<string>(PretraziSkladiste);
            textSearch = "";
            skladista = new ObservableCollection<Skladiste>();
            foreach (var item in dbContext.Skladistes.ToList())
            {
                skladista.Add(item);
            }

            DefaultView = CollectionViewSource.GetDefaultView(Skladista);
        }

        #region Constructors

        public ObservableCollection<Skladiste> Skladista
        {
            get { return skladista; }
            set { skladista = value; }
        }

        public ICollectionView DefaultView { get => defaultView; set => defaultView = value; }

        public bool SelectedInGrid
        {
            get { return selectedInGrid; }
            set
            {
                selectedInGrid = value;
                OnPropertyChanged("SelectedInGrid");
            }
        }

        public string TextSearch
        {
            get { return textSearch; }
            set
            {
                textSearch = value;
                OnPropertyChanged("TextSearch");
            }
        }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }

            set
            {
                if (_selectedIndex == value)
                {
                    if (_selectedIndex > -1)
                    {
                        SelectedInGrid = true;
                    }
                    return;
                }
                _selectedIndex = value;
                if (_selectedIndex > -1)
                {
                    SelectedInGrid = true;
                }
            }
        }

        public Skladiste SelectedValue
        {
            get { return selectedValue; }
            set
            {
                selectedValue = value;
                OnPropertyChanged("SelectedValue");
            }
        }

        public Korisnik UserOnSession
        {
            get { return userOnSession; }
            set { userOnSession = value; }
        }
        #endregion

        #region CommandsImplementation
        private void PretraziSkladiste(string obj)
        {
            throw new NotImplementedException();
        }

        private void IzbrisiSkladiste(string obj)
        {
            throw new NotImplementedException();
        }

        private void IzmeniSkladisteNav(string obj)
        {
            throw new NotImplementedException();
        }

        private void DodajSkladisteNav(string obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
