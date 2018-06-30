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
    public class DodajProizvodjacaViewModel :BindableBase
    {
        #region Members
        private int context;
        private Proizvodjac proizvodjacForEdit;
        private Korisnik userOnSession = new Korisnik();
        private ObservableCollection<grad> gradovi;
        private string gradForBind;
        private string submitButtonText;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private Proizvod proizvodBack;
        #endregion

        #region Commands
        public MyICommand<string> DodajProizvodjacaCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        public MyICommand<string> BackNavCommand { get; private set; }
        #endregion

        public DodajProizvodjacaViewModel(int v, Proizvodjac p,Proizvod proizvod)
        {
            DodajProizvodjacaCommand = new MyICommand<string>(DodajProizvodjaca);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            BackNavCommand = new MyICommand<string>(Back);
            context = v;
            proizvodBack = proizvod;
            gradovi = new ObservableCollection<grad>();
            foreach (var item in dbContext.grads)
            {
                Gradovi.Add(item);
            }

            if (v == 0)
            {
                SubmitButtonText = "Dodaj";
                ProizvodjacForEdit = new Proizvodjac();
            }
            else
            {
                SubmitButtonText = "Potvrdi izmenu";
                GradForBind = p.grad.naziv;
                ProizvodjacForEdit = p;
            }
        }

        #region CommandsImplementation

        private void Otkazi(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;

                    ((MainWindowViewModel)((MainWindow)w).DataContext).DodajProizvodViewModel = new DodajProizvodViewModel(0, proizvodBack);
                    ((MainWindowViewModel)((MainWindow)w).DataContext).DodajProizvodViewModel.UserOnSession = this.UserOnSession;
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("dodajProizvod");
                    ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Novi Proizvod";

                }
            }
        }

        private void Back(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                    
                    ((MainWindowViewModel)((MainWindow)w).DataContext).DodajProizvodViewModel = new DodajProizvodViewModel(0, proizvodBack);
                    ((MainWindowViewModel)((MainWindow)w).DataContext).DodajProizvodViewModel.UserOnSession = this.UserOnSession;
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("dodajProizvod");
                    ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Novi Proizvod";
                    
                }
            }
        }

        private void DodajProizvodjaca(string obj)
        {
            try
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;

                    }
                }
                if (context == 0)
                {
                    if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.AddProizvodjac))
                    {
                        ProizvodjacForEdit.grad_id = dbContext.grads.FirstOrDefault(x => x.naziv.Equals(GradForBind)).id;
                        dbContext.Proizvodjacs.Add(ProizvodjacForEdit);
                        dbContext.SaveChanges();

                        Success suc = new Success("Uspešno ste dodali novog proizvođača!");
                        suc.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, $"Uspesno dodavanje proizvodjaca {ProizvodjacForEdit.naziv}.", "Info");
                        Otkazi("");
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesno dodavanje proizvodjaca. Autorizacija.", "Upozorenje");
                        Otkazi("");
                    }

                }
                else
                {
                    try
                    {
                        if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.EditProizvodjac))
                        {
                            var original = dbContext.Proizvodjacs.FirstOrDefault(x => x.id == ProizvodjacForEdit.id);

                            if (original != null)
                            {
                                string stariNaziv = original.naziv;
                                original.naziv = ProizvodjacForEdit.naziv;
                                original.grad_id = dbContext.grads.FirstOrDefault(x => x.naziv.Equals(GradForBind)).id;
                                dbContext.SaveChanges();

                                Notifications.Success s = new Notifications.Success("Uspešno ste izmenili " + stariNaziv);
                                s.Show();
                                foreach (Window w in Application.Current.Windows)
                                {
                                    if (w.GetType().Equals(typeof(MainWindow)))
                                    {
                                        SecurityManager.AuditManager.AuditToDB(((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime, "Uspesno je izmenjen proizvodjac " + stariNaziv, "Info");
                                    }
                                }
                            }

                            Otkazi("");
                        }
                        else
                        {
                            Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                            er.Show();
                            SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesna izmena proizvodjaca. Autorizacija.", "Upozorenje");
                            Otkazi("");
                        }
                    }
                    catch (Exception ex)
                    {
                        Notifications.Error e = new Notifications.Error("Greška pri unosu!");
                        e.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Error er = new Error("Greška sa konekcijom!\nObratite se administratorima.");
                er.Show();
            }
        }
        #endregion

        #region Constructors
        public string SubmitButtonText { get => submitButtonText; set { submitButtonText = value; OnPropertyChanged("SubmitButtonText"); } }
        public Korisnik UserOnSession { get { return userOnSession; } set { userOnSession = value; } }
        public Proizvodjac ProizvodjacForEdit { get => proizvodjacForEdit; set { proizvodjacForEdit = value; OnPropertyChanged("ProizvodjacForEdit"); } }
        public ObservableCollection<grad> Gradovi
        {
            get => gradovi;
            set
            {
                gradovi = value;
                OnPropertyChanged("Gradovi");
            }
        }

        public string GradForBind
        {
            get => gradForBind;
            set
            {
                gradForBind = value;
                OnPropertyChanged(GradForBind);
            }
        }
        #endregion
    }
}
