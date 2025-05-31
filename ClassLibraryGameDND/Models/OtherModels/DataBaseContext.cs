using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibraryGameDND.Models.DbModels;
using MySqlConnector;

namespace ClassLibraryGameDND.Models.OtherModels
{
    public class DataBaseContext
    {
        private static MySqlConnection _con;
        public DataBaseContext(MySqlConnection connection)
            => _con = connection;

        public static List<Event> GetEvents()
        {
            return [];
        }
    }
}
