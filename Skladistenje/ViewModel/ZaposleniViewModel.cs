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
    public class ZaposleniViewModel : BindableBase
    {
        #region Members
        private Korisnik userOnSession;
        private ObservableCollection<Skladiste> ponudjenaSkladista;
        private ObservableCollection<Skladiste> dodeljenaSkladista;
        private ObservableCollection<ZaposleniKorisnik> zaposleni;
        private string zaposleniForBind;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private bool addEnabled;
        private bool removeEnabled;
        private int _selectedPonudjenaSkl = -1;
        private int _selectedDodeljenaSkl = -1;
        #endregion

        #region Commands
        public MyICommand<object> SacuvajCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        public MyICommand<int> AddCommand { get; private set; }
        public MyICommand<int> RemoveCommand { get; private set; }
        #endregion

        public ZaposleniViewModel()
        {
            SacuvajCommand = new MyICommand<object>(Sacuvaj);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            AddCommand = new MyICommand<int>(Add);
            RemoveCommand = new MyICommand<int>(Remove);
            userOnSession = new Korisnik();
            PonudjenaSkladista = new ObservableCollection<Skladiste>();
            DodeljenaSkladista = new ObservableCollection<Skladiste>();
            Zaposleni = new ObservableCollection<ZaposleniKorisnik>();
            foreach (var item in dbContext.Zaposlenis.ToList())
            {
                Zaposleni.Add(new ZaposleniKorisnik(item.ime, item.prezime, item.Korisniks.ElementAt(0).korisnickoime));
            }
        }

        #region Properties
        public Korisnik UserOnSession { get => userOnSession; set => userOnSession = value; }
        public string ZaposleniForBind
        {
            get => zaposleniForBind;
            set
            {
                zaposleniForBind = value;
                string[] pom = ZaposleniForBind.Split('(');
                string username = pom[1].Trim(')');
                foreach (var item in dbContext.ZaposleniSkladistas.ToList())
                {
                    if (item.Zaposleni.Korisniks.ElementAt(0).korisnickoime.Equals(username))
                    {
                        if (!DodeljenaSkladista.Any(x => x.naziv.Equals(item.Skladiste.naziv)))
                        {
                            DodeljenaSkladista.Add(item.Skladiste);
                        }
                    }
                    else
                    {
                        if (!PonudjenaSkladista.Any(x => x.naziv.Equals(item.Skladiste.naziv)))
                        {
                            PonudjenaSkladista.Add(item.Skladiste);
                        }
                    }
                }
                foreach (var item in dbContext.Skladistes.ToList())
                {
                    if (!DodeljenaSkladista.Any(x => x.naziv.Equals(item.naziv)) && !PonudjenaSkladista.Any(x => x.naziv.Equals(item.naziv)))
                    {
                        PonudjenaSkladista.Add(item);
                    }
                }
                    OnPropertyChanged(ZaposleniForBind);
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

        public bool AddEnabled
        {
            get => addEnabled;
            set
            {
                addEnabled = value;
                OnPropertyChanged("AddEnabled");
            }
        }

        public ObservableCollection<Skladiste> PonudjenaSkladista { get => ponudjenaSkladista; set { ponudjenaSkladista = value; OnPropertyChanged("PonudjenaSkladista"); } }
        public ObservableCollection<Skladiste> DodeljenaSkladista { get => dodeljenaSkladista; set { dodeljenaSkladista = value; OnPropertyChanged("DodeljenaSkladista"); } }
        public ObservableCollection<ZaposleniKorisnik> Zaposleni { get => zaposleni; set { zaposleni = value; OnPropertyChanged("Zaposleni"); } }

        public int SelectedPonudjenaSkl
        {
            get => _selectedPonudjenaSkl;
            set
            {
                if (_selectedPonudjenaSkl == value)
                {
                    if (_selectedPonudjenaSkl > -1)
                    {
                        AddEnabled = true;
                    }
                    else
                    {
                        AddEnabled = false;
                    }
                    return;
                }

                _selectedPonudjenaSkl = value;

                if (_selectedPonudjenaSkl > -1)
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

        public int SelectedDodeljenaSkl
        {
            get => _selectedDodeljenaSkl;
            set
            {
                if (_selectedDodeljenaSkl == value)
                {
                    if (_selectedDodeljenaSkl > -1)
                    {
                        RemoveEnabled = true;
                    }
                    else
                    {
                        RemoveEnabled = false;
                    }
                    return;
                }
                _selectedDodeljenaSkl = value;
                if (_selectedDodeljenaSkl > -1)
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
        #endregion

        #region CommandsImplementation
        private void Remove(int obj)
        {
            if (SelectedDodeljenaSkl != -1)
            {
                Skladiste s = DodeljenaSkladista.ElementAt(SelectedDodeljenaSkl);
                PonudjenaSkladista.Add(s);
                DodeljenaSkladista.RemoveAt(SelectedDodeljenaSkl);
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }

        private void Add(int obj)
        {
            if (SelectedPonudjenaSkl != -1)
            {
                Skladiste s = PonudjenaSkladista.ElementAt(SelectedPonudjenaSkl);
                DodeljenaSkladista.Add(s);
                PonudjenaSkladista.RemoveAt(SelectedPonudjenaSkl);
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }

        private void Otkazi(string obj)
        {
            DodeljenaSkladista.Clear();
            PonudjenaSkladista.Clear();
            ZaposleniForBind = null;
        }

        private void Sacuvaj(object obj)
        {
            //TO DO : ono sto smo pricali sa active treba dodati u model
        }
        #endregion

    }

    public class ZaposleniKorisnik : BindableBase
    {
        private string ime;
        private string prezime;
        private string korisnickoIme;
        private string identifikacija;

        public ZaposleniKorisnik(string ime, string prz, string ki)
        {
            this.Ime = ime;
            Prezime = prz;
            KorisnickoIme = ki;
            identifikacija = ime + " " + prezime + $"({korisnickoIme})";
        }

        public string Ime { get => ime; set { ime = value; OnPropertyChanged("Ime"); } }
        public string Prezime { get => prezime; set { prezime = value; OnPropertyChanged("Prezime"); } }
        public string KorisnickoIme { get => korisnickoIme; set { korisnickoIme = value; OnPropertyChanged("korisnickoIme"); } }
        public string Identifikacija { get => identifikacija; set { identifikacija = value; OnPropertyChanged("Identifikacija"); } }
    }
}
