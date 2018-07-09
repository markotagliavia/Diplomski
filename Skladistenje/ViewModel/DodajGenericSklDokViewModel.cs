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
using System.Windows.Forms;

namespace Skladistenje.ViewModel
{
    public class DodajGenericSklDokViewModel : BindableBase
    {
        #region Members
        private int? idFakture;
        private string tip;
        private Korisnik userOnSession;
        private string kolicinaText;
        private string rafText = "";
        private bool addEnabled;
        private bool removeEnabled;
        private int _selectedProizvodSaKolicinomLevo = -1;
        private int _selectedProizvodSaKolicinomDesno = -1;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ObservableCollection<ProizvodKolicina> proizvodiSaKolicinomLevo;
        private ObservableCollection<ProizvodKolicina> proizvodiSaKolicinomDesno;
        private SkladisteniDokument sklDokForBind;
        private string skladisteSourceForBind, skladisteDestForBind, sifraStornoForBind;
        private ObservableCollection<Skladiste> skladista;
        private ObservableCollection<SkladisteniDokument> sklDoks;
        private string sourceZalihe = "";
        private string destinationZalihe = "";
        private Visibility izdaoVisible, primioVisible, vozacVisible, regBrVisible, nacinOtpremeVisible, izSklVisible, uSklVisible, stornoVisible;
        private bool isEditable = true, isEditableIzdao = true, sifraEnabled = true;
        private Common.Model.Notification notification;

        #endregion

        #region Commands
        public MyICommand<object> DodajSklDokCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        public MyICommand<string> BackNavCommand { get; private set; }
        public MyICommand<int> AddCommand { get; private set; }
        public MyICommand<int> RemoveCommand { get; private set; }
        #endregion

        public DodajGenericSklDokViewModel() : this("STORNI")
        {
        }

