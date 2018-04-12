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

namespace Skladistenje.ViewModel
{
    public class DodajProizvodjacaViewModel : BindableBase
    {
        #region Members
        private int context;
        private Proizvodjac proizvodjacForEdit;
        private Korisnik userOnSession = new Korisnik();
        private ObservableCollection<grad> gradovi;
        private string gradForBind;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        #endregion

        #region Commands
        public MyICommand<string> DodajProizvodjacaCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        #endregion

        public DodajProizvodjacaViewModel(int v, Proizvodjac p)
        {
            DodajProizvodjacaCommand = new MyICommand<string>(DodajProizvodjaca);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            context = v;
            
            gradovi = new ObservableCollection<grad>();
            foreach (var item in dbContext.grads)
            {
                Gradovi.Add(item);
            }

            if (v == 0)
            {
                ProizvodjacForEdit = new Proizvodjac();
            }
            else
            {
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
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("proizvodjaci");
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
