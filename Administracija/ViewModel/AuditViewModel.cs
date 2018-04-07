using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administracija.ViewModel
{
    public class AuditViewModel : BindableBase
    {
        private List<Common.Model.Audit> logovi;
        private Common.Model.DeltaEximEntities dbContext = new Common.Model.DeltaEximEntities();

        public AuditViewModel()
        {
            logovi = new List<Common.Model.Audit>();
            logovi = dbContext.Audits.ToList();
        }

        public List<Common.Model.Audit> Logovi
        {
            get { return logovi; }
            set { logovi = value; }
        }
    }
}