        public DodajGenericSklDokViewModel(string tip, int? idDokumenta = null, Common.Model.Notification notification = null)
        {
            this.idFakture = idDokumenta;
            this.tip = tip;
            this.notification = notification;
            DodajSklDokCommand = new MyICommand<object>(DodajSklDok);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            BackNavCommand = new MyICommand<string>(Otkazi);
            AddCommand = new MyICommand<int>(Add);
            RemoveCommand = new MyICommand<int>(Remove);
            userOnSession = new Korisnik();
            proizvodiSaKolicinomLevo = new ObservableCollection<ProizvodKolicina>();
            proizvodiSaKolicinomDesno = new ObservableCollection<ProizvodKolicina>();
            skladista = new ObservableCollection<Skladiste>();
            sklDoks = new ObservableCollection<SkladisteniDokument>();
            sklDokForBind = new SkladisteniDokument();
            skladisteSourceForBind = "";
            skladisteDestForBind = "";
            KolicinaText = "";

            foreach (var item in dbContext.Skladistes)
            {
                skladista.Add(item);
            }

            if (tip == "INT_PR" || tip == "INT_OTP")   //interni
            {
                IzdaoVisible = Visibility.Visible;
                PrimioVisible = Visibility.Visible;
                VozacVisible = Visibility.Visible;
                RegBrVisible = Visibility.Visible;
                NacinOtpremeVisible = Visibility.Visible;
                IzSklVisible = Visibility.Visible;
                USklVisible = Visibility.Visible;
                StornoVisible = Visibility.Collapsed;
                SourceZalihe = "Svi Proizvodi : ";
                DestinationZalihe = "Izabrano : ";
                if (tip == "INT_PR")
                {
                    SklDokForBind.sifra = dbContext.SkladisteniDokuments.FirstOrDefault(x => x.id == notification.idDokumenta).sifra + "_INT_PR";
                    IsEditable = false;
                    SifraEnabled = false;
                    idFakture = idDokumenta;  //zbog dugmica
                    SkladisteniDokument otp = dbContext.SkladisteniDokuments.FirstOrDefault(x => x.id == idDokumenta);
                    SkladisteSourceForBind = otp.Skladiste.naziv;
                    SkladisteDestForBind = otp.Skladiste1.naziv;
                    SklDokForBind.vozac = otp.vozac;
                    SklDokForBind.regbr = otp.regbr;
                    SklDokForBind.nacinotpreme = otp.nacinotpreme;
                    foreach (var stavka in otp.StavkaSklDokumentas)
                    {
                        ProizvodKolicina pk = new ProizvodKolicina(stavka.Zalihe.Proizvod.naziv, stavka.Zalihe.Proizvod.sifra, stavka.kolicina.ToString());
                        ProizvodiSaKolicinomDesno.Add(pk);
                    }
                }
            }
            else if (tip == "SP_PR" || tip == "SP_OTP")  //spoljni
            {
                IzdaoVisible = Visibility.Visible;
                PrimioVisible = Visibility.Visible;
                VozacVisible = Visibility.Visible;
                RegBrVisible = Visibility.Visible;
                NacinOtpremeVisible = Visibility.Visible;
                IzSklVisible = Visibility.Visible;
                USklVisible = Visibility.Visible;
                StornoVisible = Visibility.Collapsed;
                SourceZalihe = "Svi Proizvodi : ";
                DestinationZalihe = "Izabrano : ";
            }
            else if (tip == "KOR_PR" || tip == "KOR_OTP") //korekcioni
            {
                IzdaoVisible = Visibility.Visible;
                PrimioVisible = Visibility.Hidden;
                VozacVisible = Visibility.Hidden;
                RegBrVisible = Visibility.Hidden;
                NacinOtpremeVisible = Visibility.Hidden;
                IzSklVisible = Visibility.Visible;
                USklVisible = Visibility.Hidden;
                StornoVisible = Visibility.Collapsed;
                SourceZalihe = "Svi Proizvodi : ";
                DestinationZalihe = "Izabrano : ";
            }
            else if (tip == "STORNI") //storni
            {
                isEditableIzdao = false;
                IsEditable = false;
                IzdaoVisible = Visibility.Visible;
                PrimioVisible = Visibility.Visible;
                VozacVisible = Visibility.Visible;
                RegBrVisible = Visibility.Visible;
                NacinOtpremeVisible = Visibility.Visible;
                IzSklVisible = Visibility.Visible;
                USklVisible = Visibility.Visible;
                StornoVisible = Visibility.Collapsed;
                SourceZalihe = "Svi Proizvodi : ";
                DestinationZalihe = "Izabrano : ";
                StornoVisible = Visibility.Visible;
                foreach (var item in dbContext.SkladisteniDokuments)
                {
                    if (item.tipredovnog == "INT_PR" || item.tipredovnog == "INT_OTP" || item.tipredovnog == "SP_PR" || tip == "SP_OTP")
                    {
                       if(item.active && item.storniranceo == false) sklDoks.Add(item);
                    }
                }
            }

            sklDokForBind.datum = DateTime.Now;
            foreach (Window w in System.Windows.Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    if (((MainWindowViewModel)((MainWindow)w).DataContext) != null)
                    {
                        UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                    }
                }
            }

            
        }
        #region Properties

        
        public Korisnik UserOnSession { get => userOnSession; set => userOnSession = value; }
        public SkladisteniDokument SklDokForBind { get => sklDokForBind; set { sklDokForBind = value; OnPropertyChanged("SklDokForBind"); } }
        public string SkladisteSourceForBind 
        {
            get => skladisteSourceForBind;
            set
            {
                if (tip == "INT_PR" || tip == "INT_OTP")
                {
                    if (value != null)
                    {
                        ProizvodiSaKolicinomLevo.Clear();
                        ProizvodiSaKolicinomDesno.Clear();
                        skladisteSourceForBind = value;
                        OnPropertyChanged("SkladisteSourceForBind");
                        populateProizvodiGrid(dbContext.Skladistes.FirstOrDefault(x=>x.naziv.Equals(value)));
                    }
                    else
                    {
                        ProizvodiSaKolicinomLevo.Clear();
                        ProizvodiSaKolicinomDesno.Clear();
                        skladisteSourceForBind = value;
                        OnPropertyChanged("SkladisteSourceForBind");
                    }
                }
                else
                {
                    skladisteSourceForBind = value;
                }
            }
        }

