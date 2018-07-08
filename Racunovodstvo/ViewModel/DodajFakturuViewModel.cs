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

namespace Racunovodstvo.ViewModel
{
    public class DodajFakturuViewModel: BindableBase
    {
        #region Members
        private int context;
        private Faktura fakturaForEdit;
        private Korisnik userOnSession = new Korisnik();
        private string skladisteForBind;
        private string poslovniPatnerForBind;
        private ObservableCollection<Skladiste> skladista;
        private ObservableCollection<PoslovniPartner> poslovniPartneri;
        private ObservableCollection<Proizvod> proizvodi;
        private ObservableCollection<ProizvodKolicina> proizvodiSaKolicinom;
        private int _selectedProizvod = -1;
        private int _selectedProizvodSaKolicinom = -1;
        private double bezPDV;
        private double saPDV;
        private double pdv;
        private bool addEnabled;
        private bool removeEnabled;
        private bool placanjeEnabled;
        private bool avansnoPlacanje;
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
        public DodajFakturuViewModel(int i, Faktura f)
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
                FakturaForEdit = new Faktura();
                FakturaForEdit.otpremljena = false;
                FakturaForEdit.likvidirano = false;
                FakturaForEdit.redovna = true;
                FakturaForEdit.avansnoplacanje = false;
                FakturaForEdit.upripremi = true;
                FakturaForEdit.ulazna = false;
                FakturaForEdit.datumfakturisanja = DateTime.Now;
                FakturaForEdit.datumprometadobara = DateTime.Now;
                FakturaForEdit.rokplacanja = DateTime.Now;
                FakturaForEdit.placeno = 0;
                AvansnoPlacanje = false;
                Pdv = 0;
                SaPDV = 0;
                BezPDV = 0;

            }
            else if (context == 1)
            {
                SubmitButtonText = "Potvrdi izmenu";
                BezPDV = 0;
                SaPDV = 0;
                FakturaForEdit = f;
                pdv = f?.pdv ?? 0;
                if (f.StavkaFaktures.Count > 0)
                {
                    SkladisteForBind = f.StavkaFaktures?.ElementAt(0).Zalihe.Skladiste.naziv ?? "";
                }
                else
                {
                    SkladisteForBind = "";
                }
                staroSkladiste = SkladisteForBind;
                PoslovniPartnerForBind = f.PoslovniPartner?.naziv ?? "";
                AvansnoPlacanje = (bool)f.avansnoplacanje;
                foreach (var item in f.StavkaFaktures.Where(x => x.storno == false))
                {

                    BezPDV += ((double)item.cena * item.kolicina) * (1 - ((double)(item.rabat) / 100));
                    SaPDV = (1 + (Pdv / 100)) * BezPDV;
                    ProizvodKolicina pk = new ProizvodKolicina(item.Zalihe.Proizvod,item.kolicina.ToString(),item.cena.ToString(),item.rabat.ToString());
                    ProizvodiSaKolicinom.Add(pk);
                    
                }
            }
            else if (context == 2)
            {
                SubmitButtonText = "Dodaj";
                SkladisteForBind = "";
                FakturaForEdit = new Faktura();
                FakturaForEdit.otpremljena = false;
                FakturaForEdit.likvidirano = false;
                FakturaForEdit.redovna = true;
                FakturaForEdit.avansnoplacanje = false;
                FakturaForEdit.upripremi = true;
                FakturaForEdit.ulazna = true;
                FakturaForEdit.datumfakturisanja = DateTime.Now;
                FakturaForEdit.datumprometadobara = DateTime.Now;
                FakturaForEdit.rokplacanja = DateTime.Now;
                FakturaForEdit.placeno = 0;
                AvansnoPlacanje = false;
                Pdv = 0;
                SaPDV = 0;
                BezPDV = 0;
            }
            else if (context == 3)
            {
                SubmitButtonText = "Potvrdi izmenu";
                BezPDV = 0;
                SaPDV = 0;
                FakturaForEdit = f;
                pdv = f?.pdv ?? 0;
                if (f.StavkaFaktures.Count > 0)
                {
                    SkladisteForBind = f.StavkaFaktures?.ElementAt(0).Zalihe.Skladiste.naziv ?? "";
                }
                else
                {
                    SkladisteForBind = "";
                }
                staroSkladiste = SkladisteForBind;
                PoslovniPartnerForBind = f.PoslovniPartner?.naziv ?? "";
                AvansnoPlacanje = (bool)f.avansnoplacanje;
                foreach (var item in f.StavkaFaktures.Where(x => x.storno == false))
                {
                    BezPDV += ((double)item.cena * item.kolicina) * (1 - ((double)item.rabat / 100));
                    SaPDV = (1 + (Pdv / 100)) * BezPDV;
                    ProizvodKolicina pk = new ProizvodKolicina(item.Zalihe.Proizvod, item.kolicina.ToString(), item.cena.ToString(), item.rabat.ToString());
                    ProizvodiSaKolicinom.Add(pk);

                }
            }
            
            
        }
        
        #region Commands Implements
        private void Otkazi(string obj)
        {
            if ((context == 0 || context == 1) && !SkladisteForBind.Equals(""))
            {
                if (context == 0)
                {
                    foreach (var item in ProizvodiSaKolicinom)
                    {
                        Zalihe z = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind));
                        z.rezervisano -= Double.Parse(item.Kolicina);

                    }
                }
                else
                {
                    if (staroSkladiste.Equals(SkladisteForBind))
                    {
                       
                        foreach (var item in ProizvodiSaKolicinom)
                        {
                            Zalihe z = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind));
                            z.rezervisano -= Double.Parse(item.Kolicina);
                            
                        }
                        Faktura f = dbContext.Fakturas.FirstOrDefault(x => x.id == FakturaForEdit.id);
                        foreach (var item in f.StavkaFaktures)
                        {
                            Zalihe z = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.id.Equals(item.zalihe_proizvod_id) && x.Skladiste.naziv.Equals(SkladisteForBind));
                            
                            z.rezervisano += item.kolicina;
                           
                        }
                        
                    }
                    
                }
                
                dbContext.SaveChanges();
            }
            if (context == 0 || context == 1)
            {
                MainWindowViewModel.Instance.OnNav(Navigation.izlazna);
            }
            else
            {
                MainWindowViewModel.Instance.OnNav(Navigation.ulazna);
            }
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        if (context == 0 || context == 1)
            //        {
            //            ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("izlazna");
            //        }
            //        else
            //        {
            //            ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("ulazna");
            //        }

            //    }
            //}
        }
        private void Back(string obj)
        {
            Otkazi("");
        }
        private void Dodaj(string obj)
        {
            UserOnSession = MainWindowViewModel.Instance.UserOnSession;
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
            //        UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;

            //    }
            //}
            if (context == 0)
            {
                try
                {
                    if(SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id,SecurityManager.Permission.AddIzlazna))
                    {
                        FakturaForEdit.active = true;
                        Zaposleni z = dbContext.Korisniks.FirstOrDefault(x => x.korisnickoime.Equals(UserOnSession.korisnickoime)).Zaposleni;
                        FakturaForEdit.zaposleni_id = z.id;
                        PoslovniPartner pp = dbContext.PoslovniPartners.FirstOrDefault(x => x.naziv.Equals(PoslovniPartnerForBind));
                        FakturaForEdit.PoslovniPartner = pp;
                        FakturaForEdit.likvidirano = false;
                        FakturaForEdit.upripremi = false;
                        FakturaForEdit.datumfakturisanja += new TimeSpan(0, 0, 0);
                        FakturaForEdit.datumprometadobara += new TimeSpan(0, 0, 0);
                        FakturaForEdit.rokplacanja += new TimeSpan(0, 0, 0);
                        dbContext.Fakturas.Add(FakturaForEdit);
                        dbContext.SaveChanges();
                        int i = 1;
                        foreach (var item in ProizvodiSaKolicinom)
                        {
                            StavkaFakture st = new StavkaFakture();
                            st.rednibroj = i;
                            st.storno = false;
                            st.kolicina = Double.Parse(item.Kolicina);
                            st.rabat = Double.Parse(item.Rabat);
                            st.cena = Double.Parse(item.Cena);
                            st.zalihe_proizvod_id = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind)).proizvod_id;
                            st.zalihe_skladiste_id = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind)).skladiste_id;
                            st.Faktura = dbContext.Fakturas.FirstOrDefault(x => x.oznaka.Equals(FakturaForEdit.oznaka));
                            dbContext.StavkaFaktures.Add(st);
                            dbContext.SaveChanges();
                            ++i;
                        }
                        Back("");
                        Notifications.Success s = new Notifications.Success("Uspešno ste kreirali izlaznu fakturu");
                        s.Show();
                        Common.Model.Notification n = new Common.Model.Notification();
                        n.aplikacija = "Skladistenje";
                        n.adresa = SkladisteForBind;
                        n.obradjena = false;
                        n.procitana = false;
                        n.tekst = $"Kreirana je Izlazna faktura {FakturaForEdit.oznaka}";
                        dbContext.Notifications.Add(n);
                        dbContext.SaveChanges();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, $"Uspesno je kreirana izlazna faktura {FakturaForEdit.oznaka}", "Info");
                    }
                    else
                    {
                        foreach (var item in ProizvodiSaKolicinom)
                        {
                            Zalihe z = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind));
                            z.rezervisano -= Double.Parse(item.Kolicina);

                        }
                        dbContext.SaveChanges();
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj kreiranja izlazne fakture", "Upozorenje");
                        Back("");
                    }
                    
                }
                catch (Exception ex)
                {
                    try
                    {
                        foreach (var item in ProizvodiSaKolicinom)
                        {
                            Zalihe z = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind));
                            z.rezervisano -= Double.Parse(item.Kolicina);

                        }
                        dbContext.SaveChanges();
                        Notifications.Error e = new Notifications.Error("Greška pri kreiranju fakture.");
                        e.Show();
                        Back("");
                    }
                    catch (Exception exc)
                    {
                        Error er = new Error("Greška sa konekcijom!\nObratite se administratorima.");
                        er.Show();
                    }

                }
            }
            else if (context == 1)
            {
                try
                {
                    if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.EditIzlazna))
                    {
                        var original = dbContext.Fakturas.FirstOrDefault(x => x.id == FakturaForEdit.id);

                        if (original != null)
                        {
                            
                            original.oznaka = FakturaForEdit.oznaka;
                            original.datumfakturisanja = FakturaForEdit.datumfakturisanja;
                            original.datumprometadobara = FakturaForEdit.datumprometadobara;
                            original.rokplacanja = FakturaForEdit.rokplacanja;
                            original.datumfakturisanja += new TimeSpan(0, 0, 0);
                            original.datumprometadobara += new TimeSpan(0, 0, 0);
                            original.rokplacanja += new TimeSpan(0, 0, 0);
                            original.pdv = FakturaForEdit.pdv;
                            original.placeno = FakturaForEdit.placeno;
                            original.avans = FakturaForEdit.avans;
                            original.avansnoplacanje = FakturaForEdit.avansnoplacanje;
                            original.poslovnipartner_mbr = dbContext.PoslovniPartners.FirstOrDefault(x => x.naziv.Equals(PoslovniPartnerForBind)).mbr;
                            int i = 1;
                            original.StavkaFaktures.Clear();
                            foreach (var item in ProizvodiSaKolicinom)
                            {
                                StavkaFakture st = new StavkaFakture();
                                st.rednibroj = i;
                                st.storno = item.Storno;
                                st.kolicina = Double.Parse(item.Kolicina);
                                st.rabat = Double.Parse(item.Rabat);
                                st.cena = Double.Parse(item.Cena);
                                st.zalihe_proizvod_id = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind)).proizvod_id;
                                st.zalihe_skladiste_id = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind)).skladiste_id;
                                st.Faktura = dbContext.Fakturas.FirstOrDefault(x => x.oznaka.Equals(FakturaForEdit.oznaka));
                                original.StavkaFaktures.Add(st);
                                
                                ++i;
                            }

                        }
                        dbContext.SaveChanges();
                        Notifications.Success s = new Notifications.Success("Uspešno ste izmenili fakturu.");
                        s.Show();
                        SecurityManager.AuditManager.AuditToDB(MainWindowViewModel.Instance.UserOnSession.korisnickoime, "Uspesno je izmenjena faktura " + FakturaForEdit.id, "Info");
                        MainWindowViewModel.Instance.OnNav(Navigation.izlazna);
                        
                        
                    }
                    else
                    {
                        
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj kreiranja izlazne fakture", "Upozorenje");
                        Back("");
                    }
                }
                catch (Exception)
                {
                    
                    Error er = new Error("Greška sa konekcijom!\nObratite se administratorima.");
                    er.Show();
                    
                }
            }
            else if (context == 2)
            {
                try
                {
                    if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.AddUlazna))
                    {
                        FakturaForEdit.active = true;
                        Zaposleni z = dbContext.Korisniks.FirstOrDefault(x => x.korisnickoime.Equals(UserOnSession.korisnickoime)).Zaposleni;
                        FakturaForEdit.zaposleni_id = z.id;
                        PoslovniPartner pp = dbContext.PoslovniPartners.FirstOrDefault(x => x.naziv.Equals(PoslovniPartnerForBind));
                        FakturaForEdit.PoslovniPartner = pp;
                        FakturaForEdit.likvidirano = false;
                        FakturaForEdit.upripremi = false;
                        
                        
                        int i = 1;
                        foreach (var item in ProizvodiSaKolicinom)
                        {
                            StavkaFakture st = new StavkaFakture();
                            st.rednibroj = i;
                            st.kolicina = Double.Parse(item.Kolicina);
                            st.rabat = Double.Parse(item.Rabat);
                            st.storno = false;
                            st.cena = Double.Parse(item.Cena);
                            st.zalihe_proizvod_id = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind)).proizvod_id;
                            st.zalihe_skladiste_id = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind)).skladiste_id;
                            FakturaForEdit.StavkaFaktures.Add(st);
                            ++i;
                        }
                        dbContext.Fakturas.Add(FakturaForEdit);
                        dbContext.SaveChanges();
                        Back("");
                        Notifications.Success s = new Notifications.Success("Uspešno ste kreirali ulaznu fakturu");
                        s.Show();
                        Common.Model.Notification n = new Common.Model.Notification();
                        n.aplikacija = "Skladistenje";
                        n.adresa = SkladisteForBind;
                        n.obradjena = false;
                        n.procitana = false;
                        n.tekst = $"Kreirana je Ulazna faktura {FakturaForEdit.oznaka}";
                        dbContext.Notifications.Add(n);
                        dbContext.SaveChanges();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, $"Uspesno je kreirana ulazna faktura {FakturaForEdit.oznaka}", "Info");
                    }
                    else
                    {
                        
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj kreiranja ulazne fakture", "Upozorenje");
                        Back("");
                    }
                }
                catch (Exception)
                {
                    Error er = new Error("Greška sa konekcijom!\nObratite se administratorima.");
                    er.Show();
                }
            }
            else if (context == 3)
            {
                try
                {
                    if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.EditUlazna))
                    {
                        var original = dbContext.Fakturas.FirstOrDefault(x => x.id == FakturaForEdit.id);

                        if (original != null)
                        {
                            original.oznaka = FakturaForEdit.oznaka;
                            original.datumfakturisanja = FakturaForEdit.datumfakturisanja;
                            original.datumprometadobara = FakturaForEdit.datumprometadobara;
                            original.rokplacanja = FakturaForEdit.rokplacanja;
                            original.datumfakturisanja += new TimeSpan(0, 0, 0);
                            original.datumprometadobara += new TimeSpan(0, 0, 0);
                            original.rokplacanja += new TimeSpan(0, 0, 0);
                            original.pdv = FakturaForEdit.pdv;
                            original.placeno = FakturaForEdit.placeno;
                            original.avans = FakturaForEdit.avans;
                            original.avansnoplacanje = FakturaForEdit.avansnoplacanje;
                            original.poslovnipartner_mbr = dbContext.PoslovniPartners.FirstOrDefault(x => x.naziv.Equals(PoslovniPartnerForBind)).mbr;
                            int i = 1;
                            original.StavkaFaktures.Clear();
                            foreach (var item in ProizvodiSaKolicinom)
                            {
                                StavkaFakture st = new StavkaFakture();
                                st.rednibroj = i;
                                st.kolicina = Double.Parse(item.Kolicina);
                                st.rabat = Double.Parse(item.Rabat);
                                st.cena = Double.Parse(item.Cena);
                                st.storno = item.Storno;
                                st.zalihe_proizvod_id = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind)).proizvod_id;
                                st.zalihe_skladiste_id = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(SkladisteForBind)).skladiste_id;
                                st.Faktura = dbContext.Fakturas.FirstOrDefault(x => x.oznaka.Equals(FakturaForEdit.oznaka));
                                original.StavkaFaktures.Add(st);

                                ++i;
                            }

                        }
                        dbContext.SaveChanges();
                        Notifications.Success s = new Notifications.Success("Uspešno ste izmenili fakturu.");
                        s.Show();
                        SecurityManager.AuditManager.AuditToDB(MainWindowViewModel.Instance.UserOnSession.korisnickoime, "Uspesno je izmenjena faktura " + FakturaForEdit.id, "Info");
                        MainWindowViewModel.Instance.OnNav(Navigation.ulazna);
                        //foreach (Window w in Application.Current.Windows)
                        //{
                        //    if (w.GetType().Equals(typeof(MainWindow)))
                        //    {
                        //        SecurityManager.AuditManager.AuditToDB(((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime, "Uspesno je izmenjena faktura " + FakturaForEdit.id, "Info");
                        //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("ulazna");
                        //    }
                        //}
                    }
                }
                catch (Exception)
                {
                    Error er = new Error("Greška sa konekcijom!\nObratite se administratorima.");
                    er.Show();
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
                if (context == 0 || context == 1)
                {
                    z.rezervisano -= kolicina;
                    dbContext.SaveChanges();
                }
                
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
                if (!KolicinaText.Equals("") && KolicinaText!= null && CenaText !=null && !String.IsNullOrWhiteSpace(KolicinaText) && !CenaText.Equals("") && !String.IsNullOrWhiteSpace(CenaText))
                {
                    Proizvod p = Proizvodi.ElementAt(SelectedProizvod);
                    Double kolicina = Double.Parse(KolicinaText);
                    Zalihe z = dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == p.id && x.Skladiste.naziv.Equals(SkladisteForBind));
                    if (context == 0 || context == 1)
                    {
                        if ((z.kolicina - z.rezervisano - z.minimumkolicine) >= kolicina)
                        {

                            ProizvodKolicina pk;
                            if (RabatText.Equals(""))
                            {
                                pk = new ProizvodKolicina(p, KolicinaText, CenaText,"0");
                                ProizvodiSaKolicinom.Add(pk);
                                BezPDV += (Double.Parse(CenaText) * kolicina);
                            }
                            else
                            {
                                pk = new ProizvodKolicina(p, KolicinaText, CenaText, RabatText);
                                ProizvodiSaKolicinom.Add(pk);
                                BezPDV += (Double.Parse(CenaText) * kolicina) * (1 - (Double.Parse(RabatText) / 100));
                            }
                            
                            
                            SaPDV = (1 + (Pdv / 100)) * BezPDV;
                            z.rezervisano += kolicina;
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            Notifications.Error e = new Notifications.Error("Na zalihama se ne nalazi dovoljno ovog proizvoda.");
                            e.Show();
                        }
                    }
                    else
                    {
                        ProizvodKolicina pk = new ProizvodKolicina(p, KolicinaText, CenaText,RabatText);
                        ProizvodiSaKolicinom.Add(pk);
                        BezPDV += (Double.Parse(CenaText) * kolicina) * (1 - (Double.Parse(RabatText) / 100));
                        SaPDV = (1 + (Pdv / 100)) * BezPDV;

                    }
                }
                else
                {
                    Notifications.Error e = new Notifications.Error("Morate uneti cenu i količinu.");
                    e.Show();
                }


            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }
        #endregion

        #region Constructors
        public string SubmitButtonText { get => submitButtonText; set { submitButtonText = value; OnPropertyChanged("SubmitButtonText"); } }
        public Korisnik UserOnSession { get { return userOnSession; } set { userOnSession = value; } }
        public Faktura FakturaForEdit { get => fakturaForEdit; set { fakturaForEdit = value; OnPropertyChanged("FakturaForEdit"); } }
        public bool RemoveEnabled
        {
            get => removeEnabled;
            set
            {
                if (FakturaForEdit.otpremljena != null)
                {
                    if (!(bool)FakturaForEdit.otpremljena) removeEnabled = value;
                    else removeEnabled = false;
                }
                    
                OnPropertyChanged("RemoveEnabled");
            }
        }

        public bool AddEnabled
        {
            get => addEnabled;
            set
            {
                if (FakturaForEdit.otpremljena != null)
                {
                    if (!(bool)FakturaForEdit.otpremljena) addEnabled = value;
                    else addEnabled = false;
                }
                OnPropertyChanged("AddEnabled");
                
            }
        }

        public bool PlacanjeEnabled
        {
            get => placanjeEnabled;
            set
            {
                placanjeEnabled = value;
                if (!placanjeEnabled)
                {
                    FakturaForEdit.avans = 0;
                }
                OnPropertyChanged("PlacanjeEnabled");

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
                    if (FakturaForEdit.otpremljena != null)
                    {
                        if (!(bool)FakturaForEdit.otpremljena)
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
                    }
                        
                    return;
                }
                _selectedProizvod = value;
                if (FakturaForEdit.otpremljena != null)
                {
                    if (!(bool)FakturaForEdit.otpremljena)
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
                    if (FakturaForEdit.otpremljena != null)
                    {
                        if (!(bool)FakturaForEdit.otpremljena)
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
                    }
                    
                       
                    
                    return;
                }
                _selectedProizvodSaKolicinom = value;
                if (!(bool)FakturaForEdit.otpremljena)
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

        public ObservableCollection<Skladiste> Skladista { get => skladista; set { skladista = value; OnPropertyChanged("Skladista"); } }

        public string SkladisteForBind
        {
            get => skladisteForBind;
            set {

                //izmeniti proizvode...
                string staro = skladisteForBind;
                skladisteForBind = value;
                Proizvodi.Clear();
                if ((context == 0 || context == 1) && staro != null)
                {
                    if (!staro.Equals(""))
                    {
                        foreach (var item in ProizvodiSaKolicinom)
                        {
                            Zalihe z = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.sifra.Equals(item.Sifra) && x.Skladiste.naziv.Equals(staro));
                            z.rezervisano -= Double.Parse(item.Kolicina);

                        }
                        dbContext.SaveChanges();
                    }
                    
                }
                
                BezPDV = 0;
                SaPDV = 0;
                ProizvodiSaKolicinom.Clear();
                foreach (var item in dbContext.Zalihes)
                {
                    if (item.Skladiste.naziv.Equals(skladisteForBind))
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

        public bool AvansnoPlacanje
        {
            get => avansnoPlacanje;
            set
            {
                avansnoPlacanje = value;
                FakturaForEdit.avansnoplacanje = value;
                if (avansnoPlacanje == true)
                {
                    PlacanjeEnabled = true;
                }
                else
                {
                    PlacanjeEnabled = false;
                    FakturaForEdit.avans = 0;
                }
                OnPropertyChanged("AvansnoPlacanje");
            }
        }

        public double BezPDV { get => bezPDV; set { bezPDV = value; OnPropertyChanged("BezPDV"); } }
        public double SaPDV { get => saPDV; set { saPDV = value; OnPropertyChanged("SaPDV");  } }

        public string CenaText { get => cenaText; set { cenaText = value; OnPropertyChanged("CenaText"); } }

        public double Pdv
        {
            get => pdv;
            set
            {
                pdv = value;
                FakturaForEdit.pdv = pdv;
                SaPDV = (1 + (pdv / 100)) * BezPDV;
                OnPropertyChanged("Pdv");
            }
        }

        public string RabatText { get => rabatText; set { rabatText = value; OnPropertyChanged("RabatText"); } }

        public DateTime Datumfakturisanja
        {
            get => fakturaForEdit.datumfakturisanja.Value.Date;
            set
            {
                fakturaForEdit.datumfakturisanja = value;
                OnPropertyChanged("Datumfakturisanja");
            }
        }
        #endregion
    }
    public class ProizvodKolicina : BindableBase
    {
        private string naziv;
        private string sifra;
        private string kolicina;
        private string cena;
        private string rabat;
        private bool storno;
        private int id;
        private Faktura faktura;
        private Zalihe zalihe;
        
        public ProizvodKolicina(Proizvod p, string kolicina, string cena, string rabat)
        {
            
            this.Sifra = p.sifra;
            this.Naziv = p.naziv;
            this.kolicina = kolicina;
            this.cena = cena;
            this.rabat = rabat;
            this.storno = false;
            
            
        }

        public string Naziv { get => naziv; set { naziv = value; OnPropertyChanged("Naziv"); } }
        public string Sifra { get => sifra; set { sifra = value; OnPropertyChanged("Sifra"); } }
        public string Kolicina { get => kolicina; set { kolicina = value; OnPropertyChanged("Kolicina"); } }
        public bool Storno { get => storno; set { storno = value; OnPropertyChanged("Storno"); } }
        public string Cena { get => cena; set { cena = value; OnPropertyChanged("Cena"); } }
        public string Rabat { get => rabat; set { rabat = value; OnPropertyChanged("Rabat"); } }
        public int Id { get => id; set { id = value; OnPropertyChanged("Id"); } }

        
        public Faktura Faktura { get => faktura; set => faktura = value; }
 
        public Zalihe Zalihe { get => zalihe; set => zalihe = value; }
    }
}
