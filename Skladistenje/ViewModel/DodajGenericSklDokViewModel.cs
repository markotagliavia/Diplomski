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
        private string skladisteSourceForBind, skladisteDestForBind;
        private ObservableCollection<Skladiste> skladista;
        private string sourceZalihe = "";
        private string destinationZalihe = "";
        private Visibility izdaoVisible, primioVisible, vozacVisible, regBrVisible, nacinOtpremeVisible, izSklVisible, uSklVisible;
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

        public DodajGenericSklDokViewModel(string tip, int? idFakture = null)
        {
            this.idFakture = idFakture;
            this.tip = tip;
            DodajSklDokCommand = new MyICommand<object>(DodajSklDok);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            BackNavCommand = new MyICommand<string>(Otkazi);
            AddCommand = new MyICommand<int>(Add);
            RemoveCommand = new MyICommand<int>(Remove);
            userOnSession = new Korisnik();
            proizvodiSaKolicinomLevo = new ObservableCollection<ProizvodKolicina>();
            proizvodiSaKolicinomDesno = new ObservableCollection<ProizvodKolicina>();
            skladista = new ObservableCollection<Skladiste>();
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
                SourceZalihe = "Svi Proizvodi : ";
                DestinationZalihe = "Izabrano : ";
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
                SourceZalihe = "Svi Proizvodi : ";
                DestinationZalihe = "Izabrano : ";
            }
            else if (tip == "STORNI") //storni
            {
                //ovo cu izmestiti
            }

            sklDokForBind.datum = DateTime.Now;
            foreach (Window w in Application.Current.Windows)
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
        public string SkladisteSourceForBind //nije, to je radilo kod popisa
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

        #endregion


        #region CommandsImplementation

        private void Otkazi(string obj)
        {
            foreach (Window w in Application.Current.Windows)
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
                        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("storni");
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
                    //Proizvod p = Proizvodi.ElementAt(SelectedProizvod); treba doraditi
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
                        Error er = new Error("Niste zaposleni u ovom skladistu.");
                        er.Show();
                        return;
                    }
                }
                else
                {
                    if (!dbContext.ZaposleniSkladistas.Any(x => x.Skladiste.naziv.Equals(SkladisteSourceForBind) && x.zaposleni_id == UserOnSession.zaposleni_id))
                    {
                        Error er = new Error("Niste zaposleni u ovom skladistu.");
                        er.Show();
                        return;
                    }
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
                if(tip == "INT_PR") sd.zaposleniskladista_skladiste_id = (int)sd.skladiste_id1;   //ako je prijemnica, onda korisnik radi za dest skladiste jer se tu prima
                else if(tip == "INT_OTP") sd.zaposleniskladista_skladiste_id = (int)sd.skladiste_id; //ako je otpremnica onda korisnik radi za source jer se odatle otprema
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
                dbContext.SaveChanges();
                ////ovo je ono o cemu smo pricali, da radnik mora biti zaposlen u skladistu da bi mogao da kreira dokument za to skladiste, ali ja jesam zaposlen u nisi u skladiste2,  pa onda ispada da moram biti u oba zaposlenne ne sad si pravio prjemnicu za skladiste u kom sam zaposlen (dest) jao ali nisi dobro kupio... 
                //stavke dodaj fali nam i uslov pre svega da li radi u tom skladistu za koje pravi dokument gde to da stavim
                int i = 1;
                foreach (ProizvodKolicina item in ProizvodiSaKolicinomDesno)
                {
                    StavkaSklDokumenta stavka = new StavkaSklDokumenta();
                    stavka.kolicina = Double.Parse(item.Kolicina);
                    stavka.rednibroj = i++;
                    stavka.skladistenidokument_id = sd.id;
                    stavka.stavkafakture_faktura_id = idFakture; //je l moze da se kreira interni na osnovu fakture?
                    stavka.storno = false;
                    stavka.stavkafakture_rednibroj = i;
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
                dbContext.SaveChanges();

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
                //ovo cu izmestiti
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