        public string SifraStornoForBind
        {
            get => sifraStornoForBind;
            set
            {
                if (value != "" && value != null)
                {
                    sifraStornoForBind = value;
                    OnPropertyChanged("SifraStornoForBind");
                    SkladisteniDokument s = dbContext.SkladisteniDokuments.FirstOrDefault(x => x.active == true && x.storniranceo == false && x.sifra == sifraStornoForBind);
                    SklDokForBind.primio = s.primio;
                    SklDokForBind.izdao = s.izdao;
                    SklDokForBind.nacinotpreme = s.nacinotpreme;
                    SklDokForBind.regbr = s.regbr;
                    SklDokForBind.vozac = s.vozac;
                    SklDokForBind.Skladiste.naziv = s.Skladiste.naziv;
                    SklDokForBind.Skladiste1.naziv = s.Skladiste1.naziv;
                    foreach(var stavka in s.StavkaSklDokumentas)
                    {
                        ProizvodiSaKolicinomDesno.Add(new ProizvodKolicina(stavka.Zalihe.Proizvod.naziv, stavka.Zalihe.Proizvod.sifra, stavka.kolicina.ToString()));
                    }
                }
                else
                {
                    sifraStornoForBind = value;
                    OnPropertyChanged("SifraStornoForBind");
                }
            }
        }

        public string SkladisteDestForBind
        {
            get => skladisteDestForBind;
            set
            {
                skladisteDestForBind = value;
                OnPropertyChanged("SkladisteDestForBind");
            }
        }

        public string KolicinaText { get => kolicinaText; set { kolicinaText = value; OnPropertyChanged("KolicinaText"); } }

        public ObservableCollection<Skladiste> Skladista
        {
            get => skladista;
            set
            {
                skladista = value;
                OnPropertyChanged("Skladista");
            }
        }

        public ObservableCollection<SkladisteniDokument> SklDoks
        {
            get => sklDoks;
            set
            {
                sklDoks = value;
                OnPropertyChanged("SklDoks");
            }
        }

        public bool RemoveEnabled
        {
            get => removeEnabled;
            set
            {
                if (idFakture != null || tip == "STORNI")
                {
                    removeEnabled = false;
                }
                else
                {
                    removeEnabled = value;
                    OnPropertyChanged("RemoveEnabled");
                }
            }
        }

        public bool AddEnabled
        {
            get => addEnabled;
            set
            {
                if (idFakture != null || tip == "STORNI")
                {
                    addEnabled = false;
                }
                else
                {
                    addEnabled = value;
                    OnPropertyChanged("AddEnabled");
                }
            }
        }

        public ObservableCollection<ProizvodKolicina> ProizvodiSaKolicinomLevo
        {
            get => proizvodiSaKolicinomLevo;
            set
            {
                proizvodiSaKolicinomLevo = value;
                OnPropertyChanged("ProizvodiSaKolicinomLevo");
            }
        }

        public ObservableCollection<ProizvodKolicina> ProizvodiSaKolicinomDesno
        {
            get => proizvodiSaKolicinomDesno;
            set
            {
                proizvodiSaKolicinomDesno = value;
                OnPropertyChanged("ProizvodiSaKolicinomDesno");
            }
        }

        public int SelectedProizvodSaKolicinomLevo
        {
            get => _selectedProizvodSaKolicinomLevo;
            set
            {
                _selectedProizvodSaKolicinomLevo = value;
                if (_selectedProizvodSaKolicinomLevo > -1)
                {
                    AddEnabled = true;
                }
                else
                {
                    AddEnabled = false;
                }
            }
        }

