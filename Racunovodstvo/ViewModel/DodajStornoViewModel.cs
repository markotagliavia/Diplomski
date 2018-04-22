using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Racunovodstvo.ViewModel
{
    public class DodajStornoViewModel:BindableBase
    {
        private int context;
        private Faktura fakturaForBind;
        private ObservableCollection<Faktura> redovneFakture;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();

        public DodajStornoViewModel(int i,Faktura f)
        {
            RedovneFakture = new ObservableCollection<Faktura>();
            if (context == 0)
            {
                FakturaForBind = new Faktura();
                FakturaForBind.redovna = false;
                FakturaForBind.datumfakturisanja = DateTime.Now;
                FakturaForBind.active = true;
                FakturaForBind.stornoceo = false;

                foreach (var item in dbContext.Fakturas)
                {
                    if (item.redovna == true && item.active == true)
                    {
                        RedovneFakture.Add(item);
                    }
                }
                
            }
            else if (context == 1)
            {
                FakturaForBind = f;
            }
            
        }

        #region Properties
        public Faktura FakturaForBind
        {
            get => fakturaForBind;
            set
            {
                fakturaForBind = value;
                OnPropertyChanged("FakturaForBind");
            }
        }

        public ObservableCollection<Faktura> RedovneFakture
        {
            get => redovneFakture;
            set
            {
                redovneFakture = value;
                OnPropertyChanged("RedovneFakture");
            }
        }
        #endregion
    }
}
