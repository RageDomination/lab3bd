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
        while (true)
        {
            Console.Clear();
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

            Console.WriteLine("\nОберіть дію:");
            Console.WriteLine("1 - Додати новий рядок");
            Console.WriteLine("2 - Переглянути повну інформацію про рядок");
            Console.WriteLine("3 - Видалити рядок");
            Console.WriteLine("0 - Назад");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddMaterial(connection);
                    break;
                case "2":
                    ViewMaterialById(connection);
                    break;
                case "3":
                    DeleteMaterialById(connection);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }

            Console.WriteLine("Натисніть Enter для продовження...");
            Console.ReadLine();
        }
    }

    static void AddMaterial(MySqlConnection connection)
    {
        string nazva_materialu = PromptRequired("Назва матеріалу");
        string vyrobnyk = PromptRequired("Виробник");
        string vartist_odynytsi = PromptRequired("Вартість одиниці");
        string min_partiya = PromptRequired("Мінімальна партія");
        string termin_zberihannya = PromptRequired("Термін зберігання");
        string kod_postachalnyka = PromptRequired("Код постачальника");

        try
        {
            string query = @"
        INSERT INTO materials(nazva_materialu, vyrobnyk, vartist_odynytsi, min_partiya, termin_zberihannya, kod_postachalnyka)
        VALUES (@nazva, @vyrobnyk, @vartist, @partiya, @termin, @kodPost)";

            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@nazva", nazva_materialu);
            cmd.Parameters.AddWithValue("@vyrobnyk", vyrobnyk);
            cmd.Parameters.AddWithValue("@vartist", vartist_odynytsi);
            cmd.Parameters.AddWithValue("@partiya", min_partiya);
            cmd.Parameters.AddWithValue("@termin", termin_zberihannya);
            cmd.Parameters.AddWithValue("@kodPost", kod_postachalnyka);

            cmd.ExecuteNonQuery();
            Console.WriteLine(" Рядок успішно додано.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(" Помилка додавання: " + ex.Message);
        }
    }

    static string PromptRequired(string prompt)
    {
        string value;
        do
        {
            Console.Write($"{prompt}: ");
            value = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                Console.WriteLine(" Поле не може бути порожнім. Спробуйте ще раз.");
            }
        } while (string.IsNullOrWhiteSpace(value));
        return value;
    }

    static void ViewMaterialById(MySqlConnection connection)
    {
        Console.Write("Введіть kod_materialy: ");
        string id = Console.ReadLine();

        string query = "SELECT * FROM materials WHERE kod_materialy = @id";
        var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.WriteLine($"{reader.GetName(i)}: {reader[i]}");
            }
        }
        else
        {
            Console.WriteLine("Рядок не знайдено.");
        }
    }

    static void DeleteMaterialById(MySqlConnection connection)
    {
        Console.Write("Введіть kod_materialy для видалення: ");
        string id = Console.ReadLine();

        string query = "DELETE FROM materials WHERE kod_materialy = @id";
        var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        int rowsAffected = cmd.ExecuteNonQuery();

        if (rowsAffected > 0)
            Console.WriteLine("Рядок видалено.");
        else
            Console.WriteLine("Рядок не знайдено.");
    }

    static void ShowPostachalnyky(MySqlConnection connection)
    {
        while (true)
        {
            Console.Clear();
            string query = "SELECT * FROM postachalnyky;";
            var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            Console.WriteLine("\nДані з таблиці 'Постачальники':");

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

            Console.WriteLine("\nОберіть дію:");
            Console.WriteLine("1 - Додати нового постачальника");
            Console.WriteLine("2 - Переглянути повну інформацію про постачальника");
            Console.WriteLine("3 - Видалити постачальника");
            Console.WriteLine("0 - Назад");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddPostachalnyk(connection);
                    break;
                case "2":
                    ViewPostachalnykById(connection);
                    break;
                case "3":
                    DeletePostachalnykById(connection);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }

            Console.WriteLine("Натисніть Enter для продовження...");
            Console.ReadLine();
        }
    }

    static void AddPostachalnyk(MySqlConnection connection)
    {
        string nazva_postachalnyka = PromptRequired("Назва постачальника");
        string adresa = PromptRequired("Адреса");
        string tel_number = PromptRequired("Телефонний номер");
        string surname = PromptRequired("Прізвище");

        try
        {
            string query = @"
        INSERT INTO postachalnyky(nazva_postachalnyka, adresa, tel_number, surname)
        VALUES (@nazva, @adresa, @tel, @surname)";

            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@nazva", nazva_postachalnyka);
            cmd.Parameters.AddWithValue("@adresa", adresa);
            cmd.Parameters.AddWithValue("@tel", tel_number);
            cmd.Parameters.AddWithValue("@surname", surname);

            cmd.ExecuteNonQuery();
            Console.WriteLine("Постачальник успішно доданий.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка додавання: " + ex.Message);
        }
    }

    static void ViewPostachalnykById(MySqlConnection connection)
    {
        Console.Write("Введіть kod_postachalnyka: ");
        string id = Console.ReadLine();

        string query = "SELECT * FROM postachalnyky WHERE kod_postachalnyka = @id";
        var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.WriteLine($"{reader.GetName(i)}: {reader[i]}");
            }
        }
        else
        {
            Console.WriteLine("Постачальника не знайдено.");
        }
    }

    static void DeletePostachalnykById(MySqlConnection connection)
    {
        Console.Write("Введіть kod_postachalnyka для видалення: ");
        string id = Console.ReadLine();

        string query = "DELETE FROM postachalnyky WHERE kod_postachalnyka = @id";
        var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        int rowsAffected = cmd.ExecuteNonQuery();

        if (rowsAffected > 0)
            Console.WriteLine("Постачальника видалено.");
        else
            Console.WriteLine("Постачальника не знайдено.");

        static string PromptRequired(string prompt)
        {
            string value;
            do
            {
                Console.Write($"{prompt}: ");
                value = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine("Поле не може бути порожнім. Спробуйте ще раз.");
                }
            } while (string.IsNullOrWhiteSpace(value));
            return value;
        }
    }

    static void ShowZamovlennya(MySqlConnection connection)
    {
        while (true)
        {
            Console.Clear();
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

            Console.WriteLine("\nОберіть дію:");
            Console.WriteLine("1 - Додати нове замовлення");
            Console.WriteLine("2 - Переглянути повну інформацію про замовлення");
            Console.WriteLine("3 - Видалити замовлення");
            Console.WriteLine("0 - Назад");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddZamovlennya(connection);
                    break;
                case "2":
                    ViewZamovlennyaById(connection);
                    break;
                case "3":
                    DeleteZamovlennyaById(connection);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }

            Console.WriteLine("Натисніть Enter для продовження...");
            Console.ReadLine();
        }
    }

    static void AddZamovlennya(MySqlConnection connection)
    {
        string data_zapovnennya = PromptRequired("Дата заповнення (формат: YYYY-MM-DD)");
        string nazva_materialu = PromptRequired("Назва матеріалу");
        string kod_materialu = PromptRequired("Код матеріалу");
        string kilkist = PromptRequired("Кількість");
        string odynytsya_vymiru = PromptRequired("Одиниця виміру");
        string data_postachannya = PromptRequired("Дата постачання (формат: YYYY-MM-DD)");

        try
        {
            data_zapovnennya = ConvertToDateFormat(data_zapovnennya);
            data_postachannya = ConvertToDateFormat(data_postachannya);

            if (!IsMaterialExist(connection, kod_materialu))
            {
                Console.WriteLine("Помилка: матеріал з таким кодом не існує в таблиці 'materials'.");
                return;
            }

            string query = @"
        INSERT INTO zamovlennya(data_zapovnennya, nazva_materialu, kod_materialu, kilkist, odynytsya_vymiru, data_postachannya)
        VALUES (@data_zapovnennya, @nazva_materialu, @kod_materialu, @kilkist, @odynytsya_vymiru, @data_postachannya)";

            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@data_zapovnennya", data_zapovnennya);
            cmd.Parameters.AddWithValue("@nazva_materialu", nazva_materialu);
            cmd.Parameters.AddWithValue("@kod_materialu", kod_materialu);
            cmd.Parameters.AddWithValue("@kilkist", kilkist);
            cmd.Parameters.AddWithValue("@odynytsya_vymiru", odynytsya_vymiru);
            cmd.Parameters.AddWithValue("@data_postachannya", data_postachannya);

            cmd.ExecuteNonQuery();
            Console.WriteLine("Рядок успішно додано.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка додавання: " + ex.Message);
        }
    }

    static bool IsMaterialExist(MySqlConnection connection, string kod_materialu)
    {
        string query = "SELECT COUNT(*) FROM materials WHERE kod_materialy = @kod_materialu";
        var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@kod_materialu", kod_materialu);
        int count = Convert.ToInt32(cmd.ExecuteScalar());

        return count > 0;
    }

    static string ConvertToDateFormat(string date)
    {
        DateTime parsedDate;
        if (DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parsedDate))
        {
            return parsedDate.ToString("yyyy-MM-dd");
        }
        else
        {
            throw new Exception("Невірний формат дати. Будь ласка, введіть дату в форматі YYYY-MM-DD.");
        }
    }

    static void ViewZamovlennyaById(MySqlConnection connection)
    {
        Console.Write("Введіть kod_zamovlennya: ");
        string id = Console.ReadLine();

        string query = "SELECT * FROM zamovlennya WHERE kod_zamovlennya = @id";
        var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.WriteLine($"{reader.GetName(i)}: {reader[i]}");
            }
        }
        else
        {
            Console.WriteLine("Рядок не знайдено.");
        }
    }

    static void DeleteZamovlennyaById(MySqlConnection connection)
    {
        Console.Write("Введіть kod_zamovlennya для видалення: ");
        string id = Console.ReadLine();

        string query = "DELETE FROM zamovlennya WHERE kod_zamovlennya = @id";
        var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        int rowsAffected = cmd.ExecuteNonQuery();

        if (rowsAffected > 0)
            Console.WriteLine("Рядок видалено.");
        else
            Console.WriteLine("Рядок не знайдено.");

        static string PromptRequired(string prompt)
        {
            string value;
            do
            {
                Console.Write($"{prompt}: ");
                value = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine(" Поле не може бути порожнім. Спробуйте ще раз.");
                }
            } while (string.IsNullOrWhiteSpace(value));
            return value;
        }
    }
}