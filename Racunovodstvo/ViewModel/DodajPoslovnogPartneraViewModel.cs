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
    public class DodajPoslovnogPartneraViewModel : BindableBase
    {
        #region Members
        private int context;
        private PoslovniPartner poslovniPartnerForEdit;
        private Korisnik userOnSession = new Korisnik();
        private ObservableCollection<grad> gradovi;
        private string gradForBind;
        private string submitButtonText;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        #endregion

        #region Commands
        public MyICommand<string> DodajCommand { get; private set; }
        public MyICommand<string> OtkaziCommand { get; private set; }
        public MyICommand<string> BackCommand { get; private set; }
        #endregion
        public DodajPoslovnogPartneraViewModel(int i, PoslovniPartner p)
        {
            DodajCommand = new MyICommand<string>(Dodaj);
            OtkaziCommand = new MyICommand<string>(Otkazi);
            BackCommand = new MyICommand<string>(Back);
            context = i;

            gradovi = new ObservableCollection<grad>();
            foreach (var item in dbContext.grads)
            {
                Gradovi.Add(item);
            }

            if (i == 0)
            {
                SubmitButtonText = "Dodaj";
                PoslovniPartnerForEdit = new PoslovniPartner();
            }
            else
            {
                SubmitButtonText = "Potvrdi izmenu";
                GradForBind = p.grad.naziv;
                PoslovniPartnerForEdit = p;
            }
        }
        #region Commands Implements
        private void Otkazi(string obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("poslovnipartneri");
                }
            }
        }
        private void Back(string obj)
        {
            Otkazi("");
        }
        private void Dodaj(string obj)
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
                    //if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.AddProizvodjac))
                    //{
                        PoslovniPartnerForEdit.grad_id = dbContext.grads.FirstOrDefault(x => x.naziv.Equals(GradForBind)).id;
                        PoslovniPartnerForEdit.dugovanja = 0;
                        dbContext.PoslovniPartners.Add(PoslovniPartnerForEdit);
                        dbContext.SaveChanges();

                        Success suc = new Success("Uspešno ste dodali novog poslovnog partnera!");
                        suc.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, $"Uspesno dodavanje poslovnog partnera {PoslovniPartnerForEdit.naziv}.", "Info");
                        Otkazi("");
                    //}
                    //else
                    //{
                    //    Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                    //    er.Show();
                    //    SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesno dodavanje proizvodjaca. Autorizacija.", "Upozorenje");
                    //    Otkazi("");
                    //}

                }
                else
                {
                    try
                    {
                        //if (SecurityManager.AuthorizationPolicy.HavePermission(UserOnSession.id, SecurityManager.Permission.EditProizvodjac))
                        //{
                            var original = dbContext.PoslovniPartners.FirstOrDefault(x => x.mbr == PoslovniPartnerForEdit.mbr);

                            if (original != null)
                            {
                                string stariNaziv = original.naziv;
                                original.naziv = PoslovniPartnerForEdit.naziv;
                                original.adresa = PoslovniPartnerForEdit.adresa;
                                original.brojtelefona = PoslovniPartnerForEdit.brojtelefona;
                                original.dugovanja = PoslovniPartnerForEdit.dugovanja;
                                original.email = PoslovniPartnerForEdit.email;
                                original.pib = PoslovniPartnerForEdit.pib;
                                original.tekuciracun = PoslovniPartnerForEdit.tekuciracun;
                                original.grad_id = dbContext.grads.FirstOrDefault(x => x.naziv.Equals(GradForBind)).id;
                                dbContext.SaveChanges();

                                Notifications.Success s = new Notifications.Success("Uspešno ste izmenili " + stariNaziv);
                                s.Show();
                                foreach (Window w in Application.Current.Windows)
                                {
                                    if (w.GetType().Equals(typeof(MainWindow)))
                                    {
                                        SecurityManager.AuditManager.AuditToDB(((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession.korisnickoime, "Uspesno je izmenjen poslovni partner " + stariNaziv, "Info");
                                    }
                                }
                            }

                            Otkazi("");
                        //}
                        //else
                        //{
                        //    Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        //    er.Show();
                        //    SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesna izmena proizvodjaca. Autorizacija.", "Upozorenje");
                        //    Otkazi("");
                        //}
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
        public PoslovniPartner PoslovniPartnerForEdit { get => poslovniPartnerForEdit; set { poslovniPartnerForEdit = value; OnPropertyChanged("PoslovniPartnerForEdit"); } }
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
