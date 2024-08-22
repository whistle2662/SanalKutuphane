using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebApplication2.Services
{
    public class UserStore
    {
        private readonly string _connectionString;

        public UserStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CreateUserAsync(string userName, string passwordHash)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("INSERT INTO Users (UserName, PasswordHash) VALUES (@UserName, @PasswordHash)", connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
