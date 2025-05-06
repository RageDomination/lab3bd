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
            Console.WriteLine("Підключення до бази даних lab2 успішне!");

            string queryMaterials = "SELECT * FROM materials;";
            var commandMaterials = new MySqlCommand(queryMaterials, connection);
            using var readerMaterials = commandMaterials.ExecuteReader();
            Console.WriteLine("\nДані з таблиці 'Матерiали':");
            while (readerMaterials.Read())
            {
                Console.WriteLine($"{readerMaterials[0]} | {readerMaterials[1]} | {readerMaterials[2]} | {readerMaterials[3]} | {readerMaterials[4]} | {readerMaterials[5]} | {readerMaterials[6]}");
            }
            readerMaterials.Close();

            string queryPostachalnyky = "SELECT * FROM postachalnyky;";
            var commandPostachalnyky = new MySqlCommand(queryPostachalnyky, connection);
            using var readerPostachalnyky = commandPostachalnyky.ExecuteReader();
            Console.WriteLine("\nДані з таблиці 'Постачальники':");
            while (readerPostachalnyky.Read())
            {
                Console.WriteLine($"{readerPostachalnyky[0]} | {readerPostachalnyky[1]} | {readerPostachalnyky[2]} | {readerPostachalnyky[3]} | {readerPostachalnyky[4]}");
            }
            readerPostachalnyky.Close();

            string queryZamovlennya = "SELECT * FROM zamovlennya;";
            var commandZamovlennya = new MySqlCommand(queryZamovlennya, connection);
            using var readerZamovlennya = commandZamovlennya.ExecuteReader();
            Console.WriteLine("\nДані з таблиці 'Замовлення':");
            while (readerZamovlennya.Read())
            {
                Console.WriteLine($"{readerZamovlennya[0]} | {readerZamovlennya[1]} | {readerZamovlennya[2]} | {readerZamovlennya[3]} | {readerZamovlennya[4]} | {readerZamovlennya[5]} | {readerZamovlennya[6]}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка підключення: " + ex.Message);
        }
        Console.WriteLine("Натисніть Enter для виходу...");
        Console.ReadLine();
    }
}
