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
        public MyICommand<Navigation> NavCommand { get; set; }
        public MyICommand<string> OpenMenuCommand { get; set; }
        public MyICommand<string> CloseMenuCommand { get; set; }
        public MyICommand<string> CloseCommand { get; set; }

        private ProfaktureViewModel profaktureViewModel = new ProfaktureViewModel();
        private DodajProfakturuViewModel dodajProfakturuViewModel = new DodajProfakturuViewModel(0,null);
        private FaktureViewModel faktureViewModel = new FaktureViewModel(0);
        private DodajFakturuViewModel dodajFakturu = new DodajFakturuViewModel(0,null);
        private StornoFaktureViewModel stornoFaktureViewModel = new StornoFaktureViewModel();
        private DodajStornoViewModel dodajStornoViewModel = new DodajStornoViewModel(0,null);
        private ProizvodiViewModel proizvodiViewModel = new ProizvodiViewModel();
        private DodajProizvodViewModel dodajProizvodViewModel = new DodajProizvodViewModel(0, null);
        private DodajProizvodjacaViewModel dodajProizvodjacaViewModel = new DodajProizvodjacaViewModel(0,null,null);
        private DodajZalihuViewModel dodajZalihuViewModel = new DodajZalihuViewModel();
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
        private OpomenaViewModel opomenaViewModel = new OpomenaViewModel(null);

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
        private System.Windows.Media.Brush bellColor;
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
        public DodajProizvodViewModel DodajProizvodViewModel { get => dodajProizvodViewModel; set => dodajProizvodViewModel = value; }
        public DodajProizvodjacaViewModel DodajProizvodjacaViewModel { get => dodajProizvodjacaViewModel; set => dodajProizvodjacaViewModel = value; }
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
        public StornoFaktureViewModel StornoFaktureViewModel { get => stornoFaktureViewModel; set => stornoFaktureViewModel = value; }
        public DodajStornoViewModel DodajStornoViewModel { get => dodajStornoViewModel; set => dodajStornoViewModel = value; }

        #endregion Properties


        private static MainWindowViewModel instance;
        public static MainWindowViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainWindowViewModel();
                }
                return instance;
            }
        }

        public DodajZalihuViewModel DodajZalihuViewModel { get => dodajZalihuViewModel; set => dodajZalihuViewModel = value; }
        public DodajProfakturuViewModel DodajProfakturuViewModel { get => dodajProfakturuViewModel; set => dodajProfakturuViewModel = value; }
        public Brush BellColor { get => bellColor; set { bellColor = value; OnPropertyChanged("BellColor"); } }

        public OpomenaViewModel OpomenaViewModel { get => opomenaViewModel; set => opomenaViewModel = value; }

        private MainWindowViewModel()
        {
            c1 = System.Windows.Media.Color.FromArgb(255, 53, 128, 191);
            FirmColor = new SolidColorBrush(c1);
            c2 = System.Windows.Media.Color.FromArgb(255, 37, 44, 50);
            BackgroundColor = new SolidColorBrush(c2);
            ZvonceBelo();
            NavCommand = new MyICommand<Navigation>(OnNav);
            OpenMenuCommand = new MyICommand<string>(OpenMenu);
            CloseMenuCommand = new MyICommand<string>(CloseMenu);
            CloseCommand = new MyICommand<string>(Close);
            ButtonCloseMenu = Visibility.Collapsed;
            ButtonOpenMenu = Visibility.Visible;
            OnNav(Navigation.profakture);
            dugovanja();
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

        public void OnNav(Navigation destination)
        {
            switch (destination)
            {
                case Navigation.profakture:
                    ViewModelTitle = "Profakture";
                    CurrentViewModel = new ProfaktureViewModel();
                    proveriNotifikacije();
                    break;
                case Navigation.dodajProfakturu:
                    ViewModelTitle = " Profakture -> Nova";
                    CurrentViewModel = dodajProfakturuViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.izmeniProfakturu:
                    ViewModelTitle = "Profakture -> Izmena";
                    CurrentViewModel = dodajProfakturuViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.izlazna:
                    ViewModelTitle = "Fakture -> Izlazne";
                    faktureViewModel = new FaktureViewModel(0);
                    CurrentViewModel = faktureViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.ulazna:
                    ViewModelTitle = "Fakture -> Ulazne";
                    faktureViewModel = new FaktureViewModel(1);
                    CurrentViewModel = faktureViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.storno:
                    ViewModelTitle = "Fakture -> Storne";
                    stornoFaktureViewModel = new StornoFaktureViewModel();
                    CurrentViewModel = stornoFaktureViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.proizvodi:
                    ViewModelTitle = "Proizvodi";
                    CurrentViewModel = proizvodiViewModel;
                    break;
                case Navigation.dodajProizvod:
                    ViewModelTitle = "Proizvodi -> Novi";
                    CurrentViewModel = dodajProizvodViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.dodajProizvodjaca:
                    ViewModelTitle = "Proizvođači -> Novi";
                    CurrentViewModel = dodajProizvodjacaViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.kompenzacije:
                    ViewModelTitle = "Kompenzacije";
                    CurrentViewModel = kompenzacijeViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.poslovniPartneri:
                    ViewModelTitle = "Poslovni partneri";
                    CurrentViewModel = poslovniPartnerViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.zalihe:
                    ViewModelTitle = "Zalihe";
                    CurrentViewModel = zaliheViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.naprednaPretraga:
                    ViewModelTitle = "Napredna pretraga";
                    CurrentViewModel = naprednaPretragaViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.obavestenja:
                    ViewModelTitle = "Obaveštenja";
                    CurrentViewModel = obavestenjaViewModel;
                    procitaj();
                    break;
                case Navigation.statistika:
                    ViewModelTitle = "Statistika";
                    CurrentViewModel = statistikaViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.bilansi:
                    ViewModelTitle = "Bilansi";
                    CurrentViewModel = bilansiViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.zaposleni:
                    ViewModelTitle = "Zaposleni";
                    CurrentViewModel = zaposleniViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.dodajZalihe:
                    ViewModelTitle = "Dodaj Zalihu";
                    CurrentViewModel = dodajZalihuViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.help:
                    ViewModelTitle = "Pomoć";
                    CurrentViewModel = helpViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.opomena:
                    ViewModelTitle = "Opomena";
                    CurrentViewModel = opomenaViewModel;
                    proveriNotifikacije();
                    break;
                case Navigation.info:
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

        public double UkupnaCenaSaPDV(Faktura f)
        {
            double SaPDV = 0;
            double BezPDV = 0;

            foreach (var item in f.StavkaFaktures.Where(x => x.storno == false))
            {
                BezPDV += ((double)item.cena * item.kolicina) * (1 - (double)item.rabat / 100);
            }



            SaPDV = (1 + ((double)f.pdv / 100)) * BezPDV;

            return SaPDV;
        }

        public void ZvonceBelo()
        {
            BellColor = Brushes.White; 
        }

        public void ZvonceCrveno()
        {
            BellColor = Brushes.Red;
        }

        public void proveriNotifikacije()
        {
            if (dbContext.Notifications.Any(x => x.procitana == false && x.adresa.Equals("Racunovodstvo")))
            {
                ZvonceCrveno();
            }
            else
            {
                ZvonceBelo();
            }
            
        }


        public void procitaj()
        {
            foreach (var item in dbContext.Notifications.Where(x => x.procitana == false && x.adresa.Equals("Racunovodstvo")))
            {
                item.procitana = true;
            }

            ZvonceBelo();
            dbContext.SaveChanges();
        }

        public void dugovanja()
        {
            Dictionary<int, double> poslovniPartnerDugovanja = new Dictionary<int, double>();
            foreach (var item in dbContext.PoslovniPartners)
            {
                poslovniPartnerDugovanja.Add(item.mbr, 0);
            }
            List<Faktura> fakturas = dbContext.Fakturas.ToList();
            foreach (var izlazna in fakturas.Where(x => x.redovna == true && x.ulazna == false))
            {
                Double suma = UkupnaCenaSaPDV(izlazna);
                if (suma > izlazna.placeno)
                {
                    poslovniPartnerDugovanja[izlazna.PoslovniPartner.mbr] += suma;

                    if (izlazna.rokplacanja < DateTime.Now && !dbContext.Notifications.Any(x => x.tekst.Equals("Dug na osnovu izlazne fakture "+izlazna.oznaka.ToString())))
                    {
                        Common.Model.Notification n = new Common.Model.Notification();
                        n.aplikacija = "Racunovodstvo";
                        n.adresa = "Racunovodstvo";
                        n.obradjena = false;
                        n.procitana = false;
                        n.idDokumenta = izlazna.id;
                        n.tekst = $"Dug na osnovu izlazne fakture {izlazna.oznaka}";
                        dbContext.Notifications.Add(n);
                        dbContext.SaveChanges();
                    }
                    
                    

                }

            }

            foreach (var dug in poslovniPartnerDugovanja)
            {
                dbContext.PoslovniPartners.FirstOrDefault(x => x.mbr == dug.Key).dugovanja = dug.Value;
                dbContext.SaveChanges();
            }

            foreach (var ulazna in fakturas.Where(x => x.redovna == true && x.ulazna == true))
            {
                if (UkupnaCenaSaPDV(ulazna) > ulazna.placeno)
                {
                    if (ulazna.rokplacanja < DateTime.Now && !dbContext.Notifications.Any(x => x.tekst.Equals($"Dug na osnovu ulazne fakture "+ ulazna.oznaka.ToString())))
                    {
                        Common.Model.Notification n = new Common.Model.Notification();
                        n.aplikacija = "Racunovodstvo";
                        n.adresa = "Racunovodstvo";
                        n.obradjena = false;
                        n.procitana = false;
                        n.idDokumenta = ulazna.id;
                        n.tekst = $"Dug na osnovu ulazne fakture {ulazna.oznaka}";
                        dbContext.Notifications.Add(n);
                        dbContext.SaveChanges();
                    }

                }
            }

            

        }

            #endregion
        }

    public enum Navigation {
        profakture,
        izlazna,
        ulazna,
        storno,
        help,
        info,
        proizvodi,
        dodajProizvod,
        dodajProizvodjaca,
        kompenzacije,
        poslovniPartneri,
        zalihe,
        obavestenja,
        naprednaPretraga,
        statistika,
        bilansi,
        zaposleni,
        dodajZalihe,
        dodajProfakturu,
        izmeniProfakturu,
        opomena
    }

    
  
}
