using Common;
using Common.Model;
using Notifications;
using Skladistenje.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Skladistenje.ViewModel
{
    public class PripisOtpisViewModel : BindableBase
    {
        #region Members
        private string sifraForBind = "";
        private Visibility dokumentVisible;
        private Korisnik userOnSession;
        private int idPopisa;
        private System.Windows.Media.Color c1;
        private System.Windows.Media.Brush _backgroundColor;
        private DeltaEximEntities dbContext = new DeltaEximEntities();
        private string labelText = "";
        private string dodajText = "";
        private ObservableCollection<StavkaPopisa> stavke;
        private bool imaRazlike;
        #endregion

        #region Commands
        public MyICommand<object> DodajCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        #endregion

        public PripisOtpisViewModel(int idPopisa)
        {
            DokumentVisible = Visibility.Hidden;
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                }
            }
            imaRazlike = false;         //0-nema razlike 1-ima razlike
            this.idPopisa = idPopisa;
            c1 = System.Windows.Media.Color.FromArgb(255, 68, 95, 245);
            BackgroundColor = new SolidColorBrush(c1);
            DodajCommand = new MyICommand<object>(Dodaj);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            Stavke = new ObservableCollection<StavkaPopisa>();
            populateGrid(idPopisa);
            if (!imaRazlike)
            {
                LabelText = "Prilikom popisa utvrđeno je isto stanje kao na zalihama.";
                DodajText = "OK";
            }
            else if (imaRazlike)
            {
                LabelText = "Prilikom popisa utvrđeno je različito stanje u odnosu na zalihe ovog skladišta. Da li želite da dodate korekcione dokumente za ovaj popis?";
                DodajText = "Dodaj";
            }
        }

        #region Constructors
        public Korisnik UserOnSession { get => userOnSession; set => userOnSession = value; }

        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            }
        }

        public string LabelText { get => labelText; set { labelText = value; OnPropertyChanged("LabelText"); } }

        public string DodajText { get => dodajText; set { dodajText = value; OnPropertyChanged("DodajText"); } }

        public ObservableCollection<StavkaPopisa> Stavke { get => stavke; set => stavke = value; }
        public string SifraForBind { get => sifraForBind; set { sifraForBind = value; OnPropertyChanged("SifraForBind"); } }
        public Visibility DokumentVisible { get => dokumentVisible; set { dokumentVisible = value; OnPropertyChanged("DokumentVisible"); } }
        #endregion

        #region CommandsImplementation

        private void Otkazi(string obj)
        {
            dbContext.Popis.FirstOrDefault(x => x.id == idPopisa).Zaposlenis.Clear();
            var itemsToDelete = dbContext.Popis.Where(x => x.id == idPopisa);
            dbContext.Popis.RemoveRange(itemsToDelete);
            dbContext.Popis.Remove(dbContext.Popis.FirstOrDefault(x => x.id == idPopisa));
            dbContext.SaveChanges();
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(PripisOtpisView)))
                {
                    w.Close();
                }
            }
        }

        private void Dodaj(object obj)
        {
            if (!imaRazlike)
            {
                Success s = new Success("Novi popis je uspešno dodat.");
                s.Show();
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("popisi");
                    }
                    else if (w.GetType().Equals(typeof(PripisOtpisView)))
                    {
                        w.Close();
                    }
                }
            }
            else if (imaRazlike)
            {
                Korekcija(idPopisa);
                Success s = new Success("Novi popis sa pratećim korekcionim dokumentima je uspešno dodat.");
                s.Show();
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("popisi");
                    }
                    else if (w.GetType().Equals(typeof(PripisOtpisView)))
                    {
                        w.Close();
                    }
                }
            }
        }

        #endregion

        #region HelperMethods
        private void Korekcija(int id)
        {
            Popi p = dbContext.Popis.FirstOrDefault(x => x.id == id);
            List<Zalihe> zalihe = dbContext.Zalihes.Where(x => x.skladiste_id == p.skladiste_id).ToList();

            List<StavkaPopisa> pripisStavke = new List<StavkaPopisa>();
            List<StavkaPopisa> otpisStavke = new List<StavkaPopisa>();

            foreach (var item in dbContext.StavkaPopisas.ToList())
            {
                if (item.popis_id == id)
                {
                    if (zalihe.Any(x => x.proizvod_id == item.proizvod_id))
                    {
                        Zalihe zPom = zalihe.FirstOrDefault(x => x.proizvod_id == item.proizvod_id);
                        dbContext.Zalihes.FirstOrDefault(x => x.skladiste_id == p.skladiste_id && x.proizvod_id == item.proizvod_id).kolicina += item.kolicina;
                        if (item.kolicina > 0)
                        {
                            pripisStavke.Add(item);
                        }
                        else if (item.kolicina < 0)
                        {
                            otpisStavke.Add(item);
                        }
                    }
                    else
                    {
                        Zalihe zPom = new Zalihe();
                        zPom.kolicina = item.kolicina;
                        zPom.minimumkolicine = 0;
                        zPom.proizvod_id = item.proizvod_id;
                        zPom.raf = item.raf;
                        zPom.rezervisano = 0;
                        zPom.skladiste_id = item.skladiste_id;
                        dbContext.Zalihes.Add(zPom);
                        pripisStavke.Add(item);
                    }
                }
            }

            if (pripisStavke.Count > 0)
            {
                SkladisteniDokument sd = new SkladisteniDokument();
                sd.active = true;
                sd.nacinotpreme = "";
                sd.izdao = "";
                sd.primio = "";
                sd.vozac = "";
                sd.zaposleniskladista_skladiste_id = p.skladiste_id;
                sd.zaposleniskladista_zaposleni_id = dbContext.Zaposlenis.FirstOrDefault(x => x.active == true && x.Korisniks.Any(y => y.id == UserOnSession.id)).id;
                sd.poslovnipartner_mbr = -1;
                sd.redovniskldok_id = -1;
                sd.upripremi = false;
                sd.datum = p.datum;
                sd.redovni = true;
                sd.storniranceo = false;
                sd.tipredovnog = "KOR";
                sd.skladiste_id = p.skladiste_id;
                sd.sifra = "Pripis" + SifraForBind;
                sd.regbr = "";
                dbContext.SkladisteniDokuments.Add(sd);
                dbContext.SaveChanges();

                int j = 1;
                foreach (var item in pripisStavke)
                {
                    StavkaSklDokumenta ssd = new StavkaSklDokumenta();
                    ssd.kolicina = item.kolicina;
                    ssd.rednibroj = j++;
                    ssd.storno = false;
                    ssd.zalihe_idskladista = p.skladiste_id;
                    ssd.zalihe_proizvod_id = item.proizvod_id;
                    ssd.skladistenidokument_id = dbContext.SkladisteniDokuments.FirstOrDefault(x => x.sifra.Equals(sd.sifra)).id;
                    dbContext.StavkaSklDokumentas.Add(ssd);
                    dbContext.SaveChanges();
                }
            }

            if (otpisStavke.Count > 0)
            {
                SkladisteniDokument sd = new SkladisteniDokument();
                sd.active = true;
                sd.nacinotpreme = "";
                sd.izdao = "";
                sd.primio = "";
                sd.vozac = "";
                sd.zaposleniskladista_skladiste_id = p.skladiste_id;
                sd.zaposleniskladista_zaposleni_id = dbContext.Zaposlenis.FirstOrDefault(x => x.active == true && x.Korisniks.Any(y => y.id == UserOnSession.id)).id;
                sd.poslovnipartner_mbr = -1;
                sd.redovniskldok_id = -1;
                sd.upripremi = false;
                sd.datum = p.datum;
                sd.redovni = true;
                sd.storniranceo = false;
                sd.tipredovnog = "KOR";
                sd.skladiste_id = p.skladiste_id;
                sd.sifra = "Otpis" + SifraForBind;
                sd.regbr = "";
                dbContext.SkladisteniDokuments.Add(sd);
                dbContext.SaveChanges();

                int j = 1;
                foreach (var item in otpisStavke)
                {
                    StavkaSklDokumenta ssd = new StavkaSklDokumenta();
                    ssd.kolicina = item.kolicina;
                    ssd.rednibroj = j++;
                    ssd.storno = false;
                    ssd.zalihe_idskladista = p.skladiste_id;
                    ssd.zalihe_proizvod_id = item.proizvod_id;
                    ssd.skladistenidokument_id = dbContext.SkladisteniDokuments.FirstOrDefault(x => x.sifra.Equals(sd.sifra)).id;
                    dbContext.StavkaSklDokumentas.Add(ssd);
                    dbContext.SaveChanges();
                }
            }
        }

        private void populateGrid(int id)
        {
            Popi p = dbContext.Popis.FirstOrDefault(x => x.id == id);
            List<Zalihe> zalihe = dbContext.Zalihes.Where(x => x.skladiste_id == p.skladiste_id).ToList();
            List<StavkaPopisa> pomocna = new List<StavkaPopisa>();
            foreach (var item in dbContext.StavkaPopisas.ToList())
            {
                if (item.popis_id == id)
                {
                    pomocna.Add(item);
                }
            }

            foreach (var item in pomocna)
            {
                if (!Stavke.Any(x => x.proizvod_id == item.proizvod_id && x.skladiste_id == item.skladiste_id))
                {
                    Stavke.Add(item);
                }
                else
                {
                    Stavke.FirstOrDefault(x => x.proizvod_id == item.proizvod_id && x.skladiste_id == item.skladiste_id).kolicina += item.kolicina;
                }
            }

            foreach (var item in zalihe)
            {
                if (Stavke.Any(x => x.proizvod_id == item.proizvod_id && x.skladiste_id == item.skladiste_id))
                {
                    if (item.kolicina == Stavke.FirstOrDefault(x => x.proizvod_id == item.proizvod_id && x.skladiste_id == item.skladiste_id).kolicina)
                    {
                        Stavke.Remove(Stavke.FirstOrDefault(x => x.proizvod_id == item.proizvod_id && x.skladiste_id == item.skladiste_id));
                    }
                    else
                    {
                        imaRazlike = true;
                        DokumentVisible = Visibility.Visible;
                        Stavke.FirstOrDefault(x => x.proizvod_id == item.proizvod_id && x.skladiste_id == item.skladiste_id).kolicina = Stavke.FirstOrDefault(x => x.proizvod_id == item.proizvod_id && x.skladiste_id == item.skladiste_id).kolicina - item.kolicina;
                    }
                }
            }
        }
        #endregion
    }
}
