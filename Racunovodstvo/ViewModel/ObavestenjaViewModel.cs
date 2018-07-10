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
    public class ObavestenjaViewModel:BindableBase
    {
        #region Commands
        public MyICommand<string> ObradiObavestenjeCommand { get; private set; }
        public MyICommand<string> IzbrisiObavestenjeCommand { get; private set; }
        public MyICommand<string> PretraziObavestenjaCommand { get; private set; }
        #endregion

        #region Properties
        private Korisnik userOnSession;
        private int _selectedIndex = -1;
        private bool selectedInGrid;
        private string textSearch;
        private ObservableCollection<Notification> obavestenja;
        private Notification selectedValue;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ICollectionView defaultView;
        #endregion

        public ObavestenjaViewModel()
        {
            ObradiObavestenjeCommand = new MyICommand<string>(Obradi);
            IzbrisiObavestenjeCommand = new MyICommand<string>(Izbrisi);
            PretraziObavestenjaCommand = new MyICommand<string>(Pretrazi);
            textSearch = "";
            obavestenja = new ObservableCollection<Notification>();
            foreach (var item in dbContext.Notifications.Where(x => x.adresa.Equals("Racunovodstvo")).ToList())
            {
                obavestenja.Add(item);
            }

            DefaultView = CollectionViewSource.GetDefaultView(Obavestenja);
        }

        #region Constructors

        public ObservableCollection<Notification> Obavestenja
        {
            get { return obavestenja; }
            set { obavestenja = value; }
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

        public Notification SelectedValue
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
        private void Pretrazi(string type)
        {
            if (!type.Equals("/"))
            {
                if (TextSearch != null && !String.IsNullOrWhiteSpace(TextSearch) && (TextSearch != ""))
                {
                    DefaultView = CollectionViewSource.GetDefaultView(DefaultView);
                    if (type.Equals("Tekstu"))
                    {
                        DefaultView.Filter =
                        w => ((Notification)w).tekst.ToUpper().Contains(TextSearch.ToUpper());
                    }

                    DefaultView.Refresh();
                }
                else
                {
                    DefaultView = CollectionViewSource.GetDefaultView(Obavestenja);
                    DefaultView.Filter = null;
                    DefaultView.Refresh();
                }
            }
            else
            {
                DefaultView = CollectionViewSource.GetDefaultView(Obavestenja);
                DefaultView.Filter = null;
                DefaultView.Refresh();
            }
        }

        private void Izbrisi(string obj)
        {
            //TO DO
        }

        private void Obradi(string obj)
        {
            if (SelectedValue != null)
            {
                if (SelectedValue.tekst.StartsWith("Dug na osnovu izlazne fakture"))
                {
                    MainWindowViewModel.Instance.OpomenaViewModel = new OpomenaViewModel(SelectedValue);
                    MainWindowViewModel.Instance.OnNav(Navigation.opomena);
                }
                else if(SelectedValue.tekst.StartsWith("Dug na osnovu ulazne fakture"))
                {
                    Faktura f = dbContext.Fakturas.FirstOrDefault(x => x.id == selectedValue.idDokumenta);
                    Notifications.Info i = new Notifications.Info($"Nije izmiren dug na osnovu fakture {f.oznaka}, rok je bio {f.rokplacanja.Value.ToShortDateString()} i iznosi {MainWindowViewModel.Instance.UkupnaCenaSaPDV(f)} RSD");
                    i.Show();
                }
            }
        }
        #endregion
    }
}
