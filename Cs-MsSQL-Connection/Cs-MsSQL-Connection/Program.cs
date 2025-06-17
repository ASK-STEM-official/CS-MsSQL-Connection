using System;
using System.Data.SqlClient;

class Program
{
    static string connectionString = "Server=localhost;Database=TestDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

    static void Main()
    {
        Console.WriteLine("== DB接続 & CRUDテスト ==");

        CreateUser("Alice", 25);
        CreateUser("Bob", 30);

        Console.WriteLine("\n全ユーザー:");
        ReadUsers();

        Console.WriteLine("\nBobの年齢を35に更新:");
        UpdateUserAge("Bob", 35);
        ReadUsers();

        Console.WriteLine("\nAliceを削除:");
        DeleteUser("Alice");
        ReadUsers();
    }

    static void CreateUser(string name, int age)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string sql = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age)";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine($"[Create] {name} ({age}) を追加しました");
        }
    }

    static void ReadUsers()
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string sql = "SELECT Id, Name, Age FROM Users";
            using (var cmd = new SqlCommand(sql, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"[Read] Id={reader["Id"]}, Name={reader["Name"]}, Age={reader["Age"]}");
                    }
                }
            }
        }
    }

    static void UpdateUserAge(string name, int newAge)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string sql = "UPDATE Users SET Age = @Age WHERE Name = @Name";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Age", newAge);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine($"[Update] {name} の年齢を {newAge} に更新しました");
        }
    }

    static void DeleteUser(string name)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string sql = "DELETE FROM Users WHERE Name = @Name";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine($"[Delete] {name} を削除しました");
        }
    }
}
