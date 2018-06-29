using Notifications;
using Common.Model;
using Skladistenje.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Common;

namespace Skladistenje
{
    public class MainWindowViewModel : BindableBase
    {
        #region Members
        public MyICommand<string> NavCommand { get; set; }
        public MyICommand<string> OpenMenuCommand { get; set; }
        public MyICommand<string> CloseMenuCommand { get; set; }
        public MyICommand<string> CloseCommand { get; set; }

        private SkladistaViewModel skladistaViewModel = new SkladistaViewModel();
        private PregledProizvodjacaViewModel pregledProizvodjacaViewModel = new PregledProizvodjacaViewModel();
        public DodajSkladisteViewModel dodajSkladisteViewModel = new DodajSkladisteViewModel(0, null);
        public DodajProizvodViewModel dodajProizvodViewModel = new DodajProizvodViewModel(0, null);
        public DodajProizvodjacaViewModel dodajProizvodjacaViewModel = new DodajProizvodjacaViewModel(0, null);
        private ZaliheViewModel zaliheViewModel = new ZaliheViewModel(null);
        private SkladisteniDokumentiViewModel skladisteniDokumentiViewModel = new SkladisteniDokumentiViewModel(1);
        private ProizvodiViewModel proizvodiViewModel = new ProizvodiViewModel();
        private DodajPopisViewModel dodajPopisViewModel = new DodajPopisViewModel(null);
        private PregledPopisaViewModel pregledPopisaViewModel = new PregledPopisaViewModel();
        private ZaposleniViewModel zaposleniViewModel = new ZaposleniViewModel();
        private ObavestenjaViewModel obavestenjaViewModel = new ObavestenjaViewModel();
        private NaprednaPretragaViewModel naprednapretragaViewModel = new NaprednaPretragaViewModel();
        private HelpViewModel helpViewModel = new HelpViewModel();
            
        private BindableBase currentViewModel;

        private string _imeUser;
        private string _usernameUser;
        private string _ulogaUser;
        private string _infoUser;
        private string _viewModelTitle = "Skladišta";
        private System.Windows.Media.Color c1;
        private System.Windows.Media.Brush _firmColor;
        private System.Windows.Media.Color c2;
        private System.Windows.Media.Brush _backgroundColor;

        private Visibility buttonOpenMenu;
        private Visibility buttonCloseMenu;

        private Korisnik userOnSession = new Korisnik();
        private DeltaEximEntities dbContext = new DeltaEximEntities();
        #endregion Members

        #region Properties
        public string ImeUser
        {
            get { return _imeUser; }
            set
            {
                _imeUser = value;
                OnPropertyChanged("ImeUser");
            }
        }

        public string UsernameUser
        {
            get { return _usernameUser; }
            set
            {
                _usernameUser = value;
                OnPropertyChanged("UsernameUser");
            }
        }

        public string UlogaUser
        {
            get { return _ulogaUser; }
            set
            {
                _ulogaUser = value;
                OnPropertyChanged("UlogaUser");
            }
        }

        public string InfoUser
        {
            get { return _infoUser; }
            set
            {
                _infoUser = value;
                OnPropertyChanged("InfoUser");
            }
        }

        public string ViewModelTitle
        {
            get { return _viewModelTitle; }
            set
            {
                _viewModelTitle = value;
                OnPropertyChanged("ViewModelTitle");
            }
        }

