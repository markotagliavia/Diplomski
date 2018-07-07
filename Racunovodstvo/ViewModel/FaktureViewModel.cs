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
         * 0 - izlazna, 1 - ulazna
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
                    else if (type.Equals("Poslovnom partneru"))
                    {
                        DefaultView.Filter =
                        w => ((Faktura)w).PoslovniPartner.naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Plaćanju"))
                    {
                        try
                        {
                            Double kolicina = Double.Parse(TextSearch);
                            DefaultView.Filter =
                                w => ((Faktura)w).placeno == kolicina;
                        }
                        catch (Exception ex)
                        {
                            Error e = new Error("Morate uneti cifru.");
                            e.Show();
                        }
                        
                    }
                    else if (type.Equals("Avansu"))
                    {
                        try
                        {
                            Double kolicina = Double.Parse(TextSearch);

                            DefaultView.Filter =
                                w => ((Faktura)w).avans == kolicina;
                        }
                        catch (Exception ex)
                        {
                            Error e = new Error("Morate uneti cifru.");
                            e.Show();
                        }
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

        private void Izbrisi(string obj)
        {
            //TO DO kaskadno brisanje ponuditi 
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                }
            }

            if (context == 0)
            {
                if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.DeleteIzlazna))
                {
                    
                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w.GetType().Equals(typeof(MainWindow)))
                        {
                            UserOnSession.korisnickoime = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime;
                        }
                    }

                    if (SelectedIndex > -1)
                    {
                        
                        if (dbContext.Fakturas.Any(x => x.id == SelectedValue.id))
                        {
                            dbContext.Fakturas.FirstOrDefault(x => x.id == SelectedValue.id).active = false;
                            foreach (var item in dbContext.Fakturas.FirstOrDefault(x => x.id == SelectedValue.id).StavkaFaktures)
                            {
                                Zalihe z = dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == item.zalihe_proizvod_id && x.skladiste_id == item.zalihe_skladiste_id);
                                z.rezervisano -= item.kolicina;
                            }
                            dbContext.SaveChanges();
                            Success suc = new Success("Uspešno ste obrisali fakturu.");
                            suc.Show();

                            SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Uspešno brisanje izlazne fakture {SelectedValue.id}.", "Info");
                            Fakture.Clear();
                            foreach (var item in dbContext.Fakturas)
                            {
                                if (item.redovna == true && item.active == true && item.ulazna == false)
                                {
                                    Fakture.Add(item);
                                }
                            }
                        }
                        else
                        {
                            Error er = new Error("Greška pri pronalaženju proizvođača.\nZa više informacija obratite se administratorima.");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Neuspešno brisanje izlazne fakture {SelectedValue.id}.", "Upozorenje");
                        }
                    }
                }
                else
                {

                    Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                    er.Show();
                    SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Neuspešno brisanje ulazne fakture {SelectedValue.id}.", "Upozorenje");

                }
            }
            else if (context == 1)
            {
                if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.DeleteIzlazna))
                {

                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w.GetType().Equals(typeof(MainWindow)))
                        {
                            UserOnSession.korisnickoime = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime;
                        }
                    }

                    if (SelectedIndex > -1)
                    {

                        if (dbContext.Fakturas.Any(x => x.id == SelectedValue.id))
                        {
                            dbContext.Fakturas.FirstOrDefault(x => x.id == SelectedValue.id).active = false;
                            
                            dbContext.SaveChanges();
                            Success suc = new Success("Uspešno ste obrisali fakturu.");
                            suc.Show();

                            SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Uspešno brisanje ulazne fakture {SelectedValue.id}.", "Info");
                            Fakture.Clear();
                            foreach (var item in dbContext.Fakturas)
                            {
                                if (item.redovna == true && item.active == true && item.ulazna == true)
                                {
                                    Fakture.Add(item);
                                }
                            }
                        }
                        else
                        {
                            Error er = new Error("Greška pri pronalaženju proizvođača.\nZa više informacija obratite se administratorima.");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Neuspešno brisanje ulazne fakture {SelectedValue.id}.", "Upozorenje");
                        }
                    }
                }
                else
                {

                    Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                    er.Show();
                    SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Neuspešno brisanje ulazne fakture {SelectedValue.id}.", "Upozorenje");

                }
            }
            
        }

        private void IzmeniNav(string obj)
        {
            if (SelectedIndex < -1)
            {
                return;
            }
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;

                    if (context == 0)
                    {
                        if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.EditIzlazna))
                        {
                            MainWindowViewModel.Instance.DodajFakturu = new DodajFakturuViewModel(1, SelectedValue);
                            //((MainWindowViewModel)((MainWindow)w).DataContext).DodajFakturu = new DodajFakturuViewModel(1, SelectedValue);
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajFakturu.UserOnSession = this.UserOnSession;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).DodajFakturu;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Izlazna Faktura -> Izmena";
                        }
                        else
                        {
                            Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj izmene izlazne fakture", "Upozorenje");
                        }
                    }
                    else if (context == 1)
                    {
                        if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.EditUlazna))
                        {
                            MainWindowViewModel.Instance.DodajFakturu = new DodajFakturuViewModel(3, SelectedValue);
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajFakturu.UserOnSession = this.UserOnSession;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).DodajFakturu;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Ulazna faktura -> Izmena";
                        }
                        else
                        {
                            Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj izmene ulazne fakture", "Upozorenje");
                        }

                    }


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

                    if (context == 0)
                    {
                        if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.AddIzlazna))
                        {
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajFakturu = new DodajFakturuViewModel(0, null);
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajPoslovnogPartnera.UserOnSession = this.UserOnSession;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).DodajFakturu;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Izlazna faktura -> Nova";
                        }
                        else
                        {
                            Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj kreiranja izlazne fakture", "Upozorenje");
                        }
                    }
                    else if (context == 1)
                    {
                        if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.AddUlazna))
                        {
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajFakturu = new DodajFakturuViewModel(2, null);
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajPoslovnogPartnera.UserOnSession = this.UserOnSession;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).DodajFakturu;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Ulazna faktura -> Nova";
                        }
                        else
                        {
                            Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj kreiranja ulazne fakture", "Upozorenje");
                        }

                    }
                    
                    
                }
            }
        }
        #endregion
    }
}
