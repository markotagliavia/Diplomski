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
using System.Windows.Data;

namespace Racunovodstvo.ViewModel
{
    public class ProfaktureViewModel:BindableBase
    {
        #region Members
        private ObservableCollection<Profaktura> profakture;
        private int _selectedIndex = -1;
        private bool selectedInGrid;
        private string textSearch;
        private Profaktura selectedValue;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ICollectionView defaultView;
        private int context;
        #endregion
        #region Commands
        public MyICommand<string> PretraziCommand { get; private set; }
        public MyICommand<string> DodajCommand { get; set; }
        public MyICommand<string> IzmeniCommand { get; set; }
        public MyICommand<string> ObrisiCommand { get; set; }
        #endregion
        public ProfaktureViewModel()
        {
            PretraziCommand = new MyICommand<string>(Pretrazi);
            DodajCommand = new MyICommand<string>(Dodaj);
            IzmeniCommand = new MyICommand<string>(Izmeni);
            ObrisiCommand = new MyICommand<string>(Obrisi);
            textSearch = "";
            Profakture = new ObservableCollection<Profaktura>();
            foreach (var item in dbContext.Profakturas)
            {
                if (item.active)
                {
                    profakture.Add(item);
                }
                
            }
            DefaultView = CollectionViewSource.GetDefaultView(Profakture);
        }

        private void Obrisi(string obj)
        {
            if (SelectedIndex < -1)
            {
                Error er = new Error("Morate selektovati profakturu.");
                er.Show();
                return;

            }
            if (SecurityManager.AuthorizationPolicy.HavePermission(MainWindowViewModel.Instance.UserOnSession.id, SecurityManager.Permission.DeleteProfaktura))
            {

                dbContext.Profakturas.FirstOrDefault(x => x.oznaka == SelectedValue.oznaka).active = false;
                try
                {
                    dbContext.SaveChanges();
                    Success suc = new Success("Uspešno ste obrisali profakturu.");
                    suc.Show();

                    SecurityManager.AuditManager.AuditToDB(MainWindowViewModel.Instance.UserOnSession.korisnickoime, $"Uspešno brisanje profakture {SelectedValue.oznaka}.", "Info");
                    Profakture.Clear();
                    foreach (var item in dbContext.Profakturas)
                    {
                        if (item.active)
                        {
                            profakture.Add(item);
                        }

                    }
                    DefaultView = CollectionViewSource.GetDefaultView(Profakture);
                }
                catch (Exception e)
                {
                }

            }
            else
            {
                Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                er.Show();
                SecurityManager.AuditManager.AuditToDB(MainWindowViewModel.Instance.UserOnSession.korisnickoime, "Neuspesan pokusaj brisanja profakture", "Upozorenje");
            }
        }

        private void Izmeni(string obj)
        {
            if (SelectedIndex < -1)
            {
                Error er = new Error("Morate selektovati profakturu.");
                er.Show();
                return;

            }
            if (SecurityManager.AuthorizationPolicy.HavePermission(MainWindowViewModel.Instance.UserOnSession.id, SecurityManager.Permission.EditProfaktura))
            {
                MainWindowViewModel.Instance.DodajProfakturuViewModel = new DodajProfakturuViewModel(1, SelectedValue);
                MainWindowViewModel.Instance.OnNav(Navigation.izmeniProfakturu);
            }
            else
            {
                Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                er.Show();
                SecurityManager.AuditManager.AuditToDB(MainWindowViewModel.Instance.UserOnSession.korisnickoime, "Neuspesan pokusaj izmene profakture", "Upozorenje");
            }
        }

        private void Dodaj(string obj)
        {
            //Profaktura p = new Profaktura();
            //p.active;
            //p.oznaka;
            //p.poslovnipartner_mbr;
            //p.zaposleni_id;
            //f.datum;
            //f.pdv;
            if (SecurityManager.AuthorizationPolicy.HavePermission(MainWindowViewModel.Instance.UserOnSession.id, SecurityManager.Permission.AddProfaktura))
            {
                MainWindowViewModel.Instance.DodajProfakturuViewModel = new DodajProfakturuViewModel(0, null);
                MainWindowViewModel.Instance.OnNav(Navigation.dodajProfakturu);
            }
            else
            {
                Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                er.Show();
                SecurityManager.AuditManager.AuditToDB(MainWindowViewModel.Instance.UserOnSession.korisnickoime, "Neuspesan pokusaj kreiranja profakture", "Upozorenje");
            }
            
        }

        private void Pretrazi(string type)
        {
            if (!type.Equals("/"))
            {//to do
                if (TextSearch != null && !String.IsNullOrWhiteSpace(TextSearch) && (TextSearch != ""))
                {
                    DefaultView = CollectionViewSource.GetDefaultView(DefaultView);
                    if (type.Equals("Oznaci"))
                    {
                        DefaultView.Filter =
                        w => ((Profaktura)w).oznaka.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Poslovnom partneru"))
                    {
                        DefaultView.Filter =
                        w => ((Profaktura)w).PoslovniPartner.naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Datumu"))
                    {
                        DefaultView.Filter =
                        w => ((Profaktura)w).datum.ToString().ToUpper().Contains(TextSearch.ToUpper());
                    }
                    

                    DefaultView.Refresh();
                }
                else
                {
                    DefaultView = CollectionViewSource.GetDefaultView(Profakture);
                    DefaultView.Filter = null;
                    DefaultView.Refresh();
                }
            }
            else
            {
                DefaultView = CollectionViewSource.GetDefaultView(Profakture);
                DefaultView.Filter = null;
                DefaultView.Refresh();
            }
        }
        #region Properties
        public ObservableCollection<Profaktura> Profakture { get => profakture; set => profakture = value; }
        public ICollectionView DefaultView { get => defaultView; set => defaultView = value; }
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

        public Profaktura SelectedValue
        {
            get { return selectedValue; }
            set
            {
                selectedValue = value;
                OnPropertyChanged("SelectedValue");
            }
        }
        #endregion
    }
}