        public int SelectedProizvodSaKolicinomDesno
        {
            get => _selectedProizvodSaKolicinomDesno;
            set
            {
                _selectedProizvodSaKolicinomDesno = value;
                if (_selectedProizvodSaKolicinomDesno > -1)
                {
                    RemoveEnabled = true;
                }
                else
                {
                    RemoveEnabled = false;
                }
            }
        }

        public string SourceZalihe { get => sourceZalihe; set { sourceZalihe = value; OnPropertyChanged("SourceZalihe"); } }
        public string DestinationZalihe { get => destinationZalihe; set { destinationZalihe = value; OnPropertyChanged("DestinationZalihe"); } }
        public Visibility IzdaoVisible { get => izdaoVisible; set { izdaoVisible = value; OnPropertyChanged("IzdaoVisible"); }}
        public Visibility PrimioVisible { get => primioVisible; set { primioVisible = value; OnPropertyChanged("PrimioVisible"); }}
        public Visibility VozacVisible { get => vozacVisible; set { vozacVisible = value; OnPropertyChanged("VozacVisible"); }}
        public Visibility RegBrVisible { get => regBrVisible; set { regBrVisible = value; OnPropertyChanged("RegBrVisible"); }}
        public Visibility NacinOtpremeVisible { get => nacinOtpremeVisible; set { nacinOtpremeVisible = value; OnPropertyChanged("NacinOtpremeVisible"); }}
        public Visibility IzSklVisible { get => izSklVisible; set { izSklVisible = value; OnPropertyChanged("IzSklVisible"); }}
        public Visibility USklVisible { get => uSklVisible; set { uSklVisible = value; OnPropertyChanged("USklVisible"); }}
        public Visibility StornoVisible { get => stornoVisible; set { stornoVisible = value; OnPropertyChanged("StornoVisible"); } }
        public bool IsEditable { get => isEditable; set { isEditable = value; OnPropertyChanged("IsEditable"); } }
        public bool IsEditableIzdao { get => isEditableIzdao; set { isEditableIzdao = value; OnPropertyChanged("IsEditableIzdao"); } }
        public bool SifraEnabled { get => sifraEnabled; set { sifraEnabled = value; OnPropertyChanged("SifraEnabled"); } }
        #endregion


        #region CommandsImplementation

