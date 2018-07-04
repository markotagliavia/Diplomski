using Common;
using Common.Model;
using Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Racunovodstvo.ViewModel
{
    public class DodajZalihuViewModel : BindableBase
    {
        #region Commands
        public MyICommand<string> DodajCommand { get; set; }
        public MyICommand<string> OtkaziCommand { get; set; }
        #endregion
        #region Members
        private System.Windows.Media.Color c2;
        private System.Windows.Media.Brush _backgroundColor;
        private string skladisteForBind;
        private string proizvodForBind;
        private string raf;
        private double minimumKolicine;
        private ObservableCollection<Skladiste> skladista;
        private ObservableCollection<Proizvod> proizvodi;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        public Action<string> MessageDelegate { get; set; }
        #endregion
        public DodajZalihuViewModel()
        {
            DodajCommand = new MyICommand<string>(Dodaj);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            c2 = System.Windows.Media.Color.FromArgb(255, 37, 44, 50);
            BackgroundColor = new SolidColorBrush(c2);
            Skladista = new ObservableCollection<Skladiste>();
            SkladisteForBind = "";
            foreach (var item in dbContext.Skladistes)
            {
                Skladista.Add(item);
            }
            Proizvodi = new ObservableCollection<Proizvod>();
            ProizvodForBind = "";
            foreach (var item in dbContext.Proizvods)
            {
                Proizvodi.Add(item);
            }
        }

        private void Otkazi(string obj)
        {
            MainWindowViewModel.Instance.OnNav(Navigation.zalihe);
        }

        public ObservableCollection<Skladiste> Skladista { get => skladista; set { skladista = value; OnPropertyChanged("Skladista"); } }

        public string SkladisteForBind
        {
            get => skladisteForBind;
            set
            {
                skladisteForBind = value;

                OnPropertyChanged("SkladisteForBind");
            }
        }

        public ObservableCollection<Proizvod> Proizvodi { get => proizvodi; set { proizvodi = value; OnPropertyChanged("Skladista"); } }

        public string ProizvodForBind
        {
            get => proizvodForBind;
            set
            {
                proizvodForBind = value;

                OnPropertyChanged("ProizvodForBind");
            }
        }

        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            }
        }

        public string Raf { get => raf; set { raf = value; OnPropertyChanged("Raf"); } }
        public double MinimumKolicine { get => minimumKolicine; set { minimumKolicine = value; OnPropertyChanged("MinimumKolicine"); } }

        private void Dodaj(string obj)
        {
            if (String.IsNullOrEmpty(SkladisteForBind) || String.IsNullOrEmpty(ProizvodForBind) || String.IsNullOrEmpty(Raf))
            {
                Error e = new Error("Sva polja su obavezna");
                e.Show();
                return;
            }


            //to do da li se proizvod vec nalazi tamo

            Zalihe z = new Zalihe();
            z.Skladiste = dbContext.Skladistes.FirstOrDefault(x => x.naziv.Equals(SkladisteForBind));
            z.Proizvod = dbContext.Proizvods.FirstOrDefault(x => x.naziv.Equals(ProizvodForBind));
            if (dbContext.Zalihes.Any(x => x.proizvod_id == z.Proizvod.id && x.skladiste_id == z.Skladiste.id))
            {
                Error error = new Error("Ovaj proizvod se već nalazi na zalihama ovog skladišta.");
                error.Show();
                return;
            }
            z.kolicina = 0;
            z.minimumkolicine = this.minimumKolicine;

            z.raf = this.raf;
            z.rezervisano = 0;
            //if (SecurityManager.AuthorizationPolicy.HavePermission(MainWindowViewModel.Instance.UserOnSession.id,SecurityManager.Permission.AddZalihe))
            //{
                dbContext.Zalihes.Add(z);
                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    SecurityManager.AuditManager.AuditToDB(MainWindowViewModel.Instance.UserOnSession.korisnickoime,  $"Uspesno je zaliha {z.Proizvod.id} {z.Skladiste.id}", "Info");
                }
                Success s = new Success("Uspešno je dodata zaliha");
                s.Show();
                Otkazi("");
        //}
            //else
            //{
            //    SecurityManager.AuditManager.AuditToDB(MainWindowViewModel.Instance.UserOnSession.korisnickoime,"Neuspesni pokusaj dodavanja zalihe", "Upozorenje");
            ////}



            //MessageSender();
        }

        public void MessageSender()
        {

            InvokeMessage("Message: ");
            
        }
        public void InvokeMessage(string mess)
        {
            if (MessageDelegate != null)
            {
                MessageDelegate(mess);
            }
        }
    }
}
