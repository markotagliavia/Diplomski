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
    public class DodajUloguViewModel : BindableBase
    {
        //komande su: dodaj i otkazi

        #region Commands
        public MyICommand<string> DodajUloguCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        public MyICommand<string> BackNavCommand { get; private set; }
        public MyICommand<int> AddNavCommand { get; private set; }
        public MyICommand<int> RemoveNavCommand { get; private set; }
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private int context;
        private string stariNaziv;
       
        

        private Common.Model.Uloga uloga;
        private ObservableCollection<Common.Model.Permission> permissions;
        private ObservableCollection<Common.Model.Permission> permissionsUloga;
        #endregion

        public DodajUloguViewModel()
        { }

        public DodajUloguViewModel(int i, string naziv)
        {
            DodajUloguCommand = new MyICommand<string>(DodajUlogu);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            BackNavCommand = new MyICommand<string>(Back);
            AddNavCommand = new MyICommand<int>(Add);
            RemoveNavCommand = new MyICommand<int>(Remove);
            context = i;
            stariNaziv = naziv;
            if (naziv != null)
            {
                uloga = dbContext.Ulogas.FirstOrDefault(x => x.naziv.Equals(naziv));
                
            }
            else
            {
                uloga = new Uloga();
            }
            Permissions = new ObservableCollection<Permission>();
            
            PermissionsUloga = new ObservableCollection<Permission>();
            foreach (var item in uloga.Permissions.ToList())
            {
                PermissionsUloga.Add(item);

            }
            foreach (var item in dbContext.Permissions.ToList())
            {
                if (!permissionsUloga.Any(x => x.naziv.Equals(item.naziv)))
                {
                    Permissions.Add(item);
                }
                

            }
        }




        #region CommandsImplementation


        private void Add(int index)
        {
            if (index != -1)
            {
                Permission p = Permissions.ElementAt(index);
                Permissions.RemoveAt(index);
                uloga.Permissions.Add(p);
                permissionsUloga.Add(p);
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }

        private void Remove(int index)
        {
            if (index != -1)
            {
                Permission p = PermissionsUloga.ElementAt(index);
                PermissionsUloga.RemoveAt(index);
                uloga.Permissions.Remove(p);
                permissions.Add(p);
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Morate selektovati odgovarajuću kolonu.");
                e.Show();
            }
        }


        private void Back(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    
                    ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).pregledUlogaViewModel;
                    ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Pregled uloga";
                }
            }
        }

        private void Otkazi(string obj)
        {
            Back("");
        }

        private void DodajUlogu(string obj)
        {

            if (uloga.naziv != null && !String.IsNullOrWhiteSpace(uloga.naziv) && (uloga.naziv != ""))
            {
                if (uloga.naziv.Length < 30)
                {
                    if (context == 0)
                    {
                        try
                        {
                            dbContext.Ulogas.Add(uloga);
                            dbContext.SaveChanges();
                            Notifications.Success s = new Notifications.Success("Uspešno ste kreirali ulogu");
                            foreach (Window w in Application.Current.Windows)
                            {
                                if (w.GetType().Equals(typeof(MainWindow)))
                                {
                                    SecurityManager.AuditManager.AuditToDB(((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime, "Uspesno je dodata uloga " + uloga.naziv, "Info");

                                }
                            }
                            Back("");
                           
                        }
                        catch (Exception ex)
                        {
                            Notifications.Error e = new Notifications.Error("Greška pri unosu uloge");
                            e.Show();
                        }
                    }
                    else
                    {
                        try
                        {
                            var original = dbContext.Ulogas.FirstOrDefault(x => x.naziv.Equals(stariNaziv));

                            if (original != null)
                            {
                                original.naziv = uloga.naziv;
                                original.Permissions = uloga.Permissions;
                                dbContext.SaveChanges();
                            }
                            Notifications.Success s = new Notifications.Success("Uspešno ste izmenili " + stariNaziv);
                            s.Show();
                            foreach (Window w in Application.Current.Windows)
                            {
                                if (w.GetType().Equals(typeof(MainWindow)))
                                {
                                    SecurityManager.AuditManager.AuditToDB(((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime, "Uspesno je izmenjena uloga " + stariNaziv, "Info");

                                }
                            }
                            Back("");

                        }
                        catch (Exception ex)
                        {
                            Notifications.Error e = new Notifications.Error("Greška pri unosu uloge");
                            e.Show();
                        }
                    }
                }
                else
                {
                    Notifications.Error e = new Notifications.Error("Maksimalna dužina polja \n naziv uloge je 30 karaktera");
                    e.Show();
                }
                
            }
            else
            {
                Notifications.Error e = new Notifications.Error("Naziv uloge je obavezno polje");
                e.Show();
            }

               
       
                
        }
        #endregion

        #region Properties
        public Uloga Uloga
        {
            get { return uloga; }
            set
            {
                uloga = value;
                OnPropertyChanged("Uloga");
            }
        }

        public ObservableCollection<Permission> Permissions
        {
            get { return permissions; }
            set
            {
                permissions = value;
                OnPropertyChanged("Permissions");
            }
        }

        public ObservableCollection<Permission> PermissionsUloga
        {
            get { return permissionsUloga; }
            set
            {
                permissionsUloga = value;
                OnPropertyChanged("PermissionsUloga");
            }
        }


        #endregion
    }
}
