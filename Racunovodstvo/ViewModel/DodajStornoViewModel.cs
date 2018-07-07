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
    public class DodajStornoViewModel:BindableBase
    {
        private int context;
        private Faktura fakturaForBind;
        private ObservableCollection<Faktura> redovneFakture;
        private ObservableCollection<Faktura> redovneStorno;
        private ObservableCollection<ProizvodKolicina> stavkeFaktura;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private int _selectedFaktura = -1;
        private int _selectedStorno = -1;
        private bool addEnabled = false;
        private bool removeEnabled = false;
        private string submitButtonText;
        private Korisnik userOnSession = new Korisnik();
        private Visibility dodajBtn;
        private bool edit;
        #region Commands
        public MyICommand<string> DodajCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        public MyICommand<string> BackCommand { get; private set; }
        public MyICommand<int> AddCommand { get; private set; }
        public MyICommand<int> RemoveCommand { get; private set; }
        
        #endregion

        public DodajStornoViewModel(int i,Faktura f)
        {
            DodajCommand = new MyICommand<string>(Dodaj);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            BackCommand = new MyICommand<string>(Back);
            AddCommand = new MyICommand<int>(Add);
            RemoveCommand = new MyICommand<int>(Remove);
            
            RedovneFakture = new ObservableCollection<Faktura>();
            RedovneStorno = new ObservableCollection<Faktura>();
            StavkeFaktura = new ObservableCollection<ProizvodKolicina>();
            context = i;
            if (context == 0)
            {
                
                SubmitButtonText = "Dodaj";
                FakturaForBind = new Faktura();
                FakturaForBind.redovna = false;
                FakturaForBind.ulazna = false;
                FakturaForBind.datumfakturisanja = DateTime.Now;
                FakturaForBind.active = true;
                FakturaForBind.stornoceo = false;
                FakturaForBind.upripremi = false;
                dodajBtn = Visibility.Visible;
                Edit = true;

                foreach (var item in dbContext.Fakturas)
                {
                    if (item.redovna == true && item.active == true && item.stornoceo == false)
                    {
                        if (item.otpremljena == false)
                        {
                            RedovneFakture.Add(item);
                        }
                        
                    }

                    
                }
                
            }
            else if (context == 1)
            {
                dodajBtn = Visibility.Collapsed;
                Edit = false;
                FakturaForBind = f;

                foreach (var item in f.StavkaFaktures)
                {
                    ProizvodKolicina pk = new ProizvodKolicina(dbContext.Proizvods.FirstOrDefault(x => x.id == item.zalihe_proizvod_id), item.kolicina.ToString(), item.cena.ToString(), item.rabat.ToString());
                    pk.Id = item.rednibroj;
                    pk.Faktura = item.Faktura;
                    pk.Zalihe = item.Zalihe;
                    pk.Storno = true;
                    StavkeFaktura.Add(pk);
                }

                foreach (var item in f.Fakturas)
                {
                    RedovneStorno.Add(item);
                }

                foreach (var item in dbContext.Fakturas)
                {
                    if (item.redovna == true && item.active == true && item.stornoceo == false)
                    {
                        if (item.otpremljena == false && !RedovneStorno.Any(x => x.id == item.id))
                        {
                            
                            RedovneFakture.Add(item);
                        }

                    }


                }
            }
            
        }

        #region Properties
        public string SubmitButtonText { get => submitButtonText; set { submitButtonText = value; OnPropertyChanged("SubmitButtonText"); } }
        public Faktura FakturaForBind
        {
            get => fakturaForBind;
            set
            {
                fakturaForBind = value;
                OnPropertyChanged("FakturaForBind");
            }
        }

        public ObservableCollection<Faktura> RedovneFakture
        {
            get => redovneFakture;
            set
            {
                redovneFakture = value;
                OnPropertyChanged("RedovneFakture");
            }
        }

        public ObservableCollection<Faktura> RedovneStorno
        {
            get => redovneStorno;
            set
            {
                redovneStorno = value;
                OnPropertyChanged("RedovneStorno");
            }
        }

        public ObservableCollection<ProizvodKolicina> StavkeFaktura
        {
            get => stavkeFaktura;
            set
            {
                stavkeFaktura = value;
                OnPropertyChanged("StavkeFaktura");
            }
        }
        #endregion

        public int SelectedFaktura
        {
            get => _selectedFaktura;
            set
            {
                if (_selectedFaktura == value && context == 0)
                {
                    
                    
                        if (_selectedFaktura > -1)
                        {
                            AddEnabled = true;
                        }
                        else
                        {
                            AddEnabled = false;
                        }
                    
                    

                    return;
                }
                _selectedFaktura = value;
                if (context == 0)
                {
                    
                        if (_selectedFaktura > -1)
                        {
                            AddEnabled = true;
                        }
                        else
                        {
                            AddEnabled = false;
                        }
                    
                }

                return;
            }
        }

        public int SelectedStorno
        {
            get => _selectedStorno;
            set
            {
                if (_selectedStorno == value && context == 0)
                {
                     
                    if (_selectedStorno > -1)
                    {
                        RemoveEnabled = true;
                    }
                    else
                    {
                        RemoveEnabled = false;
                    }
                       



                    return;
                }
                _selectedStorno = value;
                if (context == 0)
                {
                    if (_selectedStorno > -1)
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

        public bool RemoveEnabled
        {
            get => removeEnabled;
            set
            {
                
                if (context == 0) removeEnabled = value;
                else removeEnabled = false;
                

                OnPropertyChanged("RemoveEnabled");
            }
        }

        public bool AddEnabled
        {
            get => addEnabled;
            set
            {
                
                if (context == 0) addEnabled = value;
                else addEnabled = false;
                
                OnPropertyChanged("AddEnabled");

            }
        }

        public Korisnik UserOnSession { get => userOnSession; set => userOnSession = value; }
      
        public Visibility DodajBtn { get => dodajBtn; set { dodajBtn = value; OnPropertyChanged("DodajBtn"); } }

        public bool Edit { get => edit; set { edit = value; OnPropertyChanged("Edit"); } }

        #region CommandsImplements
        private void Otkazi(string obj)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.storno);
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w.GetType().Equals(typeof(MainWindow)))
            //    {
                    
            //        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("storno");
                    

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
                    if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.AddStorno))
                    {

                        Zaposleni z = dbContext.Korisniks.FirstOrDefault(x => x.korisnickoime.Equals(UserOnSession.korisnickoime)).Zaposleni;
                        FakturaForBind.zaposleni_id = z.id;
                        dbContext.Fakturas.Add(FakturaForBind);
                        dbContext.SaveChanges();
                        int rednibroj = 0;
                        foreach (var item in stavkeFaktura)
                        {
                            if (item.Storno)
                            {
                                StavkaFakture st = new StavkaFakture();
                                st.cena = Double.Parse(item.Cena);
                                st.kolicina = Double.Parse(item.Kolicina);
                                st.rednibroj = rednibroj;
                                st.zalihe_proizvod_id = item.Zalihe.proizvod_id;
                                st.zalihe_skladiste_id = item.Zalihe.skladiste_id;
                                st.Faktura = dbContext.Fakturas.FirstOrDefault(x => x.oznaka.Equals(FakturaForBind.oznaka));
                                dbContext.StavkaFaktures.Add(st);
                                dbContext.SaveChanges();
                                dbContext.StavkaFaktures.FirstOrDefault(x => x.faktura_id == item.Faktura.id && x.rednibroj == item.Id).storno = true;
                                if (item.Faktura.redovna== true && item.Faktura.ulazna == false)
                                {
                                    dbContext.Zalihes.FirstOrDefault(x => x.proizvod_id == item.Zalihe.proizvod_id && x.skladiste_id == item.Zalihe.skladiste_id).rezervisano -= Double.Parse(item.Kolicina);
                                }
                                rednibroj++;
                            }
                            

                        }
                        
                        
                        foreach (var item in redovneStorno)
                        {
                            FakturaForBind.Fakturas.Add(item);
                            bool storno = true;
                            foreach (var st in dbContext.Fakturas.FirstOrDefault(x => x.id == item.id).StavkaFaktures)
                            {
                                if ((bool)st.storno)
                                {
                                    storno = false;
                                    break;
                                }
                            }
                            if (storno)
                            {
                                dbContext.Fakturas.FirstOrDefault(x => x.id == item.id).stornoceo = false;
                            }
                        }
                        Back("");
                        dbContext.SaveChanges();
                        Notifications.Success s = new Notifications.Success("Uspešno ste kreirali izlaznu fakturu");
                        s.Show();
                        
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, $"Uspesno je kreirana storno faktura {FakturaForBind.oznaka}", "Info");

                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj kreiranja storno fakture", "Upozorenje");
                        Back("");
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        Notifications.Error e = new Notifications.Error("Greška pri kreiranju storno fakture.");
                        e.Show();
                        Back("");
                    }
                    catch (Exception exc)
                    {
                        Error er = new Error("Greška sa konekcijom!\nObratite se administratorima.");
                        er.Show();
                        Back("");
                    }
                }
                
               
                
            }
        }
        private void Add(int obj)
        {
            if (SelectedFaktura != -1)
            {
                Faktura pom = RedovneFakture.ElementAt(obj);
                RedovneStorno.Add(pom);
                FakturaForBind.Fakturas.Add(pom);
                RedovneFakture.RemoveAt(obj);

                foreach (var item in pom.StavkaFaktures)
                {
                    if (!(bool)item.storno)
                    {
                        ProizvodKolicina pk = new ProizvodKolicina(dbContext.Proizvods.FirstOrDefault(x => x.id == item.zalihe_proizvod_id), item.kolicina.ToString(), item.cena.ToString(), item.rabat.ToString());
                        pk.Id = item.rednibroj;
                        pk.Faktura = item.Faktura;
                        pk.Zalihe = item.Zalihe;
                        
                        StavkeFaktura.Add(pk);
                    }
                    
                    
                }
            }
        }
        private void Remove(int obj)
        {
            if (SelectedStorno != -1)
            {
                Faktura pom = RedovneStorno.ElementAt(obj);
                RedovneFakture.Add(pom);
                FakturaForBind.Fakturas.Remove(pom);
                RedovneStorno.RemoveAt(obj);

                foreach (var item in pom.StavkaFaktures)
                {
                    

                    if (StavkeFaktura.Any(x => x.Id == item.rednibroj && x.Faktura.id == item.faktura_id))
                    {
                        StavkeFaktura.Remove(StavkeFaktura.FirstOrDefault(x => x.Id == item.rednibroj && x.Faktura.id == item.faktura_id));
                    }
                    


                }
            }
        }
       
        #endregion
    }

    
}
