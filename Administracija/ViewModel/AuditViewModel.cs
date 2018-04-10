using Common;
using Common.Model;
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
    public class AuditViewModel : BindableBase
    {
        #region Members
        private ObservableCollection<Common.Model.Audit> logovi;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        
        private string textSearch;
        private ICollectionView defaultView;
        public MyICommand<string> findCommand { get; private set; }
        #endregion

        public AuditViewModel()
        {
            findCommand = new MyICommand<string>(OnFind);
            
            textSearch = "";
            logovi = new ObservableCollection<Audit>();
            foreach (var item in dbContext.Audits.ToList())
            {
                logovi.Add(item);
            }

            DefaultView = CollectionViewSource.GetDefaultView(Logovi);
        }

        #region CommandsImplementation
        private void OnFind(string type)
        {
            if (!type.Equals("/"))
            {
                if (TextSearch != null && !String.IsNullOrWhiteSpace(TextSearch) && (TextSearch != ""))
                {
                    DefaultView = CollectionViewSource.GetDefaultView(DefaultView);
                    if (type.Equals("Korisničkom imenu"))
                    {
                        DefaultView.Filter =
                        w => ((Audit)w).korisnickoime.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Akciji"))
                    {
                        DefaultView.Filter =
                        w => ((Audit)w).akcija.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else
                    {
                        DefaultView.Filter =
                        w => ((Audit)w).tip.ToUpper().Contains(TextSearch.ToUpper());
                    }

                    DefaultView.Refresh();
                }
                else
                {
                    DefaultView = CollectionViewSource.GetDefaultView(Logovi);
                    DefaultView.Filter = null;
                    DefaultView.Refresh();
                }
            }
            else
            {
                DefaultView = CollectionViewSource.GetDefaultView(Logovi);
                DefaultView.Filter = null;
                DefaultView.Refresh();
            }
            

        }
        #endregion

        #region Constructors
        public string TextSearch
        {
            get { return textSearch; }
            set
            {
                textSearch = value;
                OnPropertyChanged("TextSearch");
            }
        }

        public ICollectionView DefaultView { get => defaultView; set => defaultView = value; }

        public ObservableCollection<Common.Model.Audit> Logovi
        {
            get { return logovi; }
            set { logovi = value; }
        }
        #endregion
    }
}
