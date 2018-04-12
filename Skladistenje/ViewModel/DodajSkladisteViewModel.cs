using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Skladistenje.ViewModel
{
    public class DodajSkladisteViewModel : BindableBase
    {
        #region Members
        private int context;
        private Korisnik userOnSession;
        private Skladiste skladisteForEdit;
        private ObservableCollection<grad> gradovi;
        private string gradForBind;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        #endregion

        #region Commands
        public MyICommand<object> DodajSkladisteCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        #endregion

        public DodajSkladisteViewModel(int v, Skladiste s)
        {
            DodajSkladisteCommand = new MyICommand<object>(DodajSkladiste);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            context = v;
            userOnSession = new Korisnik();
            skladisteForEdit = new Skladiste();
            this.SkladisteForEdit = s;
            gradovi = new ObservableCollection<grad>();
            foreach (var item in dbContext.grads)
            {
                Gradovi.Add(item);
            }

        }

        #region CommandsImplementation

        private void Otkazi(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("skladista");
                }
            }
        }

        private void DodajSkladiste(object obj)
        {
            //TO DO
            /*try
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
            }*/
        }
        #endregion

        #region Properties
        public Korisnik UserOnSession { get => userOnSession; set => userOnSession = value; }
        public Skladiste SkladisteForEdit { get => skladisteForEdit; set { skladisteForEdit = value; OnPropertyChanged("SkladisteForEdit"); } }
        public ObservableCollection<grad> Gradovi
        {
            get => gradovi;
            set
            {
                gradovi = value;
                OnPropertyChanged("Gradovi");
            }
        }

        public string GradForBind
        {
            get => gradForBind;
            set
            {
                gradForBind = value;
                OnPropertyChanged(GradForBind);
            }
        }
        #endregion
    }
}
