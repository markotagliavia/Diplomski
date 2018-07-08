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

namespace Skladistenje.ViewModel
{
    public class SkladisteniDokumentiViewModel : BindableBase
    {
        #region Commands
        public MyICommand<string> DodajSklDok1NavCommand { get; private set; }
        public MyICommand<string> DodajSklDok2NavCommand { get; private set; }
        public MyICommand<string> DetaljnijeSklDokNavCommand { get; private set; }
        public MyICommand<string> IzbrisiSklDokCommand { get; private set; }
        public MyICommand<string> PretraziSklDokCommand { get; private set; }
        #endregion

        #region Properties
        private Korisnik userOnSession;
        private int _selectedIndex = -1;
        private bool selectedInGrid;
        private string textSearch;
        private ObservableCollection<SkladisteniDokument> sklDokumenti;
        private SkladisteniDokument selectedValue;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ICollectionView defaultView;
        private string novi1 = "", novi2 = "";
        private Visibility novi2Visible, novi1Visible;
        private int tip = -1;
        #endregion

        public SkladisteniDokumentiViewModel(int tip)
        {
            this.tip = tip;
            DodajSklDok1NavCommand = new MyICommand<string>(DodajSklDok1Nav);
            DodajSklDok2NavCommand = new MyICommand<string>(DodajSklDok2Nav);
            DetaljnijeSklDokNavCommand = new MyICommand<string>(DetaljnijeSklDokNav);
            IzbrisiSklDokCommand = new MyICommand<string>(IzbrisiSklDok);
            PretraziSklDokCommand = new MyICommand<string>(PretraziSklDok);
            textSearch = "";
            sklDokumenti = new ObservableCollection<SkladisteniDokument>();
            foreach (var item in dbContext.SkladisteniDokuments.ToList())
            {
                if (tip == 1)   //interni
                {
                    if (item.tipredovnog == "INT_PR" || item.tipredovnog == "INT_OTP") sklDokumenti.Add(item);
                }
                else if (tip == 2)  //spoljni
                {
                    if (item.tipredovnog == "SP_PR" || item.tipredovnog == "SP_OTP") sklDokumenti.Add(item);
                }
                else if (tip == 3) //korekcioni
                {
                    if (item.tipredovnog == "KOR_PR" || item.tipredovnog == "KOR_OTP") sklDokumenti.Add(item);
                }
                else if (tip == 4) //storni
                {
                    if (item.tipredovnog == "STORNI") sklDokumenti.Add(item);
                }
            }

            Novi2Visible = Visibility.Visible;
            Novi1Visible = Visibility.Visible;
            if (tip == 1)   //interni
            {
                Novi1 = "Nova pirjemnica";
                Novi2 = "Nova otpremnica";
                Novi1Visible = Visibility.Collapsed;
            }
            else if (tip == 2) //spoljni
            {
                Novi1 = "Nova pirjemnica";
                Novi2 = "Nova otpremnica";
                Novi2Visible = Visibility.Collapsed;
                Novi1Visible = Visibility.Collapsed;
            }
            else if (tip == 3)  //korekcioni
            {
                Novi1 = "Novi pripis";
                Novi2 = "Novi otpis";
                Novi2Visible = Visibility.Collapsed;
                Novi1Visible = Visibility.Collapsed;
            }
            else if (tip == 4) //storni
            {
                Novi1 = "Novi storni";
                Novi2Visible = Visibility.Collapsed;
            }

            DefaultView = CollectionViewSource.GetDefaultView(sklDokumenti);
        }

        #region Constructors

        public Visibility Novi2Visible
        {
            get { return novi2Visible; }
            set
            {
                novi2Visible = value;
                OnPropertyChanged("Novi2Visible");
            }
        }

        public Visibility Novi1Visible
        {
            get { return novi1Visible; }
            set
            {
                novi1Visible = value;
                OnPropertyChanged("Novi1Visible");
            }
        }

        public ObservableCollection<SkladisteniDokument> SklDokumenti
        {
            get { return sklDokumenti; }
            set
            {
                sklDokumenti = value;
                OnPropertyChanged("SelectedInGrid");
            }
        }

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

        public string Novi1
        {
            get { return novi1; }
            set
            {
                novi1 = value;
                OnPropertyChanged("Novi1");
            }
        }

        public string Novi2
        {
            get { return novi2; }
            set
            {
                novi2 = value;
                OnPropertyChanged("Novi2");
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

        public SkladisteniDokument SelectedValue
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

        #region COmmandsImplementation
        private void DodajSklDok1Nav(string obj)
        {
            if (tip == 1)   //interni
            {
                PromeniUserControlu("INT_PR");
            }
            else if (tip == 2)  //spoljni
            {
                PromeniUserControlu("SP_PR");
            }
            else if (tip == 3) //korekcioni
            {
                PromeniUserControlu("KOR_PR");
            }
            else if (tip == 4) //storni
            {
                PromeniUserControlu("STORNI");
            }
        }

        private void DodajSklDok2Nav(string obj)
        {
            if (tip == 1)   //interni
            {
                PromeniUserControlu("INT_OTP");
            }
            else if (tip == 2)  //spoljni
            {
                PromeniUserControlu("SP_OTP");
            }
            else if (tip == 3) //korekcioni
            {
                PromeniUserControlu("KOR_OTP");
            }
        }

        private void PromeniUserControlu(string v)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType().Equals(typeof(MainWindow)))
                {
                    UserOnSession = ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession;
                    if (SecurityManager.AuthorizationPolicy.HavePermission(userOnSession.id, SecurityManager.Permission.AddSklDok))
                    {
                        ((MainWindowViewModel)((MainWindow)w).DataContext).UserOnSession = this.UserOnSession;
                        ((MainWindowViewModel)((MainWindow)w).DataContext).OnNav("generic" + v);
                    }
                    else
                    {
                        Error er = new Error("Nemate ovlašćenja za izvršenje ove akcije!");
                        er.Show();
                        SecurityManager.AuditManager.AuditToDB(UserOnSession.korisnickoime, "Neuspšan pokušaj izmene skladišta.", "Upozorenje");
                    }

                }
            }
        }

        private void PretraziSklDok(string type)
        {
            if (!type.Equals("/"))
            {
                if (TextSearch != null && !String.IsNullOrWhiteSpace(TextSearch) && (TextSearch != ""))
                {
                    DefaultView = CollectionViewSource.GetDefaultView(DefaultView);
                    if (type.Equals("Šifri"))
                    {
                        DefaultView.Filter =
                        w => ((SkladisteniDokument)w).sifra.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Tipu"))
                    {
                        DefaultView.Filter =
                        w => ((SkladisteniDokument)w).tipredovnog.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Skladištu"))
                    {
                        DefaultView.Filter =
                        w => ((SkladisteniDokument)w).Skladiste.naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Datumu"))
                    {
                        DefaultView.Filter =
                        w => ((SkladisteniDokument)w).datum.ToString().Contains(TextSearch.ToUpper());
                    }

                    DefaultView.Refresh();
                }
                else
                {
                    DefaultView = CollectionViewSource.GetDefaultView(SklDokumenti);
                    DefaultView.Filter = null;
                    DefaultView.Refresh();
                }
            }
            else
            {
                DefaultView = CollectionViewSource.GetDefaultView(SklDokumenti);
                DefaultView.Filter = null;
                DefaultView.Refresh();
            }
        }

        private void IzbrisiSklDok(string obj)
        {
            throw new NotImplementedException();
        }

        private void DetaljnijeSklDokNav(string obj)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
