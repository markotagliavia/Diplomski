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
        #endregion

        public DodajKorisnikaViewModel()
        {
            DodajKorisnikaCommand = new MyICommand<object>(DodajKorisnika);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            AddNavCommand = new MyICommand<int>(Add);
            RemoveNavCommand = new MyICommand<int>(Remove);
            Korisnici = new ObservableCollection<Korisnik>();
            userForBind = new ZaposleniKorisnik();
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

            foreach (var item in dbContext.Ulogas)
            {
                uloge.Add(item);
               
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
    }
}
