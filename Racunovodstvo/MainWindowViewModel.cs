using Notifications;
using Common.Model;
using Racunovodstvo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Common;

namespace Racunovodstvo
{
    public class MainWindowViewModel:BindableBase
    {
        #region Members
        public MyICommand<string> NavCommand { get; set; }
        public MyICommand<string> OpenMenuCommand { get; set; }
        public MyICommand<string> CloseMenuCommand { get; set; }
        public MyICommand<string> CloseCommand { get; set; }

        private ProfaktureViewModel profaktureViewModel = new ProfaktureViewModel();
        private FaktureViewModel faktureViewModel = new FaktureViewModel(0);
        private DodajFakturuViewModel dodajFakturu = new DodajFakturuViewModel(0,null);
        private ProizvodiViewModel proizvodiViewModel = new ProizvodiViewModel();
        private KompenzacijeViewModel kompenzacijeViewModel = new KompenzacijeViewModel();
        private PoslovniPartneriViewModel poslovniPartnerViewModel = new PoslovniPartneriViewModel();
        private DodajPoslovnogPartneraViewModel dodajPoslovnogPartnera = new DodajPoslovnogPartneraViewModel(0,null);
        private ZaliheViewModel zaliheViewModel = new ZaliheViewModel();
        private NaprednaPretragaViewModel naprednaPretragaViewModel = new NaprednaPretragaViewModel();
        private ObavestenjaViewModel obavestenjaViewModel = new ObavestenjaViewModel();
        private StatistikaViewModel statistikaViewModel = new StatistikaViewModel();
        private BilansiViewModel bilansiViewModel = new BilansiViewModel();
        private ZaposleniViewModel zaposleniViewModel = new ZaposleniViewModel();
        private HelpViewModel helpViewModel = new HelpViewModel();

        private BindableBase currentViewModel;

        private string _imeUser;
        private string _usernameUser;
        private string _ulogaUser;
        private string _infoUser;
        private string _viewModelTitle = "Profakture";
        private System.Windows.Media.Color c1;
        private System.Windows.Media.Brush _firmColor;
        private System.Windows.Media.Color c2;
        private System.Windows.Media.Brush _backgroundColor;

        private Visibility buttonOpenMenu;
        private Visibility buttonCloseMenu;

        private Korisnik userOnSession = new Korisnik();
        private DeltaEximEntities dbContext = new DeltaEximEntities();
        #endregion

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

        public ProfaktureViewModel ProfaktureViewModel { get => profaktureViewModel; set => profaktureViewModel = value; }
        public FaktureViewModel FaktureViewModel { get => faktureViewModel; set => faktureViewModel = value; }
        public ProizvodiViewModel ProizvodiViewModel { get => proizvodiViewModel; set => proizvodiViewModel = value; }
        public KompenzacijeViewModel KompenzacijeViewModel { get => kompenzacijeViewModel; set => kompenzacijeViewModel = value; }
        public PoslovniPartneriViewModel PoslovniPartnerViewModel { get => poslovniPartnerViewModel; set => poslovniPartnerViewModel = value; }
        public ZaliheViewModel ZaliheViewModel { get => zaliheViewModel; set => zaliheViewModel = value; }
        public NaprednaPretragaViewModel NaprednaPretragaViewModel { get => naprednaPretragaViewModel; set => naprednaPretragaViewModel = value; }
        public ObavestenjaViewModel ObavestenjaViewModel { get => obavestenjaViewModel; set => obavestenjaViewModel = value; }
        public StatistikaViewModel StatistikaViewModel { get => statistikaViewModel; set => statistikaViewModel = value; }
        public BilansiViewModel BilansiViewModel { get => bilansiViewModel; set => bilansiViewModel = value; }
        public HelpViewModel HelpViewModel { get => helpViewModel; set => helpViewModel = value; }
        public ZaposleniViewModel ZaposleniViewModel { get => zaposleniViewModel; set => zaposleniViewModel = value; }
        public DodajFakturuViewModel DodajFakturu { get => dodajFakturu; set => dodajFakturu = value; }
        public DodajPoslovnogPartneraViewModel DodajPoslovnogPartnera { get => dodajPoslovnogPartnera; set => dodajPoslovnogPartnera = value; }

        #endregion Properties

        public MainWindowViewModel()
        {
            c1 = System.Windows.Media.Color.FromArgb(255, 53, 128, 191);
            FirmColor = new SolidColorBrush(c1);
            c2 = System.Windows.Media.Color.FromArgb(255, 204, 179, 255);
            BackgroundColor = new SolidColorBrush(c2);
            NavCommand = new MyICommand<string>(OnNav);
            OpenMenuCommand = new MyICommand<string>(OpenMenu);
            CloseMenuCommand = new MyICommand<string>(CloseMenu);
            CloseCommand = new MyICommand<string>(Close);
            ButtonCloseMenu = Visibility.Collapsed;
            ButtonOpenMenu = Visibility.Visible;
            CurrentViewModel = profaktureViewModel;
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
                case "profakture":
                    ViewModelTitle = "Profakture";
                    CurrentViewModel = profaktureViewModel;
                    break;
                case "izlazna":
                    ViewModelTitle = "Pregled izlaznih faktura";
                    faktureViewModel = new FaktureViewModel(0);
                    CurrentViewModel = faktureViewModel;
                    break;
                case "ulazna":
                    ViewModelTitle = "Pregled ulaznih faktura";
                    faktureViewModel = new FaktureViewModel(1);
                    CurrentViewModel = faktureViewModel;
                    break;
                case "storno":
                    ViewModelTitle = "Pregled storno faktura";
                    faktureViewModel = new FaktureViewModel(2);
                    CurrentViewModel = faktureViewModel;
                    break;
                case "proizvodi":
                    ViewModelTitle = "Proizvodi";
                    CurrentViewModel = proizvodiViewModel;
                    break;
                case "kompenzacije":
                    ViewModelTitle = "Kompenzacije";
                    CurrentViewModel = kompenzacijeViewModel;
                    break;
                case "poslovnipartneri":
                    ViewModelTitle = "Poslovni partneri";
                    CurrentViewModel = poslovniPartnerViewModel;
                    break;
                case "zalihe":
                    ViewModelTitle = "Zalihe";
                    CurrentViewModel = zaliheViewModel;
                    break;
                case "naprednaPretraga":
                    ViewModelTitle = "Napredna pretraga";
                    CurrentViewModel = naprednaPretragaViewModel;
                    break;
                case "obavestenja":
                    ViewModelTitle = "Obaveštenja";
                    CurrentViewModel = obavestenjaViewModel;
                    break;
                case "statistika":
                    ViewModelTitle = "Statistika";
                    CurrentViewModel = statistikaViewModel;
                    break;
                case "bilansi":
                    ViewModelTitle = "Bilansi";
                    CurrentViewModel = bilansiViewModel;
                    break;
                case "zaposleni":
                    ViewModelTitle = "Zaposleni";
                    CurrentViewModel = zaliheViewModel;
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
