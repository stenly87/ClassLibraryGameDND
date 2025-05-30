using MySqlConnector;

namespace ClassLibraryGameDND
{
    public class DNDWalkingPet
    {
        private readonly MySqlConnection connection;

        public DNDWalkingPet(MySqlConnection connection)
        {
            this.connection = connection;
        }

        public string Test()
        {
            return connection.ConnectionString;
        }
    }
}
