using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public class AuditManager
    {
        private static DeltaEximEntities dbContext = new DeltaEximEntities();

        static AuditManager()
        {
            
        }

        public static void AuditToDB(string user, string akcija, string tip)
        {
            DateTime dt = DateTime.Now;
            dbContext.Audits.Add(new Common.Model.Audit { akcija = akcija, korisnickoime = user, tip = tip, vreme = dt});
            dbContext.SaveChanges();
        }
    }
}
