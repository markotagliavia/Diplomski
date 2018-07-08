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
    public class PoslovniPartneriViewModel:BindableBase
    {
        #region Commands
        public MyICommand<string> DodajNavCommand { get; private set; }
        public MyICommand<string> IzmeniNavCommand { get; private set; }
        public MyICommand<string> IzbrisiCommand { get; private set; }
        public MyICommand<string> PretraziCommand { get; private set; }
        #endregion

        #region Properties
        private Korisnik userOnSession;
        private int _selectedIndex = -1;
        private bool selectedInGrid;
        private string textSearch;
        private ObservableCollection<PoslovniPartner> poslovniPartneri;
        private PoslovniPartner selectedValue;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ICollectionView defaultView;
        private int context;
        #endregion
        public PoslovniPartneriViewModel()
        {
            DodajNavCommand = new MyICommand<string>(DodajNav);
            IzmeniNavCommand = new MyICommand<string>(IzmeniNav);
            IzbrisiCommand = new MyICommand<string>(Izbrisi);
            PretraziCommand = new MyICommand<string>(Pretrazi);
            textSearch = "";
            PoslovniPartneri = new ObservableCollection<PoslovniPartner>();

            foreach (var item in dbContext.PoslovniPartners)
            {
                PoslovniPartneri.Add(item);
            }
            DefaultView = CollectionViewSource.GetDefaultView(PoslovniPartneri);
        }

        #region Constructors

        public ObservableCollection<PoslovniPartner> PoslovniPartneri
        {
            get { return poslovniPartneri; }
            set { poslovniPartneri = value; }
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

        public PoslovniPartner SelectedValue
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
                    if (type.Equals("PIB-u"))
                    {
                        DefaultView.Filter =
                        w => ((PoslovniPartner)w).pib.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Nazivu"))
                    {
                        DefaultView.Filter =
                        w => ((PoslovniPartner)w).naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Gradu"))
                    {
                        DefaultView.Filter =
                        w => ((PoslovniPartner)w).grad.naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Adresi"))
                    {
                        DefaultView.Filter =
                        w => ((PoslovniPartner)w).adresa.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Dugovanjima"))
                    {
                        DefaultView.Filter =
                        w => ((PoslovniPartner)w).dugovanja.ToString().ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Emailu"))
                    {
                        DefaultView.Filter =
                        w => ((PoslovniPartner)w).email.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Tekućem računu"))
                    {
                        DefaultView.Filter =
                        w => ((PoslovniPartner)w).tekuciracun.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Broju telefona"))
                    {
                        DefaultView.Filter =
                        w => ((PoslovniPartner)w).brojtelefona.ToUpper().Contains(TextSearch.ToUpper());
                    }

                    DefaultView.Refresh();
                }
                else
                {
                    DefaultView = CollectionViewSource.GetDefaultView(PoslovniPartneri);
                    DefaultView.Filter = null;
                    DefaultView.Refresh();
                }
            }
            else
            {
                DefaultView = CollectionViewSource.GetDefaultView(PoslovniPartneri);
                DefaultView.Filter = null;
                DefaultView.Refresh();
            }
        }

        private void Izbrisi(string obj)
        {
            //TO DO kaskadno brisanje ponuditi autorizacija
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                }
            }

            //if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.DeleteProizvodjac))
            //{
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
                    if (dbContext.PoslovniPartners.Any(x => x.mbr == SelectedValue.mbr))
                    {
                        dbContext.PoslovniPartners.Remove(dbContext.PoslovniPartners.FirstOrDefault(x => x.mbr == SelectedValue.mbr));
                        dbContext.SaveChanges();
                        Success suc = new Success("Uspešno ste obrisali poslovnog partnera.");
                        suc.Show();

                        SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Uspešno brisanje poslovnog partnera {nazivBrisanog}.", "Info");
                        PoslovniPartneri.Clear();
                        foreach (var item in dbContext.PoslovniPartners)
                        {
                            PoslovniPartneri.Add(item);
                        }
                    }
                    else
                    {
                        Error er = new Error("Greška pri pronalaženju proizvođača.\nZa više informacija obratite se administratorima.");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Neuspešno brisanje poslovnog partnera {nazivBrisanog}.", "Upozorenje");
                    }
                }
            //}
            //else
            //{

            //    Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
            //    er.Show();
            //    SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspešno brisnje proizvođača. Neuspešna autorizacija.", "Upozorenje");

            //}
        }

        private void IzmeniNav(string obj)
        {
            if (SelectedValue != null)
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                        //if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.EditProizvod))
                        //{
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajPoslovnogPartnera = new DodajPoslovnogPartneraViewModel(1, selectedValue);
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajPoslovnogPartnera.UserOnSession = this.UserOnSession;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).DodajPoslovnogPartnera;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Poslovni Partneri -> Izmena";
                        //}
                        //else
                        //{
                        //    Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        //    er.Show();
                        //    SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspšan pokušaj izmene proizvoda.", "Upozorenje");
                        //}

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
        private void DodajNav(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                    //if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.AddProizvod))
                    //{
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajPoslovnogPartnera = new DodajPoslovnogPartneraViewModel(0, null);
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajPoslovnogPartnera.UserOnSession = this.UserOnSession;
                        ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).DodajPoslovnogPartnera;
                    ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Poslovni Partneri -> Novi";
                }
            }
        }
        #endregion
    }
}
