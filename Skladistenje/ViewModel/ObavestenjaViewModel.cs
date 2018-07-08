using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Skladistenje.ViewModel
{
    public class ObavestenjaViewModel : BindableBase
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
            foreach (var item in dbContext.Notifications.ToList())
            {
                obavestenja.Add(item);
            }

            DefaultView = CollectionViewSource.GetDefaultView(Obavestenja);


            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    if (((MainWindow)w).zvonce != null)
                    {
                        ((MainWindow)w).ZvonceBelo();
                        foreach (var item in dbContext.Notifications)
                        {
                            item.procitana = true;
                        }
                        dbContext.SaveChanges();
                    }
                }
            }
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
                if (SelectedValue.tekst.StartsWith("Kreirana je nova otpremnica"))
                {
                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w.GetType().Equals(typeof(MainWindow)))
                        {
                            ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Skladišni dokumenti -> Nova Interna Prijemnica";
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajGenericSklDokViewModel = new DodajGenericSklDokViewModel("INT_PR",SelectedValue.idDokumenta, SelectedValue);
                            ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).DodajGenericSklDokViewModel;
                            dbContext.Notifications.FirstOrDefault(x => x.Id == SelectedValue.Id).obradjena = true;
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
        }
        #endregion
    }
}
