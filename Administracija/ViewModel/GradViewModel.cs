using Common;
using Common.Model;
using Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Administracija.ViewModel
{
    public class GradViewModel : BindableBase
    {
        #region Members
        private ObservableCollection<grad> gradovi;
        private ICollectionView defaultView;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private Korisnik userOnSession;
        private int _selectedIndex = -1;
        private bool selectedInGrid;
        private string textSearch;
        private grad selectedValue;
        
        
        #endregion

        #region Commands
        public MyICommand<string> DodajGradNavCommand { get; private set; }
        public MyICommand<string> IzmeniGradNavCommand { get; private set; }
        public MyICommand<string> IzbrisiGradCommand { get; private set; }
        public MyICommand<string> PretraziGradCommand { get; private set; }
        #endregion

        public GradViewModel()
        {
            DodajGradNavCommand = new MyICommand<string>(DodajGradNav);
            IzmeniGradNavCommand = new MyICommand<string>(IzmeniGradNav);
            IzbrisiGradCommand = new MyICommand<string>(IzbrisiGrad);
            PretraziGradCommand = new MyICommand<string>(PretraziGrad);
            textSearch = "";
            gradovi = new ObservableCollection<grad>();
            foreach (var item in dbContext.grads.ToList())
            {
                gradovi.Add(item);
            }

            DefaultView = CollectionViewSource.GetDefaultView(Gradovi);
        }


        #region CommandsImplementation

        private void DodajGradNav(string obj)
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
                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w.GetType().Equals(typeof(MainWindow)))
                        {
                            ((MainWindowViewModel)((MainWindow)w).DataContext).dodajGradViewModel = new DodajGradViewModel(0, null);
                            ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).dodajGradViewModel;
                            ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Gradovi -> Dodaj";
                        }
                    }
                }
                else
                {
                    Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                    er.Show();
                }
            }
            catch (Exception ex)
            {
                Notifications.Error e = new Notifications.Error("Greška pri povezivanju sa bazom");
                e.Show();
            }
            
        }

        private void IzmeniGradNav(string obj)
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
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType().Equals(typeof(MainWindow)))
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).dodajGradViewModel = new DodajGradViewModel(1, selectedValue);
                        ((MainWindowViewModel)((MainWindow)w).DataContext).CurrentViewModel = ((MainWindowViewModel)((MainWindow)w).DataContext).dodajGradViewModel;
                        ((MainWindowViewModel)((MainWindow)w).DataContext).ViewModelTitle = "Gradovi -> Izmeni";
                    }
                }
            }
            catch (Exception ex)
            {
                Notifications.Error e = new Notifications.Error("Greška pri povezivanju sa bazom");
                e.Show();
            }
            
        }

        private void IzbrisiGrad(string obj)
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

                if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.DeleteGrad))
                {


                    if (SelectedIndex > -1)
                    {
                        string nazivbrisanog = SelectedValue.naziv;
                        if (dbContext.grads.Any(x => x.id == selectedValue.id))
                        {
                            dbContext.grads.Remove(dbContext.grads.FirstOrDefault(x => x.id == selectedValue.id));
                            dbContext.SaveChanges();
                            Success suc = new Success("Uspešno ste obrisali grad.");
                            suc.Show();

                            SecurityManager.AuditManager.AuditToDB(userOnSession.korisnickoime, $"Uspešno brisanje grada {nazivbrisanog}.", "Info");
                            gradovi.Clear();
                            foreach (var item in dbContext.grads.ToList())
                            {
                                gradovi.Add(item);

                            }
                        }
                        else
                        {
                            Error er = new Error("Greška pri pronalaženju korisnika.\nZa više informacija obratite se administratorima.");
                            er.Show();

                        }
                    }



                }
                else
                {

                    Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                    er.Show();
                    SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspesan pokusaj brisnja grada", "Upozorenje");

                }

            }
            catch (Exception ex)
            {
                //to do ako ce se obraditi brisanje 
                Error er = new Error("Grad je povezan unutar baze");
                er.Show();
            }
        }

        private void PretraziGrad(string type)
        {
            if (!type.Equals("/"))
            {
                if (TextSearch != null && !String.IsNullOrWhiteSpace(TextSearch) && (TextSearch != ""))
                {
                    DefaultView = CollectionViewSource.GetDefaultView(DefaultView);
                    if (type.Equals("Nazivu"))
                    {
                        DefaultView.Filter =
                        w => ((grad)w).naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Državi"))
                    {
                        DefaultView.Filter =
                        w => ((grad)w).drzava.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Poštanskom broju"))
                    {
                        DefaultView.Filter =
                        w => ((grad)w).postanskibroj.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    

                    DefaultView.Refresh();
                }
                else
                {
                    DefaultView = CollectionViewSource.GetDefaultView(Gradovi);
                    DefaultView.Filter = null;
                    DefaultView.Refresh();
                }
            }
            else
            {
                DefaultView = CollectionViewSource.GetDefaultView(Gradovi);
                DefaultView.Filter = null;
                DefaultView.Refresh();
            }
        }
        #endregion

        #region Propeties

        public ObservableCollection<grad> Gradovi
        {
            get => gradovi;
            set
            {
                gradovi = value;
                OnPropertyChanged("Gradovi");
            }
        }

        public ICollectionView DefaultView
        {
            get => defaultView;
            set
            {
                defaultView = value;
                OnPropertyChanged("DefaultView");
            }
        }

        public bool SelectedInGrid
        {
            get { return selectedInGrid; }
            set
            {
                selectedInGrid = value;
                OnPropertyChanged("SelectedInGrid");
            }
        }

        public string TextSearch
        {
            get { return textSearch; }
            set
            {
                textSearch = value;
                OnPropertyChanged("TextSearch");
            }
        }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }

            set
            {
                if (_selectedIndex == value)
                {
                    if (_selectedIndex > -1)
                    {
                        SelectedInGrid = true;
                    }
                    return;
                }
                _selectedIndex = value;
                if (_selectedIndex > -1)
                {
                    SelectedInGrid = true;
                }
            }
        }

        public grad SelectedValue
        {
            get { return selectedValue; }
            set
            {
                selectedValue = value;
                OnPropertyChanged("SelectedValue");
            }
        }

        public Korisnik UserOnSession
        {
            get { return userOnSession; }
            set { userOnSession = value; }
        }

        #endregion

    }
}
