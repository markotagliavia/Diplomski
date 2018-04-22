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
        private int i;
        private int idPopisa;
        private System.Windows.Media.Color c1;
        private System.Windows.Media.Brush _backgroundColor;
        private DeltaEximEntities dbContext = new DeltaEximEntities();
        private string labelText = "";
        private string dodajText = "";
        private ObservableCollection<StavkaPopisa> stavke;
        #endregion

        #region Commands
        public MyICommand<object> DodajCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        #endregion

        public PripisOtpisViewModel(int i, int idPopisa)
        {
            this.i = i;         //0-nema razlike 1-pripis  2-otpis
            this.idPopisa = idPopisa;
            c1 = System.Windows.Media.Color.FromArgb(255, 68, 95, 245);
            BackgroundColor = new SolidColorBrush(c1);
            DodajCommand = new MyICommand<object>(Dodaj);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            Stavke = new ObservableCollection<StavkaPopisa>();
            populateGrid(idPopisa);
            if (i == 0)
            {
                LabelText = "Prilikom popisa utvrđeno je isto stanje kao na zalihama.";
                DodajText = "OK";
            }
            else if (i == 1)
            {
                LabelText = "Prilikom popisa utvrđen je višak u odnosu na zalihe ovog skladišta. Da li želite da dodate korekcioni dokument sa stavkama iz tabele?";
                DodajText = "Dodaj";
            }
            else if (i == 2)
            {
                LabelText = "Prilikom popisa utvrđen je manjak u odnosu na zalihe ovog skladišta. Da li želite da dodate korekcioni dokument sa stavkama iz tabele?";
                DodajText = "Dodaj";
            }
        }

        #region Constructors
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
        #endregion

        #region CommandsImplementation

        private void Otkazi(string obj)
        {
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
            if (i == 0)
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
            else if (i == 1)
            {
                Korekcija(idPopisa);
                Success s = new Success("Novi popis sa pratećim korekcionim dokumentom je uspešno dodat.");
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
            else if(i == 2)
            {
                Korekcija(idPopisa);
                Success s = new Success("Novi popis sa pratećim korekcionim dokumentom je uspešno dodat.");
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
            foreach (var item in dbContext.StavkaPopisas.ToList())
            {
                if (item.popis_id == id)
                {
                    if (zalihe.Any(x => x.proizvod_id == item.proizvod_id))
                    {
                        Zalihe zPom = zalihe.FirstOrDefault(x => x.proizvod_id == item.proizvod_id);
                        dbContext.Zalihes.FirstOrDefault(x => x.skladiste_id == p.skladiste_id && x.proizvod_id == item.proizvod_id).kolicina = 0;
                    }
                }
            }

            dbContext.SaveChanges();

            foreach (var item in dbContext.StavkaPopisas.ToList())
            {
                if (item.popis_id == id)
                {
                    if (zalihe.Any(x => x.proizvod_id == item.proizvod_id))
                    {
                        Zalihe zPom = zalihe.FirstOrDefault(x => x.proizvod_id == item.proizvod_id);
                        dbContext.Zalihes.FirstOrDefault(x => x.skladiste_id == p.skladiste_id && x.proizvod_id == item.proizvod_id).kolicina += item.kolicina;
                    }
                }
            }

            dbContext.SaveChanges();
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
                }
            }
        }
        #endregion
    }
}
