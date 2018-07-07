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

namespace Racunovodstvo.ViewModel
{
    public class ProfaktureViewModel:BindableBase
    {
        #region Members
        private ObservableCollection<Profaktura> profakture;
        private int _selectedIndex = -1;
        private bool selectedInGrid;
        private string textSearch;
        private Profaktura selectedValue;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ICollectionView defaultView;
        private int context;
        #endregion
        #region Commands
        public MyICommand<string> PretraziCommand { get; private set; }
        public MyICommand<string> DodajCommand { get; set; }
        public MyICommand<string> IzmeniCommand { get; set; }
        public MyICommand<string> ObrisiCommand { get; set; }
        #endregion
        public ProfaktureViewModel()
        {
            PretraziCommand = new MyICommand<string>(Pretrazi);
            DodajCommand = new MyICommand<string>(Dodaj);
            IzmeniCommand = new MyICommand<string>(Izmeni);
            ObrisiCommand = new MyICommand<string>(Obrisi);
            textSearch = "";
            Profakture = new ObservableCollection<Profaktura>();
            foreach (var item in dbContext.Profakturas)
            {
                profakture.Add(item);
            }
            DefaultView = CollectionViewSource.GetDefaultView(Profakture);
        }

        private void Obrisi(string obj)
        {
            throw new NotImplementedException();
        }

        private void Izmeni(string obj)
        {
            throw new NotImplementedException();
        }

        private void Dodaj(string obj)
        {
            //Profaktura p = new Profaktura();
            //p.active;
            //p.oznaka;
            //p.poslovnipartner_mbr;
            //p.zaposleni_id;
            //f.datum;
            //f.pdv;
            MainWindowViewModel.Instance.DodajProfakturuViewModel = new DodajProfakturuViewModel(0,null);
            MainWindowViewModel.Instance.OnNav(Navigation.dodajProfakturu);
            
        }

        private void Pretrazi(string obj)
        {
            throw new NotImplementedException();
        }
        #region Properties
        public ObservableCollection<Profaktura> Profakture { get => profakture; set => profakture = value; }
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

        public Profaktura SelectedValue
        {
            get { return selectedValue; }
            set
            {
                selectedValue = value;
                OnPropertyChanged("SelectedValue");
            }
        }
        #endregion
    }
}
