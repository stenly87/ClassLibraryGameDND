using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryGameDND
{
    public class MySqlDB
    {
        public static MySqlConnection Create()
        {
            return Instance.GetConnecttion();
        }

        MySqlConnection mySqlConnection;
        private static MySqlDB instance;
        public static MySqlDB Instance => instance ??= new MySqlDB();

        private MySqlDB()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.UserID = "student";
            builder.Password = "student";
            builder.Database = "UltraMegaSuperDuperDND";
            builder.Server = "192.168.200.13";
            builder.CharacterSet = "utf8mb4";
            mySqlConnection = new MySqlConnection(builder.ToString());
            OpenConnection();
        }

        private bool OpenConnection()
        {
            try
            {
                mySqlConnection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
        }
        public void CloseConnection()
        {
            try
            {
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
        public MySqlConnection GetConnecttion()
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open)
                if (!OpenConnection())
                    return null;
            return mySqlConnection;
        }

    }
}
