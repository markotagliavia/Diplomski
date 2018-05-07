using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Skladistenje.ViewModel
{
    public class SkladisteniDokumentiViewModel : BindableBase
    {
        #region Commands
        public MyICommand<string> DodajSklDokNavCommand { get; private set; }
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
        #endregion

        public SkladisteniDokumentiViewModel(int tip)
        {
            DodajSklDokNavCommand = new MyICommand<string>(DodajSklDokNav);
            DetaljnijeSklDokNavCommand = new MyICommand<string>(DetaljnijeSklDokNav);
            IzbrisiSklDokCommand = new MyICommand<string>(IzbrisiSklDok);
            PretraziSklDokCommand = new MyICommand<string>(PretraziSklDok);
            textSearch = "";
            sklDokumenti = new ObservableCollection<SkladisteniDokument>();
            foreach (var item in dbContext.SkladisteniDokuments.ToList())
            {
                sklDokumenti.Add(item);
            }

            DefaultView = CollectionViewSource.GetDefaultView(sklDokumenti);
        }

        #region Constructors

        public ObservableCollection<SkladisteniDokument> SklDokumenti
        {
            get { return sklDokumenti; }
            set { sklDokumenti = value; }
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
        private void DodajSklDokNav(string obj)
        {
            throw new NotImplementedException();
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
