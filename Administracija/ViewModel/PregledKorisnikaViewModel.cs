﻿using Common;
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

namespace Administracija.ViewModel
{
    public class PregledKorisnikaViewModel : BindableBase
    {
        //komande su : navigacija na dodavanje i izmenu i komande brisanja i pretrage
        #region Commands
        public MyICommand<string> DodajKorisnikaNavCommand { get; private set; }
        public MyICommand<string> IzmeniKorisnikaNavCommand { get; private set; }
        public MyICommand<string> IzbrisiKorisnikaCommand { get; private set; }
        public MyICommand<string> PretraziKorisnikaCommand { get; private set; }
        #endregion

        #region Properties
        private Korisnik userOnSession;
        private int _selectedIndex = -1;
        private bool selectedInGrid;
        private string textSearch;
        private ObservableCollection<ZaposleniKorisnik> korisnici;
        private ZaposleniKorisnik selectedValue;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ICollectionView defaultView;
        #endregion

        public PregledKorisnikaViewModel()
        {
            DodajKorisnikaNavCommand = new MyICommand<string>(DodajKorisnikaNav);
            IzmeniKorisnikaNavCommand = new MyICommand<string>(IzmeniKorisnikaNav);
            IzbrisiKorisnikaCommand = new MyICommand<string>(IzbrisiKorisnika);
            PretraziKorisnikaCommand = new MyICommand<string>(PretraziKorisnika);
            textSearch = "";
            korisnici = new ObservableCollection<ZaposleniKorisnik>();
            populateKorisniks();
            DefaultView = CollectionViewSource.GetDefaultView(Korisnici);
        }

        #region Constructors
       
