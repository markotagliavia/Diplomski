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
        private void PretraziSkladiste(string type)
        {
            if (!type.Equals("/"))
            {
                if (TextSearch != null && !String.IsNullOrWhiteSpace(TextSearch) && (TextSearch != ""))
                {
                    DefaultView = CollectionViewSource.GetDefaultView(DefaultView);
                    if (type.Equals("Šifri"))
                    {
                        DefaultView.Filter =
                        w => ((Skladiste)w).sifra.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Nazivu"))
                    {
                        DefaultView.Filter =
                        w => ((Skladiste)w).naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Gradu"))
                    {
                        DefaultView.Filter =
                        w => ((Skladiste)w).grad.naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Adresi"))
                    {
                        DefaultView.Filter =
                        w => ((Skladiste)w).adresa.ToUpper().Contains(TextSearch.ToUpper());
                    }

                    DefaultView.Refresh();
                }
                else
                {
                    DefaultView = CollectionViewSource.GetDefaultView(Skladista);
                    DefaultView.Filter = null;
                    DefaultView.Refresh();
                }
            }
            else
            {
                DefaultView = CollectionViewSource.GetDefaultView(Skladista);
                DefaultView.Filter = null;
                DefaultView.Refresh();
            }
        }

        private void IzbrisiSkladiste(string obj)
        {
            /*TO DO 
             * foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;

                }
            }

            if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.DeleteUser))
            {
                string korisnickoImeBrisanog = "";
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        UserOnSession.korisnickoime = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime;
                    }
                }

                if (SelectedIndex > -1)
                {
                    korisnickoImeBrisanog = SelectedValue.KorisnickoIme;
                    if (dbContext.Korisniks.Any(x => x.active == true && x.korisnickoime.Equals(korisnickoImeBrisanog)))
                    {
                        dbContext.Korisniks.Remove(dbContext.Korisniks.First(x => x.active == true && x.korisnickoime.Equals(korisnickoImeBrisanog)));
                        dbContext.SaveChanges();
                        Success suc = new Success("Uspešno ste obrisali korisnika.");
                        suc.Show();

                        SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Uspešno brisanje korisnika {korisnickoImeBrisanog}.", "Info");
                    }
                    else
                    {
                        Error er = new Error("Greška pri pronalaženju korisnika.\nZa više informacija obratite se administratorima.");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Neuspešno brisanje korisnika {korisnickoImeBrisanog}.", "Upozorenje");
                    }
                }



            }
            else
            {

                Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                er.Show();
                SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Pokusaj brisnja korisnika", "Upozorenje");

            }*/
        }

        private void IzmeniSkladisteNav(string obj)
        {
            if (SelectedValue != null)
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                        if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.EditSkladiste))
                        {
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajSkladisteViewModel = new DodajSkladisteViewModel(1, selectedValue);
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajSkladisteViewModel.UserOnSession = this.UserOnSession;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("dodajSkladiste");
                        }
                        else
                        {
                            Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspšan pokušaj izmene skladišta.", "Upozorenje");
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

        private void DodajSkladisteNav(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                    if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.AddSkladiste))
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajSkladisteViewModel = new DodajSkladisteViewModel(0, null);
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajSkladisteViewModel.UserOnSession = this.UserOnSession;
                        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("dodajSkladiste");
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspešan pokušaj dodavanja novog skladišta", "Upozorenje");
                    }
                }
            }
        }

        #endregion
    }
}
