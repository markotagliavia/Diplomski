using Common;
using Common.Model;
using Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Racunovodstvo.ViewModel
{
    public class StornoFaktureViewModel:BindableBase
    {
        #region Commands
        public MyICommand<string> DodajNavCommand { get; private set; }
        public MyICommand<string> PregledNavCommand { get; private set; }
        public MyICommand<string> PretraziCommand { get; private set; }
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


        public StornoFaktureViewModel()
        {
            DodajNavCommand = new MyICommand<string>(DodajNav);
            PregledNavCommand = new MyICommand<string>(PregledNav);
            PretraziCommand = new MyICommand<string>(Pretrazi);
            textSearch = "";
            Fakture = new ObservableCollection<Faktura>();
            foreach (var item in dbContext.Fakturas)
            {
                if (item.redovna == false && item.active == true)
                {
                    Fakture.Add(item);

                }
            }
            DefaultView = CollectionViewSource.GetDefaultView(Fakture);
        }

        private void Pretrazi(string type)
        {
            if (!type.Equals("/"))
            {
                if (TextSearch != null && !String.IsNullOrWhiteSpace(TextSearch) && (TextSearch != ""))
                {
                    DefaultView = CollectionViewSource.GetDefaultView(DefaultView);
                    if (type.Equals("Oznaci"))
                    {
                        DefaultView.Filter =
                        w => ((Faktura)w).oznaka.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Datumu"))
                    {
                        DefaultView.Filter =
                        w => ((Faktura)w).datumfakturisanja.ToString().Contains(TextSearch);
                    }
                    

                    DefaultView.Refresh();
                }
                else
                {
                    DefaultView = CollectionViewSource.GetDefaultView(Fakture);
                    DefaultView.Filter = null;
                    DefaultView.Refresh();
                }
            }
            else
            {
                DefaultView = CollectionViewSource.GetDefaultView(Fakture);
                DefaultView.Filter = null;
                DefaultView.Refresh();
            }
        }

        private void PregledNav(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                    ((MainWindowViewModel)((MainWindow)w).DataContext).DodajStornoViewModel = new DodajStornoViewModel(1, SelectedValue);
                    ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).DodajStornoViewModel;
                    ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Pregled storno fakture";
                }
            }
        }

        private void DodajNav(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                }
            }

            if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id,SecurityManager.Permission.AddStorno))
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajStornoViewModel = new DodajStornoViewModel(0,null);
                        ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).DodajStornoViewModel;
                        ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Nova storno faktura";
                    }
                }
            }
            else
            {

                Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                er.Show();
                SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Neuspešno dodavanje storno fakture.", "Upozorenje");

            }
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
    }
}
