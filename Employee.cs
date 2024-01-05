using System;
using System.Data.SqlClient;

class Program
{
    static string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\hp\\Desktop\\Assignment-5\\Assignment-5\\AssignmentFive.mdf;Integrated Security=True"; // Replace with your actual connection string

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nEmployee Database Menu:");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. Update Employee");
            Console.WriteLine("3. Delete Employee");
            Console.WriteLine("4. Search Employee ");
            Console.WriteLine("5. Display All Employees");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice (1-5): ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    AddEmployee(connectionString);
                    break;
                case "2":
                    UpdateEmployee(connectionString);
                    break;
                case "3":
                    DeleteEmployee(connectionString);
                    break;
                case "4":
                    SelectEmployeeById(connectionString);
                    break;
                case "5":
                    ReadAndPrintEmployees(connectionString);
                    break;
                case "6":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                    break;
            }

        }
    }
    static bool IsValidEmail(string email)
    {
        return email.Contains("@");
    }

    static bool IsValidPhoneNumber(string phoneNumber)
    {
        return phoneNumber.Length == 11 && phoneNumber.All(char.IsDigit);
    }

    static void ReadAndPrintEmployees(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT * FROM Employees";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["ID"]}, FirstName: {reader["FirstName"]}, LastName: {reader["LastName"]}, " +
                                              $"Email: {reader["Email"]}, PrimaryPhoneNumber: {reader["PrimaryPhoneNumber"]}, " +
                                              $"SecondaryPhoneNumber: {reader["SecondaryPhoneNumber"]}, CreatedBy: {reader["CreatedBy"]}, " +
                                              $"CreatedOn: {reader["CreatedOn"]}, ModifiedBy: {reader["ModifiedBy"]}, ModifiedOn: {reader["ModifiedOn"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No records found.");
                    }
                }
            }
        }
    }

    static void AddEmployee(string connectionString)
    {
        try
        {
            Console.WriteLine("Enter employee details:");

            string firstName;
            do
            {
                Console.Write("First Name: ");
                firstName = Console.ReadLine() ?? "";
            } while (string.IsNullOrWhiteSpace(firstName));

            string lastName;
            do
            {
                Console.Write("Last Name: ");
                lastName = Console.ReadLine() ?? "";
            } while (string.IsNullOrWhiteSpace(lastName));

            string email;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine() ?? "";
            } while (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email));

            string primaryPhoneNumber;
            do
            {
                Console.Write("Primary Phone Number: ");
                primaryPhoneNumber = Console.ReadLine() ?? "";
            } while (string.IsNullOrWhiteSpace(primaryPhoneNumber) || !IsValidPhoneNumber(primaryPhoneNumber));

            Console.Write("Secondary Phone no: ");
            string secondaryPhoneNumber = Console.ReadLine() ?? "";

            string createdBy;
            do
            {
                Console.Write("Created By: ");
                createdBy = Console.ReadLine() ?? "";
            } while (string.IsNullOrWhiteSpace(createdBy));

            DateTime createdOn = DateTime.Now;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Employees (FirstName, LastName, Email, PrimaryPhoneNumber, SecondaryPhoneNumber, CreatedBy, CreatedOn) " +
                               "VALUES (@FirstName, @LastName, @Email, @PrimaryPhoneNumber, @SecondaryPhoneNumber, @CreatedBy, @CreatedOn)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PrimaryPhoneNumber", primaryPhoneNumber);
                    command.Parameters.AddWithValue("@SecondaryPhoneNumber", secondaryPhoneNumber);
                    command.Parameters.AddWithValue("@CreatedBy", createdBy);
                    command.Parameters.AddWithValue("@CreatedOn", createdOn);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Record inserted successfully \n");
                    }
                    else
                    {
                        Console.WriteLine("Failed to insert the record :( \n");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void DeleteEmployee(string connectionString)
    {
        try
        {
            Console.Write("Enter the ID of the employee to delete:");
            int employeeIdToDelete;
            while (!int.TryParse(Console.ReadLine(), out employeeIdToDelete))
            {
                Console.Write("Invalid input!! Please enter a valid numeric ID :)");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Employees WHERE ID = @EmployeeId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeIdToDelete);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Record with ID {employeeIdToDelete} deleted successfully \n");
                    }
                    else
                    {
                        Console.WriteLine($"No record found with ID {employeeIdToDelete}. Deletion failed :( \n");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void SelectEmployeeById(string connectionString)
    {
        try
        {
            Console.Write("Enter the ID of the employee to Search");
            int employeeIdToSelect;
            while (!int.TryParse(Console.ReadLine(), out employeeIdToSelect))
            {
                Console.Write("Invalid input!! Please enter a valid numeric ID :)");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Employees WHERE ID = @EmployeeId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeIdToSelect);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"ID: {reader["ID"]}, FirstName: {reader["FirstName"]}, LastName: {reader["LastName"]}, " +
                                                  $"Email: {reader["Email"]}, PrimaryPhoneNumber: {reader["PrimaryPhoneNumber"]}, " +
                                                  $"SecondaryPhoneNumber: {reader["SecondaryPhoneNumber"]}, CreatedBy: {reader["CreatedBy"]}, " +
                                                  $"CreatedOn: {reader["CreatedOn"]}, ModifiedBy: {reader["ModifiedBy"]}, ModifiedOn: {reader["ModifiedOn"]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No record found with ID {employeeIdToSelect} :( \n");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static bool EmployeeExists(string connectionString, int employeeId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM Employees WHERE ID = @EmployeeId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EmployeeId", employeeId);
                int count = (int)command.ExecuteScalar();

                return count > 0;
            }
        }

    }

    static void UpdateEmployee(string connectionString)
    {
        Console.WriteLine("Enter details to update an employee record:");

        int employeeIdToUpdate;
        do
        {
            Console.Write("Employee ID to update: ");
        } while (!int.TryParse(Console.ReadLine(), out employeeIdToUpdate));

        if (!EmployeeExists(connectionString, employeeIdToUpdate))
        {
            Console.WriteLine($"No record found with ID {employeeIdToUpdate}!! Update failed :( \n");
            return;
        }

        string updatedFirstName;
        do
        {
            Console.Write("Updated First Name: ");
            updatedFirstName = Console.ReadLine() ?? "";
        } while (string.IsNullOrWhiteSpace(updatedFirstName));

        string updatedLastName;
        do
        {
            Console.Write("Updated Last Name: ");
            updatedLastName = Console.ReadLine() ?? "";
        } while (string.IsNullOrWhiteSpace(updatedLastName));

        string updatedEmail;
        do
        {
            Console.Write("Updated Email: ");
            updatedEmail = Console.ReadLine() ?? "";
        } while (string.IsNullOrWhiteSpace(updatedEmail) || !IsValidEmail(updatedEmail));

        string updatedPrimaryPhoneNumber;
        do
        {
            Console.Write("Updated Primary Phone Number: ");
            updatedPrimaryPhoneNumber = Console.ReadLine() ?? "";
        } while (string.IsNullOrWhiteSpace(updatedPrimaryPhoneNumber) || !IsValidPhoneNumber(updatedPrimaryPhoneNumber));

        string modifiedBy;
        do
        {
            Console.Write("Modified By: ");
            modifiedBy = Console.ReadLine() ?? "";
        } while (string.IsNullOrWhiteSpace(modifiedBy));

        DateTime modifiedOn = DateTime.Now;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string query = "UPDATE Employees SET " +
                               "FirstName = @UpdatedFirstName, " +
                               "LastName = @UpdatedLastName, " +
                               "Email = @UpdatedEmail, " +
                               "PrimaryPhoneNumber = @UpdatedPrimaryPhoneNumber, " +
                               "ModifiedBy = @ModifiedBy, " +
                               "ModifiedOn = @ModifiedOn " +
                               "WHERE ID = @EmployeeId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UpdatedFirstName", updatedFirstName);
                    command.Parameters.AddWithValue("@UpdatedLastName", updatedLastName);
                    command.Parameters.AddWithValue("@UpdatedEmail", updatedEmail);
                    command.Parameters.AddWithValue("@UpdatedPrimaryPhoneNumber", updatedPrimaryPhoneNumber);
                    command.Parameters.AddWithValue("@ModifiedBy", modifiedBy);
                    command.Parameters.AddWithValue("@ModifiedOn", modifiedOn);
                    command.Parameters.AddWithValue("@EmployeeId", employeeIdToUpdate);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Record with ID {employeeIdToUpdate} updated successfully \n");
                    }
                    else
                    {
                        Console.WriteLine($"No record found with ID {employeeIdToUpdate}!! Update failed :( \n");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
