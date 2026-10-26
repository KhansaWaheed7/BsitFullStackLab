using Npgsql;

namespace DataAccessLayer
{
    public class DBHelper
    {
        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(
                "Host=aws-0-ap-southeast-2.pooler.supabase.com;" +
                "Port=6543;Database=postgres;" +
                "Username=postgres.hdnjbnyzazjrsqbjsqpl;Password=$77hotMango;" +
                "SSL Mode=Require;Trust Server Certificate=true;" +
                "No Reset On Close=true;Timeout=30;Command Timeout=30");
        }
    }
}