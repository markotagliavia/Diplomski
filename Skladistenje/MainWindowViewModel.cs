using Notifications;
using Skladistenje.Model;
using Skladistenje.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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
        private ZaliheViewModel zaliheViewModel = new ZaliheViewModel();
        private SkladisteniDokumentiViewModel skladisteniDokumentiViewModel = new SkladisteniDokumentiViewModel();
        private ProizvodiViewModel proizvodiViewModel = new ProizvodiViewModel();
        private PopisViewModel popisViewModel = new PopisViewModel();
        private ZaposleniViewModel zaposleniViewModel = new ZaposleniViewModel();
        private ObavestenjaViewModel obavestenjaViewModel = new ObavestenjaViewModel();
        private NaprednaPretragaViewModel naprednapretragaViewModel = new NaprednaPretragaViewModel();
        private HelpViewModel helpViewModel = new HelpViewModel();
        
       
        
        private BindableBase currentViewModel;

        private string _imeUser = "Marko";
        private string _usernameUser = "max151";
        private string _ulogaUser = "Admin";
        private string _infoUser = "Marko je svecki mega car";
        private string _viewModelTitle = "Pregled korisnika";
        private System.Windows.Media.Color c1;
        private System.Windows.Media.Brush _firmColor;
        private System.Windows.Media.Color c2;
        private System.Windows.Media.Brush _backgroundColor;

        private Visibility buttonOpenMenu;
        private Visibility buttonCloseMenu;

        private Korisnik userOnSession = new Korisnik();

        
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
                OnPropertyChanged("InforUser");
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

        private void Close(string obj)
        {
            //dbContext.Korisniks.First(p => p.korisnickoime.Equals(UserOnSession.korisnickoime)).ulogovan = false;
            //dbContext.SaveChanges();
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
                    CurrentViewModel = skladistaViewModel;
                    break;
                case "zalihe":
                    ViewModelTitle = "Zalihe";
                    CurrentViewModel = zaliheViewModel;
                    break;
                case "skladisteniDokumenti":
                    ViewModelTitle = "Skladišteni dokumenti";
                    CurrentViewModel = skladisteniDokumentiViewModel;
                    break;
                case "proizvodi":
                    ViewModelTitle = "Proizvodi";
                    CurrentViewModel = proizvodiViewModel;
                    break;
                case "popis":
                    ViewModelTitle = "Popis";
                    CurrentViewModel = popisViewModel;
                    break;
                case "zaposleni":
                    ViewModelTitle = "Zaposleni";
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
    }
}
