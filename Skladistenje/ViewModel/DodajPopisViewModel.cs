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
        private int _selectedZaposleniLevo = -1;
        private int _selectedZaposleniDesno = -1;
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
            throw new NotImplementedException();
        }

        private void Add2(int obj)
        {
            throw new NotImplementedException();
        }

        private void Remove1(int obj)
        {
            throw new NotImplementedException();
        }

        private void Add1(int obj)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

        public int SelectedZaposleniLevo
        {
            get => _selectedZaposleniLevo;
            set
            {
                if (_selectedZaposleniLevo == value)
                {
                    if (DodajButtonEnabled)
                    {
                        if (_selectedZaposleniLevo > -1)
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
                _selectedZaposleniLevo = value;
                if (DodajButtonEnabled)
                {
                    if (_selectedZaposleniLevo > -1)
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

        public int SelectedZaposleniDesno
        {
            get => _selectedZaposleniDesno;
            set
            {
                if (_selectedZaposleniDesno == value)
                {
                    if (DodajButtonEnabled)
                    {
                        if (_selectedZaposleniDesno > -1)
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
                _selectedZaposleniDesno = value;
                if (DodajButtonEnabled)
                {
                    if (_selectedZaposleniDesno > -1)
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
