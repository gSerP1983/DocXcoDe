using System.Data;
using System.Data.SqlClient;

namespace DocXcoDe
{
    public static class Dao
    {
        public static void ExecuteQuery(string connectionString, string query, DataTable dt)
        {
            using (var da = new SqlDataAdapter(query, connectionString))
                da.Fill(dt);
        }
    }
}