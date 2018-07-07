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

namespace Skladistenje.ViewModel
{
    public class DodajPopisViewModel : BindableBase
    {
        #region Members
        private Korisnik userOnSession;
        private Popi popisForBind;
        private string skladisteForBind;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private string kolicinaText;
        private string rafText;
        private bool addEnabled1;
        private bool removeEnabled1;
        private bool addEnabled2;
        private bool removeEnabled2;
        private bool dodajButtonEnabled;
        private int _selectedProizvod = -1;
        private int _selectedProizvodSaKolicinom = -1;
        private int _selectedZaposleniKorisnikLevo = -1;
        private int _selectedZaposleniKorisnikDesno = -1;
        private ObservableCollection<Proizvod> proizvodi;
        private ObservableCollection<ProizvodKolicina> proizvodiSaKolicinom;
        private ObservableCollection<Skladiste> skladista;
        private ObservableCollection<ZaposleniKorisnik> zaposleniLevo;
        private ObservableCollection<ZaposleniKorisnik> zaposleniDesno;
        #endregion

        #region Commands
        public MyICommand<object> DodajPopisCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        public MyICommand<int> AddCommand1 { get; private set; }
        public MyICommand<int> RemoveCommand1 { get; private set; }
        public MyICommand<int> AddCommand2 { get; private set; }
        public MyICommand<int> RemoveCommand2 { get; private set; }
        #endregion

        public DodajPopisViewModel(Popi p)
        {
            this.popisForBind = p;
            if (popisForBind != null)
            {
                DodajButtonEnabled = false;
                addEnabled1 = false;
                addEnabled2 = false;
                removeEnabled1 = false;
                removeEnabled2 = false;
            }
            else
            {
                DodajButtonEnabled = true;
                popisForBind = new Popi();
                popisForBind.datum = DateTime.Now;
            }
            DodajPopisCommand = new MyICommand<object>(DodajPopis);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            AddCommand1 = new MyICommand<int>(Add1);
            RemoveCommand1 = new MyICommand<int>(Remove1);
            AddCommand2 = new MyICommand<int>(Add2);
            RemoveCommand2 = new MyICommand<int>(Remove2);
            userOnSession = new Korisnik();
            skladista = new ObservableCollection<Skladiste>();
            proizvodi = new ObservableCollection<Proizvod>();
            zaposleniLevo = new ObservableCollection<ZaposleniKorisnik>();
            zaposleniDesno = new ObservableCollection<ZaposleniKorisnik>();
            proizvodiSaKolicinom = new ObservableCollection<ProizvodKolicina>();
            KolicinaText = "";
            RafText = "";
            foreach (var item in dbContext.Skladistes)
            {
                skladista.Add(item);
            }

            foreach (var item in dbContext.Proizvods)
            {
                Proizvodi.Add(item);
            }

            foreach (var item in dbContext.Zaposlenis)
            {
                ZaposleniLevo.Add(new ZaposleniKorisnik(item.ime, item.prezime, item.Korisniks?.ElementAt(0).korisnickoime, true, item.id));
            }
        }

        #region CommandsImplementatios
        private void Remove2(int obj)
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

        private void Add2(int obj)
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

        private void Remove1(int obj)
        {
            if (SelectedZaposleniKorisnikDesno != -1)
            {
                ZaposleniKorisnik zk = ZaposleniDesno.ElementAt(SelectedZaposleniKorisnikDesno);
                ZaposleniDesno.RemoveAt(SelectedZaposleniKorisnikDesno);
                ZaposleniLevo.Add(zk);
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }

        private void Add1(int obj)
        {
            if (SelectedZaposleniKorisnikLevo != -1)
            {
                ZaposleniKorisnik zk = ZaposleniLevo.ElementAt(SelectedZaposleniKorisnikLevo);
                ZaposleniDesno.Add(zk);
                ZaposleniLevo.RemoveAt(SelectedZaposleniKorisnikLevo);
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }

        private void Otkazi(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("popisi");
                }
            }
        }

