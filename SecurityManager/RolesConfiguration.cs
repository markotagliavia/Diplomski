using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public enum Permissions
    {
        //TO DO
        AdminLogin = 0,
        RacunovodstvoLogin = 1,
        SkladistenjeLogin = 2

    }
    class RolesConfiguration
    {
        private static  Dictionary<string, string[]> pravaPristupa = new Dictionary<string, string[]>();
        static RolesConfiguration()
        {

        }

        public Dictionary<string, string[]> PravaPristupa
        {
            get { return pravaPristupa;  }
            set { pravaPristupa = value; }
        }

        public string[] GetPermissions(string role)
        {
            if (PravaPristupa.ContainsKey(role))
            {
                return PravaPristupa[role];
            }
            else
            {
                return null;
            }
        }
    }
}
