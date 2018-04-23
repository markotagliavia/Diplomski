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
    public class ZaliheViewModel:BindableBase
    {
        #region Commands
        public MyICommand<string> PretraziCommand { get; private set; }
        
        #endregion

        #region Properties
        private Korisnik userOnSession;
        private string textSearch;
        private ObservableCollection<Zalihe> zalihe;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();
        private ICollectionView defaultView;
       
        #endregion

        public ZaliheViewModel()
        {
            PretraziCommand = new MyICommand<string>(Pretrazi);
            textSearch = "";
            Zalihe = new ObservableCollection<Zalihe>();
            foreach (var item in dbContext.Zalihes)
            {
                zalihe.Add(item);
            }
            DefaultView = CollectionViewSource.GetDefaultView(Zalihe);
        }


        #region Commands Implements

        private void Pretrazi(string type)
        {
            if (!type.Equals("/"))
            {
                if (TextSearch != null && !String.IsNullOrWhiteSpace(TextSearch) && (TextSearch != ""))
                { 
                    DefaultView = CollectionViewSource.GetDefaultView(DefaultView);
                    if (type.Equals("Šifri"))
                    {
                        DefaultView.Filter =
                        w => ((Zalihe)w).Proizvod.sifra.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Nazivu"))
                    {
                        DefaultView.Filter =
                        w => ((Zalihe)w).Proizvod.naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Skladištu"))
                    {
                        DefaultView.Filter =
                        w => ((Zalihe)w).Skladiste.naziv.ToUpper().Contains(TextSearch.ToUpper());
                    }
                    else if (type.Equals("Količini"))
                    {
                        try
                        {
                            Double kolicina = Double.Parse(TextSearch);
                            DefaultView.Filter =
                                w => ((Zalihe)w).kolicina == kolicina;
                        }
                        catch (Exception ex)
                        {
                            Error e = new Error("Morate uneti cifru.");
                            e.Show();
                        }

                    }


                    DefaultView.Refresh();
                }
                else
                {
                    DefaultView = CollectionViewSource.GetDefaultView(Zalihe);
                    DefaultView.Filter = null;
                    DefaultView.Refresh();
                }
            }
            else
            {
                DefaultView = CollectionViewSource.GetDefaultView(Zalihe);
                DefaultView.Filter = null;
                DefaultView.Refresh();
            }
        }

        #endregion
        #region Properties

        public string TextSearch
        {
            get { return textSearch; }
            set
            {
                textSearch = value;
                OnPropertyChanged("TextSearch");
            }
        }

        public ObservableCollection<Zalihe> Zalihe
        {
            get => zalihe;
            set
            {
                zalihe = value;
                OnPropertyChanged("Zalihe");
            }
        }

        public ICollectionView DefaultView { get => defaultView; set => defaultView = value; }
        #endregion
    }
}
