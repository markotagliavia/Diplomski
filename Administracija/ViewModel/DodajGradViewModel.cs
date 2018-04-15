using Common;
using Common.Model;
using Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Administracija.ViewModel
{
    public class DodajGradViewModel:BindableBase
    {
        #region Members
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private int context;
        private string stariNaziv;
        private grad grad;
        private string submitButtonText;
        private Korisnik userOnSession;
        #endregion

        #region Commands
        public MyICommand<string> DodajCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        public MyICommand<string> BackNavCommand { get; private set; }
        
        #endregion

        public DodajGradViewModel()
        {
        }

        public DodajGradViewModel(int i,grad grad)
        {
            DodajCommand = new MyICommand<string>(Dodaj);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            BackNavCommand = new MyICommand<string>(Back);
            context = i;
            
            if (i == 0)
            {
                Grad = new grad();
                SubmitButtonText = "Dodaj";
            }
            else
            {
                Grad = grad;
                stariNaziv = grad.naziv;
                SubmitButtonText = "Potvrdi izmenu";
            }

        }

        #region CommandsImplementation
        private void Back(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {

                    ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).GradViewModel;
                    ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Pregled gradova";
                }
            }
        }

        private void Otkazi(string obj)
        {
            Back("");
        }

        private void Dodaj(string obj)
        {
            if (context == 0)
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
                    if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.AddGrad))
                    {
                        dbContext.grads.Add(Grad);
                        dbContext.SaveChanges();
                        Notifications.Success s = new Notifications.Success("Uspešno ste kreirali grad");
                        foreach (Window w in Application.Current.Windows)
                        {
                            if (w.GetType().Equals(typeof(MainWindow)))
                            {
                                SecurityManager.AuditManager.AuditToDB(((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime, "Uspesno je dodat grad " + Grad.naziv, "Info");

                            }
                        }
                        Back("");
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj dodavanja grada", "Upozorenje");
                        Back("");
                    }

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
                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w.GetType().Equals(typeof(MainWindow)))
                        {
                            UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;

                        }
                    }
                    if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.EditGrad))
                    {
                        var original = dbContext.grads.FirstOrDefault(x => x.naziv.Equals(stariNaziv));

                        if (original != null)
                        {
                            original.naziv = grad.naziv;
                            original.postanskibroj = grad.postanskibroj;
                            original.drzava = grad.drzava;
                            dbContext.SaveChanges();
                        }
                        Notifications.Success s = new Notifications.Success("Uspešno ste izmenili grad " + stariNaziv);
                        s.Show();
                        foreach (Window w in Application.Current.Windows)
                        {
                            if (w.GetType().Equals(typeof(MainWindow)))
                            {
                                SecurityManager.AuditManager.AuditToDB(((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime, "Uspesno je izmenjen grad " + stariNaziv, "Info");

                            }
                        }
                        Back("");
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj izmene grada", "Upozorenje");
                        Back("");
                    }

                }
                catch (Exception ex)
                {
                    Notifications.Error e = new Notifications.Error("Greška pri izmeni grada");
                    e.Show();
                }
            }
        }
        #endregion

        #region Propeties
        public grad Grad
        {
            get => grad;
            set
            {
                grad = value;
                OnPropertyChanged("Grad");
            }
        }

        public string SubmitButtonText { get => submitButtonText; set { submitButtonText = value; OnPropertyChanged("SubmitButtonText"); } }

        public Korisnik UserOnSession { get => userOnSession; set => userOnSession = value; }
        #endregion
    }
}
