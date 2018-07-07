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
    public class PregledProizvodjacaViewModel : BindableBase
    {
        //komande su : navigacija na dodavanje i izmenu i komande brisanja i pretrage
        #region Commands
        public MyICommand<string> DodajProizvodjacaNavCommand { get; private set; }
        public MyICommand<string> IzmeniProizvodjacaNavCommand { get; private set; }
        public MyICommand<string> IzbrisiProizvodjacaCommand { get; private set; }
        public MyICommand<string> PretraziProizvodjaceCommand { get; private set; }
        #endregion

        #region Properties
        private Korisnik userOnSession;
        private int _selectedIndex = -1;
        private bool selectedInGrid;
        private string textSearch;
        private ObservableCollection<Proizvodjac> proizvodjaci;
        private Proizvodjac selectedValue;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ICollectionView defaultView;
        #endregion

        public PregledProizvodjacaViewModel()
        {
            DodajProizvodjacaNavCommand = new MyICommand<string>(DodajProizvodjacaNav);
            IzmeniProizvodjacaNavCommand = new MyICommand<string>(IzmeniProizvodjacaNav);
            IzbrisiProizvodjacaCommand = new MyICommand<string>(IzbrisiProizvodjaca);
            PretraziProizvodjaceCommand = new MyICommand<string>(PretraziProizvodjaca);
            textSearch = "";
            proizvodjaci = new ObservableCollection<Proizvodjac>();
            foreach (var item in dbContext.Proizvodjacs.ToList())
            {
                proizvodjaci.Add(item);
            }

            DefaultView = CollectionViewSource.GetDefaultView(Proizvodjaci);
        }



        #region Constructors

        public ObservableCollection<Proizvodjac> Proizvodjaci
        {
            get { return proizvodjaci; }
            set { proizvodjaci = value; }
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

        #region COmmandsImplementation
        private void PretraziProizvodjaca(string type)
        {
            if (!type.Equals("/"))
            {
                if (TextSearch != null && !String.IsNullOrWhiteSpace(TextSearch) && (TextSearch != ""))
                {
                    DefaultView = CollectionViewSource.GetDefaultView(DefaultView);
                    if (type.Equals("Gradu"))
                    {
                        DefaultView.Filter =
                        w => ((Proizvodjac)w).grad.naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Nazivu"))
                    {
                        DefaultView.Filter =
                        w => ((Proizvodjac)w).naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }

                    DefaultView.Refresh();
                }
                else
                {
                    DefaultView = CollectionViewSource.GetDefaultView(Proizvodjaci);
                    DefaultView.Filter = null;
                    DefaultView.Refresh();
                }
            }
            else
            {
                DefaultView = CollectionViewSource.GetDefaultView(Proizvodjaci);
                DefaultView.Filter = null;
                DefaultView.Refresh();
            }
        }

        private void IzbrisiProizvodjaca(string obj)
        {
            //TO DO kaskadno brisanje ponuditi
             foreach (Window w in Application.Current.Windows)
             {
                 if (w.GetType().Equals(typeof(MainWindow)))
                 {
                     UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                 }
             }

             if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.DeleteProizvodjac))
             {
                 string nazivBrisanog = "";
                 foreach (Window w in Application.Current.Windows)
                 {
                     if (w.GetType().Equals(typeof(MainWindow)))
                     {
                         UserOnSession.korisnickoime = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime;
                     }
                 }

                 if (SelectedIndex > -1)
                 {
                     nazivBrisanog = SelectedValue.naziv;
                     if (dbContext.Proizvodjacs.Any(x => x.naziv.Equals(nazivBrisanog)))
                     {
                         dbContext.Proizvodjacs.Remove(dbContext.Proizvodjacs.First(x => x.naziv.Equals(nazivBrisanog)));
                         dbContext.SaveChanges();
                         Success suc = new Success("Uspešno ste obrisali proizvođača.");
                         suc.Show();

                         SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Uspešno brisanje proizvođača {nazivBrisanog}.", "Info");
                        Proizvodjaci.Clear();
                        foreach(var item in dbContext.Proizvodjacs)
                        {
                            Proizvodjaci.Add(item);
                        }
                    }
                     else
                     {
                         Error er = new Error("Greška pri pronalaženju proizvođača.\nZa više informacija obratite se administratorima.");
                         er.Show();
                         SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Neuspešno brisanje korisnika {nazivBrisanog}.", "Upozorenje");
                     }
                 }
             }
             else
             {

                 Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                 er.Show();
                 SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspešno brisnje proizvođača. Neuspešna autorizacija.", "Upozorenje");

             }

        }

        private void IzmeniProizvodjacaNav(string obj)
        {
            if (SelectedValue != null)
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                        if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.EditProizvodjac))
                        {
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajProizvodjacaViewModel = new DodajProizvodjacaViewModel(1, selectedValue);
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajProizvodjacaViewModel.UserOnSession = this.UserOnSession;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("dodajProizvodjaca");
                            ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Proizvođači -> Izmena";
                        }
                        else
                        {
                            Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspšna izmena proizvođača. Neuspešna autorizacija.", "Upozorenje");
                        }

                    }
                }
            }
            else
            {
                Error er = new Error("Greška pri selekciji.\nZa više informacija obratite se administratorima.");
                er.Show();
                //SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Neuspesna izmena korisnika.", "Upozorenje");
            }
        }

        private void DodajProizvodjacaNav(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                    if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.AddProizvodjac))
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajProizvodjacaViewModel = new DodajProizvodjacaViewModel(0, null);
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajProizvodjacaViewModel.UserOnSession = this.UserOnSession;
                        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("dodajProizvodjaca");
                        ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Proizvođač -> Novi";
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspešno dodavanje novog proizvođača. Autorizacija.", "Upozorenje");
                    }
                }
            }
        }
        #endregion

    }
}
