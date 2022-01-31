using StudentDiary.Properties;
using System.Data.SqlClient;

namespace StudentDiary
{
    public class ConnectionStringBuild
    {
       
        private static string _serverAdress = Settings.Default.ServerAdress;
        private static string _serverName = Settings.Default.ServerName;
        private static string _dbName = Settings.Default.DbName;
        private static string _userName = Settings.Default.UserName;
        private static string _password = Settings.Default.Password;
        

        public static string ServerAdress
        {
            get { return _serverAdress; }
            set { Settings.Default.ServerAdress = value; }
        }

        public static string ServerName
        {
            get { return _serverName; }
            set { Settings.Default.ServerName = value; }
        }

        public static string DbName
        {
            get { return _dbName; }
            set { Settings.Default.DbName = value; }
        }

        public static string UserName
        {
            get { return _userName; }
            set { Settings.Default.UserName = value; }
        }

        private static string Password
        {
            get { return _password; }
            set { Settings.Default.Password = value; }
        }       

        public static SqlConnectionStringBuilder sqlconnectionstringbuilder()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = ServerAdress+@"\"+ServerName;
            builder.InitialCatalog = DbName;
            builder.UserID = UserName;
            builder.Password = Password;
            builder.ConnectTimeout = 5;
            return builder;
        }    
        public static void SetPassword(string password)
        {
            if (string.IsNullOrEmpty(password))

                Settings.Default.Password = "";
            else
            Settings.Default.Password = password.ToString();
        }
    }
}
