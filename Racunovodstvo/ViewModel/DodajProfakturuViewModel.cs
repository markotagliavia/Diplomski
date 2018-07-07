using Common;
using Common.Model;
using Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Racunovodstvo.ViewModel
{
    public class DodajProfakturuViewModel : BindableBase
    {
        #region Members
        private int context;
        private Profaktura profakturaForEdit;
        private Korisnik userOnSession = new Korisnik();
        private string skladisteForBind;
        private string poslovniPatnerForBind;
        private ObservableCollection<Skladiste> skladista;
        private ObservableCollection<PoslovniPartner> poslovniPartneri;
        private ObservableCollection<Proizvod> proizvodi;
        private ObservableCollection<ProizvodKolicina> proizvodiSaKolicinom;
        private int _selectedProizvod = -1;
        private int _selectedProizvodSaKolicinom = -1;
        private double pdv;
        private double bezPDV;
        private double saPDV;
        private bool addEnabled;
        private bool removeEnabled;
        private string submitButtonText;
        private string kolicinaText;
        private string rabatText;
        private string cenaText;
        private string staroSkladiste;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        #endregion

        #region Commands
        public MyICommand<string> DodajCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        public MyICommand<string> BackCommand { get; private set; }
        public MyICommand<int> AddCommand { get; private set; }
        public MyICommand<int> RemoveCommand { get; private set; }
        #endregion

        public DodajProfakturuViewModel(int i, Profaktura f)
        { //0 - nova izlazna,1-izmena izlazne,2-nova ulazna,3-izmena izlazne
            DodajCommand = new MyICommand<string>(Dodaj);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            BackCommand = new MyICommand<string>(Back);
            AddCommand = new MyICommand<int>(Add);
            RemoveCommand = new MyICommand<int>(Remove);
            context = i;
            proizvodi = new ObservableCollection<Proizvod>();
            proizvodiSaKolicinom = new ObservableCollection<ProizvodKolicina>();
            Skladista = new ObservableCollection<Skladiste>();
            KolicinaText = "";
            CenaText = "";
            RabatText = "";
            foreach (var item in dbContext.Skladistes)
            {
                Skladista.Add(item);
            }
            PoslovniPartneri = new ObservableCollection<PoslovniPartner>();
            foreach (var item in dbContext.PoslovniPartners)
            {
                PoslovniPartneri.Add(item);
            }
            if (context == 0)
            {
                SubmitButtonText = "Dodaj";
                SkladisteForBind = "";
                ProfakturaForEdit = new Profaktura();
                ProfakturaForEdit.active = true;
                ProfakturaForEdit.zaposleni_id = -1;
                ProfakturaForEdit.datum = DateTime.Now;
                ProfakturaForEdit.PDV = 0;
                Pdv = 0;
                SaPDV = 0;
                BezPDV = 0;

            }
            else if (context == 1)
            {
                SubmitButtonText = "Potvrdi izmenu";
                BezPDV = 0;
                SaPDV = 0;
                ProfakturaForEdit = f;
                Pdv = f.PDV;
                
                if (f.StavkaProfaktures.Count > 0)
                {
                    SkladisteForBind = f.StavkaProfaktures?.ElementAt(0).Zalihe.Skladiste.naziv ?? "";
                }
                else
                {
                    SkladisteForBind = "";
                }
                staroSkladiste = SkladisteForBind;
                PoslovniPartnerForBind = f.PoslovniPartner?.naziv ?? "";
                foreach (var item in f.StavkaProfaktures)
                {
                    BezPDV += ((double)item.cena * item.kolicina) * (1 - ((double)(item.rabat) / 100));
                    SaPDV = (1 + (Pdv / 100)) * BezPDV;
                    ProizvodKolicina pk = new ProizvodKolicina(item.Zalihe.Proizvod, item.kolicina.ToString(), item.cena.ToString(), item.rabat.ToString());
                    ProizvodiSaKolicinom.Add(pk);

                }
            }
        }

        private void Remove(int obj)
        {
            ProizvodKolicina p = ProizvodiSaKolicinom.ElementAt(SelectedProizvodSaKolicinom);
            Double kolicina = Double.Parse(p.Kolicina);
            Zalihe z = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(p.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind));
            if (SelectedProizvodSaKolicinom != -1)
            {

                ProizvodiSaKolicinom.RemoveAt(SelectedProizvodSaKolicinom);
                BezPDV -= (Double.Parse(p.Cena) * kolicina) * (1 - (Double.Parse(p.Rabat) / 100));
                SaPDV = (1 + (Pdv / 100)) * BezPDV;
               
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }

        private void Add(int obj)
        {
            if (SelectedProizvod != -1)
            {
                if (String.IsNullOrEmpty(KolicinaText) || String.IsNullOrEmpty(RabatText) || String.IsNullOrEmpty(CenaText))
                {
                    Error er = new Error("Sva polja su obavezna");
                    er.Show();
                    return;
                }
                Proizvod p = Proizvodi.ElementAt(SelectedProizvod);
                Double kolicina = Double.Parse(KolicinaText);
                Zalihe z = dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == p.id && x.Skladiste.naziv.Equals(SkladisteForBind));
                
                
                ProizvodKolicina pk = new ProizvodKolicina(p, KolicinaText, CenaText, RabatText);
                ProizvodiSaKolicinom.Add(pk);
                BezPDV += (Double.Parse(CenaText) * kolicina) * (1 - (Double.Parse(RabatText) / 100));
                SaPDV = (1 + (Pdv / 100)) * BezPDV;

               
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }
        private void Back(string obj)
        {
            Otkazi("");
        }

        private void Otkazi(string obj)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.profakture);
        }

        private void Dodaj(string obj)
        {
            try {
                if (context == 0)
                {
                    ProfakturaForEdit.zaposleni_id = MainWindowViewModel.Instance.UserOnSession.zaposleni_id;
                    ProfakturaForEdit.poslovnipartner_mbr = dbContext.PoslovniPartners.FirstOrDefault(x => x.naziv.Equals(PoslovniPartnerForBind)).mbr;
                    
                    if (SecurityManager.AuthorizationPolicy.HavePermission(MainWindowViewModel.Instance.UserOnSession.id, SecurityManager.Permission.AddProfaktura))
                    {
                        dbContext.Profakturas.Add(ProfakturaForEdit);
                        dbContext.SaveChanges();
                        int i = 1;
                        foreach (var item in ProizvodiSaKolicinom)
                        {
                            StavkaProfakture st = new StavkaProfakture();
                            st.rednibroj = i;
                            st.kolicina = Double.Parse(item.Kolicina);
                            st.rabat = Double.Parse(item.Rabat);
                            st.cena = Double.Parse(item.Cena);
                            st.zalihe_proizvod_id = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind)).proizvod_id;
                            st.zalihe_skladiste_id = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind)).skladiste_id;
                            st.Profaktura = dbContext.Profakturas.FirstOrDefault(x => x.oznaka.Equals(ProfakturaForEdit.oznaka));
                            dbContext.StavkaProfaktures.Add(st);
                            dbContext.SaveChanges();
                            ++i;
                        }
                        Back("");
                        Notifications.Success s = new Notifications.Success("Uspešno ste kreirali profakturu");
                        s.Show();
                        
                        SecurityManager.AuditManager.AuditToDB(MainWindowViewModel.Instance.UserOnSession.korisnickoime, $"Uspesno je kreirana profaktura {ProfakturaForEdit.oznaka}", "Info");
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(MainWindowViewModel.Instance.UserOnSession.korisnickoime, "Neuspesan pokusaj kreiranja profakture", "Upozorenje");
                    }
                }
                else
                {

                    if (SecurityManager.AuthorizationPolicy.HavePermission(MainWindowViewModel.Instance.UserOnSession.id, SecurityManager.Permission.EditProfaktura))
                    {
                        var original = dbContext.Profakturas.FirstOrDefault(x => x.id == ProfakturaForEdit.id);

                        if (original != null)
                        {
                            original.oznaka = ProfakturaForEdit.oznaka;
                            original.datum = ProfakturaForEdit.datum;
                            original.PDV = ProfakturaForEdit.PDV;
                            original.poslovnipartner_mbr = ProfakturaForEdit.poslovnipartner_mbr;
                            
                            int i = 1;
                            original.StavkaProfaktures.Clear();
                            foreach (var item in ProizvodiSaKolicinom)
                            {
                                StavkaProfakture st = new StavkaProfakture();
                                st.rednibroj = i;
                                st.kolicina = Double.Parse(item.Kolicina);
                                st.rabat = Double.Parse(item.Rabat);
                                st.cena = Double.Parse(item.Cena);
                                st.zalihe_proizvod_id = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind)).proizvod_id;
                                st.zalihe_skladiste_id = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind)).skladiste_id;
                                st.Profaktura = dbContext.Profakturas.FirstOrDefault(x => x.oznaka.Equals(ProfakturaForEdit.oznaka));
                                original.StavkaProfaktures.Add(st);

                                ++i;
                            }

                        }
                        dbContext.SaveChanges();
                        Notifications.Success s = new Notifications.Success("Uspešno ste izmenili profakturu.");
                        s.Show();
                        SecurityManager.AuditManager.AuditToDB(MainWindowViewModel.Instance.UserOnSession.korisnickoime, "Uspesno je izmenjena faktura " + ProfakturaForEdit.oznaka, "Info");
                        MainWindowViewModel.Instance.OnNav(Navigation.izlazna);


                        Back("");
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(MainWindowViewModel.Instance.UserOnSession.korisnickoime, "Neuspesan pokusaj izmene profakture", "Upozorenje");
                    }
                }
            }
            catch (Exception ex)
            {
                
                Error er = new Error("Greška sa konekcijom!\nObratite se administratorima.");
                er.Show();
                

            }

        }

        #region Constructors
        public string SubmitButtonText { get => submitButtonText; set { submitButtonText = value; OnPropertyChanged("SubmitButtonText"); } }
        public Korisnik UserOnSession { get { return userOnSession; } set { userOnSession = value; } }
        public Profaktura ProfakturaForEdit { get => profakturaForEdit; set { profakturaForEdit = value; OnPropertyChanged("ProfakturaForEdit"); } }
        public bool RemoveEnabled
        {
            get => removeEnabled;
            set
            {
                removeEnabled = value;

                OnPropertyChanged("RemoveEnabled");
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

       
        public string KolicinaText { get => kolicinaText; set { kolicinaText = value; OnPropertyChanged("KolicinaText"); } }
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
                    
                    if (_selectedProizvod > -1)
                    {
                        AddEnabled = true;
                    }
                    else
                    {
                        AddEnabled = false;
                    }
                    return;
                        
                }
                _selectedProizvod = value;
                
                if (_selectedProizvod > -1)
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

        public int SelectedProizvodSaKolicinom
        {
            get => _selectedProizvodSaKolicinom;
            set
            {
                if (_selectedProizvodSaKolicinom == value)
                {
                    
                    if (_selectedProizvodSaKolicinom > -1)
                    {
                        RemoveEnabled = true;
                    }
                    else
                    {
                        RemoveEnabled = false;
                    }

                    return;
                }
                _selectedProizvodSaKolicinom = value;
                
                    if (_selectedProizvodSaKolicinom > -1)
                    {
                        RemoveEnabled = true;
                    }
                    else
                    {
                        RemoveEnabled = false;
                    }
                
            }
        }

        public ObservableCollection<Skladiste> Skladista { get => skladista; set { skladista = value; OnPropertyChanged("Skladista"); } }

        public string SkladisteForBind
        {
            get => skladisteForBind;
            set
            {
                string staro = skladisteForBind;
                skladisteForBind = value;
                Proizvodi.Clear();

                BezPDV = 0;
                SaPDV = 0;
                ProizvodiSaKolicinom.Clear();
                if (!String.IsNullOrEmpty(skladisteForBind))
                {
                    foreach (var item in dbContext.Zalihes.Where(x => x.Skladiste.naziv.Equals(skladisteForBind)))
                    {
                        
                            Proizvodi.Add(item.Proizvod);

                    }
                }
                
                OnPropertyChanged("SkladisteForBind");
            }
        }
        public ObservableCollection<PoslovniPartner> PoslovniPartneri { get => poslovniPartneri; set { poslovniPartneri = value; OnPropertyChanged("PoslovniPartneri"); } }

        public string PoslovniPartnerForBind
        {
            get => poslovniPatnerForBind;
            set
            {
                poslovniPatnerForBind = value;
                OnPropertyChanged("PoslovniPartnerForBind");
            }
        }

        

        public double BezPDV { get => bezPDV; set { bezPDV = value; OnPropertyChanged("BezPDV"); } }
        public double SaPDV { get => saPDV; set { saPDV = value; OnPropertyChanged("SaPDV"); } }

        public string CenaText { get => cenaText; set { cenaText = value; OnPropertyChanged("CenaText"); } }

        public double Pdv
        {
            get => pdv;
            set
            {
                pdv = value;
                ProfakturaForEdit.PDV= pdv;
                SaPDV = (1 + (pdv / 100)) * BezPDV;
                OnPropertyChanged("Pdv");
            }
        }

        public string RabatText { get => rabatText; set { rabatText = value; OnPropertyChanged("RabatText"); } }

        
        #endregion
    }
}
