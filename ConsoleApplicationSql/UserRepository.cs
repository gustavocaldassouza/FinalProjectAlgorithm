using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplicationSql
{
    public class UserRepository
    {
        private string _dbPath;
        public UserRepository(string dbPath)
        {
            _dbPath = dbPath;
        }
        public User AddUser(User user)
        {
            SQLiteConnection connection;
            connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("INSERT INTO User (MembershipId, Name) VALUES (@MembershipId, @Name)", connection);
            command.Parameters.AddWithValue("@MembershipId", user.MembershipId);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.ExecuteNonQuery();
            connection.Close();
            return user;
        }
        public User SearchUser(int membershipId)
        {
            SQLiteConnection connection;
            connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM User WHERE MembershipId = @membershipId", connection);
            command.Parameters.AddWithValue("@membershipId", membershipId);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                User user = new User();
                user.MembershipId = reader.GetInt32(0);
                user.Name = reader.GetString(1);
                return user;
            }
            return new User();
        }
        public User[] SearchUsers()
        {
            SQLiteConnection connection;
            connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM User", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            User[] users = new User[1];
            while (reader.Read())
            {
                User user = new User();
                user.MembershipId = reader.GetInt32(0);
                user.Name = reader.GetString(1);
                for (int i = 0; i < users.Length; i++)
                {
                    if (users[i] == null)
                    {
                        users[i] = user;
                        break;
                    }
                }

                if (users[users.Length - 1].MembershipId == user.MembershipId) continue;
                Array.Resize(ref users, users.Length + 1);
                users[users.Length - 1] = user;
            }
            return users.ToArray();
        }
    }
}