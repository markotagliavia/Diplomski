﻿using Administracija.ViewModel;
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
        private DodajKorisnikaViewModel dodajKorisnikaViewModel = new DodajKorisnikaViewModel();
        private IzmeniKorisnikaViewModel izmeniKorisnikaViewModel = new IzmeniKorisnikaViewModel();
        private PregledUlogaViewModel pregledUlogaViewModel = new PregledUlogaViewModel();
        private DodajUloguViewModel dodajUloguViewModel = new DodajUloguViewModel();
        private IzmeniUloguViewModel izmeniUloguViewModel = new IzmeniUloguViewModel();
        private NaprednaPretragaViewModel naprednaPretragaViewModel = new NaprednaPretragaViewModel();
        private HelpViewModel helpViewModel = new HelpViewModel();
        private AuditViewModel auditViewModel = new AuditViewModel();
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
            LoginWindow lw = new LoginWindow();
            lw.Show();
            ((MainWindow)Application.Current.MainWindow).Close();
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
                    ViewModelTitle = "Pregled korisnika";
                    CurrentViewModel = pregledKorisnikaViewModel;                   
                    break;
                case "addUser":
                    ViewModelTitle = "Novi korisnik";
                    CurrentViewModel = dodajKorisnikaViewModel;
                    break;
                case "editUser":
                    ViewModelTitle = "Izmena korisnika";
                    CurrentViewModel = izmeniKorisnikaViewModel;
                    break;
                case "previewRoles":
                    ViewModelTitle = "Pregled uloga";
                    CurrentViewModel = pregledUlogaViewModel;
                    break;
                case "addRole":
                    ViewModelTitle = "Nova uloga";
                    CurrentViewModel = dodajUloguViewModel;
                    break;
                case "editRole":
                    ViewModelTitle = "Izmena uloge";
                    CurrentViewModel = izmeniUloguViewModel;
                    break;
                case "advancedSearch":
                    ViewModelTitle = "Napredna pretraga";
                    CurrentViewModel = naprednaPretragaViewModel;
                    break;
                case "audit":
                    ViewModelTitle = "Pregled akcija";
                    CurrentViewModel = auditViewModel;
                    break;
                case "help":
                    ViewModelTitle = "Pomoć";
                    CurrentViewModel = helpViewModel;
                    break;
                case "info":
                    //TO DO: Tijana ubaci prozor o info
                    break;
            }
        }
        #endregion
    }
}