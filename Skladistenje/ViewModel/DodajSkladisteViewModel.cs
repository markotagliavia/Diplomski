using Common;
using Common.Model;
using Notifications;
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
        private string staroImeSkladiste = "";
        private ObservableCollection<grad> gradovi;
        private string gradForBind;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private string submitButtonText;
        private string kolicinaText;
        private string rafText;
        private bool addEnabled;
        private bool removeEnabled;
        private int _selectedProizvod = -1;
        private int _selectedProizvodSaKolicinom = -1;
        private ObservableCollection<Proizvod> proizvodi;
        private ObservableCollection<ProizvodKolicina> proizvodiSaKolicinom;
        #endregion

        #region Commands
        public MyICommand<object> DodajSkladisteCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        public MyICommand<int> AddCommand { get; private set; }
        public MyICommand<int> RemoveCommand { get; private set; }
        #endregion

        public DodajSkladisteViewModel(int v, Skladiste s)
        {
            DodajSkladisteCommand = new MyICommand<object>(DodajSkladiste);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            AddCommand = new MyICommand<int>(Add);
            RemoveCommand = new MyICommand<int>(Remove);
            context = v;
            userOnSession = new Korisnik();
            gradovi = new ObservableCollection<grad>();
            proizvodi = new ObservableCollection<Proizvod>();
            proizvodiSaKolicinom = new ObservableCollection<ProizvodKolicina>();
            foreach (var item in dbContext.grads)
            {
                Gradovi.Add(item);
            }

            if (v == 0)
            {
                SkladisteForEdit = new Skladiste();
                SubmitButtonText = "Dodaj";
                populateProizvodiGrid();
            }
            else
            {
                GradForBind = s.grad.naziv;
                SkladisteForEdit = s;
                SubmitButtonText = "Potvrdi izmenu";
                staroImeSkladiste = s.naziv;
                //TO DO : pokazati stanje sa zaliha u proizvodSaKolicinom
                populateProizvodiGrid();
            }

            KolicinaText = "";
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
                    if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.AddSkladiste))
                    {
                        Skladiste novoSkl = new Skladiste();
                        novoSkl.active = true;
                        novoSkl.adresa = SkladisteForEdit.adresa;
                        novoSkl.naziv = SkladisteForEdit.naziv;
                        novoSkl.sifra = SkladisteForEdit.sifra;
                        novoSkl.grad_id = dbContext.grads.FirstOrDefault(x => x.naziv.Equals(GradForBind)).id;
                        dbContext.Skladistes.Add(novoSkl);

                        foreach (var item in ProizvodiSaKolicinom)
                        {
                            if (dbContext.Zalihes.Any(x => x.Skladiste.naziv.Equals(SkladisteForEdit.naziv) && x.Proizvod.sifra.Equals(item.Sifra)))
                            {
                                dbContext.Zalihes.FirstOrDefault(x => x.Skladiste.naziv.Equals(SkladisteForEdit.naziv) && x.Proizvod.sifra.Equals(item.Sifra) && x.raf.Equals(item.Raf)).kolicina += float.Parse(item.Kolicina);
                            }
                            else
                            {
                                Zalihe z = new Zalihe();
                                z.kolicina = float.Parse(item.Kolicina);
                                z.proizvod_id = dbContext.Proizvods.FirstOrDefault(x => x.sifra.Equals(item.Sifra)).id;
                                z.minimumkolicine = dbContext.Proizvods.FirstOrDefault(x => x.sifra.Equals(item.Sifra)).minimumkolicine;
                                z.raf = item.Raf;
                                z.rezervisano = 0;
                                z.skladiste_id = SkladisteForEdit.id;
                                dbContext.Zalihes.Add(z);
                            }
                        }
                        dbContext.SaveChanges();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, $"Uspesno dodavanje skladista {SkladisteForEdit.naziv}", "Info");
                        Success suc = new Success("Uspešno ste dodali novo skladište!");
                        suc.Show();
                        Otkazi("");
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesno dodavanje skladista. Autorizacija.", "Upozorenje");
                        Otkazi("");
                    }

                }
                else
                {
                    try
                    {
                        if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.EditSkladiste))
                        {
                            Skladiste sklStaro = dbContext.Skladistes.FirstOrDefault(x => x.naziv.Equals(staroImeSkladiste));
                            if (!sklStaro.adresa.Equals(SkladisteForEdit.adresa)) dbContext.Skladistes.FirstOrDefault(x => x.naziv.Equals(staroImeSkladiste)).adresa = SkladisteForEdit.adresa;
                            if (!sklStaro.sifra.Equals(SkladisteForEdit.sifra)) dbContext.Skladistes.FirstOrDefault(x => x.naziv.Equals(staroImeSkladiste)).sifra = SkladisteForEdit.sifra;
                            if (!sklStaro.grad.naziv.Equals(GradForBind))
                            {
                                int grad_idNovi = dbContext.grads.FirstOrDefault(x => x.naziv.Equals(SkladisteForEdit.grad.naziv)).id;
                                dbContext.Skladistes.FirstOrDefault(x => x.naziv.Equals(staroImeSkladiste)).grad_id = grad_idNovi;
                            }
                            if (!sklStaro.naziv.Equals(SkladisteForEdit.naziv)) dbContext.Skladistes.FirstOrDefault(x => x.naziv.Equals(staroImeSkladiste)).naziv = SkladisteForEdit.naziv;
                            dbContext.SaveChanges();
                            Notifications.Success s = new Notifications.Success($"Uspešno ste izmenili skladište {staroImeSkladiste}");
                            s.Show();
                            foreach (Window w in Application.Current.Windows)
                            {
                                if (w.GetType().Equals(typeof(MainWindow)))
                                {
                                    SecurityManager.AuditManager.AuditToDB(((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime, "Uspesno je izmenjeno skladiste", "Info");
                                }
                            }
                            Otkazi("");
                        }
                        else
                        {
                            Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesna izmena skladista. Autorizacija.", "Upozorenje");
                            Otkazi("");
                        }


                    }
                    catch (Exception ex)
                    {
                        Notifications.Error e = new Notifications.Error("Greška pri unosu podataka o skladištu");
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

        private void Remove(int obj)
        {
            if (SelectedProizvodSaKolicinom != -1)
            {
                ProizvodKolicina p = ProizvodiSaKolicinom.ElementAt(SelectedProizvodSaKolicinom);
                ProizvodiSaKolicinom.RemoveAt(SelectedProizvodSaKolicinom);
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }

        private void Add(int index)
        {
            if (SelectedProizvod != -1)
            {
                Proizvod p = Proizvodi.ElementAt(SelectedProizvod);
                //TO DO validacija za kolicinu
                ProizvodKolicina pk = new ProizvodKolicina(p, KolicinaText, RafText);
                ProizvodiSaKolicinom.Add(pk);
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
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

        public string SubmitButtonText { get => submitButtonText; set { submitButtonText = value; OnPropertyChanged("SubmitButtonText"); } }

        public string KolicinaText { get => kolicinaText; set { kolicinaText = value; OnPropertyChanged("KolicinaText"); } }

        public string RafText { get => rafText; set { rafText = value; OnPropertyChanged("RafText"); } }

        public bool RemoveEnabled
        {
            get => removeEnabled;
            set
            {
                if (context == 0) removeEnabled = value;
                else removeEnabled = false;
                OnPropertyChanged("RemoveEnabled");
            }
        }

        public bool AddEnabled
        {
            get => addEnabled;
            set
            {
                if (context == 0) addEnabled = value;
                else addEnabled = false;
                OnPropertyChanged("AddEnabled");
            }
        }

        public ObservableCollection<Proizvod> Proizvodi
        {
            get => proizvodi;
            set
            {
                proizvodi = value;
                OnPropertyChanged("Proizvodi");
            }
        }

        public ObservableCollection<ProizvodKolicina> ProizvodiSaKolicinom
        {
            get => proizvodiSaKolicinom;
            set
            {
                proizvodiSaKolicinom = value;
                OnPropertyChanged("ProizvodiSaKolicinom");
            }
        }

        public int SelectedProizvod
        {
            get => _selectedProizvod;
            set
            {
                if (_selectedProizvod == value)
                {
                    if (context == 0)
                    {
                        if (_selectedProizvod > -1)
                        {
                            AddEnabled = true;
                        }
                        else
                        {
                            AddEnabled = false;
                        }
                    }
                    return;
                }
                _selectedProizvod = value;
                if (context == 0)
                {
                    if (_selectedProizvod > -1)
                    {
                        AddEnabled = true;
                    }
                    else
                    {
                        AddEnabled = false;
                    }
                }
                return;
            }
        }

        public int SelectedProizvodSaKolicinom
        {
            get => _selectedProizvodSaKolicinom;
            set
            {
                if (_selectedProizvodSaKolicinom == value)
                {
                    if (context == 0)
                    {
                        if (_selectedProizvodSaKolicinom > -1)
                        {
                            RemoveEnabled = true;
                        }
                        else
                        {
                            RemoveEnabled = false;
                        }
                    }
                    return;
                }
                _selectedProizvodSaKolicinom = value;
                if (context == 0)
                {
                    if (_selectedProizvodSaKolicinom > -1)
                    {
                        RemoveEnabled = true;
                    }
                    else
                    {
                        RemoveEnabled = false;
                    }
                }
                return;
            }
        }
        #endregion

        #region HelperMethods
        private void populateProizvodiGrid()
        {
            foreach (var item in dbContext.Proizvods.ToList())
            {
                proizvodi.Add(item);
            }
        }
        #endregion
    }

    public class ProizvodKolicina : BindableBase
    {
        private string naziv;
        private string sifra;
        private string jedinicaMere;
        private string proizvodjac;
        private string kolicina;
        private string raf;

        public ProizvodKolicina(Proizvod p, string kolicina, string raf)
        {
            this.JedinicaMere = p.jedinicamere.naziv;
            this.Sifra = p.sifra;
            this.Naziv = p.naziv;
            this.Proizvodjac = p.Proizvodjac.naziv;
            this.Kolicina = kolicina;
            this.raf = raf;
        }

        public string Naziv { get => naziv; set { naziv = value; OnPropertyChanged("Naziv"); } }
        public string Sifra { get => sifra; set { sifra = value; OnPropertyChanged("Sifra"); } }
        public string JedinicaMere { get => jedinicaMere; set { jedinicaMere = value; OnPropertyChanged("JedinicaMere"); } }
        public string Proizvodjac { get => proizvodjac; set { proizvodjac = value; OnPropertyChanged("Proizvodjac"); } }
        public string Kolicina { get => kolicina; set { kolicina = value; OnPropertyChanged("Kolicina"); } }
        public string Raf { get => raf; set { raf = value; OnPropertyChanged("Raf"); } }
    }
}
