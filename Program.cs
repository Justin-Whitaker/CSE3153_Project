using Microsoft.Data.SqlClient;
using System;

namespace CSE3153_Project
{
    internal class Program
    {
        const string ConnectionString = "Server=localhost;Database=Generic_Company;Trusted_Connection=True;";

        static bool running;

        static void Main(string[] args)
        {
            if (!CheckConnection(ConnectionString))
                return;

            running = true;

            //sentinel loop
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("----------Main Menu----------");
                Console.WriteLine("1. View Employees (SELECT)");
                Console.WriteLine("2. Add New Merchandis (INSERT)");
                Console.WriteLine("3. Update Department Budget (UPDATE)");
                Console.WriteLine("4. Remove Employee (DELETE)");
                Console.WriteLine("5. (Query)");
                Console.WriteLine("6. Exit Program");
                Console.WriteLine();
                Console.Write("Enter option number: ");

                if (!int.TryParse(Console.ReadLine(), out int selectedOption))
                {
                    Console.WriteLine("Input not a number");
                    continue;
                }

                switch (selectedOption)
                {
                    case 1:
                        ReadTable();
                        break;
                    case 2:
                        CreateRow();
                        break;
                    case 3:
                        UpdateRow();
                        break;
                    case 4:
                        DeleteRow();
                        break;
                    case 5:
                        Query();
                        break;
                    case 6:
                        Exit();
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
        }

        static bool CheckConnection(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                    connection.Open();
                return true;
            }
            catch
            {
                Console.WriteLine("Database not found");
                return false;
            }    
        }

        static void Exit()
        {
            running = false;
            Console.WriteLine("Exiting Program");
            Console.WriteLine();
        }

        static void ReadTable()
        {
            Console.WriteLine("View Employees (SELECT)  Selected");
            try
            {
                //print column names
                Console.WriteLine($"{"employee_id",-15}{"first_name",-15}{"last_name",-15}{"job_title",-15}{"salary",-15}{"dept_id",-15}{"building_id",-15}");
                Console.WriteLine(new string('-', 7 * 15));

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"SELECT * FROM company.Employee;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //print rows
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                    Console.Write($"{reader[i],-15}");
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void CreateRow()
        {
            Console.WriteLine("Add New Merchandise (INSERT)  Selected");
            try
            {
                Console.Write("Enter item name: ");
                string itemName = Console.ReadLine() ?? "";

                double cost;
                while (true)
                {
                    Console.Write("Enter item cost: ");

                    if (double.TryParse(Console.ReadLine(), out cost))
                        break;

                    Console.WriteLine("invalid input");
                }

                double price;
                while (true)
                {
                    Console.Write("Enter item price: ");

                    if (double.TryParse(Console.ReadLine(), out price))
                        break;

                    Console.WriteLine("invalid input");
                }

                int deptId;
                while (true)
                {
                    Console.Write("Enter item department id: ");

                    if (int.TryParse(Console.ReadLine(), out deptId))
                        break;

                    Console.WriteLine("invalid input");
                }


                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"INSERT INTO company.Merchandise (item_name, cost, price, dept_id) VALUES (@itemName, @cost, @price, @deptId);";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@itemName", itemName);
                        command.Parameters.AddWithValue("@cost", cost);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@deptId", deptId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void UpdateRow()
        {
            Console.WriteLine("Update Department Budget (UPDATE)  Selected");
        }

        static void DeleteRow()
        {
            Console.WriteLine("Remove Employee (DELETE)  Selected");
        }

        static void Query()
        {
            Console.WriteLine("Query Selected");
        }

    }
}
