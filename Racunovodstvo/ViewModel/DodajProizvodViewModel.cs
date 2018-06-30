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
    public class DodajProizvodViewModel : BindableBase
    {
        #region Members
        private int context;
        private ObservableCollection<string> jedinicamere;
        private ObservableCollection<Karakteristika> karakteristike;
        private ObservableCollection<Karakteristika> karakteristikaProizvod;
        private ObservableCollection<Proizvodjac> proizvodjaci;
        private string mereForBind;
        private string textBox;
        private string proizvodjacForBind;
        string stariNaziv;
        private int _selectedKarakteristika = -1;
        private int _selectedKarakteristikaProizvod = -1;
        private Proizvod proizvodForBind = new Proizvod();
        private Korisnik userOnSession = new Korisnik();
        private string submitButtonText;
        private bool addEnabled;
        private bool removeEnabled;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        #endregion

        #region Commands
        public MyICommand<string> DodajCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        public MyICommand<int> AddCommand { get; private set; }
        public MyICommand<int> RemoveCommand { get; private set; }
        public MyICommand<string> BackNavCommand { get; private set; }
        public MyICommand<string> DodajKarakteristikuCommand { get; private set; }
        public MyICommand<string> IzmeniKarakteristikuCommand { get; private set; }
        public MyICommand<string> ObrisiKarakteristikuCommand { get; private set; }
        public MyICommand<string> AddProizvodjacCommand { get; private set; }
        #endregion

        public DodajProizvodViewModel(int v, Proizvod p)
        {
            context = v;
            
            AddProizvodjacCommand = new MyICommand<string>(AddProizvodjac);
            DodajCommand = new MyICommand<string>(Dodaj);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            AddCommand = new MyICommand<int>(Add);
            RemoveCommand = new MyICommand<int>(Remove);
            BackNavCommand = new MyICommand<string>(Back);
            DodajKarakteristikuCommand = new MyICommand<string>(DodajKarakteristiku);
            IzmeniKarakteristikuCommand = new MyICommand<string>(IzmeniKarakteristiku);
            ObrisiKarakteristikuCommand = new MyICommand<string>(ObrisiKarakteristiku);
            jedinicamere = new ObservableCollection<string>();
            Karakteristike = new ObservableCollection<Karakteristika>();
            KarakteristikaProizvod = new ObservableCollection<Karakteristika>();
            Proizvodjaci = new ObservableCollection<Proizvodjac>();
            foreach (var item in dbContext.Proizvodjacs)
            {
                Proizvodjaci.Add(item);
            }
            foreach (var item in dbContext.jedinicameres)
            {
                jedinicamere.Add(item.naziv);
            }
            
            if (context == 1)
            {
                SubmitButtonText = "Potvrdi izmenu";
                ProizvodForBind = dbContext.Proizvods.FirstOrDefault(x => x.id == p.id);
                stariNaziv = p.naziv;
                MereForBind = p.jedinicamere.naziv;
                foreach (var item in ProizvodForBind.Karakteristikas)
                {
                    KarakteristikaProizvod.Add(item);
                }
                ProizvodjacForBind = p.Proizvodjac.naziv;
            }
            else
            {
                if (p != null)
                {
                    proizvodForBind = p;
                    if (p.jedinicamere != null)
                    {
                        MereForBind = p.jedinicamere.naziv;
                    }
                    
                    foreach (var item in ProizvodForBind.Karakteristikas)
                    {
                        KarakteristikaProizvod.Add(item);
                    }
                    if (p.Proizvodjac != null)
                    {
                        ProizvodjacForBind = p.Proizvodjac.naziv;
                    }
                    
                }
                else
                {
                    ProizvodForBind = new Proizvod();
                }
               
                SubmitButtonText = "Dodaj";
            }

            foreach (var item in dbContext.Karakteristikas)
            {
                if (!KarakteristikaProizvod.Any(x => x.naziv.Equals(item.naziv)))
                {
                    Karakteristike.Add(item);
                }
            }
        }




        #region CommandsImplementation
        private void DodajKarakteristiku(string obj)
        {
            //to do autorizacija

            if (TextBox != null && !String.IsNullOrWhiteSpace(TextBox) && (TextBox != ""))
            {
                try
                {
                    Karakteristika k = new Karakteristika();
                    k.naziv = TextBox;
                    dbContext.Karakteristikas.Add(k);
                    dbContext.SaveChanges();
                    Notifications.Success s = new Notifications.Success("Uspešno ste kreirali karakteristiku");
                    s.Show();
                    Karakteristike.Clear();
                    foreach (var item in dbContext.Karakteristikas)
                    {
                        if (!KarakteristikaProizvod.Any(x => x.naziv.Equals(item.naziv)))
                        {
                            Karakteristike.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Notifications.Error e = new Notifications.Error("Greška pri dodavanju karakteristike");
                    e.Show();
                }
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Naziv karakteristike je obavezno polje");
                e.Show();
            }
        }

        private void IzmeniKarakteristiku(string obj)
        {
            //to do autorizacija

            if (TextBox != null && !String.IsNullOrWhiteSpace(TextBox) && (TextBox != ""))
            {
                try
                {
                    if (SelectedKarakteristika != -1)
                    {
                        Karakteristika pom = Karakteristike.ElementAt(SelectedKarakteristika);
                        string stariNaziv = pom.naziv;
                        var original = dbContext.Karakteristikas.FirstOrDefault(x => x.naziv.Equals(pom.naziv));

                        if (original != null)
                        {
                            original.naziv = TextBox;
                            dbContext.SaveChanges();
                            Notifications.Success s = new Notifications.Success($"Uspešno ste izmenili karakteristiku {stariNaziv}");
                            s.Show();
                            Karakteristike.Clear();
                            foreach (var item in dbContext.Karakteristikas)
                            {
                                if (!KarakteristikaProizvod.Any(x => x.naziv.Equals(item.naziv)))
                                {
                                    Karakteristike.Add(item);
                                }
                            }
                        }
                    }
                    else
                    {
                        Notifications.Error e = new Notifications.Error("Morate selektovati karakteristiku");
                        e.Show();
                    }

                }
                catch (Exception ex)
                {
                    Notifications.Error e = new Notifications.Error("Greška pri izmeni karakteristike");
                    e.Show();
                }
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Naziv karakteristike je obavezno polje");
                e.Show();
            }
        }

        private void ObrisiKarakteristiku(string obj)
        {
            //to do autorizacija
            try
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;

                    }
                }
                if (SelectedKarakteristika > -1)
                {
                    string nazivbrisanog = Karakteristike.ElementAt(SelectedKarakteristika).naziv;
                    if (dbContext.Karakteristikas.Any(x => x.naziv.Equals(nazivbrisanog)))
                    {
                        dbContext.Karakteristikas.Remove(dbContext.Karakteristikas.FirstOrDefault(x => x.naziv.Equals(nazivbrisanog)));
                        dbContext.SaveChanges();
                        Success suc = new Success("Uspešno ste obrisali karakteristiku.");
                        suc.Show();

                        SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Uspešno brisanje karakteristike {nazivbrisanog}.", "Info");
                        Karakteristike.Clear();
                        foreach (var item in dbContext.Karakteristikas.ToList())
                        {
                            if (!KarakteristikaProizvod.Any(x => x.id == item.id))
                            {
                                Karakteristike.Add(item);
                            }


                        }
                    }
                    else
                    {
                        Error er = new Error("Greška pri pronalaženju karakteristike.\nZa više informacija obratite se administratorima.");
                        er.Show();

                    }
                }
                else
                {
                    Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                    e.Show();
                }
            }
            catch (Exception ex)
            {
                Notifications.Error e = new Notifications.Error("Nije moguće izvršiti brisanje ove karakteristike.");
                e.Show();
            }
        }
        
        private void Back(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("proizvodi");
                }
            }
        }
        private void Remove(int obj)
        {
            if (SelectedKarakteristikaProizvod != -1)
            {
                Karakteristika p = KarakteristikaProizvod.ElementAt(SelectedKarakteristikaProizvod);
                KarakteristikaProizvod.RemoveAt(SelectedKarakteristikaProizvod);
                ProizvodForBind.Karakteristikas.Remove(p);
                Karakteristike.Add(p);
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }
        private void Add(int index)
        {
            if (SelectedKarakteristika != -1)
            {
                Karakteristika p = Karakteristike.ElementAt(SelectedKarakteristika);
                Karakteristike.RemoveAt(SelectedKarakteristika);
                KarakteristikaProizvod.Add(p);
                ProizvodForBind.Karakteristikas.Add(p);
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }
        private void AddProizvodjac(string obj)
        {
           
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;

                    ((MainWindowViewModel)((MainWindow)w).DataContext).DodajProizvodjacaViewModel = new DodajProizvodjacaViewModel(0, null,ProizvodForBind);
                    ((MainWindowViewModel)((MainWindow)w).DataContext).DodajProizvodjacaViewModel.UserOnSession = this.UserOnSession;
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("dodajProizvodjaca");

                }
            }
        }
        private void Dodaj(string obj)
        {
            //to do autorizacija
            bool greska = false;
            try
            {
                if (MereForBind != null && !String.IsNullOrWhiteSpace(MereForBind) && (MereForBind != ""))
                {
                    if (!dbContext.jedinicameres.Any(x => x.naziv.ToUpper().Equals(MereForBind.ToUpper())))
                    {
                        jedinicamere pom = new jedinicamere();
                        pom.naziv = MereForBind;
                        try
                        {
                            dbContext.jedinicameres.Add(pom);
                            dbContext.SaveChanges();
                            Jedinicamere.Clear();
                            foreach (var item in dbContext.jedinicameres)
                            {
                                Jedinicamere.Add(item.naziv);
                            }
                        }
                        catch (Exception exc)
                        {
                            Notifications.Error e = new Notifications.Error("Greška pri dodavanju jedinice mere");
                            e.Show();
                            greska = true;
                        }
                    }


                    if (!greska)
                    {
                        if (context == 0)
                        {
                            try
                            {
                                ProizvodForBind.jedinicamere_id = dbContext.jedinicameres.FirstOrDefault(x => x.naziv.Equals(MereForBind)).id;
                                ProizvodForBind.proizvodjac_id = dbContext.Proizvodjacs.FirstOrDefault(x => x.naziv.Equals(ProizvodjacForBind)).id;
                                dbContext.Proizvods.Add(ProizvodForBind);
                                dbContext.SaveChanges();
                                Jedinicamere.Clear();
                                foreach (var item in dbContext.jedinicameres)
                                {
                                    Jedinicamere.Add(item.naziv);
                                }

                                Notifications.Success e = new Notifications.Success($"Uspešno dodavanje proizvoda {ProizvodForBind.naziv}");
                                e.Show();
                                Back("");
                            }
                            catch (Exception exc)
                            {
                                Notifications.Error e = new Notifications.Error("Greška pri dodavanju proizvoda");
                                e.Show();

                            }

                        }
                        else
                        {
                            try
                            {
                                ProizvodForBind.jedinicamere_id = dbContext.jedinicameres.FirstOrDefault(x => x.naziv.Equals(MereForBind)).id;
                                ProizvodForBind.proizvodjac_id = dbContext.Proizvodjacs.FirstOrDefault(x => x.naziv.Equals(ProizvodjacForBind)).id;
                                dbContext.SaveChanges();
                                Notifications.Success s = new Notifications.Success("Uspešno ste izmenili proizvod " + stariNaziv);
                                s.Show();
                                foreach (Window w in Application.Current.Windows)
                                {
                                    if (w.GetType().Equals(typeof(MainWindow)))
                                    {
                                        SecurityManager.AuditManager.AuditToDB(((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime, $"Uspesno je izmenjen proizvod {stariNaziv}", "Info");

                                    }
                                }
                                Back("");

                            }
                            catch (Exception ex)
                            {
                                Notifications.Error e = new Notifications.Error("Greška pri izmeni proizvoda");
                                e.Show();
                            }
                        }
                    }
                }
                else
                {
                    Notifications.Error e = new Notifications.Error("Jedinica mere je obavezno polje");
                    e.Show();
                }

            }
            catch (Exception ex)
            {
                Notifications.Error e = new Notifications.Error("Greška pri konekciji sa bazom");
                e.Show();
            }

        }
        private void Otkazi(string obj)
        {
            Back("");
        }
        #endregion

        #region Constructors
        public Proizvod ProizvodForBind
        {
            get => proizvodForBind;
            set
            {
                proizvodForBind = value;
                OnPropertyChanged("ProizvodForBind");
            }
        }
        public Korisnik UserOnSession { get => userOnSession; set => userOnSession = value; }
        public ObservableCollection<string> Jedinicamere
        {
            get => jedinicamere;
            set
            {
                jedinicamere = value;
                OnPropertyChanged("Jedinicamere");
            }
        }

        public string MereForBind
        {
            get => mereForBind;
            set
            {
                mereForBind = value;
                OnPropertyChanged("MereForBind");
            }
        }

        public ObservableCollection<Karakteristika> Karakteristike
        {
            get => karakteristike;
            set
            {
                karakteristike = value;
                OnPropertyChanged("Karakteristike");
            }
        }

        public int SelectedKarakteristika
        {
            get => _selectedKarakteristika;
            set
            {
                if (_selectedKarakteristika == value)
                {
                    if (_selectedKarakteristika > -1)
                    {
                        AddEnabled = true;
                    }
                    else
                    {
                        AddEnabled = false;
                    }
                    return;
                }
                _selectedKarakteristika = value;
                if (_selectedKarakteristika > -1)
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
        public int SelectedKarakteristikaProizvod
        {
            get => _selectedKarakteristikaProizvod;
            set
            {
                if (_selectedKarakteristikaProizvod == value)
                {
                    if (_selectedKarakteristikaProizvod > -1)
                    {
                        RemoveEnabled = true;
                    }
                    else
                    {
                        RemoveEnabled = false;
                    }
                    return;
                }
                _selectedKarakteristikaProizvod = value;
                if (_selectedKarakteristikaProizvod > -1)
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

        public ObservableCollection<Karakteristika> KarakteristikaProizvod
        {
            get => karakteristikaProizvod;
            set
            {
                karakteristikaProizvod = value;
                OnPropertyChanged("KarakteristikaProizvod");
            }
        }

        public string TextBox
        {
            get => textBox;
            set
            {
                textBox = value;
                OnPropertyChanged("TextBox");
            }
        }

        public string ProizvodjacForBind
        {
            get => proizvodjacForBind;
            set
            {
                proizvodjacForBind = value;
                OnPropertyChanged("ProizvodjacForBind");
            }
        }

        public ObservableCollection<Proizvodjac> Proizvodjaci
        {
            get => proizvodjaci;
            set
            {
                proizvodjaci = value;
                OnPropertyChanged("Proizvodjaci");
            }
        }

        public string SubmitButtonText { get => submitButtonText; set { submitButtonText = value; OnPropertyChanged("SubmitButtonText"); } }



        #endregion
    }
}
