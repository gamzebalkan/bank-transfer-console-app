using System;
using BankTransferConsoleApp.Data;
using BankTransferConsoleApp.Models;

namespace BankTransferConsoleApp.Services
{
    public class AuthService
    {
        private readonly CustomerRepository _repo;

        public AuthService(CustomerRepository repo)
        {
            _repo = repo;
        }

       public Customer? Login()
{
    int attempts = 0;
    const int maxAttempts = 3;

    while (attempts < maxAttempts)
    {
        Console.WriteLine("\nWelcome. Please enter your customer number and password.\n");

        Console.Write("Customer No: ");
        string customerNo = (Console.ReadLine() ?? "").Trim();

        Console.Write("Password: ");
        string password = ReadPasswordMasked();

        // -----------------------------
        // PIN FORMAT VALIDATION
        // -----------------------------
        if (password.Length != 4 || !int.TryParse(password, out _))
        {
            Console.WriteLine("\nInvalid PIN format. Please enter a 4-digit numeric PIN.");

            attempts++;
            Console.WriteLine($"Attempts left: {maxAttempts - attempts}\n");

            continue;
        }

        // Fetch customer from database
        var customer = _repo.GetCustomerByCustomerNo(customerNo);

        if (customer == null)
        {
            attempts++;
            Console.WriteLine("\nInvalid customer number or password.");
            Console.WriteLine($"Attempts left: {maxAttempts - attempts}\n");
            continue;
        }

        // Verify password 
        bool isValid = BCrypt.Net.BCrypt.Verify(password, customer.PasswordHash);

        if (isValid)
        {
            return new Customer
            {
                CustomerNo = customer.CustomerNo,
                FullName = customer.FullName,
                Balance = customer.Balance
            };
        }

        attempts++;
        Console.WriteLine("\nInvalid customer number or password.");
        Console.WriteLine($"Attempts left: {maxAttempts - attempts}\n");
    }

    Console.WriteLine("\nToo many failed login attempts. Exiting for security reasons...");
    return null;
}

        // Masked password input
        private string ReadPasswordMasked()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                    break;

                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[..^1];
                    Console.Write("\b \b");
                }
                else if (key.Key != ConsoleKey.Backspace)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }

            } while (true);

            Console.WriteLine();
            return password;
        }
    }
}