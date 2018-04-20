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

namespace Skladistenje.ViewModel
{

    public class PregledPopisaViewModel : BindableBase
    {
        //komande su : navigacija na dodavanje pregled i pretraga
        #region Commands
        public MyICommand<string> DodajPopisNavCommand { get; private set; }
        public MyICommand<string> PregledPopisaCommand { get; private set; }
        public MyICommand<string> PretraziPopiseCommand { get; private set; }
        #endregion

        #region Properties
        private Korisnik userOnSession;
        private int _selectedIndex = -1;
        private bool selectedInGrid;
        private string textSearch;
        private ObservableCollection<Popi> popisi;
        private Proizvodjac selectedValue;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ICollectionView defaultView;
        #endregion

        public PregledPopisaViewModel()
        {
            DodajPopisNavCommand = new MyICommand<string>(DodajPopisNav);
            PregledPopisaCommand = new MyICommand<string>(PregledPopisa);
            PretraziPopiseCommand = new MyICommand<string>(PretraziPopise);
            textSearch = "";
            popisi = new ObservableCollection<Popi>();
            foreach (var item in dbContext.Popis.ToList())
            {
                popisi.Add(item);
            }

            DefaultView = CollectionViewSource.GetDefaultView(Popisi);
        }

        #region Constructors

        public ObservableCollection<Popi> Popisi
        {
            get { return popisi; }
            set { popisi = value; }
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

        public Proizvodjac SelectedValue
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
        private void PretraziPopise(string type)
        {
            if (!type.Equals("/"))
            {
                if (TextSearch != null && !String.IsNullOrWhiteSpace(TextSearch) && (TextSearch != ""))
                {
                    DefaultView = CollectionViewSource.GetDefaultView(DefaultView);
                    if (type.Equals("Šifri"))
                    {
                        DefaultView.Filter =
                        w => ((Popi)w).oznaka.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Skladištu"))
                    {
                        DefaultView.Filter =
                        w => ((Popi)w).Skladiste.naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Datumu"))
                    {
                        DefaultView.Filter =
                        w => ((Popi)w).datum.ToString().ToUpper().Contains(TextSearch.ToUpper());
                    }

                    DefaultView.Refresh();
                }
                else
                {
                    DefaultView = CollectionViewSource.GetDefaultView(Popisi);
                    DefaultView.Filter = null;
                    DefaultView.Refresh();
                }
            }
            else
            {
                DefaultView = CollectionViewSource.GetDefaultView(Popisi);
                DefaultView.Filter = null;
                DefaultView.Refresh();
            }
        }

        private void PregledPopisa(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                    if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.PreviewPopis))
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajPopisViewModel = new DodajPopisViewModel();
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajPopisViewModel.UserOnSession = this.UserOnSession;
                        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("dodajPopis");
                        ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Novi Popis";
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspešan pregled popisa. Autorizacija.", "Upozorenje");
                    }
                }
            }
        }

        private void DodajPopisNav(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                    if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.AddPopis))
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajPopisViewModel = new DodajPopisViewModel();
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajPopisViewModel.UserOnSession = this.UserOnSession;
                        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("dodajPopis");
                        ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Novi Popis";
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspešno dodavanje novog popisa. Autorizacija.", "Upozorenje");
                    }
                }
            }
        }
        #endregion
    }
}