        private void DodajPopis(object obj)
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

                if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.AddPopis))
                {
                    Popi newPopis = new Popi();
                    newPopis.oznaka = PopisForBind.oznaka;
                    newPopis.datum = PopisForBind.datum;
                    newPopis.datum += new TimeSpan(0, 0, 0);
                    newPopis.skladiste_id = Skladista.FirstOrDefault(x => x.naziv.Equals(SkladisteForBind)).id;
                    newPopis.id = -1;
                    List<Zaposleni> zapPom = dbContext.Zaposlenis.ToList();
                    foreach (var item in ZaposleniDesno)
                    {
                        newPopis.Zaposlenis.Add(zapPom.FirstOrDefault(x => x.id == item.Id));
                    }

                    dbContext.Popis.Add(newPopis);
                    dbContext.SaveChanges();
                    int idPopisa = dbContext.Popis.FirstOrDefault(x => x.oznaka.Equals(newPopis.oznaka)).id;

                    int i = 1;
                    int skladisteId = Skladista.FirstOrDefault(x => x.naziv.Equals(SkladisteForBind)).id;
                    foreach (var item in ProizvodiSaKolicinom)
                    {
                        StavkaPopisa sp = new StavkaPopisa();
                        sp.kolicina = Double.Parse(item.Kolicina);
                        sp.proizvod_id = Proizvodi.FirstOrDefault(x => x.naziv.Equals(item.Naziv)).id;
                        sp.raf = item.Raf;
                        sp.skladiste_id = skladisteId;
                        sp.rednibroj = i++;
                        sp.popis_id = idPopisa;
                        dbContext.StavkaPopisas.Add(sp);
                    }
                    dbContext.SaveChanges();
                    foreach (var item in dbContext.Zalihes.Where(x => x.skladiste_id == skladisteId).ToList())
                    {
                        if (!dbContext.StavkaPopisas.Any(x => x.skladiste_id == skladisteId && x.proizvod_id == item.proizvod_id && x.popis_id == idPopisa))
                        {
                            StavkaPopisa sp = new StavkaPopisa();
                            sp.kolicina = 0;
                            sp.proizvod_id = item.proizvod_id;
                            sp.raf = "";
                            sp.skladiste_id = skladisteId;
                            sp.rednibroj = i++;
                            sp.popis_id = idPopisa;
                            dbContext.StavkaPopisas.Add(sp);
                        }
                    }
                    dbContext.SaveChanges();

                    //window za pripis/otpis
                    PripisOtpisView pow = new PripisOtpisView(1 ,idPopisa);
                    pow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Error er = new Error("Greška sa konekcijom!\nObratite se administratorima.");
                er.Show();
            }
        }
        #endregion

        #region Constructors
        public Korisnik UserOnSession { get { return userOnSession; } set { userOnSession = value; } }
        public Popi PopisForBind { get => popisForBind; set { popisForBind = value; OnPropertyChanged("PopisForBind"); } }
        public ObservableCollection<Skladiste> Skladista
        {
            get => skladista;
            set
            {
                skladista = value;
                OnPropertyChanged("Skladista");
            }
        }

        public string SkladisteForBind
        {
            get => skladisteForBind;
            set
            {
                skladisteForBind = value;
                OnPropertyChanged(SkladisteForBind);
            }
        }

        public bool RemoveEnabled1
        {
            get => removeEnabled1;
            set
            {
                if (DodajButtonEnabled) removeEnabled1 = value;
                else removeEnabled1 = false;
                OnPropertyChanged("RemoveEnabled1");
            }
        }

        public bool AddEnabled1
        {
            get => addEnabled1;
            set
            {
                if (DodajButtonEnabled) addEnabled1 = value;
                else addEnabled1 = false;
                OnPropertyChanged("AddEnabled1");
            }
        }

        public bool RemoveEnabled2
        {
            get => removeEnabled2;
            set
            {
                if (DodajButtonEnabled) removeEnabled2 = value;
                else removeEnabled2 = false;
                OnPropertyChanged("RemoveEnabled2");
            }
        }

        public bool AddEnabled2
        {
            get => addEnabled2;
            set
            {
                if (DodajButtonEnabled) addEnabled2 = value;
                else addEnabled2 = false;
                OnPropertyChanged("AddEnabled2");
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
                    if (DodajButtonEnabled)
                    {
                        if (_selectedProizvod > -1)
                        {
                            AddEnabled2 = true;
                        }
                        else
                        {
                            AddEnabled2 = false;
                        }
                    }
                    return;
                }
                _selectedProizvod = value;
                if (DodajButtonEnabled)
                {
                    if (_selectedProizvod > -1)
                    {
                        AddEnabled2 = true;
                    }
                    else
                    {
                        AddEnabled2 = false;
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
                    if (DodajButtonEnabled)
                    {
                        if (_selectedProizvodSaKolicinom > -1)
                        {
                            RemoveEnabled2 = true;
                        }
                        else
                        {
                            RemoveEnabled2 = false;
                        }
                    }
                    return;
                }
                _selectedProizvodSaKolicinom = value;
                if (DodajButtonEnabled)
                {
                    if (_selectedProizvodSaKolicinom > -1)
                    {
                        RemoveEnabled2 = true;
                    }
                    else
                    {
                        RemoveEnabled2 = false;
                    }
                }
                return;
            }
        }

        public string KolicinaText { get => kolicinaText; set { kolicinaText = value; OnPropertyChanged("KolicinaText"); } }

        public string RafText { get => rafText; set { rafText = value; OnPropertyChanged("RafText"); } }

        public ObservableCollection<ZaposleniKorisnik> ZaposleniLevo
        {
            get => zaposleniLevo;
            set
            {
                zaposleniLevo = value;
                OnPropertyChanged("ZaposleniLevo");
            }
        }

        public ObservableCollection<ZaposleniKorisnik> ZaposleniDesno
        {
            get => zaposleniDesno;
            set
            {
                zaposleniDesno = value;
                OnPropertyChanged("ZaposleniDesno");
            }
        }

        public int SelectedZaposleniKorisnikLevo
        {
            get => _selectedZaposleniKorisnikLevo;
            set
            {
                if (_selectedZaposleniKorisnikLevo == value)
                {
                    if (DodajButtonEnabled)
                    {
                        if (_selectedZaposleniKorisnikLevo > -1)
                        {
                            AddEnabled1 = true;
                        }
                        else
                        {
                            AddEnabled1 = false;
                        }
                    }
                    return;
                }
                _selectedZaposleniKorisnikLevo = value;
                if (DodajButtonEnabled)
                {
                    if (_selectedZaposleniKorisnikLevo > -1)
                    {
                        AddEnabled1 = true;
                    }
                    else
                    {
                        AddEnabled1 = false;
                    }
                }
                return;
            }
        }

        public int SelectedZaposleniKorisnikDesno
        {
            get => _selectedZaposleniKorisnikDesno;
            set
            {
                if (_selectedZaposleniKorisnikDesno == value)
                {
                    if (DodajButtonEnabled)
                    {
                        if (_selectedZaposleniKorisnikDesno > -1)
                        {
                            RemoveEnabled1 = true;
                        }
                        else
                        {
                            RemoveEnabled1 = false;
                        }
                    }
                    return;
                }
                _selectedZaposleniKorisnikDesno = value;
                if (DodajButtonEnabled)
                {
                    if (_selectedZaposleniKorisnikDesno > -1)
                    {
                        RemoveEnabled1 = true;
                    }
                    else
                    {
                        RemoveEnabled1 = false;
                    }
                }
                return;
            }
        }

        public bool DodajButtonEnabled
        {
            get => dodajButtonEnabled;
            set
            {
                dodajButtonEnabled = value;
                OnPropertyChanged("DodajButtonEnabled");
            }
        }
        #endregion
    }
}
