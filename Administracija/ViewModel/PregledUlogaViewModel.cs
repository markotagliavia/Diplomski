using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Administracija.ViewModel
{
    public class PregledUlogaViewModel : BindableBase
    {
        #region Commands
        private ObservableCollection<Common.Model.Uloga> uloge;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        public MyICommand<string> DodajUloguNavCommand { get; private set; }
        public MyICommand<int> IzmeniUloguNavCommand { get; private set; }
        public MyICommand<int> IzbrisiUloguCommand { get; private set; }
        

        #endregion

        public PregledUlogaViewModel()
        {
            DodajUloguNavCommand = new MyICommand<string>(DodajUloguNav);
            IzmeniUloguNavCommand = new MyICommand<int>(IzmeniUloguNav);
            IzbrisiUloguCommand = new MyICommand<int>(IzbrisiUlogu);
            Uloge = new ObservableCollection<Common.Model.Uloga>();
            foreach (var item in dbContext.Ulogas.ToList())
            {
                Uloge.Add(item);
                
            }

        }

        #region CommandImplementation
        

        private void IzbrisiUlogu(int index)
        {
            if (index != -1)
            {
                string naziv = uloge.ElementAt(index).naziv;
                var itemToRemove = dbContext.Ulogas.FirstOrDefault(x => x.naziv.Equals(naziv)); 

                if (itemToRemove != null)
                {
                    foreach (var zaposleni in dbContext.Zaposlenis)
                    {
                        if (zaposleni.Ulogas.Any(x => x.naziv.Equals(naziv)))
                        {
                            
                            zaposleni.Ulogas.Remove(itemToRemove);
                        }
                    }
                    dbContext.Ulogas.Remove(itemToRemove);
                    dbContext.SaveChanges();
                }
                Notifications.Success s = new Notifications.Success("Uspešno ste obrisali " + itemToRemove.naziv);
                s.Show();
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        SecurityManager.AuditManager.AuditToDB(((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime, "Uspesno je obrisana uloga " + itemToRemove.naziv, "Info");
                        
                    }
                }
                
                uloge.Clear();
                foreach (var item in dbContext.Ulogas.ToList())
                {
                    Uloge.Add(item);

                }
              
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }

        private void IzmeniUloguNav(int index)
        {
            if (index != -1)
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).dodajUloguViewModel = new DodajUloguViewModel(1,uloge.ElementAt(index).naziv);
                        ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).dodajUloguViewModel;
                        ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Izmeni ulogu";
                    }
                }
            }
            else
            {

                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }

        private void DodajUloguNav(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).dodajUloguViewModel = new DodajUloguViewModel(0, null);
                    ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).dodajUloguViewModel;
                    ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Dodaj ulogu";
                }
            }
        }
        #endregion

        #region Properties
        public ObservableCollection<Uloga> Uloge
        {
            get { return uloge; }
            set
            {
                uloge = value;
                OnPropertyChanged("Uloge");
            }
        }
        #endregion
    }
}
