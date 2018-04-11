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

namespace Administracija.ViewModel
{
    public class DodajKorisnikaViewModel : BindableBase
    {
        //komande su: dodaj i otkazi

        #region Commands
        public MyICommand<object> DodajKorisnikaCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        public MyICommand<int> AddNavCommand { get; private set; }
        public MyICommand<int> RemoveNavCommand { get; private set; }

        #endregion

        #region Members
        private string sefForBind;
        private ZaposleniKorisnik userForBind;
        private Korisnik userOnSession;
        private ObservableCollection<Korisnik> korisnici;
        private ObservableCollection<Uloga> uloge;
        private ObservableCollection<Uloga> ulogaKorisnik;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private int context;
        private int _selectedRoll = -1;
        private int _selectedUserRoll = -1;
        private bool addEnabled;
        private bool removeEnabled;
        private ZaposleniKorisnik zaposleni;
        #endregion

        public DodajKorisnikaViewModel(int i,ZaposleniKorisnik zk)
        {
            context = i;
            zaposleni = zk;
            DodajKorisnikaCommand = new MyICommand<object>(DodajKorisnika);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            AddNavCommand = new MyICommand<int>(Add);
            RemoveNavCommand = new MyICommand<int>(Remove);
            Korisnici = new ObservableCollection<Korisnik>();


            

            Korisnik k = new Korisnik();
            k.id = -1;
            k.korisnickoime = "Nema šefa";
            korisnici.Add(k);
            foreach (var item in dbContext.Korisniks)
            {
                korisnici.Add(item);
            }

            uloge = new ObservableCollection<Uloga>();
            ulogaKorisnik = new ObservableCollection<Uloga>();

            if (context == 1)
            {
                userForBind = zk;

                foreach (var item in dbContext.Zaposlenis.FirstOrDefault(x => x.id == zk.Idzaposlenog).Ulogas)
                {
                    ulogaKorisnik.Add(item);
                }

                if (zk.Sef.Equals("Nema"))
                {
                    sefForBind = "Nema šefa";
                }
                else
                {
                    sefForBind = zk.Sef;
                }

            }
            else
            {
                userForBind = new ZaposleniKorisnik();
                sefForBind = "Nema šefa";
            }
            foreach (var item in dbContext.Ulogas)
            {
                if (!ulogaKorisnik.Any(x => x.naziv.Equals(item.naziv)))
                {
                    uloge.Add(item);
                }
                
               
            }
        }



        #region Constructors
        public Korisnik UserOnSession { get => userOnSession; set => userOnSession = value; }

        public ObservableCollection<Korisnik> Korisnici
        {
            get { return korisnici; }
            set
            {
                korisnici = value;
                OnPropertyChanged("Korisnici");
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

        public ObservableCollection<Uloga> UlogaKorisnik
        {
            get { return ulogaKorisnik; }
            set
            {
                ulogaKorisnik = value;
                OnPropertyChanged("UlogaKorisnik");
            }
        }

        public ZaposleniKorisnik UserForBind
        {
            get { return userForBind; }
            set
            {
                userForBind = value;
                OnPropertyChanged("UserForBind");
            }
        }

        public string SefForBind
        {
            get { return sefForBind; }
            set
            {
                sefForBind = value;
                OnPropertyChanged("SefForBind");
            }
        }
        #endregion

        #region CommandsImplementation
        private void Otkazi(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("previewUsers");
                }
            }
        }

