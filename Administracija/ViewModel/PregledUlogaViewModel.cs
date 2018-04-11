using Common;
using Common.Model;
using Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Administracija.ViewModel
{
    public class PregledUlogaViewModel : BindableBase
    {
        #region Commands
        private ObservableCollection<Common.Model.Uloga> uloge;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        public MyICommand<string> DodajUloguNavCommand { get; private set; }
        public MyICommand<int> IzmeniUloguNavCommand { get; private set; }
        public MyICommand<int> IzbrisiUloguCommand { get; private set; }


        #endregion

        #region Members
        private int _selectedIndex = -1;
        private bool buttonsEnabled;
        private Korisnik UserOnSession;
        #endregion

        public PregledUlogaViewModel()
        {
            DodajUloguNavCommand = new MyICommand<string>(DodajUloguNav);
            IzmeniUloguNavCommand = new MyICommand<int>(IzmeniUloguNav);
            IzbrisiUloguCommand = new MyICommand<int>(IzbrisiUlogu);
            Uloge = new ObservableCollection<Common.Model.Uloga>();
            foreach (var item in dbContext.Ulogas.ToList())
            {
                Uloge.Add(item);
                
            }

        }

        #region CommandImplementation
        

        private void IzbrisiUlogu(int index)
        {
            if (index != -1)
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;

                    }
                }
                if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.DeleteRoll))
                {
                    try
                    {
                        string naziv = uloge.ElementAt(index).naziv;
                        var itemToRemove = dbContext.Ulogas.FirstOrDefault(x => x.naziv.Equals(naziv));

                        if (itemToRemove != null)
                        {
                            foreach (var zaposleni in dbContext.Zaposlenis)
                            {
                                if (zaposleni.Ulogas.Any(x => x.naziv.Equals(naziv)))
                                {

                                    zaposleni.Ulogas.Remove(itemToRemove);
                                }
                            }
                            dbContext.Ulogas.Remove(itemToRemove);
                            dbContext.SaveChanges();
                        }
                        Notifications.Success s = new Notifications.Success("Uspešno ste obrisali " + itemToRemove.naziv);
                        s.Show();
                        foreach (Window w in Application.Current.Windows)
                        {
                            if (w.GetType().Equals(typeof(MainWindow)))
                            {
                                SecurityManager.AuditManager.AuditToDB(((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime, "Uspesno je obrisana uloga " + itemToRemove.naziv, "Info");

                            }
                        }

                        uloge.Clear();
                        foreach (var item in dbContext.Ulogas.ToList())
                        {
                            Uloge.Add(item);

                        }
                    }
                    catch (Exception ex)
                    {
                        Error er = new Error("Greška sa konekcijom!\nObratite se administratorima.");
                        er.Show();
                    }
                    
                }
                else
                {
                    Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                    er.Show();
                    SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj brisanja uloge", "Upozorenje");
                }


                
              
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }

        private void IzmeniUloguNav(int index)
        {
            if (index != -1)
            {
                foreach (Window w in Application.Current.Windows)
                {
                    
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                        if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.EditRoll))
                        {
                            ((MainWindowViewModel)((MainWindow)w).DataContext).dodajUloguViewModel = new DodajUloguViewModel(1, uloge.ElementAt(index).naziv);
                            ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).dodajUloguViewModel;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Izmeni ulogu";
                        }
                        else
                        {
                            Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj izmene uloge", "Upozorenje");
                        }
                        
                    }
                }
            }
            else
            {

                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }

        private void DodajUloguNav(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                    if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.AddRoll))
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).dodajUloguViewModel = new DodajUloguViewModel(0, null);
                        ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).dodajUloguViewModel;
                        ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Dodaj ulogu";
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj dodavanja uloge", "Upozorenje");
                    }
                }
            }
        }
        #endregion

        #region Properties
        public bool ButtonsEnabled
        {
            get { return buttonsEnabled; }
            set
            {
                buttonsEnabled = value;
                OnPropertyChanged("ButtonsEnabled");
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
                        ButtonsEnabled = true;
                    }
                    return;
                }
                _selectedIndex = value;
                if (_selectedIndex > -1)
                {
                    ButtonsEnabled = true;
                }
            }
        }

        public ObservableCollection<Uloga> Uloge
        {
            get { return uloge; }
            set
            {
                uloge = value;
                OnPropertyChanged("Uloge");
            }
        }
        #endregion
    }
}
