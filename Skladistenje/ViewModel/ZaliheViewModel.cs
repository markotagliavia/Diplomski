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
    public class ZaliheViewModel : BindableBase
    {
        //komande su : pretraga
        #region Commands
        public MyICommand<string> PretraziZaliheCommand { get; private set; }
        #endregion

        #region Properties
        private Korisnik userOnSession;
        private string textSearch;
        private ObservableCollection<Zalihe> jediniceZaliha;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ICollectionView defaultView;
        #endregion

        public ZaliheViewModel()
        {
            PretraziZaliheCommand = new MyICommand<string>(PretraziZalihe);
            textSearch = "";
            JediniceZaliha = new ObservableCollection<Zalihe>();
            DefaultView = CollectionViewSource.GetDefaultView(jediniceZaliha);
        }

        #region Constructors

        public ObservableCollection<Zalihe> JediniceZaliha
        {
            get { return jediniceZaliha; }
            set { jediniceZaliha = value; }
        }

        public ICollectionView DefaultView { get => defaultView; set => defaultView = value; }


        public string TextSearch
        {
            get { return textSearch; }
            set
            {
                textSearch = value;
                OnPropertyChanged("TextSearch");
            }
        }

        public Korisnik UserOnSession
        {
            get { return userOnSession; }
            set { userOnSession = value; }
        }
        #endregion

        #region CommandsImplementation
        private void PretraziZalihe(string type)
        {
            if (!type.Equals("/"))
            {
                if (TextSearch != null && !String.IsNullOrWhiteSpace(TextSearch) && (TextSearch != ""))
                {
                    DefaultView = CollectionViewSource.GetDefaultView(DefaultView);
                    if (type.Equals("Proizvodu"))
                    {
                        DefaultView.Filter =
                        w => ((Zalihe)w).Proizvod.naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Skladištu"))
                    {
                        DefaultView.Filter =
                        w => ((Zalihe)w).Skladiste.naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Rafu"))
                    {
                        DefaultView.Filter =
                        w => ((Zalihe)w).raf.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Količini"))
                    {
                        DefaultView.Filter =
                        w => ((Zalihe)w).kolicina.ToString().ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Rezervisanom"))
                    {
                        DefaultView.Filter =
                        w => ((Zalihe)w).rezervisano.ToString().ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Minimumu količine"))
                    {
                        DefaultView.Filter =
                        w => ((Zalihe)w).minimumkolicine.ToString().ToUpper().Contains(TextSearch.ToUpper());
                    }


                    DefaultView.Refresh();
                }
                else
                {
                    DefaultView = CollectionViewSource.GetDefaultView(JediniceZaliha);
                    DefaultView.Filter = null;
                    DefaultView.Refresh();
                }
            }
            else
            {
                DefaultView = CollectionViewSource.GetDefaultView(JediniceZaliha);
                DefaultView.Filter = null;
                DefaultView.Refresh();
            }
        }
        #endregion
    }
}
