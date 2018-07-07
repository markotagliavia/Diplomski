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
    public class DodajGenericSklDokViewModel : BindableBase
    {
        #region Members
        private string tip;
        private Korisnik userOnSession;
        private string kolicinaText;
        private string rafText = "";
        private bool addEnabled;
        private bool removeEnabled;
        private int _selectedProizvod = -1;
        private int _selectedProizvodSaKolicinom = -1;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ObservableCollection<Proizvod> proizvodi;
        private ObservableCollection<ProizvodKolicina> proizvodiSaKolicinom;
        private SkladisteniDokument sklDokForBind;
        private Skladiste skladisteSourceForBind, skladisteDestForBind;
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

        public DodajGenericSklDokViewModel(string tip)
        {
            this.tip = tip;
            DodajSklDokCommand = new MyICommand<object>(DodajSklDok);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            BackNavCommand = new MyICommand<string>(Otkazi);
            AddCommand = new MyICommand<int>(Add);
            RemoveCommand = new MyICommand<int>(Remove);
            userOnSession = new Korisnik();
            proizvodi = new ObservableCollection<Proizvod>();
            proizvodiSaKolicinom = new ObservableCollection<ProizvodKolicina>();
            skladista = new ObservableCollection<Skladiste>();
            sklDokForBind = new SkladisteniDokument();
            skladisteSourceForBind = new Skladiste();
            skladisteDestForBind = new Skladiste();
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
        }

        #region Properties

        public Korisnik UserOnSession { get => userOnSession; set => userOnSession = value; }
        public SkladisteniDokument SklDokForBind { get => sklDokForBind; set { sklDokForBind = value; OnPropertyChanged("SklDokForBind"); } }
        public Skladiste SkladisteSourceForBind { get => skladisteSourceForBind; set { skladisteSourceForBind = value; OnPropertyChanged("SkladisteSourceForBind"); } }
        public Skladiste SkladisteDestForBind { get => skladisteDestForBind; set { skladisteDestForBind = value; OnPropertyChanged("SkladisteDestForBind"); } }
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
                _selectedProizvod = value;
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

        public int SelectedProizvodSaKolicinom
        {
            get => _selectedProizvodSaKolicinom;
            set
            {
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

        public string SourceZalihe { get => sourceZalihe; set => sourceZalihe = value; }
        public string DestinationZalihe { get => destinationZalihe; set => destinationZalihe = value; }
        public Visibility IzdaoVisible { get => izdaoVisible; set => izdaoVisible = value; }
        public Visibility PrimioVisible { get => primioVisible; set => primioVisible = value; }
        public Visibility VozacVisible { get => vozacVisible; set => vozacVisible = value; }
        public Visibility RegBrVisible { get => regBrVisible; set => regBrVisible = value; }
        public Visibility NacinOtpremeVisible { get => nacinOtpremeVisible; set => nacinOtpremeVisible = value; }
        public Visibility IzSklVisible { get => izSklVisible; set => izSklVisible = value; }
        public Visibility USklVisible { get => uSklVisible; set => uSklVisible = value; }

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
                ProizvodKolicina pk = new ProizvodKolicina(p, KolicinaText, "");
                ProizvodiSaKolicinom.Add(pk);
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
                
            }
            else if (tip == "SP_PR" || tip == "SP_OTP")  //spoljni
            {
                
            }
            else if (tip == "KOR_PR" || tip == "KOR_OTP") //korekcioni
            {
                
            }
            else if (tip == "STORNI") //storni
            {
                //ovo cu izmestiti
            }
        }

        #endregion
    }
}
