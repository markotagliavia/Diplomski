using Administracija.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Common;
using Common.Model;

namespace Administracija
{
    public class MainWindowViewModel:BindableBase
    {
        #region Members
        public MyICommand<string> NavCommand { get; set; }
        public MyICommand<string> OpenMenuCommand { get; set; }
        public MyICommand<string> CloseMenuCommand { get; set; }
        public MyICommand<string> CloseCommand { get; set; }
        private PregledKorisnikaViewModel pregledKorisnikaViewModel = new PregledKorisnikaViewModel();
        public DodajKorisnikaViewModel dodajKorisnikaViewModel = new DodajKorisnikaViewModel(0,null);
        
        public PregledUlogaViewModel pregledUlogaViewModel = new PregledUlogaViewModel();
        public DodajUloguViewModel dodajUloguViewModel = new DodajUloguViewModel();
        
        private NaprednaPretragaViewModel naprednaPretragaViewModel = new NaprednaPretragaViewModel();
        private HelpViewModel helpViewModel = new HelpViewModel();
        public AuditViewModel auditViewModel = new AuditViewModel();
        public DodajGradViewModel dodajGradViewModel = new DodajGradViewModel(0,null);
        private GradViewModel gradViewModel = new GradViewModel();
        private BindableBase currentViewModel;

        private string _imeUser;
        private string _usernameUser;
        private string _ulogaUser;
        private string _infoUser;
        private string _viewModelTitle = "Pregled Korisnika";
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

        public DodajKorisnikaViewModel DodajKorisnikaViewModel
        {
            get { return dodajKorisnikaViewModel; }
            set { dodajKorisnikaViewModel = value; }
        }

        public GradViewModel GradViewModel { get => gradViewModel; set => gradViewModel = value; }
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
            CurrentViewModel = pregledKorisnikaViewModel;
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
                    /*
                     ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).ChangeUser(CurrentUser.Username);
                    ((MainWindow)Application.Current.MainWindow).MenuTop.Visibility = Visibility.Visible;
                    ((MainWindow)Application.Current.MainWindow).MenuItemAccountDetails.Command.Execute("previewPictures");
                     */                   
                case "previewUsers":
                    ViewModelTitle = "Pregled Korisnika";
                    pregledKorisnikaViewModel.UserOnSession = userOnSession;
                    CurrentViewModel = pregledKorisnikaViewModel;                   
                    break;
                case "addUser":
                    ViewModelTitle = "Novi Korisnik";
                    CurrentViewModel = dodajKorisnikaViewModel;
                    break;
                case "previewRoles":
                    ViewModelTitle = "Pregled Uloga";
                    CurrentViewModel = pregledUlogaViewModel;
                    break;
                case "addRole":
                    ViewModelTitle = "Nova Uloga";
                    CurrentViewModel = dodajUloguViewModel;
                    break;
                case "advancedSearch":
                    ViewModelTitle = "Napredna Pretraga";
                    CurrentViewModel = naprednaPretragaViewModel;
                    break;
                case "audit":
                    ViewModelTitle = "Pregled Akcija";
                    CurrentViewModel = auditViewModel;
                    break;
                case "help":
                    ViewModelTitle = "Pomoć";
                    CurrentViewModel = helpViewModel;
                    break;
                case "grad":
                    ViewModelTitle = "Pregled Gradova";
                    CurrentViewModel = GradViewModel;
                    break;
                case "info":
                    //TO DO: Tijana ubaci prozor o info
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