        private void Otkazi(string obj)
        {
            foreach (Window w in System.Windows.Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    if (tip == "INT_PR" || tip == "INT_OTP")   //interni
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("interni");
                    }
                    else if (tip == "SP_PR" || tip == "SP_OTP")  //spoljni
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("spoljni");
                    }
                    else if (tip == "KOR_PR" || tip == "KOR_OTP") //korekcioni
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("korekcioni");
                    }
                    else if (tip == "STORNI") //storni
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("storno");
                    }
                    
                }
            }
        }

        private void Remove(int obj)
        {
            if (SelectedProizvodSaKolicinomDesno != -1)
            {
                ProizvodKolicina p = ProizvodiSaKolicinomDesno.ElementAt(SelectedProizvodSaKolicinomDesno);
                ProizvodiSaKolicinomDesno.RemoveAt(SelectedProizvodSaKolicinomDesno);
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }

        private void Add(int index)
        {
            if (SelectedProizvodSaKolicinomLevo != -1)
            {
                ProizvodKolicina p = ProizvodiSaKolicinomLevo.ElementAt(SelectedProizvodSaKolicinomLevo);
                ProizvodKolicina pk = new ProizvodKolicina(p.Naziv, p.Sifra, KolicinaText);
                //TO DO validacija za kolicinu
                if (tip == "INT_OTP")
                {
                    //Proizvod p = Proizvodi.ElementAt(SelectedProizvod); treba doraditievo me, sta radis?
                    Double kolicina = Double.Parse(pk.Kolicina); 
                    Zalihe z = dbContext.Zalihes.FirstOrDefault(x => x.Proizvod.naziv == pk.Naziv && x.Skladiste.naziv == SkladisteSourceForBind); //aj nadji ovo skladiste, nemas tu proizvod id da, znam, resicemo, daj samo skladiste
                    if ((z.kolicina - z.rezervisano - z.minimumkolicine) < kolicina)
                    {
                        Notifications.Error e = new Notifications.Error("Na zalihama se ne nalazi dovoljno ovog proizvoda.");
                        e.Show();
                        return;
                    }
                }
                ProizvodiSaKolicinomDesno.Add(pk);
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }

        private void DodajSklDok(object obj)
        {
            //pitalica
            if (tip == "INT_PR" || tip == "INT_OTP")   //interni
            {
                if (tip == "INT_PR")
                {
                    if (!dbContext.ZaposleniSkladistas.Any(x => x.Skladiste.naziv.Equals(SkladisteDestForBind) && x.zaposleni_id == UserOnSession.zaposleni_id))
                    {
                        Error er = new Error("Niste zaposleni u ovom skladištu.");
                        er.Show();
                        return;
                    }
                }
                else
                {
                    if (!dbContext.ZaposleniSkladistas.Any(x => x.Skladiste.naziv.Equals(SkladisteSourceForBind) && x.zaposleni_id == UserOnSession.zaposleni_id))
                    {
                        Error er = new Error("Niste zaposleni u ovom skladištu.");
                        er.Show();
                        return;
                    }
                }

                if (SkladisteSourceForBind.Equals(SkladisteDestForBind))
                {
                    Error er = new Error("Izvorno i odredišno skladište su identični.");
                    er.Show();
                    return;
                }

                SkladisteniDokument sd = new SkladisteniDokument();
                sd.active = true;
                sd.nacinotpreme = sklDokForBind.nacinotpreme;
                sd.izdao = sklDokForBind.izdao;
                sd.primio = sklDokForBind.primio;
                sd.vozac = sklDokForBind.vozac;
                sd.skladiste_id = dbContext.Skladistes.FirstOrDefault(x => x.naziv.Equals(SkladisteSourceForBind)).id;//SkladisteSourceForBind.id;
                sd.skladiste_id1 = sd.zaposleniskladista_skladiste_id = dbContext.Skladistes.FirstOrDefault(x => x.naziv.Equals(SkladisteDestForBind)).id;//SkladisteDestForBind.id;
                sd.zaposleniskladista_zaposleni_id = dbContext.Zaposlenis.FirstOrDefault(x => x.active == true && x.Korisniks.Any(y => y.id == UserOnSession.id)).id;
                if (tip == "INT_PR") sd.zaposleniskladista_skladiste_id = (int)sd.skladiste_id1;   //ako je prijemnica, onda korisnik radi za dest skladiste jer se tu prima
                else if (tip == "INT_OTP") sd.zaposleniskladista_skladiste_id = (int)sd.skladiste_id; //ako je otpremnica onda korisnik radi za source jer se odatle otprema
                sd.poslovnipartner_mbr = null;
                sd.redovniskldok_id = null;
                sd.upripremi = false;
                sd.datum = sklDokForBind.datum;
                sd.redovni = true;
                sd.storniranceo = false;
                sd.tipredovnog = tip;
                sd.sifra = sklDokForBind.sifra;
                sd.regbr = sklDokForBind.regbr;
                dbContext.SkladisteniDokuments.Add(sd);

                if (tip == "INT_OTP")
                {
                    Common.Model.Notification n = new Common.Model.Notification();
                    n.adresa = sd.Skladiste1.naziv;
                    n.aplikacija = "Skladistenje";
                    n.obradjena = false;
                    n.procitana = false;
                    n.tekst = "Kreirana je nova otpremnica";
                    n.idDokumenta = dbContext.SkladisteniDokuments.FirstOrDefault(x => x.sifra == sd.sifra).id;
                    dbContext.Notifications.Add(n);
                    foreach (Window w in System.Windows.Application.Current.Windows)
                    {
                        if (w.GetType().Equals(typeof(MainWindow)))
                        {
                            ((MainWindow)w).ZvonceCrveno();
                        }
                    }

                    dbContext.SaveChanges();
                }
                ////ovo je ono o cemu smo pricali, da radnik mora biti zaposlen u skladistu da bi mogao da kreira dokument za to skladiste, ali ja jesam zaposlen u nisi u skladiste2,  pa onda ispada da moram biti u oba zaposlenne ne sad si pravio prjemnicu za skladiste u kom sam zaposlen (dest) jao ali nisi dobro kupio... 
                //stavke dodaj fali nam i uslov pre svega da li radi u tom skladistu za koje pravi dokument gde to da stavim
                int i = 1;
                foreach (ProizvodKolicina item in ProizvodiSaKolicinomDesno)
                {
                    StavkaSklDokumenta stavka = new StavkaSklDokumenta();
                    stavka.kolicina = Double.Parse(item.Kolicina);
                    stavka.rednibroj = i;
                    stavka.skladistenidokument_id = sd.id;
                    //stavka.stavkafakture_faktura_id = idFakture; 
                    stavka.storno = false;

                    if (tip == "INT_PR") stavka.zalihe_idskladista = (int)sd.skladiste_id1;   //ako je prijemnica, onda korisnik radi za dest skladiste jer se tu prima
                    else if (tip == "INT_OTP") stavka.zalihe_idskladista = (int)sd.skladiste_id; //ako je otpremnica onda korisnik radi za source jer se odatle otprema
                    stavka.zalihe_proizvod_id = dbContext.Proizvods.FirstOrDefault(x => x.sifra == item.Sifra).id;
                    dbContext.StavkaSklDokumentas.Add(stavka);
                    if (dbContext.Zalihes.Any(x => x.proizvod_id == stavka.zalihe_proizvod_id && x.skladiste_id == stavka.zalihe_idskladista))
                    {
                        if (tip == "INT_PR")
                        {
                            dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == stavka.zalihe_proizvod_id && x.skladiste_id == stavka.zalihe_idskladista).kolicina += stavka.kolicina;
                        }
                        else
                        {
                            dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == stavka.zalihe_proizvod_id && x.skladiste_id == stavka.zalihe_idskladista).kolicina -= stavka.kolicina;
                        }
                    }
                    else
                    {
                        dbContext.Zalihes.Add(new Zalihe() { kolicina = stavka.kolicina, proizvod_id = stavka.zalihe_proizvod_id, raf = "", minimumkolicine = 0, rezervisano = 0, skladiste_id = stavka.zalihe_idskladista });
                    }
                }
                if (notification != null)
                {
                    notification.obradjena = true;

                }
                dbContext.SaveChanges();//kako se stavke popune koje stavke dokumenta
                Success s = new Success("Uspešno ste dodali novi interni skladišni dokument.");
                s.Show();

            }
            else if (tip == "SP_PR" || tip == "SP_OTP")  //spoljni
            {
                //a ne tu pa kad testiras ovo da uradimo spoljnu otp hajde testiraj ovo 
            }
            else if (tip == "KOR_PR" || tip == "KOR_OTP") //korekcioni
            {
                //TO DO nema trenutno
            }
            else if (tip == "STORNI") //storni
            {
                if (SifraStornoForBind != null && SifraStornoForBind != "")
                {
                    SkladisteniDokument s = dbContext.SkladisteniDokuments.FirstOrDefault(x => x.active == true && x.storniranceo == false && x.sifra == sifraStornoForBind);

                    if (s.tipredovnog == "INT_PR" || s.tipredovnog == "INT_OTP")
                    {
                        DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Storniranje internog skladišnog dokumenta stornira i prijemnicu i otpremnicu. Da li zaista želite da stornirate?", "Upit", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            s.storniranceo = true;
                            if (s.tipredovnog == "INT_PR")
                            {
                                foreach (var stavka in s.StavkaSklDokumentas)
                                {
                                    dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == stavka.zalihe_proizvod_id && x.skladiste_id == stavka.zalihe_idskladista).kolicina -= stavka.kolicina;
                                    if (dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == stavka.zalihe_proizvod_id && x.skladiste_id == stavka.zalihe_idskladista).kolicina < 0)
                                    {
                                        Error e = new Error("Greška! Storniranjem stavki ovog dokumenta zalihe odlaze ispod dozvoljene količine.");
                                        e.Show();
                                        return;
                                    }
                                    stavka.storno = true;
                                }
                                //odgovarajucu otpremnicu sredi
                                SkladisteniDokument sPom = dbContext.SkladisteniDokuments.FirstOrDefault(x => x.active == true && x.sifra.Equals(x.sifra.Substring(0, x.sifra.Length - 7)));
                                foreach (var stavka1 in sPom.StavkaSklDokumentas)
                                {
                                    dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == stavka1.zalihe_proizvod_id && x.skladiste_id == stavka1.zalihe_idskladista).kolicina += stavka1.kolicina;
                                    stavka1.storno = true;
                                }
                            }
                            else if (s.tipredovnog == "INT_OTP")
                            {
                                foreach (var stavka in s.StavkaSklDokumentas)
                                {
                                    dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == stavka.zalihe_proizvod_id && x.skladiste_id == stavka.zalihe_idskladista).kolicina += stavka.kolicina;
                                    stavka.storno = true;
                                }
                                //odgovarajucu prijemnicu sredi
                                SkladisteniDokument sPom = dbContext.SkladisteniDokuments.FirstOrDefault(x => x.active == true && x.sifra.Equals(x.sifra + "_INT_PR"));
                                foreach (var stavka1 in sPom.StavkaSklDokumentas)
                                {
                                    dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == stavka1.zalihe_proizvod_id && x.skladiste_id == stavka1.zalihe_idskladista).kolicina -= stavka1.kolicina;
                                    if (dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == stavka1.zalihe_proizvod_id && x.skladiste_id == stavka1.zalihe_idskladista).kolicina < 0)
                                    {
                                        Error e = new Error("Greška! Storniranjem stavki ovog dokumenta zalihe odlaze ispod dozvoljene količine.");
                                        e.Show();
                                        return;
                                    }
                                    stavka1.storno = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        s.storniranceo = true;
                        if (s.tipredovnog == "SP_PR")
                        {
                            foreach (var stavka in s.StavkaSklDokumentas)
                            {
                                dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == stavka.zalihe_proizvod_id && x.skladiste_id == stavka.zalihe_idskladista).kolicina -= stavka.kolicina;
                                if (dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == stavka.zalihe_proizvod_id && x.skladiste_id == stavka.zalihe_idskladista).kolicina < 0)
                                {
                                    Error e = new Error("Greška! Storniranjem stavki ovog dokumenta zalihe odlaze ispod dozvoljene količine.");
                                    e.Show();
                                    return;
                                }
                                stavka.storno = true;
                            }
                        }
                        else if (s.tipredovnog == "SP_OTP")
                        {
                            foreach (var stavka in s.StavkaSklDokumentas)
                            {
                                dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == stavka.zalihe_proizvod_id && x.skladiste_id == stavka.zalihe_idskladista).kolicina += stavka.kolicina;
                                stavka.storno = true;
                            }
                        }
                    }

                    dbContext.SaveChanges();

                }
                else
                {
                    Error e = new Error("Izaberite skladišni dokument koji želite da stornirate.");
                    e.Show();
                }
            }
        }

        #endregion

        #region HelperMethods
        private void populateProizvodiGrid(Skladiste s)
        {
            foreach (var item in dbContext.Zalihes.ToList())
            {
                if(s.id == item.skladiste_id) proizvodiSaKolicinomLevo.Add(new ProizvodKolicina(item.Proizvod, item.kolicina.ToString(), item.raf));
            }
        }
        #endregion
    }
}
