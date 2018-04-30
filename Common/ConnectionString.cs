using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ConnectionString
    {
        public static string secureConnectionString()
        {
            return @"Data source = www.deltaexim.net,1433; initial catalog = DeltaExim; user id = vasa;Password=12345; MultipleActiveResultSets=True;App=EntityFramework";
            //return "Data Source=DESKTOP-JKCQPAE\\SQLEXPRESS;Initial Catalog=DeltaExim;Integrated Security=True;";
        }
    }
}