        public Brush FirmColor
        {
            get { return _firmColor; }
            set
            {
                _firmColor = value;
                OnPropertyChanged("FirmColor");
            }
        }

        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            }
        }

        public Visibility ButtonOpenMenu
        {
            get { return buttonOpenMenu; }
            set
            {
                buttonOpenMenu = value;
                OnPropertyChanged("ButtonOpenMenu");
            }
        }

        public Visibility ButtonCloseMenu
        {
            get { return buttonCloseMenu; }
            set
            {
                buttonCloseMenu = value;
                OnPropertyChanged("ButtonCloseMenu");
            }
        }

        public Korisnik UserOnSession
        {
            get { return userOnSession; }
            set
            {
                userOnSession = value;
                OnPropertyChanged("UserOnSession");
            }
        }

        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }

        public DodajSkladisteViewModel DodajSkladisteViewModel
        {
            get { return dodajSkladisteViewModel; }
            set { dodajSkladisteViewModel = value; }
        }

        public DodajProizvodViewModel DodajProizvodViewModel
        {
            get { return dodajProizvodViewModel; }
            set { dodajProizvodViewModel = value; }
        }

        public DodajProizvodjacaViewModel DodajProizvodjacaViewModel
        {
            get { return dodajProizvodjacaViewModel; }
            set { dodajProizvodjacaViewModel = value; }
        }

        public DodajPopisViewModel DodajPopisViewModel
        {
            get { return dodajPopisViewModel; }
            set { dodajPopisViewModel = value; }
        }

        public ZaliheViewModel ZaliheViewModel { get => zaliheViewModel; set => zaliheViewModel = value; }
        public SkladisteniDokumentiViewModel SkladisteniDokumentiViewModel { get => skladisteniDokumentiViewModel; set => skladisteniDokumentiViewModel = value; }

        #endregion Properties

        public MainWindowViewModel()
        {
            c1 = System.Windows.Media.Color.FromArgb(255, 53, 128, 191);
            FirmColor = new SolidColorBrush(c1);
            c2 = System.Windows.Media.Color.FromArgb(255, 37, 44, 50);
            BackgroundColor = new SolidColorBrush(c2);
            NavCommand = new MyICommand<string>(OnNav);
            OpenMenuCommand = new MyICommand<string>(OpenMenu);
            CloseMenuCommand = new MyICommand<string>(CloseMenu);
            CloseCommand = new MyICommand<string>(Close);
            ButtonCloseMenu = Visibility.Collapsed;
            ButtonOpenMenu = Visibility.Visible;
            CurrentViewModel = skladistaViewModel;
        }

        #region CommandsImplementation
        private void CloseMenu(string obj)
        {
            ButtonOpenMenu = Visibility.Visible;
            ButtonCloseMenu = Visibility.Collapsed;
        }

        private void OpenMenu(string obj)
        {
            ButtonOpenMenu = Visibility.Collapsed;
            ButtonCloseMenu = Visibility.Visible;
        }

        public void Close(string obj)
        {
            dbContext.Korisniks.First(p => p.korisnickoime.Equals(UserOnSession.korisnickoime)).ulogovan = false;
            dbContext.SaveChanges();
            LoginWindow lw = new LoginWindow();
            lw.Show();
            Application.Current.Shutdown();
        }

        public void OnNav(string destination)
        {
            switch (destination)
            {
                
                case "skladista":
                    ViewModelTitle = "Skladišta";
                    skladistaViewModel = new SkladistaViewModel();
                    CurrentViewModel = skladistaViewModel;
                    break;
                case "dodajSkladiste":
                    ViewModelTitle = "Novo Skladište";
                    CurrentViewModel = dodajSkladisteViewModel;
                    break;
                case "zalihe":
                    ViewModelTitle = "Zalihe";
                    ZaliheViewModel = new ZaliheViewModel(UserOnSession);
                    CurrentViewModel = ZaliheViewModel;
                    break;
                case "interni":
                    ViewModelTitle = "Interni Skladišteni Dokumenti";
                    CurrentViewModel = new SkladisteniDokumentiViewModel(1);
                    break;
                case "spoljni":
                    ViewModelTitle = "Spoljni Skladišteni Dokumenti";
                    CurrentViewModel = new SkladisteniDokumentiViewModel(2);
                    break;
                case "korekcioni":
                    ViewModelTitle = "Korekcioni Skladišteni Dokumenti";
                    CurrentViewModel = new SkladisteniDokumentiViewModel(3);
                    break;
                case "storni":
                    ViewModelTitle = "Storni Skladišteni Dokumenti";
                    CurrentViewModel = new SkladisteniDokumentiViewModel(4);
                    break;
                case "proizvodi":
                    ViewModelTitle = "Pregled Proizvoda";
                    proizvodiViewModel = new ProizvodiViewModel();
                    CurrentViewModel = proizvodiViewModel;
                    break;
                case "dodajProizvod":
                    ViewModelTitle = "Novi Proizvod";
                    CurrentViewModel = dodajProizvodViewModel;
                    break;
                case "proizvodjaci":
                    ViewModelTitle = "Pregled Proizvođača";
                    pregledProizvodjacaViewModel = new PregledProizvodjacaViewModel();
                    CurrentViewModel = pregledProizvodjacaViewModel;
                    break;
                case "dodajProizvodjaca":
                    ViewModelTitle = "Novi Proizvođač";
                    CurrentViewModel = dodajProizvodjacaViewModel;
                    break;
                case "popisi":
                    ViewModelTitle = "Pregled Popisa";
                    CurrentViewModel = new PregledPopisaViewModel();
                    break;
                case "dodajPopis":
                    ViewModelTitle = "Novi Popisa";
                    CurrentViewModel = new DodajPopisViewModel(null);
                    break;
                case "zaposleni":
                    ViewModelTitle = "Zaposleni";
                    zaposleniViewModel = new ZaposleniViewModel();
                    CurrentViewModel = zaposleniViewModel;
                    break;
                case "obavestenja":
                    ViewModelTitle = "Obaveštenja";
                    CurrentViewModel = obavestenjaViewModel;
                    break;
                case "naprednaPretraga":
                    ViewModelTitle = "Napredna pretraga";
                    CurrentViewModel = naprednapretragaViewModel;
                    break;
                case "help":
                    ViewModelTitle = "Pomoć";
                    CurrentViewModel = helpViewModel;
                    break;
                case "info":
                    Info i = new Info("Vlasnici ovog softvera su \n Marko Tagliavia i Tijana Lalošević");
                    i.Show();
                    break;
            }
        }
        #endregion

        #region HelperMethods
        public void setUserInformations()
        {
            UsernameUser = userOnSession.korisnickoime;
            try
            {
                Zaposleni z = dbContext.Zaposlenis.First(x => x.active == true && x.id == userOnSession.zaposleni_id);
                if (z.Ulogas.Count > 0)
                {
                    ImeUser = z.ime;
                    Uloga u = z.Ulogas.ElementAt(0);
                    UlogaUser = u.naziv;
                    InfoUser = $"Ime : {z.ime}{Environment.NewLine}Prezime : {z.prezime}{Environment.NewLine}JMBG : {z.jmbg}{Environment.NewLine}" +
                        $"Adresa : {z.adresa}{Environment.NewLine}Grad : {z.grad.naziv}{Environment.NewLine}E-mail : {z.email}";
                }
            }
            catch (Exception ex)
            {
                Notifications.Error e = new Notifications.Error("Problemi sa konekcijom!");
                e.Show();
            }
        }
        #endregion
    }
}
