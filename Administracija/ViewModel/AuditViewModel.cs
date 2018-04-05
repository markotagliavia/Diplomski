using Administracija.Model;
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
        private List<SecurityManager.Model.Audit> logovi;
        private SecurityManager.Model.DeltaEximEntities dbContext = new SecurityManager.Model.DeltaEximEntities();

        public AuditViewModel()
        {
            logovi = new List<SecurityManager.Model.Audit>();
            logovi = dbContext.Audits.ToList();
        }

        public List<SecurityManager.Model.Audit> Logovi
        {
            get { return logovi; }
            set { logovi = value; }
        }
    }
}