        public ObservableCollection<ZaposleniKorisnik> Korisnici
        {
            get { return korisnici; }
            set { korisnici = value; }
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

        public ZaposleniKorisnik SelectedValue
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
        private void PretraziKorisnika(string type)
        {
            if (!type.Equals("/"))
            {
                if (TextSearch != null && !String.IsNullOrWhiteSpace(TextSearch) && (TextSearch != ""))
                {
                    DefaultView = CollectionViewSource.GetDefaultView(DefaultView);
                    if (type.Equals("Korisničkom imenu"))
                    {
                        DefaultView.Filter =
                        w => ((ZaposleniKorisnik)w).KorisnickoIme.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Imenu"))
                    {
                        DefaultView.Filter =
                        w => ((ZaposleniKorisnik)w).Ime.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Prezimenu"))
                    {
                        DefaultView.Filter =
                        w => ((ZaposleniKorisnik)w).Prezime.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("JMBG-u"))
                    {
                        DefaultView.Filter =
                        w => ((ZaposleniKorisnik)w).JMBG.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Telefonu"))
                    {
                        DefaultView.Filter =
                        w => ((ZaposleniKorisnik)w).Telefon.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("E-mailu"))
                    {
                        DefaultView.Filter =
                        w => ((ZaposleniKorisnik)w).Email.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Adresi"))
                    {
                        DefaultView.Filter =
                        w => ((ZaposleniKorisnik)w).Adresa.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Gradu"))
                    {
                        DefaultView.Filter =
                        w => ((ZaposleniKorisnik)w).Grad.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Tekućem računu"))
                    {
                        DefaultView.Filter =
                        w => ((ZaposleniKorisnik)w).Racun.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Šefu"))
                    {
                        DefaultView.Filter =
                        w => ((ZaposleniKorisnik)w).Sef.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Ulozi"))
                    {
                        DefaultView.Filter =
                        w => ((ZaposleniKorisnik)w).Uloga.ToUpper().Contains(TextSearch.ToUpper());
                    }

                    DefaultView.Refresh();
                }
                else
                {
                    DefaultView = CollectionViewSource.GetDefaultView(Korisnici);
                    DefaultView.Filter = null;
                    DefaultView.Refresh();
                }
            }
            else
            {
                DefaultView = CollectionViewSource.GetDefaultView(Korisnici);
                DefaultView.Filter = null;
                DefaultView.Refresh();
            }
        }

        private void IzbrisiKorisnika(string obj)
        {
            try
            {
                foreach (Window w in Application.Current.Windows)
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
                            foreach (var item in dbContext.ZaposleniSkladistas.ToList())
                            {
                                if (item.active == true && item.Zaposleni.id == dbContext.Korisniks.First(x => x.active == true && x.korisnickoime.Equals(korisnickoImeBrisanog)).zaposleni_id)
                                {
                                    dbContext.ZaposleniSkladistas.FirstOrDefault(x => x.active == true && x.zaposleni_id == item.zaposleni_id && x.skladiste_id == item.skladiste_id).active = false;
                                }
                            }
                            dbContext.Korisniks.Remove(dbContext.Korisniks.First(x => x.active == true && x.korisnickoime.Equals(korisnickoImeBrisanog)));
                            dbContext.SaveChanges();
                            Success suc = new Success("Uspešno ste obrisali korisnika.");
                            suc.Show();
                            SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Uspešno brisanje korisnika {korisnickoImeBrisanog}.", "Info");
                            Korisnici.Clear();
                            populateKorisniks();
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
                    SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj brisnja korisnika", "Upozorenje");
                }

            }
            catch (Exception ex)
            {
                Error er = new Error("Greška sa konekcijom!\nObratite se administratorima.");
                er.Show();
            }
            

        }

        private void IzmeniKorisnikaNav(string obj)
        {
            if (SelectedValue != null)
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                        if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id,SecurityManager.Permission.EditUser))
                        {
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajKorisnikaViewModel = new DodajKorisnikaViewModel(1, selectedValue);
                            ((MainWindowViewModel)((MainWindow)w).DataContext).DodajKorisnikaViewModel.UserOnSession = this.UserOnSession;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("addUser");
                            ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Korisnici -> Izmena";
                        }
                        else
                        {
                            Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan okusaj izmene korisnika", "Upozorenje");
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

        private void DodajKorisnikaNav(string obj)
        {
            
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                    if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.AddUser))
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajKorisnikaViewModel = new DodajKorisnikaViewModel(0, null);
                        ((MainWindowViewModel)((MainWindow)w).DataContext).DodajKorisnikaViewModel.UserOnSession = this.UserOnSession;
                        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("addUser");
                        ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Korisnici -> Novi";
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj dodavanja korisnika", "Upozorenje");
                    }
                }
            }
        }
        #endregion

        #region HelperMethods
        private void populateKorisniks()
        {
            foreach (var item in dbContext.Zaposlenis.ToList())
            {
                string sefStr = "Nema";
                if (item.sef_id != null)
                {
                    if (dbContext.Zaposlenis.Any(x => x.active == true && x.id == item.sef_id))
                    {
                        Zaposleni sef = dbContext.Zaposlenis.First(x => x.active == true && x.id == item.sef_id);
                        sefStr = sef.Korisniks.ElementAt(0).korisnickoime;
                    }
                    else
                    {
                        sefStr = "Nema";
                    }
                }
                ZaposleniKorisnik zk = new ZaposleniKorisnik(item, sefStr);
                korisnici.Add(zk);
            }
        }
        #endregion
    }

    public class ZaposleniKorisnik : BindableBase
    {
        private string korisnickoIme;
        private int idzaposlenog;
        private string ime;
        private string prezime;
        private string jmbg;
        private string plata;
        private string bonusi;
        private string doprinosi;
        private string email;
        private string telefon;
        private string adresa;
        private string grad;
        private string sef;
        private string uloga;
        private string racun;
        private string lozinka;
        private bool active;

        public ZaposleniKorisnik(Zaposleni z, string sef)
        {
            korisnickoIme = z.Korisniks.ElementAt(0).korisnickoime;
            idzaposlenog = z.id;
            Ime = z.ime;
            Prezime = z.prezime;
            JMBG = z.jmbg;
            Plata = z.plata.ToString();
            Bonusi = z.bonus.ToString();
            Doprinosi = z.doprinosi.ToString();
            Email = z.email;
            Telefon = z.brojtelefona;
            Adresa = z.adresa;
            Grad = z.grad.naziv;
            this.sef = sef;
            Racun = z.tekuciracun;
            uloga = z.Ulogas.ElementAt(0).naziv;
            lozinka = z.Korisniks.ElementAt(0).lozinka;
            Active = z.active;
        }

        public ZaposleniKorisnik()
        {
        }

        public string Racun { get => racun; set { racun = value; OnPropertyChanged("Racun"); } }
        public string Uloga { get => uloga; set { uloga = value; OnPropertyChanged("Uloga"); } }
        public string Sef { get => sef; set { sef = value; OnPropertyChanged("Sef"); } }
        public string Grad { get => grad; set { grad = value; OnPropertyChanged("Grad"); } }
        public string Adresa { get => adresa; set { adresa = value; OnPropertyChanged("Adresa"); } }
        public string Telefon { get => telefon; set { telefon = value; OnPropertyChanged("Telefon"); } }
        public string Email { get => email; set { email = value; OnPropertyChanged("Email"); } }
        public string Doprinosi { get => doprinosi; set { doprinosi = value; OnPropertyChanged("Doprinosi"); } }
        public string Bonusi { get => bonusi; set { bonusi = value; OnPropertyChanged("Bonusi"); } }
        public string Plata { get => plata; set { plata = value; OnPropertyChanged("Plata"); } }
        public string JMBG { get => jmbg; set { jmbg = value; OnPropertyChanged("JMBG"); } }
        public string Prezime { get => prezime; set { prezime = value; OnPropertyChanged("Prezime"); } }
        public string Ime { get => ime; set { ime = value; OnPropertyChanged("Ime"); } }
        public string KorisnickoIme { get => korisnickoIme; set { korisnickoIme = value; OnPropertyChanged("KorisnickoIme"); } }
        public string Lozinka
        {
            get => lozinka;
            set
            {
                lozinka = value;
                OnPropertyChanged("Lozinka");
            }
        }
        public bool Active { get => active; set { active = value; OnPropertyChanged("Active"); } }

        public int Idzaposlenog { get => idzaposlenog; set { idzaposlenog = value; OnPropertyChanged("Idzaposlenog"); } }
    }
}
