using Common;
using Common.Model;
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
                //ProfakturaForEdit.datum = DateTime.Now();
                //ProfakturaForEdit.PDV = 0;
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
            throw new NotImplementedException();
        }

        private void Add(int obj)
        {
            throw new NotImplementedException();
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
            if (context == 0)
            {
                ProfakturaForEdit.zaposleni_id = MainWindowViewModel.Instance.UserOnSession.zaposleni_id;
            }
            throw new NotImplementedException();
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
                //Profaktura.pdv = pdv;
                SaPDV = (1 + (pdv / 100)) * BezPDV;
                OnPropertyChanged("Pdv");
            }
        }

        public string RabatText { get => rabatText; set { rabatText = value; OnPropertyChanged("RabatText"); } }

        
        #endregion
    }
}
