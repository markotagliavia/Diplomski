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
    public class FaktureViewModel:BindableBase
    {
        #region Commands
        public MyICommand<string> DodajFakturuNavCommand { get; private set; }
        public MyICommand<string> IzmeniFakturuNavCommand { get; private set; }
        public MyICommand<string> IzbrisiFakturuCommand { get; private set; }
        public MyICommand<string> PretraziFakturuCommand { get; private set; }
        #endregion

        #region Properties
        private Korisnik userOnSession;
        private int _selectedIndex = -1;
        private bool selectedInGrid;
        private string textSearch;
        private ObservableCollection<Faktura> fakture;
        private Faktura selectedValue;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ICollectionView defaultView;
        private int context;
        #endregion

        /*
         * 0 - izlazna, 1 - ulazna, 2 - storno
         */

        public FaktureViewModel(int i)
        {

            DodajFakturuNavCommand = new MyICommand<string>(DodajNav);
            IzmeniFakturuNavCommand = new MyICommand<string>(IzmeniNav);
            IzbrisiFakturuCommand = new MyICommand<string>(Izbrisi);
            PretraziFakturuCommand = new MyICommand<string>(Pretrazi);
            textSearch = "";
            context = i;
            Fakture = new ObservableCollection<Faktura>();
            if (context == 0)
            {
                foreach (var item in dbContext.Fakturas)
                {
                    if (item.redovna == true && item.active == true && item.ulazna == false)
                    {
                        Fakture.Add(item);
                    }
                }

            }
            else if (context == 1)
            {
                foreach (var item in dbContext.Fakturas)
                {
                    if (item.redovna == true && item.active == true && item.ulazna == true)
                    {
                        Fakture.Add(item);
                    }
                }
            }
            else if (context == 2)
            {
                foreach (var item in dbContext.Fakturas)
                {
                    if (item.redovna == false && item.active == true)
                    {
                        Fakture.Add(item);
                        
                    }
                }

               
            }
            DefaultView = CollectionViewSource.GetDefaultView(Fakture);
        }

        

        #region Constructors

        public ObservableCollection<Faktura> Fakture
        {
            get { return fakture; }
            set { fakture = value; }
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

        public Faktura SelectedValue
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
        private void Pretrazi(string obj)
        {
            throw new NotImplementedException();
        }

        private void Izbrisi(string obj)
        {
            throw new NotImplementedException();
        }

        private void IzmeniNav(string obj)
        {
            throw new NotImplementedException();
        }

        private void DodajNav(string obj)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
