using System;
using MySql.Data.MySqlClient;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        string connectionString = "server=127.0.0.1;port=3306;database=lab2;user=root;password=15011989;SslMode=none;";

        using var connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine(" Підключення до бази даних lab2 успішне!");

            while (true)
            {
                Console.WriteLine("\nОберіть таблицю для перегляду:");
                Console.WriteLine("1 - Матеріали");
                Console.WriteLine("2 - Постачальники");
                Console.WriteLine("3 - Замовлення");
                Console.WriteLine("0 - Вихід");
                Console.Write("Ваш вибір: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ShowMaterials(connection);
                        break;
                    case "2":
                        ShowPostachalnyky(connection);
                        break;
                    case "3":
                        ShowZamovlennya(connection);
                        break;
                    case "0":
                        Console.WriteLine(" Програму завершено.");
                        return;
                    default:
                        Console.WriteLine(" Невірний вибір. Спробуйте ще раз.");
                        break;
                }

                Console.WriteLine("\nНатисніть Enter, щоб повернутись до меню...");
                Console.ReadLine();
                Console.Clear();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(" Помилка підключення: " + ex.Message);
        }
    }

    static void ShowMaterials(MySqlConnection connection)
    {
        string query = "SELECT * FROM materials;";
        var command = new MySqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        Console.WriteLine("\nДані з таблиці 'Матеріали':");

        for (int i = 0; i < reader.FieldCount; i++)
        {
            string columnName = reader.GetName(i);
            if (columnName.Length > 17)
                columnName = columnName.Substring(0, 17) + "...";
            Console.Write($"{columnName.PadRight(20)}");
            if (i < reader.FieldCount - 1) Console.Write(" | ");
        }
        Console.WriteLine();

        Console.WriteLine(new string('-', reader.FieldCount * 22));

        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string cell = reader[i].ToString();
                if (cell.Length > 17)
                    cell = cell.Substring(0, 17) + "...";
                Console.Write($"{cell.PadRight(20)}");
                if (i < reader.FieldCount - 1) Console.Write(" | ");
            }
            Console.WriteLine();
        }

        reader.Close();
    }


    static void ShowPostachalnyky(MySqlConnection connection)
    {
        string query = "SELECT * FROM postachalnyky;";
        var command = new MySqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        Console.WriteLine("\nДані з таблиці 'Постачальники':");

        for (int i = 0; i < reader.FieldCount; i++)
        {
            string columnName = reader.GetName(i);
            Console.Write($"{columnName.PadRight(20)}");
            if (i < reader.FieldCount - 1) Console.Write(" | ");
        }
        Console.WriteLine();

        Console.WriteLine(new string('-', reader.FieldCount * 22));

        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string cell = reader[i].ToString();
                if (cell.Length > 20)
                    cell = cell.Substring(0, 17) + "...";
                Console.Write($"{cell.PadRight(20)}");
                if (i < reader.FieldCount - 1) Console.Write(" | ");
            }
            Console.WriteLine();
        }

        reader.Close();
    }

    static void ShowZamovlennya(MySqlConnection connection)
    {
        string query = "SELECT * FROM zamovlennya;";
        var command = new MySqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        Console.WriteLine("\nДані з таблиці 'Замовлення':");

        for (int i = 0; i < reader.FieldCount; i++)
        {
            string columnName = reader.GetName(i);
            if (columnName.Length > 17)
                columnName = columnName.Substring(0, 17) + "...";
            Console.Write($"{columnName.PadRight(20)}");
            if (i < reader.FieldCount - 1) Console.Write(" | ");
        }
        Console.WriteLine();

        Console.WriteLine(new string('-', reader.FieldCount * 22));

        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string cell = reader[i].ToString();
                if (cell.Length > 17)
                    cell = cell.Substring(0, 17) + "...";
                Console.Write($"{cell.PadRight(20)}");
                if (i < reader.FieldCount - 1) Console.Write(" | ");
            }
            Console.WriteLine();
        }

        reader.Close();
    }
}