        private void DodajKorisnika(object obj)
        {
            //TO DO : autorizacija
            try
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;

                    }
                }
                if (context == 0)
                {
                    if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.AddUser))
                    {
                        string pass = ((System.Windows.Controls.PasswordBox)obj).Password;
                        string hashedPass = SecurityManager.Encryption.sha256(pass);
                        Zaposleni z = new Zaposleni();
                        z.active = true;
                        z.adresa = UserForBind.Adresa;
                        z.bonus = Double.Parse(UserForBind.Bonusi);
                        z.brojtelefona = UserForBind.Telefon;
                        z.doprinosi = Double.Parse(UserForBind.Doprinosi);
                        z.email = UserForBind.Email;
                        z.grad_id = dbContext.grads.First(x => x.naziv.Equals(UserForBind.Grad)).id;
                        z.ime = UserForBind.Ime;
                        z.jmbg = UserForBind.JMBG;
                        z.plata = Double.Parse(UserForBind.Plata);
                        z.prezime = UserForBind.Prezime;
                        if (sefForBind.Equals("Nema šefa"))
                        {
                            z.sef_id = null;
                        }
                        else
                        {
                            foreach (var item in dbContext.Zaposlenis)
                            {
                                if (item.Korisniks.Any(x => x.korisnickoime.Equals(sefForBind)))
                                {
                                    z.sef_id = item.id;
                                }
                            }
                        }
                        z.tekuciracun = UserForBind.Racun;
                        z.Ulogas = UlogaKorisnik;

                        dbContext.Zaposlenis.Add(z);
                        dbContext.SaveChanges();

                        Korisnik k = new Korisnik();
                        k.korisnickoime = UserForBind.KorisnickoIme;
                        k.lozinka = hashedPass;
                        k.active = true;
                        k.zaposleni_id = dbContext.Zaposlenis.First(x => x.active == true && x.jmbg.Equals(UserForBind.JMBG)).id;
                        k.ulogovan = false;

                        dbContext.Korisniks.Add(k);
                        dbContext.SaveChanges();
                        Success suc = new Success("Uspešno ste dodali novog korisnika!");
                        suc.Show();
                        Otkazi("");
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Pokusaj dodavanja korisnika", "Upozorenje");
                        Otkazi("");
                    }
                    
                }
                else
                {
                    try
                    {
                        if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.EditUser))
                        {
                            var original = dbContext.Zaposlenis.FirstOrDefault(x => x.id == UserForBind.Idzaposlenog);

                            if (original != null)
                            {
                                string staroKorisnicko = original.Korisniks.ElementAt(0).korisnickoime;
                                string pass = ((System.Windows.Controls.PasswordBox)obj).Password;
                                string hashedPass = SecurityManager.Encryption.sha256(pass);
                                original.active = true;
                                original.adresa = UserForBind.Adresa;
                                original.bonus = Double.Parse(UserForBind.Bonusi);
                                original.brojtelefona = UserForBind.Telefon;
                                original.doprinosi = Double.Parse(UserForBind.Doprinosi);
                                original.email = UserForBind.Email;
                                original.grad_id = dbContext.grads.First(x => x.naziv.Equals(UserForBind.Grad)).id;
                                original.ime = UserForBind.Ime;
                                original.jmbg = UserForBind.JMBG;
                                original.plata = Double.Parse(UserForBind.Plata);
                                original.prezime = UserForBind.Prezime;
                                if (sefForBind.Equals("Nema šefa"))
                                {
                                    original.sef_id = null;
                                }
                                else
                                {
                                    foreach (var item in dbContext.Zaposlenis)
                                    {
                                        if (item.Korisniks.Any(x => x.korisnickoime.Equals(sefForBind)))
                                        {
                                            original.sef_id = item.id;
                                        }
                                    }
                                }
                                original.tekuciracun = UserForBind.Racun;
                                original.Ulogas = UlogaKorisnik;

                                dbContext.SaveChanges();


                                var korisnik = dbContext.Korisniks.FirstOrDefault(x => x.korisnickoime.Equals(staroKorisnicko));

                                if (korisnik != null)
                                {
                                    korisnik.korisnickoime = userForBind.KorisnickoIme;
                                    korisnik.lozinka = hashedPass;
                                    korisnik.active = true;

                                    dbContext.SaveChanges();
                                }

                                Notifications.Success s = new Notifications.Success("Uspešno ste izmenili " + staroKorisnicko);
                                s.Show();
                                foreach (Window w in Application.Current.Windows)
                                {
                                    if (w.GetType().Equals(typeof(MainWindow)))
                                    {
                                        SecurityManager.AuditManager.AuditToDB(((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime, "Uspesno je izmenjena uloga " + staroKorisnicko, "Info");

                                    }
                                }

                            }

                            Otkazi("");
                        }
                        else
                        {
                            Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Pokusaj izmene korisnika", "Upozorenje");
                            Otkazi("");
                        }


                    }
                    catch (Exception ex)
                    {
                        Notifications.Error e = new Notifications.Error("Greška pri unosu uloge");
                        e.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Error er = new Error("Greška sa konekcijom!\nObratite se administratorima.");
                er.Show();
            }

            
        }

        private void Remove(int index)
        {
            if (index != -1)
            {
                Uloga u = UlogaKorisnik.ElementAt(index);
                UlogaKorisnik.RemoveAt(index);
                uloge.Add(u);
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }

        private void Add(int index)
        {
            if (index != -1)
            {
                Uloga u = Uloge.ElementAt(index);
                Uloge.RemoveAt(index);
                ulogaKorisnik.Add(u);
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }
        #endregion

        #region Properties
        public int SelectedRoll
        {
            get => _selectedRoll;
            set
            {

                if (_selectedRoll == value)
                {
                    if (_selectedRoll > -1)
                    {
                        AddEnabled = true;
                    }
                    else
                    {
                        AddEnabled = false;
                    }
                    return;
                }
                _selectedRoll = value;
                if (_selectedRoll > -1)
                {
                    AddEnabled = true;
                }
                else
                {
                    AddEnabled = false;
                }
                return;
            }
        }
        public bool AddEnabled
        {
            get => addEnabled;
            set
            {
                addEnabled = value;
                OnPropertyChanged("AddEnabled");
            }
        }

        public int SelectedUserRoll
        {
            get => _selectedUserRoll;
            set
            {

                if (_selectedUserRoll == value)
                {
                    if (_selectedUserRoll > -1)
                    {
                        RemoveEnabled = true;
                    }
                    else
                    {
                        RemoveEnabled = false;
                    }
                    return;
                }
                _selectedUserRoll = value;
                if (_selectedUserRoll > -1)
                {
                    RemoveEnabled = true;
                }
                else
                {
                    RemoveEnabled = false;
                }
                return;
            }
        }
        public bool RemoveEnabled
        {
            get => removeEnabled;
            set
            {
                removeEnabled = value;
                OnPropertyChanged("RemoveEnabled");
            }
        }
        #endregion
    }
}
