using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.PERSISTANCE
{
    public class Helper
    {
        public static string ConnexionString
        {
            get { return ConfigurationManager.ConnectionStrings["cn"].ConnectionString; }
        }
    }
}
