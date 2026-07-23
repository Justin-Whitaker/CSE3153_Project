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
                Console.WriteLine("2. Add New Merchandise (INSERT)");
                Console.WriteLine("3. Update Department Budget (UPDATE)");
                Console.WriteLine("4. Remove Employee (DELETE)");
                Console.WriteLine("5. View Event Locations (Join Query)");
                Console.WriteLine("6. Exit Program");
                Console.WriteLine();
                Console.Write("Enter option number: ");

                //get menu input
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
                        JoinQuery();
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
            //catch error to determine if database can be reached
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                    connection.Open();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
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

                    string query = "SELECT * FROM company.Employee;";

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
                Console.WriteLine();
            }
        }

        static void CreateRow()
        {
            Console.WriteLine("Add New Merchandise (INSERT)  Selected");
            try
            {
                //get name
                Console.Write("Enter item name: ");
                string itemName = Console.ReadLine() ?? "";

                //get cost
                double cost;
                while (true)
                {
                    Console.Write("Enter item cost: ");

                    if (double.TryParse(Console.ReadLine(), out cost))
                        break;

                    Console.WriteLine("Invalid input");
                }

                //get price
                double price;
                while (true)
                {
                    Console.Write("Enter item price: ");

                    if (double.TryParse(Console.ReadLine(), out price))
                        break;

                    Console.WriteLine("Invalid input");
                }

                //get dept id
                int deptId;
                while (true)
                {
                    Console.Write("Enter item department id: ");

                    if (int.TryParse(Console.ReadLine(), out deptId))
                        break;

                    Console.WriteLine("Invalid input");
                }

                //create row using input
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO company.Merchandise (item_name, cost, price, dept_id) VALUES (@itemName, @cost, @price, @deptId);";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        //attrach inptus
                        command.Parameters.AddWithValue("@itemName", itemName);
                        command.Parameters.AddWithValue("@cost", cost);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@deptId", deptId);
                        int r = command.ExecuteNonQuery();

                        //success check
                        if (r > 0)
                            Console.WriteLine("Merchandise added");
                        else 
                            Console.WriteLine("Failed to add merchandise");
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                Console.WriteLine("The Department ID does not exist, please enter a valid department ID");
                Console.WriteLine("Current Department ID's: 1, 2, 3, 4");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine();
            }
        }

        static void UpdateRow()
        {
            Console.WriteLine("Update Department Budget (UPDATE)  Selected");
            try
            {
                //get dept id
                int id;
                while (true)
                {
                    Console.Write("Enter department Id: ");

                    if (int.TryParse(Console.ReadLine(), out id))
                        break;

                    Console.WriteLine("Invalid input");
                }

                //get budget
                int budget;
                while (true)
                {
                    Console.Write("Enter new budget: ");

                    if (int.TryParse(Console.ReadLine(), out budget))
                        break;

                    Console.WriteLine("Invalid input");
                }

                //sql update to department using input
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "UPDATE company.Department SET budget = @budget WHERE dept_id = @id;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {                   
                        //attach inputs
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@budget", budget);

                        int r = command.ExecuteNonQuery();

                        //success check
                        if (r > 0)
                            Console.WriteLine("Department budget updated");
                        else
                        {
                            Console.WriteLine("Department ID not found");
                            Console.WriteLine("Current Department ID's: 1, 2, 3, 4");
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
            }
        }

        static void DeleteRow()
        {
            Console.WriteLine("Remove Employee (DELETE)  Selected");
            try
            {
                //get employee id to delete
                int id;
                while (true)
                {
                    Console.Write("Enter the employee Id to delete: ");

                    if (int.TryParse(Console.ReadLine(), out id))
                        break;

                    Console.WriteLine("Invalid input");
                }

                //sql to delete employee
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM company.Employee WHERE employee_id = @id;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        //attach input
                        command.Parameters.AddWithValue("@id", id);
                        int r = command.ExecuteNonQuery();

                        //success check
                        if(r > 0) 
                            Console.WriteLine("Employee deleted");
                        else
                            Console.WriteLine("Employee ID not found");
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine();
            }
        }

        static void JoinQuery()
        {
            Console.WriteLine("View Event Locations (Join Query) Selected");
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    //join location to event
                    string query = @"SELECT e.event_id, e.event_name, l.building_name, l.location_address, l.us_state, l.country, l.zip_code
                                    FROM company.Event e
                                    JOIN company.Location l ON e.building_id = l.building_id
                                    ORDER BY e.event_date;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine();

                            //print headers
                            Console.WriteLine($"{reader.GetName(0),-10}{reader.GetName(1),-30}{reader.GetName(2),-20}{reader.GetName(3),-25}{reader.GetName(4),-12}" +
                                                $"{reader.GetName(5),-15}{reader.GetName(6),-6}");

                            Console.WriteLine(new string('-', 125));

                            //print rows
                            while (reader.Read())
                                Console.WriteLine($"{reader[0],-10}{reader[1],-30}{reader[2],-20}{reader[3],-25}{reader[4],-12}{reader[5],-15}{reader[6],-6}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
            }
        }

    }
}
