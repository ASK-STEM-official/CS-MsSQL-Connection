using System;
using System.Data.SqlClient;

class Program
{
    static string connectionString = "Server=localhost;Database=TestDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

    static void Main()
    {
        Console.WriteLine("== DB接続 & CRUD メニュー ==");

        while (true)
        {
            Console.WriteLine("\n=== メニュー ===");
            Console.WriteLine("1. ユーザー追加");
            Console.WriteLine("2. ユーザー一覧表示");
            Console.WriteLine("3. ユーザーの年齢を更新");
            Console.WriteLine("4. ユーザーを削除");
            Console.WriteLine("5. 終了");
            Console.Write("選択肢を入力してください（1-5）: ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    AddUser();
                    break;
                case "2":
                    ReadUsers();
                    break;
                case "3":
                    UpdateUser();
                    break;
                case "4":
                    DeleteUser();
                    break;
                case "5":
                    Console.WriteLine("終了します。");
                    return;
                default:
                    Console.WriteLine("無効な選択です。1〜5を入力してください。");
                    break;
            }
        }
    }

    static void AddUser()
    {
        Console.Write("名前を入力してください: ");
        string name = Console.ReadLine();
        Console.Write("年齢を入力してください: ");
        if (!int.TryParse(Console.ReadLine(), out int age))
        {
            Console.WriteLine("年齢は数値で入力してください。");
            return;
        }

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
            Console.WriteLine($"[Create] {name}（{age}）を追加しました。");
        }
    }

    static void ReadUsers()
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string sql = "SELECT Id, Name, Age FROM Users";
            using (var cmd = new SqlCommand(sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n--- ユーザー一覧 ---");
                while (reader.Read())
                {
                    Console.WriteLine($"Id={reader["Id"]}, Name={reader["Name"]}, Age={reader["Age"]}");
                }
            }
        }
    }

    static void UpdateUser()
    {
        Console.Write("更新するユーザー名を入力してください: ");
        string name = Console.ReadLine();
        Console.Write("新しい年齢を入力してください: ");
        if (!int.TryParse(Console.ReadLine(), out int newAge))
        {
            Console.WriteLine("年齢は数値で入力してください。");
            return;
        }

        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string sql = "UPDATE Users SET Age = @Age WHERE Name = @Name";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Age", newAge);
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                    Console.WriteLine($"[Update] {name} の年齢を {newAge} に更新しました。");
                else
                    Console.WriteLine("指定されたユーザーが見つかりませんでした。");
            }
        }
    }

    static void DeleteUser()
    {
        Console.Write("削除するユーザー名を入力してください: ");
        string name = Console.ReadLine();

        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string sql = "DELETE FROM Users WHERE Name = @Name";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                    Console.WriteLine($"[Delete] {name} を削除しました。");
                else
                    Console.WriteLine("指定されたユーザーが見つかりませんでした。");
            }
        }
    }
}
